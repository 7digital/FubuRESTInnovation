﻿using System;
using System.Net;
using FubuMVC.Core;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Continuations;
using FubuMVC.Core.Http;
using FubuRESTInnovation.Infrastructure.Errors;
using FubuRESTInnovation.Infrastructure.Output;

namespace FubuRESTInnovation.Infrastructure.Behaviours
{
    public class ApiErrorsHandler : IActionBehavior
    {
        private readonly IHttpWriter _writer;
        private readonly IRequestHeaders _headers;

        public ApiErrorsHandler(IHttpWriter writer, IRequestHeaders headers)
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
                ResponseContentSetter.SetErrorResponse(ex.Message, _writer, _headers);
                // _TODO _writer.Write(ResponseConentSetter.GetResponseContent) #timeconstraint
                _writer.WriteResponseCode(ex.Status);
            }
        }
    }

    public class ErrorResponse
    {
        public string Error { get; set; }
    }
}