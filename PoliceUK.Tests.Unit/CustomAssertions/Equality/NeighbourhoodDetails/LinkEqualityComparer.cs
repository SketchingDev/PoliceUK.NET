namespace PoliceUk.Tests.Unit.CustomAssertions.Equality.NeighbourhoodDetails
{
    using Entities.Neighbourhood;
    using NUnit.Framework;

    public class LinkEqualityComparer : AbstractEqualityComparer<Link>
    {
        public override bool AreEqual(Link LinkOne, Link LinkTwo)
        {
            Assert.AreEqual(LinkOne.Description, LinkTwo.Description);
            Assert.AreEqual(LinkOne.Title, LinkTwo.Title);
            Assert.AreEqual(LinkOne.Url, LinkTwo.Url);

            return true;
        }
    }
}
