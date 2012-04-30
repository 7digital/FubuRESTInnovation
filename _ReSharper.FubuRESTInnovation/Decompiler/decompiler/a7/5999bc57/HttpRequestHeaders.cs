// Type: FubuMVC.Core.Http.HttpRequestHeaders
// Assembly: FubuMVC.Core, Version=0.9.5.0, Culture=neutral, PublicKeyToken=null
// Assembly location: C:\dev\git projectsx\FubuRESTInnovation\packages\FubuMVC.References.0.9.5.817\lib\net40\FubuMVC.Core.dll

using FubuCore.Util;
using System;
using System.Net;
using System.Reflection;

namespace FubuMVC.Core.Http
{
  public class HttpRequestHeaders : HttpGeneralHeaders
  {
    public static readonly string Accept = "Accept";
    public static readonly string AcceptCharset = "Accept-Charset";
    public static readonly string AcceptEncoding = "Accept-Encoding";
    public static readonly string AcceptLanguage = "Accept-Language";
    public static readonly string Authorization = "Authorization";
    public static readonly string Cookie = "Cookie";
    public static readonly string ContentLength = "Content-Length";
    public static readonly string ContentMd5 = "Content-MD5";
    public static readonly string ContentType = "Content-Type";
    public static readonly string Expect = "Expect";
    public static readonly string From = "From";
    public static readonly string Host = "Host";
    public static readonly string IfMatch = "If-Match";
    public static readonly string IfModifiedSince = "If-Modified-Since";
    public static readonly string IfNoneMatch = "If-None-Match";
    public static readonly string IfRange = "If-Range";
    public static readonly string IfUnmodifiedSince = "If-Unmodified-Since";
    public static readonly string MaxForwards = "Max-Forwards";
    public static readonly string ProxyAuthorization = "Proxy-Authorization";
    public static readonly string Range = "Range";
    public static readonly string Referer = "Referer";
    public static readonly string Te = "TE";
    public static readonly string UserAgent = "User-Agent";
    public static readonly string KeepAlive = "keep-alive";
    public static readonly string Translate = "Translate";
    private static readonly Cache<HttpRequestHeader, string> _headerNames = new Cache<HttpRequestHeader, string>();
    private static readonly Cache<string, string> _propertyAliases = new Cache<string, string>((Func<string, string>) (name =>
    {
      local_0 = typeof (HttpRequestHeaders).GetField(name, BindingFlags.Static | BindingFlags.Public);
      if (!(local_0 == (FieldInfo) null))
        return (string) local_0.GetValue((object) null);
      else
        return name;
    }));

