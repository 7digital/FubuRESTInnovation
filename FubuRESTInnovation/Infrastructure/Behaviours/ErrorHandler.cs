using System;
using System.Net;
using FubuMVC.Core;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Runtime;
using FubuRESTInnovation.Handlers.Error;
using FubuRESTInnovation.Infrastructure.Errors;

namespace FubuRESTInnovation.Infrastructure.Behaviours
{
    public class ErrorHandler : IActionBehavior
    {
        private readonly IFubuRequest _request;
        private readonly IOutputWriter _writer;
        private readonly IPartialFactory _factory;

        public ErrorHandler(IFubuRequest request, IOutputWriter writer, IPartialFactory factory)
        {
            _request = request;
            _writer = writer;
            _factory = factory;
        }

        public IActionBehavior InsideBehavior { get; set; }

        public void Invoke()
        {
            ExceptionHandle(b => b.Invoke());
        }

        public void InvokePartial()
        {
            ExceptionHandle(b => b.InvokePartial());
        }

        private void ExceptionHandle(Action<IActionBehavior> actionBehaviour)
        {
            if (InsideBehavior == null) return;

            try
            {
                actionBehaviour(InsideBehavior);
            }
            catch (ApiException ex)
            {
                var request = new ErrorRequest {Message = ex.Message};
                _request.Set(request);
                _factory.BuildPartial(request.GetType()).InvokePartial();
                _writer.WriteResponseCode(ex.Status);
            }
        }

       
    }
}