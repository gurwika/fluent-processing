using FluentAssertions;
using FluentProcessing.Builders;
using FluentProcessing.Contracts.Builders;
using FluentProcessing.Test.Builders.Queries;
using FluentProcessing.Test.Builders.Results;
using FluentProcessing.Test.Builders.Steps;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using FluentProcessing.Contracts.Steps;
using Xunit;

namespace FluentProcessing.Test.Builders
{
    public class ProcessFlowBuilderTests
    {
        private readonly ServiceProvider _serviceProvider;
        private IProcessFlowBuilder ProcessFlowBuilder => new ProcessFlowBuilder(_serviceProvider);

        public ProcessFlowBuilderTests()
        {
            _serviceProvider = new ServiceCollection()
                .AddSingleton<IStepBody<StartWithQuery, StartingQueryResult>, GenerateStartPointStep>()
                .AddSingleton<GenerateFinishingStep>()
                .BuildServiceProvider();
        }

        [Fact]
        public void ProcessFlowBuilder_ShouldThrowInnerExceptions()
        {
            // Arrange
            var sut = ProcessFlowBuilder;
            var query = new StartWithQuery { };

            // Act
            Func<Task> f = async () => _ = await sut
                .StartWith<ThrowExceptionStep, StartWithQuery, Task>(query)
                .ExecuteAsync();

            // Assert
            _ = f.Should().ThrowExactly<ApplicationException>();
        }

        [Fact]
        public async Task StartWith_ShouldReturnRequest()
        {
            // Arrange
            var sut = ProcessFlowBuilder;
            var query = new StartWithQuery { };

            // Act
            var result = await sut
                .StartWith(query)
                .ExecuteAsync();

            // Assert
            _ = result.Should().Be(query);
        }

        [Fact]
        public async Task StartWith_ShouldProcessStep()
        {
            // Arrange
            var sut = ProcessFlowBuilder;
            var query = new StartWithQuery { };

            // Act
            var result = await sut
                .StartWith<GenerateStartPointStep, StartWithQuery, StartingQueryResult>(query)
                .ExecuteAsync();

            // Assert
            _ = result.Started.Should().BeTrue();
        }
        
        [Fact]
        public async Task StartWith_ShouldProcessStep_WhenNewRequestArePassed()
        {
            // Arrange
            var sut = ProcessFlowBuilder;
            var query = new StartWithQuery { };

            // Act
            var result = await sut
                .StartWith(query)
                .Then<GenerateStartPointStep, StartWithQuery, StartingQueryResult>(query)
                .ExecuteAsync();

            // Assert
            _ = result.Started.Should().BeTrue();
        }
        
        [Fact]
        public async Task StartWith_ShouldProcessStep_WhenNewRequestAreNotPassed()
        {
            // Arrange
            var sut = ProcessFlowBuilder;
            var query = new StartWithQuery { };

            // Act
            var result = await sut
                .StartWith(query)
                .Then<GenerateStartPointStep, StartingQueryResult>()
                .ExecuteAsync();

            // Assert
            _ = result.Started.Should().BeTrue();
        }
        
        [Fact]
        public async Task StartWith_ShouldProcessStep_WhenStepIsInterface()
        {
            // Arrange
            var sut = ProcessFlowBuilder;
            var query = new StartWithQuery { };

            // Act
            var result = await sut
                .StartWith<IStepBody<StartWithQuery, StartingQueryResult>, StartWithQuery, StartingQueryResult>(query)
                .ExecuteAsync();

            // Assert
            _ = result.Started.Should().BeTrue();
        }

        [Fact]
        public async Task StartWithParallel_ShouldRunParallelSteps()
        {
            // Arrange
            var sut = ProcessFlowBuilder;
            var query = new StartWithQuery { };

            // Act
            var result = await sut
                .StartWithParallel<ParallResult>(x => x
                    .Execute<GenerateStartPointStep, StartWithQuery, StartingQueryResult>(query)
                    .Execute<GenerateFinishingStep, StartWithQuery, FinishingQueryResult>(query)
                )
                .ExecuteAsync();

            // Assert
            _ = result.Starting.Started.Should().BeTrue();
            _ = result.Finishing.Finished.Should().BeTrue();
        }

        [Fact]
        public async Task StartWithParallel_ShouldRunParallelSteps_AfterStart()
        {
            // Arrange
            var sut = ProcessFlowBuilder;
            var query = new StartWithQuery { };

            // Act
            var result = await sut
                .StartWith(query)
                .ThenParallel<ParallResult>(x => x
                    .Execute<GenerateStartPointStep, StartWithQuery, StartingQueryResult>(query)
                    .Execute<GenerateFinishingStep, StartWithQuery, FinishingQueryResult>(query)
                )
                .ExecuteAsync();

            // Assert
            _ = result.Starting.Started.Should().BeTrue();
            _ = result.Finishing.Finished.Should().BeTrue();
        }

        [Fact]
        public async Task StartWith_ShouldProcessSwitchSteps()
        {
            // Arrange
            var sut = ProcessFlowBuilder;
            var query = new StartWithQuery { };

            // Act
            var result = await sut
                .StartWith(query)
                .ThenSwitch<FinishingQueryResult>(x => x
                    .CaseWhen<SwitchResultNonExecutorStep, StartWithQuery, FinishingQueryResult>(_ => false)
                    .CaseWhen<SwitchResultExecutorStep, StartWithQuery, FinishingQueryResult>(query, _ => true)
                )
                .ExecuteAsync();

            // Assert
            _ = result.Finished.Should().BeTrue();
        }
        
        [Fact]
        public void StartWith_ShouldProcessSwitchSteps_WhenInnerExceptionIsThrown()
        {
            // Arrange
            var sut = ProcessFlowBuilder;
            var query = new StartWithQuery { };

            // Act
            Func<Task> f = async () => _ = await sut
                .StartWith(query)
                .ThenSwitch<FinishingQueryResult>(x => x
                    .CaseWhen<SwitchResultNonExecutorStep, StartWithQuery, FinishingQueryResult>(_ => true)
                )
                .ExecuteAsync();

            // Assert
            _ = f.Should().ThrowExactly<ApplicationException>();
        }

        [Fact]
        public async Task StartWith_ShouldProcessSwitchSteps_WhenNonOfTheSwitchContextSucceed()
        {
            // Arrange
            var sut = ProcessFlowBuilder;
            var query = new StartWithQuery { };

            // Act
            var result = await sut
                .StartWith(query)
                .ThenSwitch<FinishingQueryResult>(x => x
                    .CaseWhen<SwitchResultNonExecutorStep, StartWithQuery, FinishingQueryResult>(_ => false)
                    .CaseWhen<SwitchResultExecutorStep, StartWithQuery, FinishingQueryResult>(query, _ => false)
                )
                .ExecuteAsync();

            // Assert
            _ = result.Finished.Should().BeFalse();
        }
    }
}
