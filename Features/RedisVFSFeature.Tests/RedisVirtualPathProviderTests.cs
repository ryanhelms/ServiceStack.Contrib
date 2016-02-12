using ServiceStack.IO;
using ServiceStack.VirtualPath;
using ServiceStack;
using System.Threading;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceStack.Contrib.Features.RedisVFSFeature;
using ServiceStack.Contrib.Features.RedisVFSFeature.Providers;
using ServiceStack.Contrib.Testing.MSTest;

namespace RedisVFSFeature.Tests
{
    [TestClass]
    public class RedisVirtualPathProviderTests : AppHostTestBase
    {
        public RedisVirtualPathProviderTests()
        {
            var redisVfsFeature = new RedisVfsFeature();

            AppHost.Plugins.Add(redisVfsFeature);

            redisVfsFeature.Register(AppHost);
        }

        public IVirtualPathProvider GetPathProvider()
        {
            return new RedisVirtualPathProvider(AppHost);
        }
   
        [TestMethod]
        public void Can_create_file()
        {
            var pathProvider = GetPathProvider();

            var filePath = "dir/file.txt";
            pathProvider.WriteFile(filePath, "file");

            var file = pathProvider.GetFile(filePath);

            Assert.IsTrue(file.ReadAllText() == "file");
            Assert.IsTrue(file.ReadAllText() == "file"); //can read twice

            Assert.IsTrue(file.VirtualPath == filePath);
            Assert.IsTrue(file.Name == "file.txt");
            Assert.IsTrue(file.Directory.Name == "dir");
            Assert.IsTrue(file.Directory.VirtualPath == "dir");
            Assert.IsTrue(file.Extension == "txt");

            Assert.IsTrue(file.Directory.Name == "dir");

            pathProvider.DeleteFolder("dir");
        }

        [TestMethod]
        public void Does_refresh_LastModified()
        {
            var pathProvider = GetPathProvider();

            var filePath = "dir/file.txt";
            pathProvider.WriteFile(filePath, "file1");

            var file = pathProvider.GetFile(filePath);
            var prevLastModified = file.LastModified;

            file.Refresh();
            Assert.IsTrue(file.LastModified ==prevLastModified);

            pathProvider.WriteFile(filePath, "file2");
            file.Refresh();

            if (file.GetType().Name == "S3VirtualFile" && file.LastModified == prevLastModified)
            {
                Thread.Sleep(1000);
                pathProvider.WriteFile(filePath, "file3");
                file.Refresh();
            }

            Assert.IsTrue(file.LastModified !=(prevLastModified));

            pathProvider.DeleteFolder("dir");
        }

        [TestMethod]
        public void Can_create_file_from_root()
        {
            var pathProvider = GetPathProvider();

            var filePath = "file.txt";
            pathProvider.WriteFile(filePath, "file");

            var file = pathProvider.GetFile(filePath);

            Assert.IsTrue(file.ReadAllText() == "file");
            Assert.IsTrue(file.Name ==filePath);
            Assert.IsTrue(file.Extension == "txt");

            Assert.IsTrue(file.Directory.VirtualPath == null);
            Assert.IsTrue(file.Directory.Name == null || file.Directory.Name == "App_Data");

            pathProvider.DeleteFiles(new[] { "file.txt" });
        }

        [TestMethod]
        public void Does_override_existing_file()
        {
            var pathProvider = GetPathProvider();

            pathProvider.WriteFile("file.txt", "original");
            pathProvider.WriteFile("file.txt", "updated");
            Assert.IsTrue(pathProvider.GetFile("file.txt").ReadAllText() == "updated");

            pathProvider.WriteFile("/a/file.txt", "original");
            pathProvider.WriteFile("/a/file.txt", "updated");
            Assert.IsTrue(pathProvider.GetFile("/a/file.txt").ReadAllText() == "updated");

            pathProvider.DeleteFiles(new[] { "file.txt", "/a/file.txt" });
            pathProvider.DeleteFolder("a");
        }

        [TestMethod]
        public void Can_view_files_in_Directory()
        {
            var pathProvider = GetPathProvider();

            var testdirFileNames = new[]
            {
                "testdir/a.txt",
                "testdir/b.txt",
                "testdir/c.txt",
            };

            testdirFileNames.Each(x => pathProvider.WriteFile(x, "textfile"));

            var testdir = pathProvider.GetDirectory("testdir");
            var filePaths = testdir.Files.Map(x => x.VirtualPath);

            Assert.IsTrue(filePaths.ToList() == testdirFileNames.ToList());

            var fileNames = testdir.Files.Map(x => x.Name);
            Assert.IsTrue(fileNames == testdirFileNames.Map(x => x.SplitOnLast('/').Last()));

            pathProvider.DeleteFolder("testdir");
        }

