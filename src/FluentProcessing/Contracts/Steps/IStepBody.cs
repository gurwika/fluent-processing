using System.Threading.Tasks;

namespace FluentProcessing.Contracts.Steps
{
    public interface IStepBody<in TRequest, TResponse>
    {
        Task<TResponse> RunAsync(TRequest request);
    }
}
