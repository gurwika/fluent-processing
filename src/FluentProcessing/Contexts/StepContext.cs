using FluentProcessing.Builders;
using FluentProcessing.Contracts.Builders;
using FluentProcessing.Contracts.Contexts;
using FluentProcessing.Contracts.Steps;
using System;
using System.Threading.Tasks;

namespace FluentProcessing.Contexts
{
    public class StepContext<TResponse> : IStepContext<TResponse>
        where TResponse : class
    {
        private readonly ProcessFlowBuilder _processFlowBuilder;

        public StepContext(ProcessFlowBuilder processFlowBuilder)
        {
            _processFlowBuilder = processFlowBuilder;
        }

        public Task<TResponse> ExecuteAsync()
        {
            return _processFlowBuilder.ExecuteAsync<TResponse>();
        }

        public IStepContext<TNewResponse> Then<TStepBody, TRequest, TNewResponse>(TRequest request)
                where TStepBody : class, IStepBody<TRequest, TNewResponse>
                where TNewResponse : class
        {
            return _processFlowBuilder.StartWith<TStepBody, TRequest, TNewResponse>(request);
        }

        public IStepContext<TNewResponse> Then<TStepBody, TNewResponse>()
                where TStepBody : class, IStepBody<TResponse, TNewResponse>
                where TNewResponse : class
        {
            return _processFlowBuilder.StartWith<TStepBody, TResponse, TNewResponse>(null);
        }

        public IStepContext<TNewResponse> ThenParallel<TNewResponse>(Action<IParallelProcessFlowBuilder> builder) where TNewResponse : class
        {
            return _processFlowBuilder.StartWithParallel<TNewResponse>(builder);
        }

        public IStepContext<TNewResponse> ThenSwitch<TNewResponse>(Action<ISwitchProcessFlowBuilder> builder) where TNewResponse : class
        {
            return _processFlowBuilder.StartWithSwitch<TNewResponse>(builder);
        }

    }
}
