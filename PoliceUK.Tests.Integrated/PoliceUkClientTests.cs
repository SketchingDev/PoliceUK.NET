namespace PoliceUK.Tests.Integrated
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PoliceUk;
    using PoliceUk.Entities;
    using PoliceUK.Entities;
    using PoliceUK.Entities.Force;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    [TestClass]
    public class PoliceUkClientTests
    {
        [TestMethod]
        public void CrimeCategories_Call_Returns_Results()
        {
            PoliceUkClient policeApi = new PoliceUkClient();

            IEnumerable<Category> categories = policeApi.CrimeCategories(DateTime.Now);

            Assert.IsNotNull(categories);
            Assert.AreEqual(true, categories.Count() > 0);

            Category category = categories.First();
            Assert.IsNotNull(category.Url);
        }

        [TestMethod]
        public void StreetLevelCrimes_Point_Call_Returns_Results()
        {
            PoliceUkClient policeApi = new PoliceUkClient();

            StreetLevelCrimeResults result = policeApi.StreetLevelCrimes(new Geoposition(51.513016, -0.10231));

            Assert.IsNotNull(result);
            Assert.IsFalse(result.TooManyCrimesOrError);

            Assert.IsNotNull(result.Crimes);
            Assert.AreEqual(true, result.Crimes.Count() > 0);

            Crime crime = result.Crimes.First();
            Assert.IsNotNull(crime.Id);
        }

        [TestMethod]
        public void StreetLevelCrimes_Polygon_Call_Returns_Results()
        {
            PoliceUkClient policeApi = new PoliceUkClient();

            var polygon = new Geoposition[4] {
                new Geoposition(50.181460, -5.419521),
                new Geoposition(50.193935,-5.394716),
                new Geoposition(50.196353,-5.402184),
                new Geoposition(50.184263,-5.424070)
            };

            StreetLevelCrimeResults result = policeApi.StreetLevelCrimes(polygon);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.TooManyCrimesOrError);

            Assert.IsNotNull(result.Crimes);
            Assert.AreEqual(true, result.Crimes.Count() > 0);

            Crime crime = result.Crimes.First();
            Assert.IsNotNull(crime.Id);
        }

        [TestMethod]
        public void Forces_Call_Returns_Results()
        {
            PoliceUkClient policeApi = new PoliceUkClient();

            IEnumerable<ForceSummary> forces = policeApi.Forces();

            Assert.IsNotNull(forces);
            Assert.AreEqual(true, forces.Count() > 0);

            ForceSummary force = forces.First();
            Assert.IsNotNull(force.Id);
        }

        [TestMethod]
        public void Force_Call_Returns_Result()
        {
            PoliceUkClient policeApi = new PoliceUkClient();

            ForceDetails force = policeApi.Force("leicestershire");

            Assert.IsNotNull(force);
            Assert.IsNotNull(force.Id);
        }

        [TestMethod]
        public void Force_Call_With_No_Result_Returns_Null()
        {
            PoliceUkClient policeApi = new PoliceUkClient();

            ForceDetails force = policeApi.Force("");

            Assert.IsNull(force);
        }
    }
}
