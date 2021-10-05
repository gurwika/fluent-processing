using FluentProcessing.Contracts.Abstraction;
using FluentProcessing.Contracts.Steps;
using System.Collections.Generic;

namespace FluentProcessing.Contracts.Builders
{
    public interface IParallelProcessFlowBuilder
    {
        IList<IStep> Steps { get; }

        IParallelProcessFlowBuilder Execute<TStepBody, TRequest, TResponse>(TRequest request = default)
            where TStepBody : IStepBody<TRequest, TResponse>
            where TResponse : class;
    }
}
