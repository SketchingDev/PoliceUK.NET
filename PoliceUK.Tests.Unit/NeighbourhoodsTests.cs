namespace PoliceUk.Tests.Unit
{
    using CustomAssertions;
    using NUnit.Framework;
    using PoliceUk;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using CustomAssertions.Equality;
    using Entities.Neighbourhood;

    [TestFixture]
    public class NeighbourhoodsTests : BaseMethodTests
    {
        #region Dummy data

        private static readonly NeighbourhoodSummary NeighbourhoodOne = new NeighbourhoodSummary
        {
            Id = "C01",
            Name = "New Parks"
        };

        private static readonly NeighbourhoodSummary NeighbourhoodTwo = new NeighbourhoodSummary
        {
            Id = "C02",
            Name = "Abbey"
        };

        #endregion

        private static readonly object[] NoNeighbourhood =
        {
            new object[]
            {
                EmptyArrayTestDataResource,
                new NeighbourhoodSummary[] {}
            }
        };

        private static readonly object[] DummyNeighbourhood =
        {
            new object[]
            {
                "PoliceUK.Tests.Unit.TestData.Neighbourhoods.Single.json",
                new[]
                {
                    NeighbourhoodOne
                }
            }
        };

        private static readonly object[] DummyNeighbourhoods =
        {
            new object[]
            {
                "PoliceUK.Tests.Unit.TestData.Neighbourhoods.Multiple.json",
                new[]
                {
                    NeighbourhoodOne,
                    NeighbourhoodTwo
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

                policeApi.Neighbourhoods("");
            }
        }

        [Test, TestCaseSource("NoNeighbourhood"), TestCaseSource("DummyNeighbourhood"),
         TestCaseSource("DummyNeighbourhoods")]
        public void Call_Parses_Elements_From_Json_Repsonse(string jsonResourceName,
            NeighbourhoodSummary[] expectedNeighbourhoods)
        {
            using (Stream stream = GetTestDataFromResource(jsonResourceName))
            {
                var policeApi = new PoliceUkClient
                {
                    RequestFactory = CreateRequestFactory(stream)
                };
                IEnumerable<NeighbourhoodSummary> availableNeighbourhood = policeApi.Neighbourhoods("");

                // Assert
                Assert.That(availableNeighbourhood, Is.Not.Null.And.Length.EqualTo(expectedNeighbourhoods.Length));

                int total = availableNeighbourhood.Count();
                for (int i = 0; i < total; i++)
                {
                    NeighbourhoodSummary expected = expectedNeighbourhoods[i];
                    NeighbourhoodSummary actual = availableNeighbourhood.ElementAtOrDefault(i);

                    CustomAssert.AreEqual(expected, actual, new NeighbourhoodSummaryEqualityComparer());
                }
            }
        }
    }
}
