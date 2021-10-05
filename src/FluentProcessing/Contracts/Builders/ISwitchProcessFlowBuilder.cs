using FluentProcessing.Contracts.Abstraction;
using FluentProcessing.Contracts.Steps;
using System;
using System.Collections.Generic;

namespace FluentProcessing.Contracts.Builders
{
    public interface ISwitchProcessFlowBuilder
    {
        IList<IStep> Steps { get; }

        ISwitchProcessFlowBuilder CaseWhen<TStepBody, TRequest, TResponse>(Func<TRequest, bool> caseFunction)
            where TStepBody : IStepBody<TRequest, TResponse>
            where TResponse : class;

        ISwitchProcessFlowBuilder CaseWhen<TStepBody, TRequest, TResponse>(TRequest request, Func<TRequest, bool> caseFunction)
            where TStepBody : IStepBody<TRequest, TResponse>
            where TResponse : class;
    }
}
