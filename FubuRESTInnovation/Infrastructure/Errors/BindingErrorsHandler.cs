using System;
using System.Linq;
using System.Net;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Http;
using FubuMVC.Core.Runtime;
using FubuRESTInnovation.Infrastructure.Output;

namespace FubuRESTInnovation.Infrastructure.Errors
{
    public class BindingErrorsHandler<TInput> : IActionBehavior where TInput : class
    {
        private readonly IFubuRequest _request;
        private readonly IHttpWriter _writer;

        public BindingErrorsHandler(IFubuRequest request, IHttpWriter writer)
        {
            _request = request;
            _writer = writer;
        }

        public IActionBehavior InsideBehavior { get; set; }

        public void Invoke()
        {
            InsideBehavior.Invoke();

            var bindingErrors = _request.ProblemsFor<TInput>().ToList();

            if (!bindingErrors.Any()) return;

            var enumError = bindingErrors.FirstOrDefault(b => b.Property.PropertyType.IsEnum);
            if (enumError != null)
            {
                _writer.WriteResponseCode(HttpStatusCode.BadRequest);
            }

        }

        public void InvokePartial()
        {
            throw new NotImplementedException();
        }
    }
}