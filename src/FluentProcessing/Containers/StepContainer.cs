using FluentProcessing.Contracts.Abstraction;
using System;
using System.Collections.Generic;
using FluentProcessing.Enumaretions;

namespace FluentProcessing.Containers
{
    public class StepContainer
    {
        public StepType Type { get; set; }
        public Type ResponseType { get; set; }
        public IList<IStep> Steps { get; set; }
    }
}
