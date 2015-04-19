namespace PoliceUk.Tests.Unit
{
    using NUnit.Framework;
    using PoliceUk;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    [TestFixture]
    public class StreetLevelAvailabilityTests : BaseMethodTests
    {
        #region Dummy data

        private static readonly DateTime AvailabilityOne = new DateTime(2011, 9, 1);

        private static readonly DateTime AvailabilityTwo = new DateTime(2011, 8, 1);

        #endregion

        private static readonly object[] NoAvailability = 
            {
                new object[]
                {
                    EmptyArrayTestDataResource, 
                    new DateTime[]{}
                }
            };

        private static readonly object[] DummyAvailability = 
            {
                new object[]
                {
                    "PoliceUK.Tests.Unit.TestData.StreetLevelAvailability.Single.json", 
                    new DateTime[]
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
                    new DateTime[]
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
        public void Call_Parses_Elements_From_Json_Repsonse(string jsonResourceName, DateTime[] expectedAvailabilities)
        {
            using (Stream stream = GetTestDataFromResource(jsonResourceName))
            {
                var policeApi = new PoliceUkClient
                {
                    RequestFactory = CreateRequestFactory(stream)
                };
                IEnumerable<DateTime> availableCrimeDates = policeApi.StreetLevelAvailability();

                // Assert
                Assert.That(availableCrimeDates, Is.Not.Null.And.Length.EqualTo(expectedAvailabilities.Length));

                int total = availableCrimeDates.Count();
                for (int i = 0; i < total; i++)
                {
                    DateTime expected = expectedAvailabilities[i];
                    DateTime actual = availableCrimeDates.ElementAtOrDefault(i);

                    Assert.AreEqual(expected, actual);
                }
            }
        }
    }
}
