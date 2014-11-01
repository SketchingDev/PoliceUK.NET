namespace PoliceUK.Tests.Unit
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PoliceUk;
    using PoliceUk.Tests.Unit;
    using PoliceUK.Entities.Force;
    using PoliceUK.Tests.Unit.CustomAssertions;
    using PoliceUK.Tests.Unit.CustomAssertions.Equality.ForceDetails;
    using System.IO;
    using System.Linq;

    [TestClass]
    public class Force : BaseMethodTests
    {
        [TestMethod]
        [ExpectedException(typeof(PoliceUk.Exceptions.InvalidDataException))]
        public void Call_With_Malformed_Response_Throwns_InvalidDataException()
        {
            using (Stream stream = GetTestDataFromResource("PoliceUK.Tests.Unit.TestData.Malformed.json"))
            {
                PoliceUkClient policeApi = new PoliceUkClient()
                {
                    RequestFactory = CreateRequestFactory(stream)
                };

                policeApi.Force("");
            }
        }

        // TODO Force_Call_Parses_No_Elements_From_Json_Repsonse()

        [TestMethod]
        public void Call_Parses_Single_Element_From_Json_Repsonse()
        {
            using (Stream stream = GetTestDataFromResource("PoliceUK.Tests.Unit.TestData.Force.json"))
            {
                IPoliceUkClient policeApi = new PoliceUkClient()
                {
                    RequestFactory = CreateRequestFactory(stream)
                };
                ForceDetails force = policeApi.Force("");

                // Assert
                Assert.IsNotNull(force);

                CustomAssert.AreEqual(new ForceDetails()
                {
                    Id = "leicestershire",
                    Name = "Leicestershire Police",
                    Telephone = "101",
                    Url = "http://www.leics.police.uk/",
                    Description = "This is an example description"
                }, force, new ForceDetailsEqualityComparer());


                ForceEngagementMethod engagementMethod = force.EngagementMethods.First();
                Assert.IsNotNull(engagementMethod);

                CustomAssert.AreEqual(new ForceEngagementMethod()
                {
                    Url = "http://www.facebook.com/leicspolice",
                    Type = "facebook",
                    Description = "This is another example description",
                    Title = "facebook"
                }, engagementMethod, new ForceEngagementMethodEqualityComparer());

                engagementMethod = force.EngagementMethods.Last();
                Assert.IsNotNull(engagementMethod);

                CustomAssert.AreEqual(new ForceEngagementMethod()
                {
                    Url = "",
                    Type = "telephone",
                    Description = "This is yet another example description",
                    Title = "telephone"
                }, engagementMethod, new ForceEngagementMethodEqualityComparer());
            }
        }
    }
}
