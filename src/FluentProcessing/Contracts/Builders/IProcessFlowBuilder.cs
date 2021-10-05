using FluentProcessing.Contracts.Contexts;
using FluentProcessing.Contracts.Steps;
using System;

namespace FluentProcessing.Contracts.Builders
{
    public interface IProcessFlowBuilder
    {
        IStepContext<TResponse> StartWith<TStepBody, TRequest, TResponse>(TRequest request)
            where TStepBody : IStepBody<TRequest, TResponse>
            where TResponse : class;

        IStepContext<TRequest> StartWith<TRequest>(TRequest request)
            where TRequest : class;

        IStepContext<TResponse> StartWithParallel<TResponse>(Action<IParallelProcessFlowBuilder> builder)
                    where TResponse : class;
    }
}
