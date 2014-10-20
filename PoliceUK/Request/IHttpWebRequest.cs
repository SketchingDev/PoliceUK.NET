namespace PoliceUk.Request
{
    using System;
    using System.Net;

    public interface IHttpWebRequest
    {
        Uri RequestUri { get; }

        string Method { get; set; }

        IWebProxy Proxy { get; set; }

        IWebResponse GetResponse();
    }
}
