namespace PoliceUK.Tests.Unit
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PoliceUk;
    using PoliceUk.Tests.Unit;
    using PoliceUK.Entities.Force;
    using PoliceUK.Tests.Unit.CustomAssertions;
    using PoliceUK.Tests.Unit.CustomAssertions.Equality;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    [TestClass]
    public class Forces : BaseMethodTests
    {
        [TestMethod]
        [ExpectedException(typeof(PoliceUk.Exceptions.InvalidDataException))]
        public void Call_With_Malformed_Response_Throwns_InvalidDataException()
        {
            using (Stream stream = GetTestDataFromResource(MalformedTestDataResource))
            {
                PoliceUkClient policeApi = new PoliceUkClient()
                {
                    RequestFactory = CreateRequestFactory(stream)
                };

                policeApi.Forces();
            }
        }

        [TestMethod]
        public void Call_Parses_No_Elements_From_Json_Repsonse()
        {
            using (Stream stream = GetTestDataFromResource(EmptyArrayTestDataResource))
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
        public void Call_Parses_Single_Element_From_Json_Repsonse()
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
        public void Call_Parses_Multiple_Elements_From_Json_Repsonse()
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
    }
}
