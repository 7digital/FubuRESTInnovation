using System;
using System.Net;
using FubuMVC.Core;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Continuations;
using FubuMVC.Core.Http;
using FubuRESTInnovation.Infrastructure.Errors;
using FubuRESTInnovation.Infrastructure.Output;

namespace FubuRESTInnovation.Infrastructure.Behaviours
{
    public class ErrorHandler : IActionBehavior
    {
        private readonly IHttpWriter _writer;
        private readonly IRequestHeaders _headers;

        public ErrorHandler(IHttpWriter writer, IRequestHeaders headers)
        {
            _writer = writer;
            _headers = headers;
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
                var model = new ErrorResponse {Error = ex.Message};
                CodecSelector<ErrorResponse>.SetResponseFor(model, _headers, _writer);
                 _writer.WriteResponseCode(ex.Status);
            }
        }
    }

    public class ErrorResponse
    {
        public string Error { get; set; }
    }
}