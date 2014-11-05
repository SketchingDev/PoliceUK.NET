namespace PoliceUK.Tests.Unit
{
    using NUnit.Framework;
    using PoliceUk;
    using PoliceUk.Tests.Unit;
    using PoliceUK.Entities.Force;
    using PoliceUK.Tests.Unit.CustomAssertions;
    using PoliceUK.Tests.Unit.CustomAssertions.Equality.ForceDetails;
    using System;
    using System.IO;
    using System.Linq;

    [TestFixture]
    public class Force : BaseMethodTests
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Call_With_Null_Id_Throws_ArgumentNullException()
        {
            (new PoliceUkClient()).Force(null);
        }

        [Test]
        [ExpectedException(typeof(PoliceUk.Exceptions.InvalidDataException))]
        public void Call_With_Malformed_Response_Throwns_InvalidDataException()
        {
            using (Stream stream = GetTestDataFromResource(MalformedTestDataResource))
            {
                PoliceUkClient policeApi = new PoliceUkClient()
                {
                    RequestFactory = CreateRequestFactory(stream)
                };

                policeApi.Force("");
            }
        }

        [Test]
        public void Call_Parses_No_Element_From_Json_Repsonse()
        {
            using (Stream stream = GetTestDataFromResource("PoliceUK.Tests.Unit.TestData.Force.NotFound.txt"))
            {
                IPoliceUkClient policeApi = new PoliceUkClient()
                {
                    RequestFactory = CreateRequestFactory(stream, System.Net.HttpStatusCode.NotFound)
                };

                ForceDetails force = policeApi.Force("");

                // Assert
                Assert.IsNull(force);
            }
        }

        [Test]
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
