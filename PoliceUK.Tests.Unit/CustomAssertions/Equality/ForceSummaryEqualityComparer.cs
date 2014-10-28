namespace PoliceUK.Tests.Unit.CustomAssertions.Equality
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PoliceUK.Entities;
    using PoliceUK.Entities.Force;

    public class ForceSummaryEqualityComparer : AbstractEqualityComparer<ForceSummary>
    {
        public override bool AreEqual(ForceSummary forceDescOne, ForceSummary forceDescTwo)
        {
            Assert.AreEqual(forceDescOne.Id, forceDescTwo.Id);
            Assert.AreEqual(forceDescOne.Name, forceDescTwo.Name);

            return true;
        }
    }
}
