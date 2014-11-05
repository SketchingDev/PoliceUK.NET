namespace PoliceUK.Tests.Unit
{
    using CustomAssertions;
    using CustomAssertions.Equality;
    using Entities;
    using FakeItEasy;
    using NUnit.Framework;
    using PoliceUk;
    using PoliceUk.Entities;
    using PoliceUk.Entities.Location;
    using PoliceUk.Request;
    using PoliceUk.Tests.Unit;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class StreetLevelCrimes : BaseMethodTests
    {
        public class LatLngOverride
        {
            private static readonly Crime TestCrime = new Crime
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

            [Test]
            [ExpectedException(typeof(ArgumentNullException))]
            public void Call_With_Null_Position_Throws_ArgumentNullException()
            {
                (new PoliceUkClient()).StreetLevelCrimes((IGeoposition)null);
            }

            [Test]
            [ExpectedException(typeof(PoliceUk.Exceptions.InvalidDataException))]
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
                    IHttpWebRequestFactory factory = policeApi.RequestFactory;
                    A.CallTo(() => factory.Create(A<string>.That.Contains(geoPosition.Latitiude.ToString()))).MustHaveHappened();
                    A.CallTo(() => factory.Create(A<string>.That.Contains(geoPosition.Longitude.ToString()))).MustHaveHappened();
                }
            }

            [Test]
            public void Call_Parses_No_Elements_From_Json_Repsonse()
            {
                using (Stream stream = GetTestDataFromResource(EmptyArrayTestDataResource))
                {
                    var policeApi = new PoliceUkClient
                    {
                        RequestFactory = CreateRequestFactory(stream)
                    };

                    StreetLevelCrimeResults result = policeApi.StreetLevelCrimes(A.Fake<IGeoposition>());

                    // Assert
                    Assert.IsNotNull(result);
                    Assert.IsNotNull(result.Crimes);
                    Assert.AreEqual(0, result.Crimes.Count());
                }
            }

            [Test]
            public void Call_Parses_Single_Element_From_Json_Repsonse()
            {
                using (Stream stream = GetTestDataFromResource("PoliceUK.Tests.Unit.TestData.StreetLevelCrimes.Single.json"))
                {
                    var policeApi = new PoliceUkClient
                    {
                        RequestFactory = CreateRequestFactory(stream)
                    };

                    StreetLevelCrimeResults result = policeApi.StreetLevelCrimes(A.Fake<IGeoposition>());

                    // Assert
                    Assert.IsNotNull(result);
                    Assert.IsNotNull(result.Crimes);

                    Crime crime = result.Crimes.First();
                    CustomAssert.AreEqual(TestCrime, crime, new CrimeEqualityComparer());
                }
            }

            [Test]
            public void Call_Parses_Multiple_Elements_From_Json_Repsonse()
            {
                using (Stream stream = GetTestDataFromResource("PoliceUK.Tests.Unit.TestData.StreetLevelCrimes.Multiple.json"))
                {
                    IPoliceUkClient policeApi = new PoliceUkClient
                    {
                        RequestFactory = CreateRequestFactory(stream)
                    };

                    StreetLevelCrimeResults result = policeApi.StreetLevelCrimes(A.Fake<IGeoposition>());

                    // Assert
                    Assert.IsNotNull(result);
                    Assert.IsNotNull(result.Crimes);

                    Assert.AreEqual(2, result.Crimes.Count());

                    Crime crime = result.Crimes.First();
                    CustomAssert.AreEqual(new Crime
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
                    }, crime, new CrimeEqualityComparer());

                    crime = result.Crimes.Last();
                    CustomAssert.AreEqual(TestCrime, crime, new CrimeEqualityComparer());
                }
            }
        }

        public class PolygonOverride
        {
            [Test]
            [ExpectedException(typeof(ArgumentNullException))]
            public void Call_With_Null_Position_Throws_ArgumentNullException()
            {
                (new PoliceUkClient()).StreetLevelCrimes((IEnumerable<IGeoposition>)null);
            }

            [Test]
            [ExpectedException(typeof(PoliceUk.Exceptions.InvalidDataException))]
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

                    policeApi.StreetLevelCrimes(A.Fake<IEnumerable<IGeoposition>>(), nowDateTime);

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

                    var geoPositions = new[] {
                        new Geoposition(123, 456),
                        new Geoposition(789, 012)
                    };

                    policeApi.StreetLevelCrimes(geoPositions);

                    // Assert
                    IHttpWebRequestFactory factory = policeApi.RequestFactory;
                    A.CallTo(() => factory.Create(A<string>.That.Contains(geoPositions[0].Latitiude.ToString()))).MustHaveHappened();
                    A.CallTo(() => factory.Create(A<string>.That.Contains(geoPositions[0].Longitude.ToString()))).MustHaveHappened();

                    A.CallTo(() => factory.Create(A<string>.That.Contains(geoPositions[1].Latitiude.ToString()))).MustHaveHappened();
                    A.CallTo(() => factory.Create(A<string>.That.Contains(geoPositions[1].Longitude.ToString()))).MustHaveHappened();
                }
            }
        }
    }
}
