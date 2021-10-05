using FluentProcessing.Concrete;
using FluentProcessing.Contracts.Abstraction;
using FluentProcessing.Contracts.Builders;
using FluentProcessing.Contracts.Steps;
using System;
using System.Collections.Generic;

namespace FluentProcessing.Builders
{
    public class SwitchProcessFlowBuilder : ISwitchProcessFlowBuilder
    {
        public IList<IStep> Steps { get; }

        public SwitchProcessFlowBuilder()
        {
            Steps = new List<IStep>();
        }

        public ISwitchProcessFlowBuilder CaseWhen<NStepBody, NRequest, NResponse>(Func<NRequest, bool> caseFunction)
            where NStepBody :  IStepBody<NRequest, NResponse>
            where NResponse : class
        {
            Steps.Add(
                    new Step<NStepBody, NRequest, NResponse>
                    {
                        CaseDelegate = (r) => caseFunction((NRequest)r)
                    }
            );

            return this;
        }

        public ISwitchProcessFlowBuilder CaseWhen<NStepBody, NRequest, NResponse>(NRequest request, Func<NRequest, bool> caseFunction)
            where NStepBody : IStepBody<NRequest, NResponse>
            where NResponse : class
        {
            Steps.Add(
                    new Step<NStepBody, NRequest, NResponse>
                    {
                        Request = request,
                        CaseDelegate = (r) => caseFunction((NRequest)r)
                    }
            );

            return this;
        }
    }
}
