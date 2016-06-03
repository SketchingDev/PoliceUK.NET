namespace PoliceUk
{
    using Entities;
    using Entities.Force;
    using Entities.StreetLevel;
    using Newtonsoft.Json;
    using PoliceUk.Entities.Neighbourhood;
    using Request;
    using Request.Response;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;

    // TODO Make Async
    public class PoliceUkClient : HttpClient, IPoliceUkClient
    {
        private const string ApiPath = "https://data.police.uk/api/";

        public PoliceUkClient(): base(new HttpWebRequestFactory()){}

        public StreetLevelCrimeResults StreetLevelCrimes(IGeoposition position, DateTime? date = null)
        {
            if (position == null)
            {
                throw new ArgumentNullException("position");
            }

            string url = string.Format("{0}crimes-street/all-crime?lat={1}&lng={2}", ApiPath,
                position.Latitude,
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
            IEnumerable<string> polygonSections = polygon.Select(T => T.Latitude.ToString() + "," + T.Longitude.ToString());
            string postData = "poly=" + string.Join(":", polygonSections.ToArray());
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

        public IEnumerable<DateTime> StreetLevelAvailability()
        {
            string url = string.Format("{0}crimes-street-dates", ApiPath);

            IHttpWebRequest request = BuildGetWebRequest(url);
            ParsedResponse<Availability[]> response = ProcessJsonRequest<Availability[]>(request);

            Availability[] availabilities = response.Data;
            return availabilities.Select(x => x.Date).ToArray();
        }

        public IEnumerable<NeighbourhoodSummary> Neighbourhoods(string forceId)
        {
            if (forceId == null)
            {
                throw new ArgumentNullException("forceId");
            }

            string url = string.Format("{0}{1}/neighbourhoods", ApiPath, forceId);

            IHttpWebRequest request = BuildGetWebRequest(url);
            ParsedResponse<NeighbourhoodSummary[]> response = ProcessJsonRequest<NeighbourhoodSummary[]>(request);

            return response.Data;
        }

        public NeighbourhoodDetails Neighbourhood(string forceId, string id)
        {
            if (forceId == null)
            {
                throw new ArgumentNullException("forceId");
            }

            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            string url = string.Format("{0}{1}/{2}", ApiPath, forceId, id);

            IHttpWebRequest request = BuildGetWebRequest(url);

            ParsedResponse<NeighbourhoodDetails> response = ProcessRequest(request, x =>
            {
                // Do not automatically parse response, as if neighbourhood is not found then non-json response returned
                return (x.StatusCode == HttpStatusCode.OK) ? JsonResponseProcessor<NeighbourhoodDetails>(x) : null;
            });

            return response.Data;
        }

        public IEnumerable<Geoposition> NeighbourhoodBoundary(string forceId, string id)
        {
            if (forceId == null)
            {
                throw new ArgumentNullException("forceId");
            }

            if (id == null)
            {
                throw new ArgumentNullException("id");
            }
        
            string url = string.Format("{0}{1}/{2}/boundary", ApiPath, forceId, id);

            IHttpWebRequest request = BuildGetWebRequest(url);

            ParsedResponse<Geoposition[]> response = ProcessRequest(request, x =>
            {
                // Do not automatically parse response, as if neighbourhood is not found then non-json response returned
                return (x.StatusCode == HttpStatusCode.OK) ? JsonResponseProcessor<Geoposition[]>(x) : null;
            });

            return response.Data;
        }

        public NeighbourhoodForce LocateNeighbourhood(IGeoposition position) // TODO Rename to just 'neighbourhood'
        {
            if (position == null)
            {
                throw new ArgumentNullException("position");
            }

            string url = string.Format("{0}locate-neighbourhood?q={1},{2}", ApiPath,
                position.Latitude, 
                position.Longitude);

            IHttpWebRequest request = BuildGetWebRequest(url);
            ParsedResponse<NeighbourhoodForce> response = ProcessRequest(request, x =>
            {
                // Do not automatically parse response, as if force is not found then non-json response returned
                return (x.StatusCode == HttpStatusCode.OK) ? JsonResponseProcessor<NeighbourhoodForce>(x) : null;
            });

            return response.Data;
        }

        public IEnumerable<Crime> Crimes(string forceId, string category, DateTime? date = null)
        {
            if (category == null || forceId == null)
            {
                throw new ArgumentNullException();
            }

            string url = string.Format("{0}crimes-no-location?category={1}&force={2}", ApiPath,
                category,
                forceId);

            if (date.HasValue) url += string.Format("&date={0:yyyy'-'MM}", date.Value);

            IHttpWebRequest request = BuildGetWebRequest(url);
            ParsedResponse<Crime[]> response = ProcessJsonRequest<Crime[]>(request);

            return response.Data;
        }

        public IEnumerable<Crime> CrimesAtLocation(string locationId, DateTime date)
        {
            if (locationId == null)
            {
                throw new ArgumentNullException("locationId");
            }

            string url = string.Format("{0}crimes-at-location?location_id={1}&date={2:yyyy'-'MM}", ApiPath,
                locationId,
                date);

            IHttpWebRequest request = BuildGetWebRequest(url);
            ParsedResponse<Crime[]> response = ProcessJsonRequest<Crime[]>(request);

            return response.Data;
        }

        public IEnumerable<Crime> CrimesAtLocation(IGeoposition position, DateTime date)
        {
            if (position == null)
            {
                throw new ArgumentNullException("position");
            }

            string url = string.Format("{0}crimes-at-location?lat={1}&lng={2}&date={3:yyyy'-'MM}", ApiPath,
                position.Latitude,
                position.Longitude,
                date);

            IHttpWebRequest request = BuildGetWebRequest(url);
            ParsedResponse<Crime[]> response = ProcessJsonRequest<Crime[]>(request);

            return response.Data;
        }

        public DateTime LastUpdated()
        {
            string url = string.Format("{0}crime-last-updated", ApiPath);

            IHttpWebRequest request = BuildGetWebRequest(url);
            ParsedResponse<LastUpdated> response = ProcessJsonRequest<LastUpdated>(request);

            LastUpdated lastUpdated = response.Data;
            return lastUpdated.Date;
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


        public IEnumerable<NeighbourhoodTeamMember> NeighbourhoodTeam(string forceId, string neighbourhoodId)
        {
            if (forceId == null)
            {
                throw new ArgumentNullException("forceId");
            }

            if (neighbourhoodId == null)
            {
                throw new ArgumentNullException("neighbourhoodId");
            }

            string url = string.Format("{0}{1}/{2}/people", ApiPath, forceId, neighbourhoodId);

            IHttpWebRequest request = BuildGetWebRequest(url);

            ParsedResponse<NeighbourhoodTeamMember[]> response = ProcessRequest(request, x =>
            {
                // Do not automatically parse response, as if neighbourhood is not found then non-json response returned
                return (x.StatusCode == HttpStatusCode.OK) ? JsonResponseProcessor<NeighbourhoodTeamMember[]>(x) : null;
            });

            return response.Data;
        }

        public IEnumerable<NeighbourhoodEvent> NeighbourhoodEvents(string forceId, string neighbourhoodId)
        {
            if (forceId == null)
            {
                throw new ArgumentNullException("forceId");
            }

            if (neighbourhoodId == null)
            {
                throw new ArgumentNullException("neighbourhoodId");
            }

            string url = string.Format("{0}{1}/{2}/events", ApiPath, forceId, neighbourhoodId);

            IHttpWebRequest request = BuildGetWebRequest(url);

            ParsedResponse<NeighbourhoodEvent[]> response = ProcessRequest(request, x =>
            {
                // Do not automatically parse response, as if neighbourhood is not found then non-json response returned
                return (x.StatusCode == HttpStatusCode.OK) ? JsonResponseProcessor<NeighbourhoodEvent[]>(x) : null;
            });

            return response.Data;
        }
    }
}
