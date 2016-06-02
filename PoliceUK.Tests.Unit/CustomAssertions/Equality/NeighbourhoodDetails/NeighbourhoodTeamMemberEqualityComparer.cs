using NUnit.Framework;
using PoliceUk.Entities.Neighbourhood;
using PoliceUk.Tests.Unit.CustomAssertions.Equality;
using PoliceUk.Tests.Unit.CustomAssertions.Equality.NeighbourhoodDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PoliceUK.Tests.Unit.CustomAssertions.Equality.NeighbourhoodDetails
{
    public class NeighbourhoodTeamMemberEqualityComparer : AbstractEqualityComparer<NeighbourhoodTeamMember>
    {
        private readonly IEqualityComparer<ContactDetails> contactDetailsComparer = new ContactDetailsEqualityComparer();

        public override bool AreEqual(NeighbourhoodTeamMember x, NeighbourhoodTeamMember y)
        {
            Assert.AreEqual(x.Name, y.Name);
            Assert.AreEqual(x.Rank, y.Rank);
            Assert.AreEqual(x.Bio, y.Bio);
            return contactDetailsComparer.Equals(x.ContactDetails,y.ContactDetails);
        }
    }
}
