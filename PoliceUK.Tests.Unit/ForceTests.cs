namespace PoliceUk.Tests.Unit
{
    using CustomAssertions;
    using CustomAssertions.Equality.ForceDetails;
    using Entities.Force;
    using NUnit.Framework;
    using PoliceUk;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    [TestFixture]
    public class ForceTests : BaseMethodTests
    {
        #region Dummy data

        private static readonly ForceDetails DummyForceDetails = new ForceDetails()
            {
                Id = "leicestershire",
                Name = "Leicestershire Police",
                Telephone = "101",
                Url = "http://www.leics.police.uk/",
                Description = "This is an example description",
                EngagementMethods = new ForceEngagementMethod[]
                {
                    new ForceEngagementMethod
                    {
                        Url = "http://www.facebook.com/leicspolice",
                        Type = "facebook",
                        Description = "This is another example description",
                        Title = "facebook"
                    },
                    new ForceEngagementMethod
                    {
                        Url = "",
                        Type = "telephone",
                        Description = "This is yet another example description",
                        Title = "telephone"
                    }
                }
            };

        #endregion

        private static readonly object[] DummyForce = 
            {
                new object[]
                {
                    "PoliceUK.Tests.Unit.TestData.Force.json", 
                    DummyForceDetails
                }
            };

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Call_With_Null_Id_Throws_ArgumentNullException()
        {
            (new PoliceUkClient()).Force(null);
        }

        [Test]
        [ExpectedException(typeof(Exceptions.InvalidDataException))]
        public void Call_With_Malformed_Response_Throwns_InvalidDataException()
        {
            using (Stream stream = GetTestDataFromResource(MalformedTestDataResource))
            {
                var policeApi = new PoliceUkClient
                {
                    RequestFactory = CreateRequestFactory(stream)
                };

                policeApi.Force("");
            }
        }

        [Test]
        public void Call_Parses_No_Element_From_Json_Repsonse()
        {
            using (Stream stream = GetTestDataFromResource("PoliceUK.Tests.Unit.TestData.NotFound.txt"))
            {
                var policeApi = new PoliceUkClient
                {
                    RequestFactory = CreateRequestFactory(stream, System.Net.HttpStatusCode.NotFound)
                };

                ForceDetails force = policeApi.Force("");

                // Assert
                Assert.IsNull(force);
            }
        }

        [Test, TestCaseSource("DummyForce")]
        public void Call_Parses_Element_From_Json_Repsonse(string jsonResourceName, ForceDetails expectedForceDetails)
        {
            using (Stream stream = GetTestDataFromResource(jsonResourceName))
            {
                var policeApi = new PoliceUkClient
                {
                    RequestFactory = CreateRequestFactory(stream)
                };
                ForceDetails force = policeApi.Force("");

                // Assert
                Assert.IsNotNull(force);

                CustomAssert.AreEqual(expectedForceDetails, force, new ForceDetailsEqualityComparer());

                IEnumerable<ForceEngagementMethod> actualEngagementMethods = force.EngagementMethods;
                ForceEngagementMethod[] expectedEngagementMethods = expectedForceDetails.EngagementMethods.ToArray();

                Assert.That(actualEngagementMethods, Is.Not.Null.And.Count.EqualTo(expectedEngagementMethods.Length));

                int total = actualEngagementMethods.Count();
                for (int i = 0; i < total; i++)
                {
                    ForceEngagementMethod expected = expectedEngagementMethods[i];
                    ForceEngagementMethod actual   = actualEngagementMethods.ElementAtOrDefault(i);

                    CustomAssert.AreEqual(expected, actual, new ForceEngagementMethodEqualityComparer());
                }
            }
        }
    }
}
