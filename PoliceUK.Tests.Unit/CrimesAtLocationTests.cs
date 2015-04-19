namespace PoliceUk.Tests.Unit
{
    using Entities.StreetLevel;
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
    using TestDataFactories;

    public class CrimesAtLocationTests : BaseMethodTests
    {
        [TestFixture]
        public class LocationIdOverride
        {
            [Test]
            [ExpectedException(typeof (ArgumentNullException))]
            public void Call_With_Null_LocationId_Throws_ArgumentNullException()
            {
                (new PoliceUkClient()).CrimesAtLocation((string)null, DateTime.Now);
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

                    policeApi.CrimesAtLocation("", nowDateTime);

                    // Assert
                    IHttpWebRequestFactory factory = policeApi.RequestFactory;
                    A.CallTo(() => factory.Create(A<string>.That.Contains(formattedDateTime))).MustHaveHappened();
                }
            }

            [Test]
            public void Call_Contains_LocationId_In_Request()
            {
                using (Stream stream = GetTestDataFromResource(EmptyArrayTestDataResource))
                {
                    var policeApi = new PoliceUkClient
                    {
                        RequestFactory = CreateRequestFactory(stream)
                    };

                    const string locationId = "123";

                    policeApi.CrimesAtLocation(locationId, DateTime.Now);

                    // Assert
                    IHttpWebRequestFactory factory = policeApi.RequestFactory;
                    A.CallTo(() => factory.Create(A<string>.That.Contains(locationId))).MustHaveHappened();
                }
            }

            //[Test]
            //public void Call_Contains_LatLng_In_Request()
            //{
            //    using (Stream stream = GetTestDataFromResource(EmptyArrayTestDataResource))
            //    {
            //        var policeApi = new PoliceUkClient
            //        {
            //            RequestFactory = CreateRequestFactory(stream)
            //        };

            //        var geoPosition = new Geoposition(123, 456);

            //        policeApi.StreetLevelCrimes(geoPosition);

            //        // Assert
            //        string latitude  = geoPosition.Latitiude.ToString();
            //        string longitude = geoPosition.Longitude.ToString();

            //        IHttpWebRequestFactory factory = policeApi.RequestFactory;
            //        A.CallTo(() => factory.Create(A<string>.That.Contains(latitude ))).MustHaveHappened();
            //        A.CallTo(() => factory.Create(A<string>.That.Contains(longitude))).MustHaveHappened();
            //    }
            //}

            [Test, TestCaseSource(typeof (CrimeDataFactory), "NoCrime"),
             TestCaseSource(typeof (CrimeDataFactory), "DummyCrime"),
             TestCaseSource(typeof (CrimeDataFactory), "DummyCrimes")]
            public void Call_Parses_Elements_From_Json_Repsonse(string jsonResourceName, Crime[] expectedCrimes)
            {
                using (Stream stream = GetTestDataFromResource(jsonResourceName))
                {
                    IPoliceUkClient policeApi = new PoliceUkClient
                    {
                        RequestFactory = CreateRequestFactory(stream)
                    };

                    IEnumerable<Crime> result = policeApi.CrimesAtLocation("", DateTime.Now);

                    // Assert
                    Assert.IsNotNull(result);
                    Assert.That(result, Is.Not.Null.And.Length.EqualTo(expectedCrimes.Length));

                    int total = result.Count();
                    for (int i = 0; i < total; i++)
                    {
                        Crime expected = expectedCrimes[i];
                        Crime actual = result.ElementAtOrDefault(i);

                        CustomAssert.AreEqual(expected, actual, new CrimeEqualityComparer());
                    }
                }
            }
        }
        
        [TestFixture]
        public class LatLngOverride
        {
            [Test]
            [ExpectedException(typeof(ArgumentNullException))]
            public void Call_With_Null_GeoPosition_Throws_ArgumentNullException()
            {
                (new PoliceUkClient()).CrimesAtLocation((IGeoposition)null, DateTime.Now);
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

                    policeApi.CrimesAtLocation(new Geoposition(0, 0), nowDateTime);

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

                    policeApi.CrimesAtLocation(geoPosition, DateTime.Now);

                    // Assert
                    string latitude = geoPosition.Latitiude.ToString();
                    string longitude = geoPosition.Longitude.ToString();

                    IHttpWebRequestFactory factory = policeApi.RequestFactory;
                    A.CallTo(() => factory.Create(A<string>.That.Contains(latitude))).MustHaveHappened();
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

                    IEnumerable<Crime> result = policeApi.CrimesAtLocation(new Geoposition(0,0), DateTime.Now);

                    // Assert
                    Assert.IsNotNull(result);
                    Assert.That(result, Is.Not.Null.And.Length.EqualTo(expectedCrimes.Length));

                    int total = result.Count();
                    for (int i = 0; i < total; i++)
                    {
                        Crime expected = expectedCrimes[i];
                        Crime actual = result.ElementAtOrDefault(i);

                        CustomAssert.AreEqual(expected, actual, new CrimeEqualityComparer());
                    }
                }
            }
        }
    }
}
