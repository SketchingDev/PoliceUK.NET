namespace PoliceUk.Tests.Unit
{
    using FakeItEasy;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PoliceUk.Entities;
    using PoliceUk.Request;
    using PoliceUK.Entities;
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
            var response = A.Fake<IWebResponse>();
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
        public void CrimeRategories_Call_Contains_Date_In_Request()
        {
            using (Stream stream = GetTestDataFromResource("PoliceUkApi.Tests.Unit.TestData.EmptyArray.json"))
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
            using (Stream stream = GetTestDataFromResource("PoliceUkApi.Tests.Unit.TestData.EmptyArray.json"))
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
            using (Stream stream = GetTestDataFromResource("PoliceUkApi.Tests.Unit.TestData.CrimeCatagories.Single.json"))
            {
                IPoliceUkClient policeApi = new PoliceUkClient()
                {
                    RequestFactory = CreateRequestFactory(stream)
                };
                IEnumerable<Category> categories = policeApi.CrimeCategories(DateTime.Now);

                // Assert
                Assert.IsNotNull(categories);

                Category category = categories.First();
                Assert.AreEqual(new Category()
                {
                    Url = "burglary",
                    Name = "Burglary"
                }, category);
            }
        }

        [TestMethod]
        public void CrimeCategories_Call_Parses_Multiple_Elements_From_Json_Repsonse()
        {
            using (Stream stream = GetTestDataFromResource("PoliceUkApi.Tests.Unit.TestData.CrimeCatagories.Multiple.json"))
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
                Assert.AreEqual(new Category()
                {
                    Url = "all-crime",
                    Name = "All crime and ASB"
                }, category);

                category = categories.Last();
                Assert.AreEqual(new Category()
                {
                    Url = "burglary",
                    Name = "Burglary"
                }, category);
            }
        }

        #endregion

        #region Street-level Crimes
        #endregion

        #region Forces

        [TestMethod]
        public void Forces_Call_Parses_No_Elements_From_Json_Repsonse()
        {
            using (Stream stream = GetTestDataFromResource("PoliceUkApi.Tests.Unit.TestData.EmptyArray.json"))
            {
                IPoliceUkClient policeApi = new PoliceUkClient()
                {
                    RequestFactory = CreateRequestFactory(stream)
                };

                IEnumerable<ForceShortDescription> forces = policeApi.Forces();

                // Assert
                Assert.IsNotNull(forces);
                Assert.AreEqual(0, forces.Count());
            }
        }

        [TestMethod]
        public void Forces_Call_Parses_Single_Element_From_Json_Repsonse()
        {
            using (Stream stream = GetTestDataFromResource("PoliceUkApi.Tests.Unit.TestData.Forces.Single.json"))
            {
                IPoliceUkClient policeApi = new PoliceUkClient()
                {
                    RequestFactory = CreateRequestFactory(stream)
                };
                IEnumerable<ForceShortDescription> forces = policeApi.Forces();

                // Assert
                Assert.IsNotNull(forces);

                ForceShortDescription force = forces.First();
                Assert.AreEqual(new ForceShortDescription()
                {
                    ID = "avon-and-somerset",
                    Name = "Avon and Somerset Constabulary"
                }, force);
            }
        }

        [TestMethod]
        public void Forces_Call_Parses_Multiple_Elements_From_Json_Repsonse()
        {
            using (Stream stream = GetTestDataFromResource("PoliceUkApi.Tests.Unit.TestData.Forces.Multiple.json"))
            {
                IPoliceUkClient policeApi = new PoliceUkClient()
                {
                    RequestFactory = CreateRequestFactory(stream)
                };
                IEnumerable<ForceShortDescription> forces = policeApi.Forces();

                // Assert
                Assert.IsNotNull(forces);
                Assert.AreEqual(2, forces.Count());

                ForceShortDescription force = forces.First();
                Assert.AreEqual(new ForceShortDescription()
                {
                    ID = "avon-and-somerset",
                    Name = "Avon and Somerset Constabulary"
                }, force);

                force = forces.Last();
                Assert.AreEqual(new ForceShortDescription()
                {
                    ID = "bedfordshire",
                    Name = "Bedfordshire Police"
                }, force);
            }
        }

        #endregion
    }
}
