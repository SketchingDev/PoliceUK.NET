namespace PoliceUk.Tests.Unit
{
    using FakeItEasy;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PoliceUk.Entities;
    using PoliceUk.Entities.Location;
    using PoliceUk.Request;
    using PoliceUK.Entities.Force;
    using PoliceUK.Tests.Unit.CustomAssertions;
    using PoliceUK.Tests.Unit.CustomAssertions.Equality;
    using PoliceUK.Tests.Unit.CustomAssertions.Equality.ForceDetails;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    [TestClass]
    public class PoliceUkClientTests
    {
        private static IHttpWebRequestFactory CreateRequestFactory(Stream streamResponse)
        {
            var response = A.Fake<IHttpWebResponse>();
            A.CallTo(() => response.GetResponseStream()).Returns(streamResponse);

            var request = A.Fake<IHttpWebRequest>();
            A.CallTo(() => request.GetResponse()).Returns(response);

            var requestFactory = A.Fake<IHttpWebRequestFactory>();
            A.CallTo(() => requestFactory.Create(A<string>.Ignored)).Returns(request);

            return requestFactory;
        }

        private static Stream GetTestDataFromResource(string name)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Stream stream = assembly.GetManifestResourceStream(name);

            if (stream == null)
            {
                Assert.Fail("Failed to get resource '{0}' from the calling assembly", name);
            }

            return stream;
        }

        #region Crime Categories tests

        [TestMethod]
        [ExpectedException(typeof(PoliceUk.Exceptions.InvalidDataException))]
        public void CrimeCategories_Call_With_Malformed_Response_Throwns_InvalidDataException()
        {
            using (Stream stream = GetTestDataFromResource("PoliceUK.Tests.Unit.TestData.Malformed.json"))
            {
                PoliceUkClient policeApi = new PoliceUkClient()
                {
                    RequestFactory = CreateRequestFactory(stream)
                };

                policeApi.CrimeCategories(DateTime.Now);
            }
        }

        [TestMethod]
        public void CrimeCategories_Call_Contains_Date_In_Request()
        {
            using (Stream stream = GetTestDataFromResource("PoliceUK.Tests.Unit.TestData.EmptyArray.json"))
            {
                PoliceUkClient policeApi = new PoliceUkClient()
                {
                    RequestFactory = CreateRequestFactory(stream)
                };

                DateTime nowDateTime = DateTime.Now;
                string formattedDateTime = nowDateTime.ToString("yyyy'-'MM");

                IEnumerable<Category> categories = policeApi.CrimeCategories(nowDateTime);

                // Assert
                IHttpWebRequestFactory factory = policeApi.RequestFactory;
                A.CallTo(() => factory.Create(A<string>.That.Contains(formattedDateTime))).MustHaveHappened();
            }
        }

        [TestMethod]
        public void CrimeCategories_Call_Parses_No_Elements_From_Json_Repsonse()
        {
            using (Stream stream = GetTestDataFromResource("PoliceUK.Tests.Unit.TestData.EmptyArray.json"))
            {
                IPoliceUkClient policeApi = new PoliceUkClient()
                {
                    RequestFactory = CreateRequestFactory(stream)
                };

                IEnumerable<Category> categories = policeApi.CrimeCategories(DateTime.Now);

                // Assert
                Assert.IsNotNull(categories);
                Assert.AreEqual(0, categories.Count());
            }
        }

        [TestMethod]
        public void CrimeCategories_Call_Parses_Single_Element_From_Json_Repsonse()
        {
            using (Stream stream = GetTestDataFromResource("PoliceUK.Tests.Unit.TestData.CrimeCatagories.Single.json"))
            {
                IPoliceUkClient policeApi = new PoliceUkClient()
                {
                    RequestFactory = CreateRequestFactory(stream)
                };
                IEnumerable<Category> categories = policeApi.CrimeCategories(DateTime.Now);

                // Assert
                Assert.IsNotNull(categories);

                Category category = categories.First();
                CustomAssert.AreEqual(new Category()
                {
                    Url = "burglary",
                    Name = "Burglary"
                }, category, new CategoryEqualityComparer());
            }
        }

        [TestMethod]
        public void CrimeCategories_Call_Parses_Multiple_Elements_From_Json_Repsonse()
        {
            using (Stream stream = GetTestDataFromResource("PoliceUK.Tests.Unit.TestData.CrimeCatagories.Multiple.json"))
            {
                IPoliceUkClient policeApi = new PoliceUkClient()
                {
                    RequestFactory = CreateRequestFactory(stream)
                };
                IEnumerable<Category> categories = policeApi.CrimeCategories(DateTime.Now);

                // Assert
                Assert.IsNotNull(categories);
                Assert.AreEqual(2, categories.Count());

                Category category = categories.First();
                CustomAssert.AreEqual(new Category()
                {
                    Url = "all-crime",
                    Name = "All crime and ASB"
                }, category, new CategoryEqualityComparer());

                category = categories.Last();
                CustomAssert.AreEqual(new Category()
                {
                    Url = "burglary",
                    Name = "Burglary"
                }, category, new CategoryEqualityComparer());
            }
        }

        #endregion

        #region Street-level Crimes (Latitude/Longitude)

        [TestMethod]
        [ExpectedException(typeof(PoliceUk.Exceptions.InvalidDataException))]
        public void StreetLevelLatLng_Call_With_Malformed_Response_Throwns_InvalidDataException()
        {
            using (Stream stream = GetTestDataFromResource("PoliceUK.Tests.Unit.TestData.Malformed.json"))
            {
                PoliceUkClient policeApi = new PoliceUkClient()
                {
                    RequestFactory = CreateRequestFactory(stream)
                };

                policeApi.StreetLevelCrimes(A.Fake<IGeoposition>(), DateTime.Now);
            }
        }

        [TestMethod]
        public void StreetLevelLatLng_Call_Contains_Date_In_Request()
        {
            using (Stream stream = GetTestDataFromResource("PoliceUK.Tests.Unit.TestData.EmptyArray.json"))
            {
                PoliceUkClient policeApi = new PoliceUkClient()
                {
                    RequestFactory = CreateRequestFactory(stream)
                };

                DateTime nowDateTime = DateTime.Now;
                string formattedDateTime = nowDateTime.ToString("yyyy'-'MM");

                IEnumerable<Crime> crimes = policeApi.StreetLevelCrimes(A.Fake<IGeoposition>(), nowDateTime);

                // Assert
                IHttpWebRequestFactory factory = policeApi.RequestFactory;
                A.CallTo(() => factory.Create(A<string>.That.Contains(formattedDateTime))).MustHaveHappened();
            }
        }

        [TestMethod]
        public void StreetLevelLatLng_Call_Contains_LatLng_In_Request()
        {
            using (Stream stream = GetTestDataFromResource("PoliceUK.Tests.Unit.TestData.EmptyArray.json"))
            {
                PoliceUkClient policeApi = new PoliceUkClient()
                {
                    RequestFactory = CreateRequestFactory(stream)
                };

                var geoPosition = new Geoposition(123, 456);

                IEnumerable<Crime> crimes = policeApi.StreetLevelCrimes(geoPosition);

                // Assert
                IHttpWebRequestFactory factory = policeApi.RequestFactory;
                A.CallTo(() => factory.Create(A<string>.That.Contains(geoPosition.Latitiude.ToString()))).MustHaveHappened();
                A.CallTo(() => factory.Create(A<string>.That.Contains(geoPosition.Longitude.ToString()))).MustHaveHappened();
            }
        }

        [TestMethod]
        public void StreetLevelLatLng_Call_Parses_No_Elements_From_Json_Repsonse()
        {
            using (Stream stream = GetTestDataFromResource("PoliceUK.Tests.Unit.TestData.EmptyArray.json"))
            {
                IPoliceUkClient policeApi = new PoliceUkClient()
                {
                    RequestFactory = CreateRequestFactory(stream)
                };

                IEnumerable<Crime> crimes = policeApi.StreetLevelCrimes(A.Fake<IGeoposition>());

                // Assert
                Assert.IsNotNull(crimes);
                Assert.AreEqual(0, crimes.Count());
            }
        }

        [TestMethod]
        public void StreetLevelLatLng_Call_Parses_Single_Element_From_Json_Repsonse()
        {
            using (Stream stream = GetTestDataFromResource("PoliceUK.Tests.Unit.TestData.StreetLevelCrimes.Single.json"))
            {
                IPoliceUkClient policeApi = new PoliceUkClient()
                {
                    RequestFactory = CreateRequestFactory(stream)
                };
                IEnumerable<Crime> crimes = policeApi.StreetLevelCrimes(A.Fake<IGeoposition>());

                // Assert
                Assert.IsNotNull(crimes);

                Crime crime = crimes.First();
                CustomAssert.AreEqual(new Crime()
                {
                    Category = "burglary",
                    PersistentId = "aebd220e869a235ba92cde43f7e0df29001573b3df1b094bb952820b2b8f44b0",
                    LocationType = "Force",
                    LocationSubtype = "",
                    Id = "20604632",
                    Location = new CrimeLocation(){
                        Latitude = 52.6271606,
                        Longitude = -1.1485111,
                        Street = new Street(){
                            Id = 882208,
                            Name = "On or near Norman Street"
                        }
                    },
                    Context = "Example context",
                    Month = "2013-01",
                    OutcomeStatus = new OutcomeStatus() {
                        Category = "Under investigation",
                        Date = "2013-01"
                    }
                }, crime, new CrimeEqualityComparer());
            }
        }

        [TestMethod]
        public void StreetLevelLatLng_Call_Parses_Multiple_Elements_From_Json_Repsonse()
        {
            using (Stream stream = GetTestDataFromResource("PoliceUK.Tests.Unit.TestData.StreetLevelCrimes.Multiple.json"))
            {
                IPoliceUkClient policeApi = new PoliceUkClient()
                {
                    RequestFactory = CreateRequestFactory(stream)
                };
                IEnumerable<Crime> crimes = policeApi.StreetLevelCrimes(A.Fake<IGeoposition>());

                // Assert
                Assert.IsNotNull(crimes);
                Assert.AreEqual(2, crimes.Count());

                Crime crime = crimes.First();
                CustomAssert.AreEqual(new Crime()
                {
                    Category = "anti-social-behaviour",
                    PersistentId = "",
                    LocationType = "Force",
                    LocationSubtype = "",
                    Id = "20599642",
                    Location = new CrimeLocation()
                    {
                        Latitude = 52.6269479,
                        Longitude = -1.1121716,
                        Street = new Street()
                        {
                            Id = 882380,
                            Name = "On or near Cedar Road"
                        }
                    },
                    Context = "",
                    Month = "2013-01",
                    OutcomeStatus = null
                }, crime, new CrimeEqualityComparer());

                crime = crimes.Last();
                CustomAssert.AreEqual(new Crime()
                {
                    Category = "burglary",
                    PersistentId = "aebd220e869a235ba92cde43f7e0df29001573b3df1b094bb952820b2b8f44b0",
                    LocationType = "Force",
                    LocationSubtype = "",
                    Id = "20604632",
                    Location = new CrimeLocation()
                    {
                        Latitude = 52.6271606,
                        Longitude = -1.1485111,
                        Street = new Street()
                        {
                            Id = 882208,
                            Name = "On or near Norman Street"
                        }
                    },
                    Context = "Example context",
                    Month = "2013-01",
                    OutcomeStatus = new OutcomeStatus()
                    {
                        Category = "Under investigation",
                        Date = "2013-01"
                    }
                }, crime, new CrimeEqualityComparer());
            }
        }
        
        #endregion

        #region Forces

        [TestMethod]
        [ExpectedException(typeof(PoliceUk.Exceptions.InvalidDataException))]
        public void Forces_Call_With_Malformed_Response_Throwns_InvalidDataException()
        {
            using (Stream stream = GetTestDataFromResource("PoliceUK.Tests.Unit.TestData.Malformed.json"))
            {
                PoliceUkClient policeApi = new PoliceUkClient()
                {
                    RequestFactory = CreateRequestFactory(stream)
                };

                policeApi.Forces();
            }
        }

        [TestMethod]
        public void Forces_Call_Parses_No_Elements_From_Json_Repsonse()
        {
            using (Stream stream = GetTestDataFromResource("PoliceUK.Tests.Unit.TestData.EmptyArray.json"))
            {
                IPoliceUkClient policeApi = new PoliceUkClient()
                {
                    RequestFactory = CreateRequestFactory(stream)
                };

                IEnumerable<ForceSummary> forces = policeApi.Forces();

                // Assert
                Assert.IsNotNull(forces);
                Assert.AreEqual(0, forces.Count());
            }
        }

        [TestMethod]
        public void Forces_Call_Parses_Single_Element_From_Json_Repsonse()
        {
            using (Stream stream = GetTestDataFromResource("PoliceUK.Tests.Unit.TestData.Forces.Single.json"))
            {
                IPoliceUkClient policeApi = new PoliceUkClient()
                {
                    RequestFactory = CreateRequestFactory(stream)
                };
                IEnumerable<ForceSummary> forces = policeApi.Forces();

                // Assert
                Assert.IsNotNull(forces);

                ForceSummary force = forces.First();
                CustomAssert.AreEqual(new ForceSummary()
                {
                    Id = "avon-and-somerset",
                    Name = "Avon and Somerset Constabulary"
                }, force, new ForceSummaryEqualityComparer());
            }
        }

        [TestMethod]
        public void Forces_Call_Parses_Multiple_Elements_From_Json_Repsonse()
        {
            using (Stream stream = GetTestDataFromResource("PoliceUK.Tests.Unit.TestData.Forces.Multiple.json"))
            {
                IPoliceUkClient policeApi = new PoliceUkClient()
                {
                    RequestFactory = CreateRequestFactory(stream)
                };
                IEnumerable<ForceSummary> forces = policeApi.Forces();

                // Assert
                Assert.IsNotNull(forces);
                Assert.AreEqual(2, forces.Count());

                ForceSummary force = forces.First();
                CustomAssert.AreEqual(new ForceSummary()
                {
                    Id = "avon-and-somerset",
                    Name = "Avon and Somerset Constabulary"
                }, force, new ForceSummaryEqualityComparer());

                force = forces.Last();
                CustomAssert.AreEqual(new ForceSummary()
                {
                    Id = "bedfordshire",
                    Name = "Bedfordshire Police"
                }, force, new ForceSummaryEqualityComparer());
            }
        }

        #endregion

        #region Force

        [TestMethod]
        [ExpectedException(typeof(PoliceUk.Exceptions.InvalidDataException))]
        public void Force_Call_With_Malformed_Response_Throwns_InvalidDataException()
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
        public void Forces_Call_Parses_Single_Element_From_Json_Repsonse()
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

        [TestMethod]
        public void Forces_Call_Parses_Multiple_Elements_From_Json_Repsonse()
        {
            using (Stream stream = GetTestDataFromResource("PoliceUK.Tests.Unit.TestData.Forces.Multiple.json"))
            {
                IPoliceUkClient policeApi = new PoliceUkClient()
                {
                    RequestFactory = CreateRequestFactory(stream)
                };
                IEnumerable<ForceSummary> forces = policeApi.Forces();

                // Assert
                Assert.IsNotNull(forces);
                Assert.AreEqual(2, forces.Count());

                ForceSummary force = forces.First();
                CustomAssert.AreEqual(new ForceSummary()
                {
                    Id = "avon-and-somerset",
                    Name = "Avon and Somerset Constabulary"
                }, force, new ForceSummaryEqualityComparer());

                force = forces.Last();
                CustomAssert.AreEqual(new ForceSummary()
                {
                    Id = "bedfordshire",
                    Name = "Bedfordshire Police"
                }, force, new ForceSummaryEqualityComparer());
            }
        }

        #endregion
    }
}