    static HttpRequestHeaders()
    {
      HttpRequestHeaders._headerNames[HttpRequestHeader.CacheControl] = HttpGeneralHeaders.CacheControl;
      HttpRequestHeaders._headerNames[HttpRequestHeader.Connection] = HttpGeneralHeaders.Connection;
      HttpRequestHeaders._headerNames[HttpRequestHeader.Date] = HttpGeneralHeaders.Date;
      HttpRequestHeaders._headerNames[HttpRequestHeader.KeepAlive] = HttpRequestHeaders.KeepAlive;
      HttpRequestHeaders._headerNames[HttpRequestHeader.Pragma] = HttpGeneralHeaders.Pragma;
      HttpRequestHeaders._headerNames[HttpRequestHeader.Trailer] = HttpGeneralHeaders.Trailer;
      HttpRequestHeaders._headerNames[HttpRequestHeader.TransferEncoding] = HttpGeneralHeaders.TransferEncoding;
      HttpRequestHeaders._headerNames[HttpRequestHeader.Upgrade] = HttpGeneralHeaders.Upgrade;
      HttpRequestHeaders._headerNames[HttpRequestHeader.Via] = HttpGeneralHeaders.Via;
      HttpRequestHeaders._headerNames[HttpRequestHeader.Warning] = HttpGeneralHeaders.Warning;
      HttpRequestHeaders._headerNames[HttpRequestHeader.Allow] = HttpGeneralHeaders.Allow;
      HttpRequestHeaders._headerNames[HttpRequestHeader.ContentLength] = HttpRequestHeaders.ContentLength;
      HttpRequestHeaders._headerNames[HttpRequestHeader.ContentType] = HttpRequestHeaders.ContentType;
      HttpRequestHeaders._headerNames[HttpRequestHeader.ContentEncoding] = HttpGeneralHeaders.ContentEncoding;
      HttpRequestHeaders._headerNames[HttpRequestHeader.ContentLanguage] = HttpGeneralHeaders.ContentLanguage;
      HttpRequestHeaders._headerNames[HttpRequestHeader.ContentLocation] = HttpGeneralHeaders.ContentLocation;
      HttpRequestHeaders._headerNames[HttpRequestHeader.ContentMd5] = HttpRequestHeaders.ContentMd5;
      HttpRequestHeaders._headerNames[HttpRequestHeader.ContentRange] = HttpGeneralHeaders.ContentRange;
      HttpRequestHeaders._headerNames[HttpRequestHeader.Expires] = HttpGeneralHeaders.Expires;
      HttpRequestHeaders._headerNames[HttpRequestHeader.LastModified] = HttpGeneralHeaders.LastModified;
      HttpRequestHeaders._headerNames[HttpRequestHeader.Accept] = HttpRequestHeaders.Accept;
      HttpRequestHeaders._headerNames[HttpRequestHeader.AcceptCharset] = HttpRequestHeaders.AcceptCharset;
      HttpRequestHeaders._headerNames[HttpRequestHeader.AcceptEncoding] = HttpRequestHeaders.AcceptEncoding;
      HttpRequestHeaders._headerNames[HttpRequestHeader.AcceptLanguage] = HttpRequestHeaders.AcceptLanguage;
      HttpRequestHeaders._headerNames[HttpRequestHeader.Authorization] = HttpRequestHeaders.Authorization;
      HttpRequestHeaders._headerNames[HttpRequestHeader.Cookie] = HttpRequestHeaders.Cookie;
      HttpRequestHeaders._headerNames[HttpRequestHeader.Expect] = HttpRequestHeaders.Expect;
      HttpRequestHeaders._headerNames[HttpRequestHeader.From] = HttpRequestHeaders.From;
      HttpRequestHeaders._headerNames[HttpRequestHeader.Host] = HttpRequestHeaders.Host;
      HttpRequestHeaders._headerNames[HttpRequestHeader.IfMatch] = HttpRequestHeaders.IfMatch;
      HttpRequestHeaders._headerNames[HttpRequestHeader.IfModifiedSince] = HttpRequestHeaders.IfModifiedSince;
      HttpRequestHeaders._headerNames[HttpRequestHeader.IfNoneMatch] = HttpRequestHeaders.IfNoneMatch;
      HttpRequestHeaders._headerNames[HttpRequestHeader.IfRange] = HttpRequestHeaders.IfRange;
      HttpRequestHeaders._headerNames[HttpRequestHeader.IfUnmodifiedSince] = HttpRequestHeaders.IfUnmodifiedSince;
      HttpRequestHeaders._headerNames[HttpRequestHeader.MaxForwards] = HttpRequestHeaders.MaxForwards;
      HttpRequestHeaders._headerNames[HttpRequestHeader.ProxyAuthorization] = HttpRequestHeaders.ProxyAuthorization;
      HttpRequestHeaders._headerNames[HttpRequestHeader.Referer] = HttpRequestHeaders.Referer;
      HttpRequestHeaders._headerNames[HttpRequestHeader.Range] = HttpRequestHeaders.Range;
      HttpRequestHeaders._headerNames[HttpRequestHeader.Te] = HttpRequestHeaders.Te;
      HttpRequestHeaders._headerNames[HttpRequestHeader.Translate] = HttpRequestHeaders.Translate;
      HttpRequestHeaders._headerNames[HttpRequestHeader.UserAgent] = HttpRequestHeaders.UserAgent;
    }

    protected HttpRequestHeaders()
    {
    }

    public static string HeaderNameFor(HttpRequestHeader header)
    {
      return HttpRequestHeaders._headerNames[header];
    }

    public static string HeaderDictionaryNameForProperty(string propertyName)
    {
      return HttpRequestHeaders._propertyAliases[propertyName];
    }
  }
}
