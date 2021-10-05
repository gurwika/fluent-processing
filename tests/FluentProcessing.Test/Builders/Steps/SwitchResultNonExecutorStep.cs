using System;
using System.Threading.Tasks;
using FluentProcessing.Contracts.Steps;
using FluentProcessing.Test.Builders.Queries;
using FluentProcessing.Test.Builders.Results;

namespace FluentProcessing.Test.Builders.Steps
{
    public class SwitchResultNonExecutorStep: IStepBody<StartWithQuery, FinishingQueryResult>
    {
        public Task<FinishingQueryResult> RunAsync(StartWithQuery request)
        {
            throw new ApplicationException();
        }
    }
}