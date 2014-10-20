namespace PoliceUk
{
    using PoliceUk.Entities;
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

        // TODO IEnumerable<ForceShortDescription> Forces(); //http://data.police.uk/docs/method/forces/

        // TODO ForceFullDescription Force(string id); //http://data.police.uk/api/forces/leicestershire

    }
}