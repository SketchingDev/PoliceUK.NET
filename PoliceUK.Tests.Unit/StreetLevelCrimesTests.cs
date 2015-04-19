namespace PoliceUk.Tests.Unit
{
    using CustomAssertions;
    using CustomAssertions.Equality;
    using FakeItEasy;
    using NUnit.Framework;
    using PoliceUk;
    using Request;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using InvalidDataException = Exceptions.InvalidDataException;
    using Entities.StreetLevel;
    using TestDataFactories;

    public class StreetLevelCrimesTests : BaseMethodTests
    {
        [TestFixture]
        public class LatLngOverride
        {
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

            [Test, TestCaseSource(typeof(CrimeDataFactory), "NoCrime"), 
                   TestCaseSource(typeof(CrimeDataFactory), "DummyCrime"), 
                   TestCaseSource(typeof(CrimeDataFactory), "DummyCrimes")]
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
