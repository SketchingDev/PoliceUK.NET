namespace PoliceUk.Tests.Unit.CustomAssertions.Equality.NeighbourhoodDetails
{
    using Entities.Neighbourhood;
    using NUnit.Framework;

    public class NeighbourhoodDetailsEqualityComparer : AbstractEqualityComparer<NeighbourhoodDetails>
    {
        public override bool AreEqual(NeighbourhoodDetails NeighbourhoodDetailsOne, NeighbourhoodDetails NeighbourhoodDetailsTwo)
        {
            Assert.AreEqual(NeighbourhoodDetailsOne.Id, NeighbourhoodDetailsTwo.Id);
            Assert.AreEqual(NeighbourhoodDetailsOne.Name, NeighbourhoodDetailsTwo.Name);            
            Assert.AreEqual(NeighbourhoodDetailsOne.UrlBoundary, NeighbourhoodDetailsTwo.UrlBoundary);
            Assert.AreEqual(NeighbourhoodDetailsOne.UrlForce, NeighbourhoodDetailsTwo.UrlForce);            
            Assert.AreEqual(NeighbourhoodDetailsOne.WelcomeMessage, NeighbourhoodDetailsTwo.WelcomeMessage);
            Assert.AreEqual(NeighbourhoodDetailsOne.Population, NeighbourhoodDetailsTwo.Population);
            Assert.AreEqual(NeighbourhoodDetailsOne.Description, NeighbourhoodDetailsTwo.Description);

            if (NeighbourhoodDetailsOne.Centre != null || NeighbourhoodDetailsTwo.Centre != null)
            {
                Assert.AreEqual(NeighbourhoodDetailsOne.Centre.Latitude, NeighbourhoodDetailsTwo.Centre.Latitude);
                Assert.AreEqual(NeighbourhoodDetailsOne.Centre.Longitude, NeighbourhoodDetailsTwo.Centre.Longitude);
            }

            return true;
        }
    }
}
