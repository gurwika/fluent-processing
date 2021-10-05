using FluentProcessing.Contracts.Steps;
using FluentProcessing.Test.Builders.Queries;
using FluentProcessing.Test.Builders.Results;
using System.Threading.Tasks;

namespace FluentProcessing.Test.Builders.Steps
{
    public class GenerateFinishingStep : IStepBody<StartWithQuery, FinishingQueryResult>
    {
        public Task<FinishingQueryResult> RunAsync(StartWithQuery request)
        {
            return Task.FromResult(new FinishingQueryResult { Finished = true });
        }
    }
}
