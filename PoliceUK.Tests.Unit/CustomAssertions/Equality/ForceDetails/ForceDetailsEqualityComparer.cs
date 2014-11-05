namespace PoliceUK.Tests.Unit.CustomAssertions.Equality.ForceDetails
{
    using NUnit.Framework;
    using PoliceUK.Entities.Force;
    using System.Collections.Generic;

    public class ForceDetailsEqualityComparer : AbstractEqualityComparer<ForceDetails>
    {
        public override bool AreEqual(ForceDetails forceDetailsOne, ForceDetails forceDetailsTwo)
        {
            Assert.AreEqual(forceDetailsOne.Id, forceDetailsTwo.Id);
            Assert.AreEqual(forceDetailsOne.Name, forceDetailsTwo.Name);
            Assert.AreEqual(forceDetailsOne.Telephone, forceDetailsTwo.Telephone);
            Assert.AreEqual(forceDetailsOne.Url, forceDetailsTwo.Url);
            Assert.AreEqual(forceDetailsOne.Description, forceDetailsTwo.Description);

            return true;
        }
    }
}
