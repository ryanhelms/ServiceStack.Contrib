using System;
using System.Collections.Generic;
using ServiceStack.IO;
using System.Collections;
using ServiceStack.Contrib.Features.RedisVFSFeature.Interfaces;
using System.IO;

namespace ServiceStack.Contrib.Features.RedisVFSFeature.Providers
{
    public class RedisVirtualPathProvider : IRedisVirtualFiles
    {
        public string CombineVirtualPath(string basePath, string relativePath)
        {
            throw new System.NotImplementedException();
        }

        public bool FileExists(string virtualPath)
        {
            throw new System.NotImplementedException();
        }

        public bool DirectoryExists(string virtualPath)
        {
            throw new System.NotImplementedException();
        }

        public IVirtualFile GetFile(string virtualPath)
        {
            throw new System.NotImplementedException();
        }

        public string GetFileHash(string virtualPath)
        {
            throw new System.NotImplementedException();
        }

        public string GetFileHash(IVirtualFile virtualFile)
        {
            throw new System.NotImplementedException();
        }

        public IVirtualDirectory GetDirectory(string virtualPath)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable GetAllMatchingFiles(string globPattern, int maxDepth = 2147483647)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable GetAllFiles()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable GetRootFiles()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable GetRootDirectories()
        {
            throw new System.NotImplementedException();
        }

        public bool IsSharedFile(IVirtualFile virtualFile)
        {
            throw new System.NotImplementedException();
        }

        public bool IsViewFile(IVirtualFile virtualFile)
        {
            throw new System.NotImplementedException();
        }

        IEnumerable<IVirtualFile> IVirtualPathProvider.GetAllMatchingFiles(string globPattern, int maxDepth)
        {
            throw new NotImplementedException();
        }

        IEnumerable<IVirtualFile> IVirtualPathProvider.GetAllFiles()
        {
            throw new NotImplementedException();
        }

        IEnumerable<IVirtualFile> IVirtualPathProvider.GetRootFiles()
        {
            throw new NotImplementedException();
        }

        IEnumerable<IVirtualDirectory> IVirtualPathProvider.GetRootDirectories()
        {
            throw new NotImplementedException();
        }

        public void WriteFile(string filePath, string textContents)
        {
            throw new NotImplementedException();
        }

        public void WriteFile(string filePath, Stream stream)
        {
            throw new NotImplementedException();
        }

        public void WriteFiles(IEnumerable<IVirtualFile> files, Func<IVirtualFile, string> toPath = null)
        {
            throw new NotImplementedException();
        }

        public void DeleteFile(string filePath)
        {
            throw new NotImplementedException();
        }

        public void DeleteFiles(IEnumerable<string> filePaths)
        {
            throw new NotImplementedException();
        }

        public void DeleteFolder(string dirPath)
        {
            throw new NotImplementedException();
        }

        public IVirtualDirectory RootDirectory { get; }
        public string VirtualPathSeparator { get; }
        public string RealPathSeparator { get; }
    }
}