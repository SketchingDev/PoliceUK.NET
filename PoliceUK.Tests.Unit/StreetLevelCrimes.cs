namespace PoliceUk.Tests.Unit
{
    using CustomAssertions;
    using CustomAssertions.Equality;
    using Entities;
    using FakeItEasy;
    using NUnit.Framework;
    using PoliceUk;
    using Entities.Location;
    using Request;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using InvalidDataException = Exceptions.InvalidDataException;

    public class StreetLevelCrimes : BaseMethodTests
    {
        #region Dummy data

        private static readonly Crime DummyStreetLevelCrimeOne = new Crime
        {
            Category = "anti-social-behaviour",
            PersistentId = "",
            LocationType = "Force",
            LocationSubtype = "",
            Id = "20599642",
            Location = new CrimeLocation
            {
                Latitude = 52.6269479,
                Longitude = -1.1121716,
                Street = new Street
                {
                    Id = 882380,
                    Name = "On or near Cedar Road"
                }
            },
            Context = "",
            Month = "2013-01",
            OutcomeStatus = null
        };

        private static readonly Crime DummyStreetLevelCrimeTwo = new Crime
        {
            Category = "burglary",
            PersistentId = "aebd220e869a235ba92cde43f7e0df29001573b3df1b094bb952820b2b8f44b0",
            LocationType = "Force",
            LocationSubtype = "",
            Id = "20604632",
            Location = new CrimeLocation
            {
                Latitude = 52.6271606,
                Longitude = -1.1485111,
                Street = new Street
                {
                    Id = 882208,
                    Name = "On or near Norman Street"
                }
            },
            Context = "Example context",
            Month = "2013-01",
            OutcomeStatus = new OutcomeStatus
            {
                Category = "Under investigation",
                Date = "2013-01"
            }
        };

        #endregion

        [TestFixture]
        public class LatLngOverride
        {
            private static readonly object[] NoStreetLevelCrime = 
            {
                new object[]
                {
                    EmptyArrayTestDataResource, 
                    new Crime[]{}
                }
            };

            private static readonly object[] DummyStreetLevelCrime = 
            {
                new object[]
                {
                    "PoliceUK.Tests.Unit.TestData.StreetLevelCrimes.Single.json", 
                    new Crime[]
                    {
                        DummyStreetLevelCrimeOne
                    }
                }
            };

            private static readonly object[] DummyStreetLevelCrimes = 
            {
                new object[]
                {
                    "PoliceUK.Tests.Unit.TestData.StreetLevelCrimes.Multiple.json", 
                    new Crime[]
                    {
                        DummyStreetLevelCrimeOne, 
                        DummyStreetLevelCrimeTwo
                    }
                }
            };

            [Test]
            [ExpectedException(typeof(ArgumentNullException))]
            public void Call_With_Null_Position_Throws_ArgumentNullException()
            {
                (new PoliceUkClient()).StreetLevelCrimes((IGeoposition)null);
            }

            [Test]
            [ExpectedException(typeof(InvalidDataException))]
            public void Call_With_Malformed_Response_Throwns_InvalidDataException()
            {
                using (Stream stream = GetTestDataFromResource(MalformedTestDataResource))
                {
                    var policeApi = new PoliceUkClient
                    {
                        RequestFactory = CreateRequestFactory(stream)
                    };

                    policeApi.StreetLevelCrimes(A.Fake<IGeoposition>(), DateTime.Now);
                }
            }

            [Test]
            public void Call_Contains_Date_In_Request()
            {
                using (Stream stream = GetTestDataFromResource(EmptyArrayTestDataResource))
                {
                    var policeApi = new PoliceUkClient
                    {
                        RequestFactory = CreateRequestFactory(stream)
                    };

                    DateTime nowDateTime = DateTime.Now;
                    string formattedDateTime = nowDateTime.ToString("yyyy'-'MM");

                    policeApi.StreetLevelCrimes(A.Fake<IGeoposition>(), nowDateTime);

                    // Assert
                    IHttpWebRequestFactory factory = policeApi.RequestFactory;
                    A.CallTo(() => factory.Create(A<string>.That.Contains(formattedDateTime))).MustHaveHappened();
                }
            }

            [Test]
            public void Call_Contains_LatLng_In_Request()
            {
                using (Stream stream = GetTestDataFromResource(EmptyArrayTestDataResource))
                {
                    var policeApi = new PoliceUkClient
                    {
                        RequestFactory = CreateRequestFactory(stream)
                    };

                    var geoPosition = new Geoposition(123, 456);

                    policeApi.StreetLevelCrimes(geoPosition);

                    // Assert
                    string latitude  = geoPosition.Latitiude.ToString();
                    string longitude = geoPosition.Longitude.ToString();

                    IHttpWebRequestFactory factory = policeApi.RequestFactory;
                    A.CallTo(() => factory.Create(A<string>.That.Contains(latitude ))).MustHaveHappened();
                    A.CallTo(() => factory.Create(A<string>.That.Contains(longitude))).MustHaveHappened();
                }
            }

            [Test, TestCaseSource("NoStreetLevelCrime"), TestCaseSource("DummyStreetLevelCrime"), TestCaseSource("DummyStreetLevelCrimes")]
            public void Call_Parses_Elements_From_Json_Repsonse(string jsonResourceName, Crime[] expectedCrimes)
            {
                using (Stream stream = GetTestDataFromResource(jsonResourceName))
                {
                    IPoliceUkClient policeApi = new PoliceUkClient
                    {
                        RequestFactory = CreateRequestFactory(stream)
                    };

                    StreetLevelCrimeResults result = policeApi.StreetLevelCrimes(A.Fake<IGeoposition>());

                    // Assert
                    Assert.IsNotNull(result);
                    Assert.That(result.Crimes, Is.Not.Null.And.Length.EqualTo(expectedCrimes.Length));

                    int total = result.Crimes.Count();
                    for (int i = 0; i < total; i++)
                    {
                        Crime expected = expectedCrimes[i];
                        Crime actual   = result.Crimes.ElementAtOrDefault(i);

                        CustomAssert.AreEqual(expected, actual, new CrimeEqualityComparer());
                    }
                }
            }
        }

        [TestFixture]
        public class PolygonOverride
        {
            [Test]
            [ExpectedException(typeof(ArgumentNullException))]
            public void Call_With_Null_Position_Throws_ArgumentNullException()
            {
                (new PoliceUkClient()).StreetLevelCrimes((IEnumerable<IGeoposition>)null);
            }

            [Test]
            [ExpectedException(typeof(InvalidDataException))]
            public void Call_With_Malformed_Response_Throwns_InvalidDataException()
            {
                using (Stream stream = GetTestDataFromResource(MalformedTestDataResource))
                {
                    var policeApi = new PoliceUkClient
                    {
                        RequestFactory = CreateRequestFactory(stream)
                    };

                    policeApi.StreetLevelCrimes(A.Fake<IEnumerable<IGeoposition>>(), DateTime.Now);
                }
            }
        }
    }
}
