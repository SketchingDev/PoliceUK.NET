namespace PoliceUK.Tests.Unit.CustomAssertions.Equality
{
    using Entities.Force;
    using NUnit.Framework;

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
