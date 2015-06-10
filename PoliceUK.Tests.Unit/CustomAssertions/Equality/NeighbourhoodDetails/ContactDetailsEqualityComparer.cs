namespace PoliceUk.Tests.Unit.CustomAssertions.Equality.NeighbourhoodDetails
{
    using Entities.Neighbourhood;
    using NUnit.Framework;

    public class ContactDetailsEqualityComparer : AbstractEqualityComparer<ContactDetails>
    {
        public override bool AreEqual(ContactDetails ContactDetailsOne, ContactDetails ContactDetailsTwo)
        {
            Assert.AreEqual(ContactDetailsOne.Address, ContactDetailsTwo.Address);
            Assert.AreEqual(ContactDetailsOne.Bebo, ContactDetailsTwo.Bebo);
            Assert.AreEqual(ContactDetailsOne.Blog, ContactDetailsTwo.Blog);
            Assert.AreEqual(ContactDetailsOne.Email, ContactDetailsTwo.Email);
            Assert.AreEqual(ContactDetailsOne.Emessaging, ContactDetailsTwo.Emessaging);
            Assert.AreEqual(ContactDetailsOne.Facebook, ContactDetailsTwo.Facebook);
            Assert.AreEqual(ContactDetailsOne.Fax, ContactDetailsTwo.Fax);
            Assert.AreEqual(ContactDetailsOne.Flickr, ContactDetailsTwo.Flickr);
            Assert.AreEqual(ContactDetailsOne.Forum, ContactDetailsTwo.Forum);
            Assert.AreEqual(ContactDetailsOne.GooglePlus, ContactDetailsTwo.GooglePlus);
            Assert.AreEqual(ContactDetailsOne.Mobile, ContactDetailsTwo.Mobile);
            Assert.AreEqual(ContactDetailsOne.MySpace, ContactDetailsTwo.MySpace);
            Assert.AreEqual(ContactDetailsOne.Rss, ContactDetailsTwo.Rss);
            Assert.AreEqual(ContactDetailsOne.Telephone, ContactDetailsTwo.Telephone);
            Assert.AreEqual(ContactDetailsOne.Twitter, ContactDetailsTwo.Twitter);
            Assert.AreEqual(ContactDetailsOne.Web, ContactDetailsTwo.Web);
            Assert.AreEqual(ContactDetailsOne.YouTube, ContactDetailsTwo.YouTube);

            return true;
        }
    }
}
