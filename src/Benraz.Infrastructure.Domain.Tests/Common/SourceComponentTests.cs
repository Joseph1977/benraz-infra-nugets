using FluentAssertions;
using NUnit.Framework;
using Benraz.Infrastructure.Domain.Common;

namespace Benraz.Infrastructure.Domain.Tests.Common
{
    [TestFixture]
    public class SourceComponentTests
    {
        [Test]
        public void PspV21_ReturnsGuid()
        {
            SourceComponent.Scheduler.Should().NotBeEmpty();
        }
    }
}




