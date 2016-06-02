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
    public class NeighbourhoodEventEqualityComparer : AbstractEqualityComparer<NeighbourhoodEvent>
    {
        private readonly IEqualityComparer<ContactDetails> contactDetailsComparer = new ContactDetailsEqualityComparer();

        public override bool AreEqual(NeighbourhoodEvent x, NeighbourhoodEvent y)
        {
            Assert.AreEqual(x.Address, y.Address);
            Assert.AreEqual(x.Description, y.Description);
            Assert.AreEqual(x.StartDate, y.StartDate);
            Assert.AreEqual(x.Title, y.Title);
            Assert.AreEqual(x.Type, y.Type);
            return contactDetailsComparer.Equals(x.ContactDetails,y.ContactDetails);
        }
    }
}
