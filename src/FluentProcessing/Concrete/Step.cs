using FluentProcessing.Contracts.Abstraction;
using FluentProcessing.Contracts.Steps;
using System;

namespace FluentProcessing.Concrete
{
    public class Step<TStepBody, TRequest, TResponse> : IStep
        where TStepBody : IStepBody<TRequest, TResponse>
    {
        public override Func<object, bool> CaseDelegate { get; set; }
        public override object Request { get; set; }
        public override Type BodyType => typeof(TStepBody);
    }
}
