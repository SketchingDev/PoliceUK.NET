namespace PoliceUk.Tests.Unit.CustomAssertions.Equality
{
    using NUnit.Framework;
    using Entities.Neighbourhood;

    public class NeighbourhoodSummaryEqualityComparer : AbstractEqualityComparer<NeighbourhoodSummary>
    {
        public override bool AreEqual(NeighbourhoodSummary neighbourhoodOne, NeighbourhoodSummary neighbourhoodTwo)
        {
            Assert.AreEqual(neighbourhoodOne.Id, neighbourhoodTwo.Id);
            Assert.AreEqual(neighbourhoodOne.Name, neighbourhoodTwo.Name);

            return true;
        }
    }
}
