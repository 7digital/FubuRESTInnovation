// Type: FubuMVC.Core.Http.IHttpWriter
// Assembly: FubuMVC.Core, Version=0.9.5.0, Culture=neutral, PublicKeyToken=null
// Assembly location: C:\dev\git projectsx\FubuRESTInnovation\FubuRESTInnovation\bin\FubuMVC.Core.dll

using System;
using System.IO;
using System.Net;
using System.Web;

namespace FubuMVC.Core.Http
{
  public interface IHttpWriter
  {
    void AppendHeader(string key, string value);

    void WriteFile(string file);

    void WriteContentType(string contentType);

    void Write(string content);

    void Redirect(string url);

    void WriteResponseCode(HttpStatusCode status, string description = null);

    void AppendCookie(HttpCookie cookie);

    void Write(Action<Stream> output);
  }
}
