namespace PoliceUk
{
    using Newtonsoft.Json;
    using PoliceUk.Entities;
    using PoliceUk.Exceptions;
    using PoliceUk.Request;
    using PoliceUK.Entities;
    using PoliceUK.Entities.Force;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;

    // TODO Handle not found
    // TODO Make Async
    public class PoliceUkClient : IPoliceUkClient
    {
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

            return ProcessRequest<Crime[]>(request);
        }

        public IEnumerable<Category> CrimeCategories(DateTime date)
        {
            string url = string.Format("{0}crime-categories?date={1:yyyy'-'MM}", ApiPath,
                date);

            IHttpWebRequest request = BuildWebRequest(this.RequestFactory, url, this.Proxy);

            return ProcessRequest<Category[]>(request);
        }

        public IEnumerable<ForceSummary> Forces()
        {
            string url = string.Format("{0}forces", ApiPath);

            IHttpWebRequest request = BuildWebRequest(this.RequestFactory, url, this.Proxy);

            return ProcessRequest<ForceSummary[]>(request);
        }

        //TODO Handle not found status code
        public IEnumerable<ForceSummary> Force(string id)
        {
            string url = string.Format("{0}forces/{1}", ApiPath, id);

            IHttpWebRequest request = BuildWebRequest(this.RequestFactory, url, this.Proxy);

            return ProcessRequest<ForceSummary[]>(request);
        }

        private T ProcessRequest<T>(IHttpWebRequest request) where T : class
        {
            T data = null;
            try
            {
                using (IHttpWebResponse response = request.GetResponse())
                {
                    data = ProcessWebResponse<T>(response);
                }
            }
            catch (Exception e)
            {
                string message = "Failed to request crime data from " + request.RequestUri;
                throw new DataRequestException(message, e);
            }

            return data;
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
                catch (Exception e)
                {
                    // Error parsing JSON data
                    throw new PoliceUk.Exceptions.InvalidDataException("Failed to deserialise crime data", e);
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