        [TestMethod]
        public void Does_resolve_nested_files_and_folders()
        {
            var pathProvider = GetPathProvider();

            var allFilePaths = new[] {
                "testfile.txt",
                "a/testfile-a1.txt",
                "a/testfile-a2.txt",
                "a/b/testfile-ab1.txt",
                "a/b/testfile-ab2.txt",
                "a/b/c/testfile-abc1.txt",
                "a/b/c/testfile-abc2.txt",
                "a/d/testfile-ad1.txt",
                "e/testfile-e1.txt",
            };

            allFilePaths.Each(x => pathProvider.WriteFile(x, x.SplitOnLast('.').First().SplitOnLast('/').Last()));

            Assert.IsTrue(allFilePaths.All(x => pathProvider.IsFile(x)));
            Assert.IsTrue(new[] { "a", "a/b", "a/b/c", "a/d", "e" }.All(x => pathProvider.IsDirectory(x)));

            Assert.IsTrue(!pathProvider.IsFile("notfound.txt"));
            Assert.IsTrue(!pathProvider.IsFile("a/notfound.txt"));
            Assert.IsTrue(!pathProvider.IsDirectory("f"));
            Assert.IsTrue(!pathProvider.IsDirectory("a/f"));
            Assert.IsTrue(!pathProvider.IsDirectory("testfile.txt"));
            Assert.IsTrue(!pathProvider.IsDirectory("a/testfile-a1.txt"));

            AssertContents(pathProvider.RootDirectory, new[] {
                    "testfile.txt",
                }, new[] {
                    "a",
                    "e"
                });

            AssertContents(pathProvider.GetDirectory("a"), new[] {
                    "a/testfile-a1.txt",
                    "a/testfile-a2.txt",
                }, new[] {
                    "a/b",
                    "a/d"
                });

            AssertContents(pathProvider.GetDirectory("a/b"), new[] {
                    "a/b/testfile-ab1.txt",
                    "a/b/testfile-ab2.txt",
                }, new[] {
                    "a/b/c"
                });

            AssertContents(pathProvider.GetDirectory("a").GetDirectory("b"), new[] {
                    "a/b/testfile-ab1.txt",
                    "a/b/testfile-ab2.txt",
                }, new[] {
                    "a/b/c"
                });

            AssertContents(pathProvider.GetDirectory("a/b/c"), new[] {
                    "a/b/c/testfile-abc1.txt",
                    "a/b/c/testfile-abc2.txt",
                }, new string[0]);

            AssertContents(pathProvider.GetDirectory("a/d"), new[] {
                    "a/d/testfile-ad1.txt",
                }, new string[0]);

            AssertContents(pathProvider.GetDirectory("e"), new[] {
                    "e/testfile-e1.txt",
                }, new string[0]);

            Assert.IsTrue(pathProvider.GetFile("a/b/c/testfile-abc1.txt").ReadAllText() == "testfile-abc1");
            Assert.IsTrue(pathProvider.GetDirectory("a").GetFile("b/c/testfile-abc1.txt").ReadAllText() == "testfile-abc1");
            Assert.IsTrue(pathProvider.GetDirectory("a/b").GetFile("c/testfile-abc1.txt").ReadAllText() == "testfile-abc1");
            Assert.IsTrue(pathProvider.GetDirectory("a").GetDirectory("b").GetDirectory("c").GetFile("testfile-abc1.txt").ReadAllText() == "testfile-abc1");

            var dirs = pathProvider.RootDirectory.Directories.Map(x => x.VirtualPath);
            Assert.IsTrue(dirs == new[] { "a", "e" }.ToList());

            var rootDirFiles = pathProvider.RootDirectory.GetAllMatchingFiles("*", 1).Map(x => x.VirtualPath);
            Assert.IsTrue(rootDirFiles == new[] { "testfile.txt" }.ToList());

            var allFiles = pathProvider.GetAllMatchingFiles("*").Map(x => x.VirtualPath);
            Assert.IsTrue(allFiles == allFilePaths.ToList());

            allFiles = pathProvider.GetAllFiles().Map(x => x.VirtualPath);
            Assert.IsTrue(allFiles == allFilePaths.ToList());

            pathProvider.DeleteFile("testfile.txt");
            pathProvider.DeleteFolder("a");
            pathProvider.DeleteFolder("e");

            Assert.IsTrue(pathProvider.GetAllFiles().ToList().Count ==0);
        }

        public void AssertContents(IVirtualDirectory dir, string[] expectedFilePaths, string[] expectedDirPaths)
        {
            var filePaths = dir.Files.Map(x => x.VirtualPath);
            Assert.IsTrue(filePaths == expectedFilePaths.ToList());

            var fileNames = dir.Files.Map(x => x.Name);
            Assert.IsTrue(fileNames == expectedFilePaths.Map(x => x.SplitOnLast('/').Last()));

            var dirPaths = dir.Directories.Map(x => x.VirtualPath);
            Assert.IsTrue(dirPaths == expectedDirPaths.ToList());

            var dirNames = dir.Directories.Map(x => x.Name);
            Assert.IsTrue(dirNames == expectedDirPaths.Map(x => x.SplitOnLast('/').Last()));
        }
    }
}

