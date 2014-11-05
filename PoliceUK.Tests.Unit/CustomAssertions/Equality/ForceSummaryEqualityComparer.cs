namespace PoliceUK.Tests.Unit.CustomAssertions.Equality
{
    using NUnit.Framework;
    using PoliceUK.Entities;
    using PoliceUK.Entities.Force;

    public class ForceSummaryEqualityComparer : AbstractEqualityComparer<ForceSummary>
    {
        public override bool AreEqual(ForceSummary forceSummeryOne, ForceSummary forceSummeryTwo)
        {
            Assert.AreEqual(forceSummeryOne.Id, forceSummeryTwo.Id);
            Assert.AreEqual(forceSummeryOne.Name, forceSummeryTwo.Name);

            return true;
        }
    }
}
