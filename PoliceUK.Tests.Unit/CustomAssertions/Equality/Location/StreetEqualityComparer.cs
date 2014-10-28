namespace PoliceUK.Tests.Unit.CustomAssertions.Equality.Location
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
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
