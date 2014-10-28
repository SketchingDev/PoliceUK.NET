namespace PoliceUk.Request
{
    using System;
    using System.Net;

    public class HttpWebRequestWrapper : IHttpWebRequest
    {
        public Uri RequestUri
        {
            get { return this.webRequest.RequestUri; }
        }

        public string Method
        {
            get { return this.webRequest.Method; }
            set { this.webRequest.Method = value; }
        }

        public IWebProxy Proxy
        {
            get { return this.webRequest.Proxy; }
            set { this.webRequest.Proxy = value; }
        }

        private readonly HttpWebRequest webRequest;

        public HttpWebRequestWrapper(HttpWebRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            this.webRequest = request;
        }

        public IHttpWebResponse GetResponse()
        {
            var response = (HttpWebResponse)this.webRequest.GetResponse();

            return new HttpWebResponseWrapper(response);
        }
    }
}
