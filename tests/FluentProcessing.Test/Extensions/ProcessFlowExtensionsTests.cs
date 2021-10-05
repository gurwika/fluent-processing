using FluentAssertions;
using FluentProcessing.Contracts.Builders;
using FluentProcessing.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Xunit;

namespace FluentProcessing.Test.Extensions
{
    public class ProcessFlowExtensionsTests
    {
        private readonly IServiceCollection _serviceCollection;

        public ProcessFlowExtensionsTests()
        {
            _serviceCollection = new ServiceCollection();
        }

        [Fact]
        public void ProcessFlowExtensions_ShouldInstallWorkFlow()
        {
            // Arrange
            var sut = _serviceCollection;

            // Act
            _ = sut.InstallFluentProcessing();

            // Assert
            _ = sut.Any(x => x.ServiceType == typeof(IProcessFlowBuilder)).Should().BeTrue();
        }
    }
}
