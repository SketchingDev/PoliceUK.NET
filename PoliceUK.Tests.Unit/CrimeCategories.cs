namespace PoliceUK.Tests.Unit
{
    using FakeItEasy;
    using NUnit.Framework;
    using PoliceUk;
    using PoliceUk.Entities;
    using PoliceUk.Request;
    using PoliceUk.Tests.Unit;
    using PoliceUK.Tests.Unit.CustomAssertions;
    using PoliceUK.Tests.Unit.CustomAssertions.Equality;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    [TestFixture]
    public class CrimeCategories : BaseMethodTests
    {
        private static readonly Category TestCategory = 
            new Category()
                {
                    Url = "burglary",
                    Name = "Burglary"
                };

        [Test]
        [ExpectedException(typeof(PoliceUk.Exceptions.InvalidDataException))]
        public void Call_With_Malformed_Response_Throwns_InvalidDataException()
        {
            using (Stream stream = GetTestDataFromResource(MalformedTestDataResource))
            {
                PoliceUkClient policeApi = new PoliceUkClient()
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

        [Test]
        public void Call_Parses_No_Elements_From_Json_Repsonse()
        {
            using (Stream stream = GetTestDataFromResource(EmptyArrayTestDataResource))
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

        [Test]
        public void Call_Parses_Single_Element_From_Json_Repsonse()
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
                CustomAssert.AreEqual(TestCategory, category, new CategoryEqualityComparer());
            }
        }

        [Test]
        public void Call_Parses_Multiple_Elements_From_Json_Repsonse()
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
                CustomAssert.AreEqual(TestCategory, category, new CategoryEqualityComparer());
            }
        }
    }
}
