using PoliceUk.Tests.Unit;

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
    using System;

    [TestFixture]
    public class NeighbourhoodBoundaryTests : BaseMethodTests
    {
        #region Dummy data

        private static readonly Geoposition GeoPostion1 = new Geoposition(52.6288723701, -1.2054650828);

        private static readonly Geoposition GeoPostion2 = new Geoposition(52.6289334321, -1.2029954288);

        private static readonly Geoposition GeoPostion3 = new Geoposition(52.6289644676, -1.2026124939);

        private static readonly Geoposition GeoPostion4 = new Geoposition(52.6288723701, -1.2054650828);

        #endregion

        private static readonly object[] NoGeoPositions =
        {
            new object[]
            {
                EmptyArrayTestDataResource,
                new Geoposition[] {}
            }
        };

        private static readonly object[] DummyNeighbourhoodBoundary =
        {
            new object[]
            {
                "PoliceUK.Tests.Unit.TestData.NeighbourhoodBoundary.Multiple.json",
                new[]
                {
                    GeoPostion1,
                    GeoPostion2,
                    GeoPostion3,
                    GeoPostion4
                }
            }
        };

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Call_With_Null_ForceId_Throws_ArgumentNullException()
        {
            (new PoliceUkClient()).NeighbourhoodBoundary(null, "ABC");
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Call_With_Null_Id_Throws_ArgumentNullException()
        {
            (new PoliceUkClient()).NeighbourhoodBoundary("ABC", null);
        }

        [Test]
        [ExpectedException(typeof(Exceptions.InvalidDataException))]
        public void Call_With_Malformed_Response_Throws_InvalidDataException()
        {
            using (Stream stream = GetTestDataFromResource(MalformedTestDataResource))
            {
                var policeApi = new PoliceUkClient
                {
                    RequestFactory = CreateRequestFactory(stream)
                };

                policeApi.NeighbourhoodBoundary("", "");
            }
        }

        [Test, TestCaseSource("NoGeoPositions"), TestCaseSource("DummyNeighbourhoodBoundary")]
        public void Call_Parses_Elements_From_Json_Repsonse(string jsonResourceName,
            Geoposition[] expectedGeopositions)
        {
            using (Stream stream = GetTestDataFromResource(jsonResourceName))
            {
                var policeApi = new PoliceUkClient
                {
                    RequestFactory = CreateRequestFactory(stream)
                };
                IEnumerable<Geoposition> availableGeopositions = policeApi.NeighbourhoodBoundary("", "");

                // Assert
                Assert.That(availableGeopositions, Is.Not.Null.And.Length.EqualTo(expectedGeopositions.Length));

                int total = availableGeopositions.Count();
                for (int i = 0; i < total; i++)
                {
                    Geoposition expected = expectedGeopositions[i];
                    Geoposition actual = availableGeopositions.ElementAtOrDefault(i);

                    CustomAssert.AreEqual(expected, actual, new GeopositionEqualityComparer());
                }
            }
        }
    }
}
