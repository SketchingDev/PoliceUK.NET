namespace PoliceUK.Tests.Unit.CustomAssertions.Equality.ForceDetails
{
    using NUnit.Framework;
    using PoliceUK.Entities.Force;

    public class ForceEngagementMethodEqualityComparer : AbstractEqualityComparer<ForceEngagementMethod>
    {
        public override bool AreEqual(ForceEngagementMethod engagementMethodOne, ForceEngagementMethod engagementMethodTwo)
        {
            Assert.AreEqual(engagementMethodOne.Url, engagementMethodTwo.Url);
            Assert.AreEqual(engagementMethodOne.Type, engagementMethodTwo.Type);
            Assert.AreEqual(engagementMethodOne.Description, engagementMethodTwo.Description);
            Assert.AreEqual(engagementMethodOne.Title, engagementMethodTwo.Title);

            return true;
        }
    }
}
