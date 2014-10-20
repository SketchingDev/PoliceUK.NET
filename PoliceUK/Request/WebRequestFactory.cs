namespace PoliceUk.Request
{
    using System.Net;

    public class HttpWebRequestFactory : IHttpWebRequestFactory
    {
        public IHttpWebRequest Create(string uri)
        {
            return new HttpWebRequestWrapper((HttpWebRequest)WebRequest.Create(uri));
        }
    }
}
