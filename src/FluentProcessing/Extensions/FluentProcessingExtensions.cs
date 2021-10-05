using FluentProcessing.Builders;
using FluentProcessing.Contracts.Builders;
using Microsoft.Extensions.DependencyInjection;

namespace FluentProcessing.Extensions
{
    public static class ProcessFlowExtensions
    {
        public static IServiceCollection InstallFluentProcessing(this IServiceCollection services)
        {
            _ = services.AddTransient<ISwitchProcessFlowBuilder, SwitchProcessFlowBuilder>();
            _ = services.AddTransient<IParallelProcessFlowBuilder, ParallelProcessFlowBuilder>();
            _ = services.AddTransient<IProcessFlowBuilder, ProcessFlowBuilder>();

            return services;
        }
    }
}
