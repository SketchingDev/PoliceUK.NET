namespace PoliceUk.Request
{
    using System;
    using System.IO;
    using System.Net;

    public class WebResponseWrapper : IWebResponse
    {
        private WebResponse response;

        public WebResponseWrapper(WebResponse response)
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
