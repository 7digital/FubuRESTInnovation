using FubuMVC.Core.Registration.Nodes;
using FubuRESTInnovation.Configuration;

namespace FubuRESTInnovation.Infrastructure.Output
{
    public class CodecSelectorNode<TOutputModel> : OutputNode<CodecSelector<TOutputModel>> where TOutputModel : class
    {
    }
}