namespace PoliceUk.Tests.Unit.CustomAssertions.Equality.NeighbourhoodDetails
{
    using Entities.Neighbourhood;
    using NUnit.Framework;

    public class NeighbourhoodLocationEqualityComparer : AbstractEqualityComparer<NeighbourhoodLocation>
    {
        public override bool AreEqual(NeighbourhoodLocation NeighbourhoodLocationOne, NeighbourhoodLocation NeighbourhoodLocationTwo)
        {
            Assert.AreEqual(NeighbourhoodLocationOne.Address, NeighbourhoodLocationTwo.Address);
            Assert.AreEqual(NeighbourhoodLocationOne.Description, NeighbourhoodLocationTwo.Description);
            Assert.AreEqual(NeighbourhoodLocationOne.Name, NeighbourhoodLocationTwo.Name);
            Assert.AreEqual(NeighbourhoodLocationOne.Latitude, NeighbourhoodLocationTwo.Latitude);
            Assert.AreEqual(NeighbourhoodLocationOne.Longitude, NeighbourhoodLocationTwo.Longitude);
            Assert.AreEqual(NeighbourhoodLocationOne.PostCode, NeighbourhoodLocationTwo.PostCode);
            Assert.AreEqual(NeighbourhoodLocationOne.Telephone, NeighbourhoodLocationTwo.Telephone);
            Assert.AreEqual(NeighbourhoodLocationOne.Type, NeighbourhoodLocationTwo.Type);

            return true;
        }
    }
}
