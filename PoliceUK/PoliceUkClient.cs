﻿using PoliceUk.Entities.StreetLevel;

namespace PoliceUk
{
    using Entities;
    using Newtonsoft.Json;
    using Entities.Force;
    using Request.Response;
    using Request;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;

    // TODO Make Async
    public class PoliceUkClient : HttpClient, IPoliceUkClient
    {
        private const string ApiPath = "http://data.police.uk/api/";

        public PoliceUkClient(): base(new HttpWebRequestFactory()){}

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

            IHttpWebRequest request = BuildGetWebRequest(url);
            ParsedResponse<Crime[]> response = ProcessJsonRequest<Crime[]>(request);

            return new StreetLevelCrimeResults
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
            IEnumerable<string> polygonSections = polygon.Select(T => T.Latitiude.ToString() + "," + T.Longitude.ToString());
            string postData = "poly=" + String.Join(":", polygonSections.ToArray());
            if (date.HasValue) postData += string.Format("&date={0:yyyy'-'MM}", date.Value);

            var ascii = new ASCIIEncoding();
            byte[] postBytes = ascii.GetBytes(postData);

            IHttpWebRequest request = BuildPostWebRequest(url, postBytes);
            ParsedResponse<Crime[]> response = ProcessJsonRequest<Crime[]>(request);

            return new StreetLevelCrimeResults
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

            IHttpWebRequest request = BuildGetWebRequest(url);
            ParsedResponse<Category[]> response = ProcessJsonRequest<Category[]>(request);

            return response.Data;
        }

        public IEnumerable<ForceSummary> Forces()
        {
            string url = string.Format("{0}forces", ApiPath);

            IHttpWebRequest request = BuildGetWebRequest(url);
            ParsedResponse<ForceSummary[]> response = ProcessJsonRequest<ForceSummary[]>(request);

            return response.Data;
        }

        public ForceDetails Force(string id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            string url = string.Format("{0}forces/{1}", ApiPath, id);

            IHttpWebRequest request = BuildGetWebRequest(url);

            ParsedResponse<ForceDetails> response = ProcessRequest(request, x =>
            {
                // Do not automatically parse response, as if force is not found then non-json response returned
                return (x.StatusCode == HttpStatusCode.OK) ? JsonResponseProcessor<ForceDetails>(x) : null; 
            });

            return response.Data;
        }

        public IEnumerable<Availability> StreetLevelAvailability()
        {
            string url = string.Format("{0}crimes-street-dates", ApiPath);

            IHttpWebRequest request = BuildGetWebRequest(url);
            ParsedResponse<Availability[]> response = ProcessJsonRequest<Availability[]>(request);

            return response.Data;
        }

        /// <summary>
        /// Processes the JSON response from a HTTP request.
        /// This can be used in most cases except for API calls that respond with
        /// a non-json string, such as <see cref="Force(string)"/> which returns 'Not Found'.
        /// </summary>
        private ParsedResponse<T> ProcessJsonRequest<T>(IHttpWebRequest request) where T : class
        {
            return this.ProcessRequest(request, JsonResponseProcessor<T>);
        }

        private static T JsonResponseProcessor<T>(IHttpWebResponse response) where T : class
        {
            T data = null;

            using (var streamReader = new StreamReader(response.GetResponseStream()))
            using (var jsonReader = new JsonTextReader(streamReader))
            {
                var serialiser = new JsonSerializer();
                try
                {
                    data = serialiser.Deserialize<T>(jsonReader);
                }
                catch (JsonException ex)
                {
                    throw new Exceptions.InvalidDataException("Failed to deserialise JSON crime data", ex);
                }
            }

            return data;
        }
    }
}
