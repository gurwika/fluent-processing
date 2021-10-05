using FluentProcessing.Concrete;
using FluentProcessing.Contracts.Abstraction;
using FluentProcessing.Contracts.Builders;
using FluentProcessing.Contracts.Steps;
using System.Collections.Generic;

namespace FluentProcessing.Builders
{
    public class ParallelProcessFlowBuilder : IParallelProcessFlowBuilder
    {
        public IList<IStep> Steps { get; }

        public ParallelProcessFlowBuilder()
        {
            Steps = new List<IStep>();
        }

        public IParallelProcessFlowBuilder Execute<NStepBody, NRequest, NResponse>(NRequest request = default)
            where NStepBody : IStepBody<NRequest, NResponse>
            where NResponse : class
        {
            Steps.Add(
                new Step<NStepBody, NRequest, NResponse>
                {
                    Request = request
                }
            );

            return this;
        }
    }
}
