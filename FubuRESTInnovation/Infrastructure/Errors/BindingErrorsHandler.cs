using System;
using System.Linq;
using System.Net;
using System.Web;
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
        private readonly IRequestHeaders _headers;
        private readonly HttpContextBase _context; // Low level access to the current context handled by IoC

        public BindingErrorsHandler(IFubuRequest request, IHttpWriter writer, IRequestHeaders headers, HttpContextBase context)
        {
            _request = request;
            _writer = writer;
            _headers = headers;
            _context = context;
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
                _context.Response.Clear();
                ResponseContentSetter.SetErrorResponse(enumError.Value.RawValue + " is not a valid " + enumError.Property.Name.ToLower(), _writer, _headers);
                _writer.WriteResponseCode(HttpStatusCode.BadRequest);
            }

        }

        public void InvokePartial()
        {
            throw new NotImplementedException();
        }
    }
}