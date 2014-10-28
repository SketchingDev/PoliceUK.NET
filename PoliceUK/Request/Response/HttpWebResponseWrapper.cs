namespace PoliceUk.Request
{
    using System;
    using System.IO;
    using System.Net;

    public class HttpWebResponseWrapper : IHttpWebResponse
    {
        private HttpWebResponse response;

        public HttpStatusCode StatusCode
        {
            get
            {
                return this.response.StatusCode;
            }
        }

        public HttpWebResponseWrapper(HttpWebResponse response)
        {
            if (response == null)
            {
                throw new ArgumentNullException("response");
            }

            this.response = response;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.response != null)
                {
                    ((IDisposable)this.response).Dispose();
                    this.response = null;
                }
            }
        }

        public Stream GetResponseStream()
        {
            return this.response.GetResponseStream();
        }
    }
}
