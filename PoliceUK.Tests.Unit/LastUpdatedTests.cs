namespace PoliceUk.Tests.Unit
{
    using NUnit.Framework;
    using PoliceUk;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    [TestFixture]
    public class LastUpdatedTests : BaseMethodTests
    {
        #region Dummy data

        private static readonly DateTime LastUpdatedOne = new DateTime(2015, 4, 1);

        #endregion

        private static readonly object[] DummyLastUpdated = 
            {
                new object[]
                {
                    "PoliceUK.Tests.Unit.TestData.LastUpdated.json", 
                    LastUpdatedOne
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

                policeApi.LastUpdated();
            }
        }

        [Test, TestCaseSource("DummyLastUpdated")]
        public void Call_Parses_Elements_From_Json_Repsonse(string jsonResourceName, DateTime expectedLastUpdated)
        {
            using (Stream stream = GetTestDataFromResource(jsonResourceName))
            {
                var policeApi = new PoliceUkClient
                {
                    RequestFactory = CreateRequestFactory(stream)
                };
                DateTime lastUpdated = policeApi.LastUpdated();

                // Assert
                Assert.That(lastUpdated, Is.Not.Null);

                Assert.AreEqual(expectedLastUpdated, lastUpdated);
            }
        }
    }
}
