namespace PoliceUK.Tests.Unit
{
    using CustomAssertions;
    using CustomAssertions.Equality;
    using Entities.Force;
    using NUnit.Framework;
    using PoliceUk;
    using PoliceUk.Tests.Unit;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    [TestFixture]
    public class Forces : BaseMethodTests
    {
        private static readonly ForceSummary TestSummary = new ForceSummary
                {
                    Id = "avon-and-somerset",
                    Name = "Avon and Somerset Constabulary"
                };

        [Test]
        [ExpectedException(typeof(PoliceUk.Exceptions.InvalidDataException))]
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

        [Test]
        public void Call_Parses_No_Elements_From_Json_Repsonse()
        {
            using (Stream stream = GetTestDataFromResource(EmptyArrayTestDataResource))
            {
                var policeApi = new PoliceUkClient
                {
                    RequestFactory = CreateRequestFactory(stream)
                };

                IEnumerable<ForceSummary> forces = policeApi.Forces();

                // Assert
                Assert.IsNotNull(forces);
                Assert.AreEqual(0, forces.Count());
            }
        }

        [Test]
        public void Call_Parses_Single_Element_From_Json_Repsonse()
        {
            using (Stream stream = GetTestDataFromResource("PoliceUK.Tests.Unit.TestData.Forces.Single.json"))
            {
                var policeApi = new PoliceUkClient
                {
                    RequestFactory = CreateRequestFactory(stream)
                };
                IEnumerable<ForceSummary> forces = policeApi.Forces();

                // Assert
                Assert.IsNotNull(forces);

                ForceSummary force = forces.First();
                CustomAssert.AreEqual(TestSummary, force, new ForceSummaryEqualityComparer());
            }
        }

        [Test]
        public void Call_Parses_Multiple_Elements_From_Json_Repsonse()
        {
            using (Stream stream = GetTestDataFromResource("PoliceUK.Tests.Unit.TestData.Forces.Multiple.json"))
            {
                var policeApi = new PoliceUkClient
                {
                    RequestFactory = CreateRequestFactory(stream)
                };
                IEnumerable<ForceSummary> forces = policeApi.Forces();

                // Assert
                Assert.IsNotNull(forces);
                Assert.AreEqual(2, forces.Count());

                ForceSummary force = forces.First();
                CustomAssert.AreEqual(TestSummary, force, new ForceSummaryEqualityComparer());

                force = forces.Last();
                CustomAssert.AreEqual(new ForceSummary
                {
                    Id = "bedfordshire",
                    Name = "Bedfordshire Police"
                }, force, new ForceSummaryEqualityComparer());
            }
        }
    }
}
