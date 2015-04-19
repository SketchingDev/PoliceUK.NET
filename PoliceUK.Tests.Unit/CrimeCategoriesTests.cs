namespace PoliceUk.Tests.Unit
{
    using FakeItEasy;
    using NUnit.Framework;
    using PoliceUk;
    using Entities;
    using Request;
    using CustomAssertions;
    using CustomAssertions.Equality;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    [TestFixture]
    public class CrimeCategoriesTests : BaseMethodTests
    {
        #region Dummy data

        private static readonly Category CategoryOne = 
            new Category
            {
                Url = "burglary",
                Name = "Burglary"
            };

        private static readonly Category CategoryTwo =
            new Category
            {
                Url = "all-crime",
                Name = "All crime and ASB"
            };

        #endregion

        private static readonly object[] NoCategory = 
            {
                new object[]
                {
                    EmptyArrayTestDataResource, 
                    new Category[]{}
                }
            };

        private static readonly object[] DummyCategory = 
            {
                new object[]
                {
                    "PoliceUK.Tests.Unit.TestData.CrimeCatagories.Single.json", 
                    new Category[]
                    {
                        CategoryOne
                    }
                }
            };

        private static readonly object[] DummyCategories = 
            {
                new object[]
                {
                    "PoliceUK.Tests.Unit.TestData.CrimeCatagories.Multiple.json", 
                    new Category[]
                    {
                        CategoryOne, 
                        CategoryTwo
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

                policeApi.CrimeCategories(DateTime.Now);
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

                policeApi.CrimeCategories(nowDateTime);

                // Assert
                IHttpWebRequestFactory factory = policeApi.RequestFactory;
                A.CallTo(() => factory.Create(A<string>.That.Contains(formattedDateTime))).MustHaveHappened();
            }
        }

        [Test, TestCaseSource("NoCategory"), TestCaseSource("DummyCategory"), TestCaseSource("DummyCategories")]
        public void Call_Parses_Elements_From_Json_Repsonse(string jsonResourceName, Category[] expectedCategorys)
        {
            using (Stream stream = GetTestDataFromResource(jsonResourceName))
            {
                var policeApi = new PoliceUkClient
                {
                    RequestFactory = CreateRequestFactory(stream)
                };
                IEnumerable<Category> categories = policeApi.CrimeCategories(DateTime.Now);

                // Assert
                Assert.That(categories, Is.Not.Null.And.Length.EqualTo(expectedCategorys.Length));

                int total = categories.Count();
                for (int i = 0; i < total; i++)
                {
                    Category expected = expectedCategorys[i];
                    Category actual   = categories.ElementAtOrDefault(i);

                    CustomAssert.AreEqual(expected, actual, new CategoryEqualityComparer());
                }
            }
        }
    }
}
