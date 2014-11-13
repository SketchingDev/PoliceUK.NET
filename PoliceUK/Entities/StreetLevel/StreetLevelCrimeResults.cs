namespace PoliceUk.Entities.StreetLevel
{
    using System.Collections.Generic;

    public class StreetLevelCrimeResults
    {
        /// <summary>
        /// The API has returned an Internal Error status code, which it
        /// does when more than 10,000 crimes are requested or an internal 
        /// error has actually occurred.
        /// </summary>
        public bool TooManyCrimesOrError { get; set; }

        public IEnumerable<Crime> Crimes { get; set; }
    }
}
