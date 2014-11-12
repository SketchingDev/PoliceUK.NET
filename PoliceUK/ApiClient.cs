namespace PoliceUk
{
    using Exceptions;
    using Request;
    using System;
    using System.IO;
    using System.Net;
    using Request.Response;

    public class HttpClient
    {
        protected class ParsedResponse<T>
        {
            public HttpStatusCode StatusCode;
            public T Data;
        }

        /// <summary>
        /// Gets and sets the factory used to create requests.
        /// </summary>
        public IHttpWebRequestFactory RequestFactory { get; set; }

        /// <summary>
        /// Gets and sets the Proxy used when requesting data from the API.
        /// </summary>
        public IWebProxy Proxy { get; set; }

        protected HttpClient(IHttpWebRequestFactory requestFactory)
        {
            this.RequestFactory = requestFactory;
        }

        /// <param name="responseProcessor">Delegate for defining the processor of the response.</param>
        protected ParsedResponse<T> ProcessRequest<T>(IHttpWebRequest request, Func<IHttpWebResponse, T> responseProcessor) where T : class
        {
            var response = new ParsedResponse<T>();
            try
            {
                using (IHttpWebResponse httpResponse = request.GetResponse())
                {
                    response.StatusCode = httpResponse.StatusCode;
                    response.Data = responseProcessor(httpResponse);
                }
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError && ex.Response != null)
                {
                    var httpResponse = (HttpWebResponse)ex.Response;
                    response.StatusCode = httpResponse.StatusCode;
                }
                else
                {
                    string message = "Failed to request from from " + request.RequestUri;
                    throw new DataRequestException(message, ex);
                }
            }

            return response;
        }

        protected IHttpWebRequest BuildGetWebRequest(string uri)
        {
            IHttpWebRequest request = this.RequestFactory.Create(uri);
            if (this.Proxy != null) request.Proxy = this.Proxy;
            request.Method = "GET";

            return request;
        }

        protected IHttpWebRequest BuildPostWebRequest(string uri, byte[] postBytes)
        {
            IHttpWebRequest request = this.RequestFactory.Create(uri);
            if (this.Proxy != null) request.Proxy = this.Proxy;
            request.Method = "POST";

            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = postBytes.Length;

            using (Stream postStream = request.GetRequestStream())
            {
                postStream.Write(postBytes, 0, postBytes.Length);
                postStream.Flush();
            }

            return request;
        }
    }
}
