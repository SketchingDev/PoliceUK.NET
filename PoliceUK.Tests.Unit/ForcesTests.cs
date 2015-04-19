namespace PoliceUk.Tests.Unit
{
    using CustomAssertions;
    using CustomAssertions.Equality;
    using Entities.Force;
    using NUnit.Framework;
    using PoliceUk;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    [TestFixture]
    public class ForcesTests : BaseMethodTests
    {
        #region Dummy data

        private static readonly ForceSummary ForceSummaryOne = new ForceSummary
            {
                Id = "avon-and-somerset",
                Name = "Avon and Somerset Constabulary"
            };

        private static readonly ForceSummary ForceSummaryTwo = new ForceSummary
            {
                Id = "bedfordshire",
                Name = "Bedfordshire Police"
            };

        #endregion

        private static readonly object[] NoForceSummary = 
            {
                new object[]
                {
                    EmptyArrayTestDataResource, 
                    new ForceSummary[]{}
                }
            };

        private static readonly object[] DummyForceSummary = 
            {
                new object[]
                {
                    "PoliceUK.Tests.Unit.TestData.Forces.Single.json", 
                    new ForceSummary[]
                    {
                        ForceSummaryOne
                    }
                }
            };

        private static readonly object[] DummyForceSummeries = 
            {
                new object[]
                {
                    "PoliceUK.Tests.Unit.TestData.Forces.Multiple.json", 
                    new ForceSummary[]
                    {
                        ForceSummaryOne, 
                        ForceSummaryTwo
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

                policeApi.Forces();
            }
        }

        [Test, TestCaseSource("NoForceSummary"), TestCaseSource("DummyForceSummary"), TestCaseSource("DummyForceSummeries")]
        public void Call_Parses_Elements_From_Json_Repsonse(string jsonResourceName, ForceSummary[] expectedForceSummaries)
        {
            using (Stream stream = GetTestDataFromResource(jsonResourceName))
            {
                var policeApi = new PoliceUkClient
                {
                    RequestFactory = CreateRequestFactory(stream)
                };
                IEnumerable<ForceSummary> forces = policeApi.Forces();

                // Assert
                Assert.That(forces, Is.Not.Null.And.Length.EqualTo(expectedForceSummaries.Length));

                int total = forces.Count();
                for (int i = 0; i < total; i++)
                {
                    ForceSummary expected = expectedForceSummaries[i];
                    ForceSummary actual = forces.ElementAtOrDefault(i);

                    CustomAssert.AreEqual(expected, actual, new ForceSummaryEqualityComparer());
                }
            }
        }
    }
}
