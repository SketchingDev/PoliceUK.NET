namespace PoliceUk.Tests.Unit.CustomAssertions.Equality.Location
{
    using NUnit.Framework;
    using PoliceUk.Entities.Location;
    using System.Collections.Generic;

    public class CrimeLocationEqualityComparer : AbstractEqualityComparer<CrimeLocation>
    {
        private readonly IEqualityComparer<Street> streetComparer
            = new StreetEqualityComparer();

        public override bool AreEqual(CrimeLocation locationOne, CrimeLocation locationTwo)
        {
            Assert.AreEqual(locationOne.Latitude, locationTwo.Latitude);
            Assert.AreEqual(locationOne.Longitude, locationTwo.Longitude);

            return streetComparer.Equals(locationOne.Street, locationTwo.Street);
        }
    }
}
