using System;

namespace FluentProcessing.Contracts.Abstraction
{
    public abstract class IStep
    {
        public abstract Func<object, bool> CaseDelegate { get; set; }
        public abstract object Request { get; set; }
        public abstract Type BodyType { get; }
    }
}
