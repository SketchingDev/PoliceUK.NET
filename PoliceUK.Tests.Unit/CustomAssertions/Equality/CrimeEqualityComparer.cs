namespace PoliceUk.Tests.Unit.CustomAssertions.Equality
{
    using Entities.Location;
    using Entities.StreetLevel;
    using Location;
    using NUnit.Framework;
    using System.Collections.Generic;

    public class CrimeEqualityComparer : AbstractEqualityComparer<Crime>
    {
        private readonly IEqualityComparer<CrimeLocation> locationComparer 
            = new CrimeLocationEqualityComparer();

        public override bool AreEqual(Crime crimeOne, Crime crimeTwo)
        {
            Assert.AreEqual(crimeOne.Id, crimeTwo.Id);
            Assert.AreEqual(crimeOne.Month, crimeTwo.Month);
            Assert.AreEqual(crimeOne.Category, crimeTwo.Category);
            Assert.AreEqual(crimeOne.LocationType, crimeTwo.LocationType);
            Assert.AreEqual(crimeOne.PersistentId, crimeTwo.PersistentId);
            Assert.AreEqual(crimeOne.LocationSubtype, crimeTwo.LocationSubtype);

            return locationComparer.Equals(crimeOne.Location, crimeTwo.Location);
        }
    }
}
