namespace PoliceUk
{
    using Entities;
    using Entities.Force;
    using Entities.StreetLevel;
    using Entities.Neighbourhood;
    using System;
    using System.Collections.Generic;

    public interface IPoliceUkClient
    {
        /// <summary>
        /// Crimes at street-level; within a 1 mile radius of a single point.
        /// 
        /// IMPORTANT NOTE: The street-level crimes returned in the API are only an approximation of where 
        /// the actual crimes occurred, they are NOT the exact locations.
        /// </summary>
        /// <param name="position">Latitude and Longitude  of the requested crime area</param>
        /// <param name="date">Optional. (YYYY-MM) Limit results to a specific month.
        /// The latest month will be shown by default.
        /// </param>
        StreetLevelCrimeResults StreetLevelCrimes(IGeoposition position, DateTime? date = null);

        /// <summary>
        /// Crimes at street-level; within a custom area.
        /// 
        /// IMPORTANT NOTE: The street-level crimes returned in the API are only an approximation of where 
        /// the actual crimes occurred, they are NOT the exact locations.
        /// </summary>
        /// <param name="polygon">The lat/lng pairs which define the boundary of the custom area</param>
        /// <param name="date">Optional. (YYYY-MM) Limit results to a specific month.
        /// The latest month will be shown by default.
        /// </param>
        StreetLevelCrimeResults StreetLevelCrimes(IEnumerable<IGeoposition> polygon, DateTime? date = null);

        /// <summary>
        /// Returns a list of valid categories for a given data set date.
        /// </summary>
        IEnumerable<Category> CrimeCategories(DateTime date);

        /// <summary>
        /// A list of all the police forces available via the API.
        /// Unique force identifiers obtained here are used in requests for force-specific data via other methods. 
        /// </summary>
        IEnumerable<ForceSummary> Forces();

        /// <summary>
        /// Get details about a specific force.
        /// </summary>
        /// <param name="id">Unique force identifier. These are available from <see cref="Forces()"/>.</param>
        ForceDetails Force(string id);

        /// <summary>
        /// Return a list of available data sets.
        /// </summary>
        IEnumerable<DateTime> StreetLevelAvailability();

        /// <summary>
        /// Returns a list of neighbourhoods for a specific force.
        /// </summary>
        /// <param name="forceId">Unique force identifier. These are available from <see cref="Forces()"/>.</param>
        IEnumerable<NeighbourhoodSummary> Neighbourhoods(string forceId); // TODO Also accept ForceDetails as an argument.

        /// <summary>
        /// Returns the neighbourhood and policing team responsible for a particular area. 
        /// </summary>
        /// <param name="position">Latitude and Longitude of the area</param>
        NeighbourhoodForce LocateNeighbourhood(IGeoposition position);

        /// <summary>
        /// Returns a list of crimes where the responsible force hasn't specified a location.
        /// </summary>
        /// <param name="category">The <see cref="CrimeCategories(DateTime)">category</see> of the crimes</param>
        /// <param name="forceId">Specific police force</param>
        /// <param name="date">
        /// Limit results to a specific Year and Month.
        /// The latest month will be shown by default
        /// </param>
        /// <returns>Crimes where the responsible force hasn't specified a location</returns>
        IEnumerable<Crime> Crimes(string forceId, string category, DateTime? date = null);

        /// <summary>
        /// Returns just the crimes which occurred at the specified location, rather than those within a radius.
        /// If given latitude and longitude, finds the nearest pre-defined location and returns the crimes which occurred there.
        /// </summary>
        /// <param name="locationId">
        /// Crimes and outcomes are mapped to specific locations on the map.
        /// Valid IDs are returned by other methods (new and existing) which return location information.
        /// </param>
        /// <param name="date">
        /// Limit results to a specific Year and Month.
        /// The latest month will be shown by default.
        /// </param>
        /// <returns>Crimes which occurred at the specified location, rather than those within a radius</returns>
        IEnumerable<Crime> CrimesAtLocation(string locationId, DateTime date); // TODO Also accept Street as an argument (location comes from its ID)

        /// <summary>
        /// Returns just the crimes which occurred at the specified location, rather than those within a radius.
        /// If given latitude and longitude, finds the nearest pre-defined location and returns the crimes which occurred there.
        /// </summary>
        /// <param name="position">Latitude/Longitude of the requested crime area</param>
        /// <param name="date">
        /// Limit results to a specific Year and Month.
        /// The latest month will be shown by default.
        /// </param>
        /// <returns>Crimes which occurred at the specified location, rather than those within a radius</returns>
        IEnumerable<Crime> CrimesAtLocation(IGeoposition position, DateTime date);

        /// <summary>
        /// Returns the members of the team of a specific neighbourhood for a specific police force
        /// </summary>
        /// <param name="forceId">police force id of the requested neighbourhood team</param>
        /// <param name="neighbourhoodId">neighbourhood id of the requested team</param>
        /// <returns>List of Team Members for the requested neighbourhood</returns>
        IEnumerable<NeighbourhoodTeamMember> NeighbourhoodTeam(string forceId,string neighbourhoodId);

        /// <summary>
        /// Returns the events of a specific neighbourhood for a specific police force
        /// </summary>
        /// <param name="forceId">police force id of the requested neighbourhood events</param>
        /// <param name="neighbourhoodId">neighbourhood id of the requested events</param>
        /// <returns>List of events for the requested neighbourhood</returns>
        IEnumerable<NeighbourhoodEvent> NeighbourhoodEvents(string forceId, string neighbourhoodId);
    }
}