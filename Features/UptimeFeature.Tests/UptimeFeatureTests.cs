using System;
using NUnit.Framework;
using ServiceStack.Contrib.Features.UptimeFeature.ServiceInterface;
using ServiceStack.Contrib.Testing.NUnit;

namespace ServiceStack.Contrib.Features.Uptime.Tests
{
    [TestFixture]
    public class UptimeServiceTests : AppHostTestBase
    {
        public DateTime OriginalStartedAt { get; set; }

        public UptimeService SUT { get; set; }

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            OriginalStartedAt = AppHost.StartedAt;
            SUT = new UptimeService();
        }

        [OneTimeTearDown]
        public void OneTimeTeardown()
        {
            AppHost.Dispose();
        }

        [Test]
        public void Should_return_valid_uptime_string_with_seconds()
        {
            // Arrange
            AppHost.StartedAt = OriginalStartedAt.Subtract(new TimeSpan(0, 0, 0, 30));

            // Act
            var response = SUT.Any(new UptimeFeature.ServiceModel.Uptime());

            // Assert
            Assert.IsTrue(response.Duration.Contains(" Second"));
            Assert.IsFalse(response.Duration.Contains(" Minute"));

            Console.WriteLine("Duration: {0}".Fmt(response.Duration));
        }

        [Test]
        public void Should_return_valid_uptime_string_with_minutes()
        {
            // Arrange
            AppHost.StartedAt = OriginalStartedAt.Subtract(new TimeSpan(0, 0, 1, 0));

            // Act
            var response = SUT.Any(new UptimeFeature.ServiceModel.Uptime());

            // Assert
            Assert.IsTrue(response.Duration.Contains(" Minute"));
            Assert.IsFalse(response.Duration.Contains(" Hour"));
        }

        [Test]
        public void Should_return_valid_uptime_string_with_hours()
        {
            // Arrange
            AppHost.StartedAt = OriginalStartedAt.Subtract(new TimeSpan(0, 1, 0, 0));

            // Act
            var response = SUT.Any(new UptimeFeature.ServiceModel.Uptime());

            // Assert
            Assert.IsTrue(response.Duration.Contains(" Hour"));
            Assert.IsFalse(response.Duration.Contains(" Day"));
        }

        [Test]
        public void Should_return_valid_uptime_string_with_days()
        {
            // Arrange
            AppHost.StartedAt = OriginalStartedAt.Subtract(new TimeSpan(1, 0, 0, 0));

            // Act
            var response = SUT.Any(new UptimeFeature.ServiceModel.Uptime());

            // Assert
            Assert.IsTrue(response.Duration.Contains(" Day"));
        }
    }
}
