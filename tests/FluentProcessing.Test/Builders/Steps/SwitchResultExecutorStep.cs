using System.Threading.Tasks;
using FluentProcessing.Contracts.Steps;
using FluentProcessing.Test.Builders.Queries;
using FluentProcessing.Test.Builders.Results;

namespace FluentProcessing.Test.Builders.Steps
{
    public class SwitchResultExecutorStep : IStepBody<StartWithQuery, FinishingQueryResult>
    {
        private readonly GenerateFinishingStep _step;

        public SwitchResultExecutorStep(GenerateFinishingStep step)
        {
            _step = step;
        }
        
        public Task<FinishingQueryResult> RunAsync(StartWithQuery request)
        {
            return _step.RunAsync(request);
        }
    }
}