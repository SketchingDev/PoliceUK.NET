namespace PoliceUk
{
    using Newtonsoft.Json;
    using PoliceUk.Entities;
    using PoliceUk.Exceptions;
    using PoliceUk.Request;
    using PoliceUK.Entities.Force;
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Text;
    using PoliceUK.Entities;

    // TODO Make Async
    public class PoliceUkClient : IPoliceUkClient
    {
        private class ParsedResponse<T>
        {
            public HttpStatusCode StatusCode;
            public T Data;
        }

        private const string UserAgent = "PoliceUkClient";

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

        public StreetLevelCrimeResults StreetLevelCrimes(IGeoposition position, DateTime? date = null)
        {
            if (position == null)
            {
                throw new ArgumentNullException("position");
            }

            string url = string.Format("{0}crimes-street/all-crime?lat={1}&lng={2}", ApiPath,
                position.Latitiude,
                position.Longitude);

            if (date.HasValue) url += string.Format("&date={0:yyyy'-'MM}", date.Value);

            IHttpWebRequest request = BuildGetWebRequest(this.RequestFactory, url, this.Proxy);
            ParsedResponse<Crime[]> response = ProcessRequest<Crime[]>(request);

            return new StreetLevelCrimeResults()
            {
                // API returns status code 503 if area contains more than 10,000 crimes, or error has actually occurred.
                TooManyCrimesOrError = (response.StatusCode == HttpStatusCode.ServiceUnavailable),
                Crimes = response.Data
            };
        }

        public StreetLevelCrimeResults StreetLevelCrimes(IEnumerable<IGeoposition> polygon, DateTime? date = null)
        {
            if (polygon == null)
            {
                throw new ArgumentNullException("polygon");
            }

            string url = string.Format("{0}crimes-street/all-crime", ApiPath);

            // Post data
            string postData = "poly=" + String.Join(":", polygon.Select(T => T.Latitiude.ToString() + "," + T.Longitude.ToString()));
            if (date.HasValue) postData += string.Format("&date={0:yyyy'-'MM}", date.Value);

            ASCIIEncoding ascii = new ASCIIEncoding();
            byte[] postBytes = ascii.GetBytes(postData.ToString());

            IHttpWebRequest request = BuildPostWebRequest(this.RequestFactory, url, this.Proxy, postBytes);
            ParsedResponse<Crime[]> response = ProcessRequest<Crime[]>(request);

            return new StreetLevelCrimeResults()
            {
                // API returns status code 503 if area contains more than 10,000 crimes, or error has actually occurred.
                TooManyCrimesOrError = (response.StatusCode == HttpStatusCode.ServiceUnavailable),
                Crimes = response.Data
            };
        }

        public IEnumerable<Category> CrimeCategories(DateTime date)
        {
            string url = string.Format("{0}crime-categories?date={1:yyyy'-'MM}", ApiPath,
                date);

            IHttpWebRequest request = BuildGetWebRequest(this.RequestFactory, url, this.Proxy);
            ParsedResponse<Category[]> response = ProcessRequest<Category[]>(request);

            return response.Data;
        }

        public IEnumerable<ForceSummary> Forces()
        {
            string url = string.Format("{0}forces", ApiPath);

            IHttpWebRequest request = BuildGetWebRequest(this.RequestFactory, url, this.Proxy);
            ParsedResponse<ForceSummary[]> response = ProcessRequest<ForceSummary[]>(request);

            return response.Data;
        }

        public ForceDetails Force(string id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            string url = string.Format("{0}forces/{1}", ApiPath, id);

            IHttpWebRequest request = BuildGetWebRequest(this.RequestFactory, url, this.Proxy);

            ParsedResponse<ForceDetails> response = ProcessRequest<ForceDetails>(request, (x) =>
            {
                // Do not automatically parse response, as if force is not found then non-json response returned
                return (x.StatusCode == HttpStatusCode.OK) ? ProcessJsonResponse<ForceDetails>(x) : null; 
            });

            return response.Data;
        }

        /// <summary>
        /// Processes the JSON response from a HTTP request.
        /// This can be used in most cases except for API calls that response with
        /// a non-json string, such as <see cref="Force(string)"/> which returns 'Not Found'.
        /// </summary>
        private ParsedResponse<T> ProcessRequest<T>(IHttpWebRequest request) where T : class
        {
            return this.ProcessRequest<T>(request, ProcessJsonResponse<T>);
        }

        /// <param name="responseProcessor">Delegate for defining the processor of the response.</param>
        private ParsedResponse<T> ProcessRequest<T>(IHttpWebRequest request, Func<IHttpWebResponse, T> responseProcessor) where T : class
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
                    string message = "Failed to request crime data from " + request.RequestUri;
                    throw new DataRequestException(message, ex);
                }
            }

            return response;
        }

        private static T ProcessJsonResponse<T>(IHttpWebResponse response) where T : class
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
                catch (JsonException ex)
                {
                    throw new PoliceUk.Exceptions.InvalidDataException("Failed to deserialise crime data", ex);
                }
            }

            return data;
        }

        private static IHttpWebRequest BuildGetWebRequest(IHttpWebRequestFactory factory, string uri, IWebProxy proxy)
        {
            IHttpWebRequest request = factory.Create(uri);
            if (proxy != null) request.Proxy = proxy;
            request.Method = "GET";

            return request;
        }

        private static IHttpWebRequest BuildPostWebRequest(IHttpWebRequestFactory factory, string uri, IWebProxy proxy, byte[] postBytes)
        {
            IHttpWebRequest request = factory.Create(uri);
            if (proxy != null) request.Proxy = proxy;
            request.Method = "POST";

            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = postBytes.Length;

            using(Stream postStream = request.GetRequestStream())
            {
                postStream.Write(postBytes, 0, postBytes.Length);
                postStream.Flush();
            }

            return request;
        }
    }
}
