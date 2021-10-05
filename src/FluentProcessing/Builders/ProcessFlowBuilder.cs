using FluentProcessing.Concrete;
using FluentProcessing.Containers;
using FluentProcessing.Contexts;
using FluentProcessing.Contracts.Abstraction;
using FluentProcessing.Contracts.Builders;
using FluentProcessing.Contracts.Contexts;
using FluentProcessing.Contracts.Steps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FluentProcessing.Enumaretions;

namespace FluentProcessing.Builders
{
    public class ProcessFlowBuilder : IProcessFlowBuilder
    {
        private object _response = default;
        private readonly IList<StepContainer> _steps;
        private readonly IServiceProvider _serviceProvider;

        public ProcessFlowBuilder(IServiceProvider serviceProvider)
        {
            _steps = new List<StepContainer>();
            _serviceProvider = serviceProvider;
        }

        public IStepContext<TResponse> StartWith<TResponse>(TResponse request) where TResponse : class
        {
            _response = request;

            return new StepContext<TResponse>(this);
        }

        public IStepContext<TResponse> StartWith<TStepBody, TRequest, TResponse>(TRequest request) 
            where TStepBody : IStepBody<TRequest, TResponse>
            where TResponse : class
        {
            AddStep(new StepContainer
            {
                Type = StepType.Step,
                Steps = new List<IStep>
                {
                    new Step<TStepBody, TRequest, TResponse>
                    {
                        Request = request
                    }
                }
            });

            return new StepContext<TResponse>(this);
        }

        public IStepContext<TResponse> StartWithParallel<TResponse>(Action<IParallelProcessFlowBuilder> builder) where TResponse : class
        {
            var parallelProcessFlowBuilder = new ParallelProcessFlowBuilder { };
            builder.Invoke(parallelProcessFlowBuilder);

            AddStep(new StepContainer
            {
                Type = StepType.ParallelSteps,
                ResponseType = typeof(TResponse),
                Steps = parallelProcessFlowBuilder.Steps
            });

            return new StepContext<TResponse>(this);
        }

        internal IStepContext<TResponse> StartWithSwitch<TResponse>(Action<ISwitchProcessFlowBuilder> builder)
            where TResponse : class
        {
            var switchProcessFlowBuilder = new SwitchProcessFlowBuilder { };
            builder.Invoke(switchProcessFlowBuilder);

            AddStep(new StepContainer
            {
                Type = StepType.SwitchSteps,
                ResponseType = typeof(TResponse),
                Steps = switchProcessFlowBuilder.Steps
            });

            return new StepContext<TResponse>(this);
        }

        #region InternalMethods
        internal async Task<TResponse> ExecuteAsync<TResponse>()
            where TResponse : class
        {
            var response = _response;
            foreach (var step in _steps)
            {
                if (step.Type == StepType.Step)
                {
                    response = await ExecuteStepAsync(step.Steps[0].BodyType, step.Steps[0].Request ?? response);
                }
                else if (step.Type == StepType.SwitchSteps)
                {
                    var executed = false;
                    foreach (var process in step.Steps)
                    {
                        if (process.CaseDelegate(process.Request ?? response))
                        {
                            response = await ExecuteStepAsync(process.BodyType, process.Request ?? response);
                            executed = true;
                            break;
                        }
                    }

                    if (!executed)
                    {
                        var responseMatchesToRequest = (response != null && response.GetType() == step.ResponseType);
                        var stepResponseObject = responseMatchesToRequest ? response : Activator.CreateInstance(step.ResponseType);
                        response = stepResponseObject;
                    }
                }
                else if (step.Type == StepType.ParallelSteps)
                {
                    var asyncStepsList = new List<Task>();
                    var asyncStepsDynamicList = new List<dynamic>();
                    foreach (var process in step.Steps)
                    {
                        var task = ExecuteStepAsync(process.BodyType, process.Request ?? response);
                        asyncStepsDynamicList.Add((object)task);
                        asyncStepsList.Add((Task)task);
                    }

                    await Task.WhenAll(asyncStepsList);

                    var stepResponseProperties = step.ResponseType.GetProperties();
                    var responseMatchesToRequest = (response != null && response.GetType() == step.ResponseType);
                    var stepResponseObject = responseMatchesToRequest ? response : Activator.CreateInstance(step.ResponseType);
                    for (var i = 0; i < step.Steps.Count; i++)
                    {
                        var asyncResponseObject = await asyncStepsDynamicList[i];
                        var asyncResponseType = asyncResponseObject.GetType();
                        var stepProperty = stepResponseProperties.FirstOrDefault(x => x.PropertyType.Equals(asyncResponseType));
                        stepProperty?.SetValue(stepResponseObject, asyncResponseObject);
                    }

                    response = stepResponseObject;
                }
            }

            _steps.Clear();

            return response as TResponse;
        }
        #endregion

        #region PrivateMethods
        private void AddStep(StepContainer step)
        {
            _steps.Add(step);
        }

        private dynamic ExecuteStepAsync(Type type, object requestParam)
        {
            object stepObject = null;
            if (!type.IsInterface)
            {
                var constructor = type.GetConstructors().First();
                var parameters = constructor.GetParameters()
                    .Select(x => _serviceProvider.GetService(x.ParameterType)
                );
                stepObject = Activator.CreateInstance(type, parameters.ToArray());
            }
            else
            {
                stepObject = _serviceProvider.GetService(type);
            }
            
            var stepMethod = type.GetMethod("RunAsync");
            try
            {
                return stepMethod.Invoke(stepObject, new object[] { requestParam });
            }
            catch (TargetInvocationException ex) when(ex.InnerException != null)
            {
                throw ex.InnerException;
            }
        }
        #endregion
    }
}
