using System.Linq;
using FubuMVC.Core;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Http;
using FubuMVC.Core.Runtime;
using FubuRESTInnovation.Handlers.Home;

namespace FubuRESTInnovation.Infrastructure.Output
{
    public class CodecSelector<TOutputModel> : BasicBehavior where TOutputModel : class
    {
        private readonly IFubuRequest _request;
        private readonly IHttpWriter _writer;
        private readonly IRequestHeaders _headers;

        public CodecSelector(IFubuRequest request, IHttpWriter writer, IRequestHeaders headers) : base(PartialBehavior.Ignored)
        {
            _request = request;
            _writer = writer;
            _headers = headers;
        }

        protected override DoNext performInvoke()
        {
            var model = _request.Find<TOutputModel>().First();

            ResponseContentSetter.SetResponseContentFor(model, _headers, _writer);

            return DoNext.Continue;
        }
      
    }
}