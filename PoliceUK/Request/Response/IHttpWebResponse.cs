namespace PoliceUk.Request.Response
{
    using System;
    using System.IO;
    using System.Net;

    public interface IHttpWebResponse : IDisposable
    {
        HttpStatusCode StatusCode {get;}
        Stream GetResponseStream();
    }
}
