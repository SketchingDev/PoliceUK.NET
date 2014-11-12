namespace PoliceUk.Tests.Unit.CustomAssertions.Equality.Location
{
    using NUnit.Framework;
    using PoliceUk.Entities.Location;

    public class StreetEqualityComparer : AbstractEqualityComparer<Street>
    {
        public override bool AreEqual(Street streetOne, Street streetTwo)
        {
            Assert.AreEqual(streetOne.Id, streetTwo.Id);
            Assert.AreEqual(streetOne.Name, streetTwo.Name);

            return true;
        }
    }
}
