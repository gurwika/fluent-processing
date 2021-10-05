using FluentProcessing.Contracts.Steps;
using FluentProcessing.Test.Builders.Queries;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FluentProcessing.Test.Builders.Steps
{
    public class ThrowExceptionStep : IStepBody<StartWithQuery, Task>
    {
        public Task<Task> RunAsync(StartWithQuery request)
        {
            throw new ApplicationException();
        }
    }
}
