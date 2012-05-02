using System;
using System.Collections.Generic;
using System.Linq;
using FubuMVC.Core;
using FubuMVC.Core.Registration;
using FubuMVC.Core.Registration.Nodes;
using FubuRESTInnovation.Handlers.Home;
using FubuRESTInnovation.Infrastructure.Behaviours;
using FubuRESTInnovation.Infrastructure.Errors;
using FubuRESTInnovation.Infrastructure.Output;

namespace FubuRESTInnovation.Configuration
{
    public class SevenDizzleRegistry : FubuRegistry
    {
        public SevenDizzleRegistry()
        {
            IncludeDiagnostics(true);

            Actions
                .IncludeTypes(
                    handlerype => handlerype.Namespace.Contains("FubuRESTInnovation.Handlers") // all handlers live in this namespace
                    && 
                    (new [] {"get", "put", "post", "delete"}.Any(verb => verb == handlerype.Name.ToLower()) // handlers must be a http verb
                    || handlerype.Name == "Index") // or handlers must be called Index
                 );

            Routes
                .IgnoreNamespaceText("FubuRESTInnovation.Handlers")
                .IgnoreNamespaceText("FubuRESTInnovation.Handlers.Home")
                .IgnoreControllerNamesEntirely()
                .IgnoreMethodsNamed("Invoke")
                .ConstrainToHttpMethod(x => x.HandlerType.Name.ToLower() == "get", "Get")
                .ConstrainToHttpMethod(x => x.HandlerType.Name.ToLower() == "post", "Post")
                .ConstrainToHttpMethod(x => x.HandlerType.Name.ToLower() == "put", "Put")
                .RootAtAssemblyNamespace()
                .HomeIs<Get>(x => x.Invoke());

            Output.To(x => (OutputNode) Activator.CreateInstance(typeof (CodecSelectorNode<>).MakeGenericType(x.OutputType())));

            Policies
                .WrapBehaviorChainsWith<ApiErrorsHandler>();

            ApplyConvention<BindingErrorsConvention>();
        }

        public SevenDizzleRegistry(Action<FubuRegistry> configure) : base(configure)
        {
        }
    }

    public class BindingErrorsConvention : IConfigurationAction
    {
        public void Configure(BehaviorGraph graph)
        {
            // handle model binding errors when there is an input model
            graph
                .Actions()
                .Where(g => g.InputType() != null)
                .Each(g => g.AddBefore(new Wrapper(typeof (BindingErrorsHandler<>).MakeGenericType(g.InputType()))));
        }
    }
}