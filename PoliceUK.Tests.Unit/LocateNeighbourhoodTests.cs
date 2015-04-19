namespace PoliceUk.Tests.Unit
{
    using CustomAssertions;
    using NUnit.Framework;
    using PoliceUk;
    using System.IO;
    using CustomAssertions.Equality;
    using Entities.Neighbourhood;
    using System;

    [TestFixture]
    public class LocateNeighbourhoodTests : BaseMethodTests
    {
        private static readonly object[] DummyNeighbourhoodForce =
        {
            new object[]
            {
                "PoliceUK.Tests.Unit.TestData.LocateNeighbourhood.json",
                new NeighbourhoodForce
                {
                    ForceId = "metropolitan",
                    NeighbourhoodId = "00BKX6"
                }                
            }
        };

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Call_With_Null_Position_Throws_ArgumentNullException()
        {
            (new PoliceUkClient()).LocateNeighbourhood(null);
        }

        [Test]
        public void Call_Parses_No_Element_From_Json_Repsonse()
        {
            using (Stream stream = GetTestDataFromResource("PoliceUK.Tests.Unit.TestData.NotFound.txt"))
            {
                var policeApi = new PoliceUkClient
                {
                    RequestFactory = CreateRequestFactory(stream, System.Net.HttpStatusCode.NotFound)
                };

                NeighbourhoodForce neighbourhoodForce = policeApi.LocateNeighbourhood(new Geoposition(0,0));

                // Assert
                Assert.IsNull(neighbourhoodForce);
            }
        }

        [Test, TestCaseSource("DummyNeighbourhoodForce")]
        public void Call_Parses_Element_From_Json_Repsonse(string jsonResourceName, NeighbourhoodForce expectedNeighbourhoodForce)
        {
            using (Stream stream = GetTestDataFromResource(jsonResourceName))
            {
                var policeApi = new PoliceUkClient
                {
                    RequestFactory = CreateRequestFactory(stream)
                };
                NeighbourhoodForce neighbourhoodForce = policeApi.LocateNeighbourhood(new Geoposition(0, 0));

                // Assert
                Assert.IsNotNull(neighbourhoodForce);

                CustomAssert.AreEqual(expectedNeighbourhoodForce, neighbourhoodForce, new NeighbourhoodForceEqualityComparer());
            }
        }
    }
}
