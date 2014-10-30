namespace PoliceUk
{
    using Newtonsoft.Json;
    using PoliceUk.Entities;
    using PoliceUk.Exceptions;
    using PoliceUk.Request;
    using PoliceUK.Entities.Force;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;

    // TODO Make Async
    public class PoliceUkClient : IPoliceUkClient
    {
        private class ParsedResponse<T>
        {
            public HttpStatusCode StatusCode;
            public T Data;
        }

        private const string ApiPath = "http://data.police.uk/api/";

        /// <summary>
        /// Gets and sets the Proxy used when requesting data from the API.
        /// </summary>
        public IWebProxy Proxy { get; set; }

        /// <summary>
        /// Gets and sets the factory used to create requests.
        /// </summary>
        public IHttpWebRequestFactory RequestFactory { get; set; }

        public PoliceUkClient()
        {
            this.RequestFactory = new HttpWebRequestFactory();
        }

        public IEnumerable<Crime> StreetLevelCrimes(IGeoposition position, DateTime? date = null)
        {
            string url = string.Format("{0}crimes-street/all-crime?lat={1}&lng={2}", ApiPath,
                position.Latitiude,
                position.Longitude);

            if (date.HasValue) url += string.Format("&date={0:yyyy'-'MM}", date.Value);

            IHttpWebRequest request = BuildWebRequest(this.RequestFactory, url, this.Proxy);
            ParsedResponse<Crime[]> response = ProcessRequest<Crime[]>(request);

            /* If a custom area contains more than 10,000 crimes, the API will return a 503 status code.
             * response.StatusCode == HttpStatusCode.ServiceUnavailable
             */

            return response.Data;
        }

        public IEnumerable<Category> CrimeCategories(DateTime date)
        {
            string url = string.Format("{0}crime-categories?date={1:yyyy'-'MM}", ApiPath,
                date);

            IHttpWebRequest request = BuildWebRequest(this.RequestFactory, url, this.Proxy);
            ParsedResponse<Category[]> response = ProcessRequest<Category[]>(request);

            return response.Data;
        }

        public IEnumerable<ForceSummary> Forces()
        {
            string url = string.Format("{0}forces", ApiPath);

            IHttpWebRequest request = BuildWebRequest(this.RequestFactory, url, this.Proxy);
            ParsedResponse<ForceSummary[]> response = ProcessRequest<ForceSummary[]>(request);

            return response.Data;
        }

        //TODO Handle not found status code
        public ForceDetails Force(string id)
        {
            string url = string.Format("{0}forces/{1}", ApiPath, id);

            IHttpWebRequest request = BuildWebRequest(this.RequestFactory, url, this.Proxy);
            ParsedResponse<ForceDetails> response = ProcessRequest<ForceDetails>(request);

            return response.Data;
        }

        private ParsedResponse<T> ProcessRequest<T>(IHttpWebRequest request) where T : class
        {
            var response = new ParsedResponse<T>();
            try
            {
                using (IHttpWebResponse httpResponse = request.GetResponse())
                {
                    response.StatusCode = httpResponse.StatusCode;
                    response.Data = ProcessWebResponse<T>(httpResponse);
                }
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError && ex.Response != null)
                {
                    var httpResponse = (HttpWebResponse) ex.Response;
                    response.StatusCode = httpResponse.StatusCode;
                }
                else
                {
                    string message = "Failed to request crime data from " + request.RequestUri;
                    throw new DataRequestException(message, ex);
                }
            }

            return response;
        }

        private T ProcessWebResponse<T>(IHttpWebResponse response) where T : class
        {
            T data = null;

            using (var streamReader = new StreamReader(response.GetResponseStream()))
            using (JsonTextReader jsonReader = new JsonTextReader(streamReader))
            {
                Newtonsoft.Json.JsonSerializer serialiser = new JsonSerializer();
                try
                {
                    data = serialiser.Deserialize<T>(jsonReader);
                }
                catch (JsonReaderException ex)
                {
                    // Error parsing JSON data
                    throw new PoliceUk.Exceptions.InvalidDataException("Failed to deserialise crime data", ex);
                }
            }

            return data;
        }

        private static IHttpWebRequest BuildWebRequest(IHttpWebRequestFactory factory, string uri, IWebProxy proxy)
        {
            IHttpWebRequest request = factory.Create(uri);
            if (proxy != null) request.Proxy = proxy;
            request.Method = "GET";

            return request;
        }
    }
}
