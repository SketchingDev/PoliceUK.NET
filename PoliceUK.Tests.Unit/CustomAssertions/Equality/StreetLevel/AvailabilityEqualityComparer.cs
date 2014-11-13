namespace PoliceUk.Tests.Unit.CustomAssertions.Equality.StreetLevel
{
    using Entities.StreetLevel;
    using NUnit.Framework;

    public class AvailabilityEqualityComparer : AbstractEqualityComparer<Availability>
    {
        public override bool AreEqual(Availability availabilityOne, Availability availabilityTwo)
        {
            Assert.AreEqual(availabilityOne.Date, availabilityTwo.Date);

            return true;
        }
    }
}
