using FluentProcessing.Contracts.Steps;
using FluentProcessing.Test.Builders.Queries;
using FluentProcessing.Test.Builders.Results;
using System.Threading.Tasks;

namespace FluentProcessing.Test.Builders.Steps
{
    public class GenerateStartPointStep : IStepBody<StartWithQuery, StartingQueryResult>
    {
        public Task<StartingQueryResult> RunAsync(StartWithQuery request)
        {
            return Task.FromResult(new StartingQueryResult { Started = true });
        }
    }
}
