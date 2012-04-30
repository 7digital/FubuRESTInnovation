// Type: FubuMVC.Core.Http.RequestHeaders
// Assembly: FubuMVC.Core, Version=0.9.5.0, Culture=neutral, PublicKeyToken=null
// Assembly location: C:\dev\git projectsx\FubuRESTInnovation\packages\FubuMVC.References.0.9.5.817\lib\net40\FubuMVC.Core.dll

using FubuCore.Binding;
using FubuCore.Binding.Values;
using FubuCore.Conversion;
using System;

namespace FubuMVC.Core.Http
{
  public class RequestHeaders : IRequestHeaders
  {
    private readonly IObjectConverter _converter;
    private readonly IObjectResolver _resolver;
    private readonly IValueSource _values;

    public RequestHeaders(IObjectConverter converter, IObjectResolver resolver, IRequestData requestData)
    {
      this._converter = converter;
      this._resolver = resolver;
      this._values = RequestDataExtensions.ValuesFor(requestData, RequestDataSource.Header);
    }

    public void Value<T>(string header, Action<T> callback)
    {
      this._values.Value(header, (Action<BindingValue>) (value => callback(this._converter.FromString<T>(value.RawValue.ToString()))));
    }

    public T BindToHeaders<T>()
    {
      BindResult bindResult = ObjectResolverExtensions.BindModel<T>(this._resolver, this._values);
      bindResult.AssertNoProblems(typeof (T));
      return (T) bindResult.Value;
    }

    public bool HasHeader(string header)
    {
      return this._values.Has(header);
    }
  }
}
