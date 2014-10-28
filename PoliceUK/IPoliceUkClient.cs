namespace PoliceUk
{
    using PoliceUk.Entities;
    using PoliceUK.Entities.Force;
    using System;
    using System.Collections.Generic;

    public interface IPoliceUkClient
    {
        /// <summary>
        /// Crimes at street-level; either within a 1 mile radius of a single point, or within a custom area.
        /// 
        /// IMPORTANT NOTE: The street-level crimes returned in the API are only an approximation of where 
        /// the actual crimes occurred, they are NOT the exact locations.
        /// </summary>
        /// <param name="position">Latitude and Longitude  of the requested crime area</param>
        /// <param name="date">Optional. (YYYY-MM) Limit results to a specific month.
        /// The latest month will be shown by default.
        /// </param>
        IEnumerable<Crime> StreetLevelCrimes(IGeoposition position, DateTime? date = null);

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
        IEnumerable<ForceSummary> Force(string id);
    }
}