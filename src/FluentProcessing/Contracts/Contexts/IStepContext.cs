using FluentProcessing.Contracts.Builders;
using FluentProcessing.Contracts.Steps;
using System;
using System.Threading.Tasks;

namespace FluentProcessing.Contracts.Contexts
{
    public interface IStepContext<TResponse>
        where TResponse : class
    {
        IStepContext<NResponse> Then<NStepBody, NRequest, NResponse>(NRequest request)
                where NStepBody : class, IStepBody<NRequest, NResponse>
                where NResponse : class;

        IStepContext<NResponse> Then<NStepBody, NResponse>()
                where NStepBody : class, IStepBody<TResponse, NResponse>
                where NResponse : class;

        IStepContext<NResponse> ThenParallel<NResponse>(Action<IParallelProcessFlowBuilder> builder)
            where NResponse : class;

        IStepContext<NResponse> ThenSwitch<NResponse>(Action<ISwitchProcessFlowBuilder> builder)
            where NResponse : class;

        Task<TResponse> ExecuteAsync();
    }
}
