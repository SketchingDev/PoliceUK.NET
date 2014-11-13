namespace PoliceUk.Tests.Unit
{
    using CustomAssertions;
    using CustomAssertions.Equality.StreetLevel;
    using Entities.StreetLevel;
    using NUnit.Framework;
    using PoliceUk;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    [TestFixture]
    public class StreetLevelAvailability : BaseMethodTests
    {
        #region Dummy data

        private static readonly Availability AvailabilityOne = new Availability
            {
                Date = "2011-09"
            };

        private static readonly Availability AvailabilityTwo = new Availability
            {
                Date = "2011-08"
            };

        #endregion

        private static readonly object[] NoAvailability = 
            {
                new object[]
                {
                    EmptyArrayTestDataResource, 
                    new Availability[]{}
                }
            };

        private static readonly object[] DummyAvailability = 
            {
                new object[]
                {
                    "PoliceUK.Tests.Unit.TestData.StreetLevelAvailability.Single.json", 
                    new Availability[]
                    {
                        AvailabilityOne
                    }
                }
            };

        private static readonly object[] DummyAvailabilities = 
            {
                new object[]
                {
                    "PoliceUK.Tests.Unit.TestData.StreetLevelAvailability.Multiple.json", 
                    new Availability[]
                    {
                        AvailabilityOne, 
                        AvailabilityTwo
                    }
                }
            };

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

                policeApi.StreetLevelAvailability();
            }
        }

        [Test, TestCaseSource("NoAvailability"), TestCaseSource("DummyAvailability"), TestCaseSource("DummyAvailabilities")]
        public void Call_Parses_Elements_From_Json_Repsonse(string jsonResourceName, Availability[] expectedAvailabilities)
        {
            using (Stream stream = GetTestDataFromResource(jsonResourceName))
            {
                var policeApi = new PoliceUkClient
                {
                    RequestFactory = CreateRequestFactory(stream)
                };
                IEnumerable<Availability> availableCrimeDates = policeApi.StreetLevelAvailability();

                // Assert
                Assert.That(availableCrimeDates, Is.Not.Null.And.Length.EqualTo(expectedAvailabilities.Length));

                int total = availableCrimeDates.Count();
                for (int i = 0; i < total; i++)
                {
                    Availability expected = expectedAvailabilities[i];
                    Availability actual = availableCrimeDates.ElementAtOrDefault(i);

                    CustomAssert.AreEqual(expected, actual, new AvailabilityEqualityComparer());
                }
            }
        }
    }
}
