using System;
using System.Linq;
using FubuMVC.Core;
using FubuRESTInnovation.Handlers.Home;
using FubuRESTInnovation.Infrastructure.Behaviours;
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
                    handlerype => handlerype.Namespace.Contains("FubuRESTInnovation.Handlers") 
                    && new [] {"get", "put", "post", "delete"}.Any(verb => verb == handlerype.Name.ToLower())
                 );

            Routes
                .IgnoreNamespaceText("FubuRESTInnovation.Handlers")
                .IgnoreNamespaceText("FubuRESTInnovation.Handlers.Home")
                .IgnoreControllerNamesEntirely()
                .IgnoreMethodsNamed("Invoke")
                .ConstrainToHttpMethod(x => x.HandlerType.Name.ToLower() == "get", "Get")
                .ConstrainToHttpMethod(x => x.HandlerType.Name.ToLower() == "post", "Post")
                .RootAtAssemblyNamespace()
                .HomeIs<Get>(x => x.Invoke());

            Output.To<CodecSelectorNode>();

            Policies
                .WrapBehaviorChainsWith<ErrorHandler>();

        }

        public SevenDizzleRegistry(Action<FubuRegistry> configure) : base(configure)
        {
        }
    }
}