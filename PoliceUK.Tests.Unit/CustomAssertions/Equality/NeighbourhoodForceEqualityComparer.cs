namespace PoliceUk.Tests.Unit.CustomAssertions.Equality
{
    using Entities.Force;
    using NUnit.Framework;
    using PoliceUk.Entities.Neighbourhood;

    public class NeighbourhoodForceEqualityComparer : AbstractEqualityComparer<NeighbourhoodForce>
    {
        public override bool AreEqual(NeighbourhoodForce neighbourhoodForceOne, NeighbourhoodForce neighbourhoodForceTwo)
        {
            Assert.AreEqual(neighbourhoodForceOne.ForceId, neighbourhoodForceTwo.ForceId);
            Assert.AreEqual(neighbourhoodForceOne.NeighbourhoodId, neighbourhoodForceTwo.NeighbourhoodId);

            return true;
        }
    }
}
