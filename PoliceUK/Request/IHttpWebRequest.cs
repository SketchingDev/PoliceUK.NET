namespace PoliceUk.Request
{
    using PoliceUK.Request.Response;
    using System;
    using System.IO;
    using System.Net;

    public interface IHttpWebRequest
    {
        Uri RequestUri { get; }

        string Method { get; set; }

        string ContentType { get; set; }

        long ContentLength { get; set; }

        IWebProxy Proxy { get; set; }

        Stream GetRequestStream();

        IHttpWebResponse GetResponse();
    }
}
