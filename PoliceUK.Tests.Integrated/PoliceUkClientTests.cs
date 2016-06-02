namespace PoliceUk.Tests.Integrated
{
    using Entities;
    using Entities.Force;
    using Entities.StreetLevel;
    using NUnit.Framework;
    using PoliceUk;
    using Entities.Neighbourhood;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    [TestFixture]
    public class PoliceUkClientTests
    {
        private const string ForceId = "metropolitan";

        private const string NeighbourhoodId = "E05000029"; //Chadwell Heath

        private const string LocationId = "883456";

        #region CrimeCategories

        [Test]
        public void CrimeCategories_Call_Returns_Results()
        {
            var policeApi = new PoliceUkClient();

            IEnumerable<Category> categories = policeApi.CrimeCategories(DateTime.Now);

            // Assert
            Assert.That(categories, Is.Not.Null.And.Not.Empty);
            Assert.IsNotNull(categories.First().Url);
        }

        #endregion

        #region StreetLevelCrimes

        [Test]
        public void StreetLevelCrimes_Point_Call_Returns_Results()
        {
            var policeApi = new PoliceUkClient();

            StreetLevelCrimeResults result = policeApi.StreetLevelCrimes(new Geoposition(51.513016, -0.10231));

            // Assert
            Assert.IsNotNull(result);

            Assert.IsFalse(result.TooManyCrimesOrError);
            Assert.That(result.Crimes, Is.Not.Null.And.Not.Empty);

            Crime crime = result.Crimes.First();
            Assert.IsNotNull(crime.Id);
        }

        [Test]
        public void StreetLevelCrimes_Polygon_Call_Returns_Results()
        {
            var policeApi = new PoliceUkClient();

            var polygon = new[] {
                new Geoposition(50.181460, -5.419521),
                new Geoposition(50.193935,-5.394716),
                new Geoposition(50.196353,-5.402184),
                new Geoposition(50.184263,-5.424070)
            };

            StreetLevelCrimeResults result = policeApi.StreetLevelCrimes(polygon);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.TooManyCrimesOrError);

            Assert.That(result.Crimes, Is.Not.Null.And.Not.Empty);

            Crime crime = result.Crimes.First();
            Assert.IsNotNull(crime.Id);
        }

        #endregion

        #region Forces

        [Test]
        public void Forces_Call_Returns_Results()
        {
            var policeApi = new PoliceUkClient();

            IEnumerable<ForceSummary> forces = policeApi.Forces();

            // Assert
            Assert.That(forces, Is.Not.Null.And.Not.Empty);

            ForceSummary force = forces.First();
            Assert.IsNotNull(force.Id);
        }

        #endregion

        #region Force

        [Test]
        public void Force_Call_Returns_Result()
        {
            var policeApi = new PoliceUkClient();

            ForceDetails force = policeApi.Force(ForceId);

            // Assert
            Assert.IsNotNull(force);
            Assert.IsNotNull(force.Id);
        }

        [Test]
        public void Force_Call_With_No_Result_Returns_Null()
        {
            var policeApi = new PoliceUkClient();

            ForceDetails force = policeApi.Force("");

            // Assert
            Assert.IsNull(force);
        }

        #endregion

        #region StreetLevelAvailability

        [Test]
        public void StreetLevelAvailability_Call_Returns_Results()
        {
            var policeApi = new PoliceUkClient();

            IEnumerable<DateTime> availableCrimeDates 
                = policeApi.StreetLevelAvailability();

            // Assert
            Assert.That(availableCrimeDates, Is.Not.Null.And.Not.Empty);
        }

        #endregion

        #region Neighbourhoods

        [Test]
        public void Neighbourhoods_Call_Returns_Result()
        {
            var policeApi = new PoliceUkClient();

            IEnumerable<NeighbourhoodSummary> summeries
                = policeApi.Neighbourhoods(ForceId);

            // Assert
            Assert.That(summeries, Is.Not.Null.And.Not.Empty);
        }

        #endregion

        #region Neighbourhood

        [Test]
        public void Neighbourhood_Call_Returns_Result()
        {
            var policeApi = new PoliceUkClient();

            NeighbourhoodDetails neighbourhood = policeApi.Neighbourhood(ForceId, NeighbourhoodId);

            // Assert
            Assert.IsNotNull(neighbourhood);
            Assert.IsNotNull(neighbourhood.Id);
        }

        [Test]
        public void Neighbourhood_Call_With_No_Force_Returns_Null()
        {
            var policeApi = new PoliceUkClient();

            NeighbourhoodDetails neighbourhood = policeApi.Neighbourhood("", NeighbourhoodId);

            // Assert
            Assert.IsNull(neighbourhood);
        }

        [Test]
        public void Neighbourhood_Call_With_No_ID_Returns_Null()
        {
            var policeApi = new PoliceUkClient();

            NeighbourhoodDetails neighbourhood = policeApi.Neighbourhood("", NeighbourhoodId);

            // Assert
            Assert.IsNull(neighbourhood);
        }

        #endregion

        #region NeighbourhoodBoundary

        [Test]
        public void NeighbourhoodBoundary_Call_Returns_Result()
        {
            var policeApi = new PoliceUkClient();

            IEnumerable<Geoposition> positions = policeApi.NeighbourhoodBoundary(ForceId, NeighbourhoodId);

            // Assert
            Assert.That(positions, Is.Not.Null.And.Not.Empty);
        }

        [Test]
        public void NeighbourhoodBoundary_Call_With_No_Force_Returns_Null()
        {
            var policeApi = new PoliceUkClient();

            IEnumerable<Geoposition> positions = policeApi.NeighbourhoodBoundary("", NeighbourhoodId);

            // Assert
            Assert.IsNull(positions);
        }

        [Test]
        public void NeighbourhoodBoundary_Call_With_No_ID_Returns_Null()
        {
            var policeApi = new PoliceUkClient();

            IEnumerable<Geoposition> positions = policeApi.NeighbourhoodBoundary("", NeighbourhoodId);

            // Assert
            Assert.IsNull(positions);
        }

        #endregion

        #region NeighbourhoodTeam

        [Test]
        public void NeighbourhoodTeam_Call_Returns_Result()
        {
            var policeApi = new PoliceUkClient();

            IEnumerable<NeighbourhoodTeamMember> members = policeApi.NeighbourhoodTeam(ForceId, NeighbourhoodId);

            // Assert
            Assert.That(members, Is.Not.Null.And.Not.Empty);
        }

        [Test]
        public void NeighbourhoodTeam_Call_With_No_Force_Returns_Null()
        {
            var policeApi = new PoliceUkClient();

            IEnumerable<NeighbourhoodTeamMember> members = policeApi.NeighbourhoodTeam("", NeighbourhoodId);

            // Assert
            Assert.IsNull(members);
        }

        [Test]
        public void NeighbourhoodTeam_Call_With_No_ID_Returns_Null()
        {
            var policeApi = new PoliceUkClient();

            IEnumerable<NeighbourhoodTeamMember> members = policeApi.NeighbourhoodTeam(ForceId, "");

            // Assert
            Assert.IsNull(members);
        }

        #endregion

        #region NeighbourhoodEvents

        [Test]
        public void NeighbourhoodEvents_Call_Returns_Result()
        {
            var policeApi = new PoliceUkClient();

            IEnumerable<NeighbourhoodEvent> events = policeApi.NeighbourhoodEvents(ForceId, NeighbourhoodId);

            // Assert
            Assert.That(events, Is.Not.Null.And.Not.Empty);
        }

        [Test]
        public void NeighbourhoodEvents_Call_With_No_Force_Returns_Null()
        {
            var policeApi = new PoliceUkClient();

            IEnumerable<NeighbourhoodEvent> events = policeApi.NeighbourhoodEvents("", NeighbourhoodId);

            // Assert
            Assert.IsNull(events);
        }

        [Test]
        public void NeighbourhoodEvents_Call_With_No_ID_Returns_Null()
        {
            var policeApi = new PoliceUkClient();

            IEnumerable<NeighbourhoodEvent> events = policeApi.NeighbourhoodEvents(ForceId, "");

            // Assert
            Assert.IsNull(events);
        }

        #endregion

        #region LocateNeighbourhood

        [Test]
        public void LocateNeighbourhood_Call_Returns_Null()
        {
            var policeApi = new PoliceUkClient();

            NeighbourhoodForce neighbourhoodForce
                = policeApi.LocateNeighbourhood(new Geoposition(42.3329154, 10.30963898));

            // Assert
            Assert.That(neighbourhoodForce, Is.Null);
        }

        [Test]
        public void LocateNeighbourhood_Call_Returns_Result()
        {
            var policeApi = new PoliceUkClient();

            NeighbourhoodForce neighbourhoodForce
                = policeApi.LocateNeighbourhood(new Geoposition(51.13317391, -0.98909205));

            // Assert
            Assert.That(neighbourhoodForce, Is.Not.Null);
        }

        #endregion

        #region Crimes
        
        [Test]
        public void Crimes_Call_Without_Date_Returns_Results()
        {
            var policeApi = new PoliceUkClient();

            IEnumerable<Crime> crimes = policeApi.Crimes(ForceId, "all-crime");

            // Assert
            Assert.That(crimes, Is.Not.Null.And.Not.Empty);

            Crime crime = crimes.First();
            Assert.IsNotNull(crime.Id);
        }

        [Test]
        public void Crimes_Call_With_Date_Returns_Results()
        {
            var policeApi = new PoliceUkClient();

            DateTime thisDateLastYear = DateTime.Now.Subtract(TimeSpan.FromDays(365));
            IEnumerable<Crime> crimes = policeApi.Crimes(ForceId, "all-crime", thisDateLastYear);

            // Assert
            Assert.That(crimes, Is.Not.Null.And.Not.Empty);

            Crime crime = crimes.First();
            Assert.IsNotNull(crime.Id);
        }

        #endregion

        #region CrimesAtLocation

        [Test]
        public void CrimesAtLocation_Call_With_LocationId_Returns_Results()
        {
            var policeApi = new PoliceUkClient();

            DateTime thisDateLastYear = DateTime.Now.Subtract(TimeSpan.FromDays(365));
            IEnumerable<Crime> crimes = policeApi.CrimesAtLocation(LocationId, thisDateLastYear);

            // Assert
            Assert.That(crimes, Is.Not.Null.And.Not.Empty);

            Crime crime = crimes.First();
            Assert.IsNotNull(crime.Id);
        }

        [Test]
        public void CrimesAtLocation_Call_With_GeoPosition_Returns_Results()
        {
            var policeApi = new PoliceUkClient();

            Geoposition position = new Geoposition(51.516674, -0.176933);
            DateTime thisDateLastYear = DateTime.Now.Subtract(TimeSpan.FromDays(365));

            IEnumerable<Crime> crimes = policeApi.CrimesAtLocation(position, thisDateLastYear);

            // Assert
            Assert.That(crimes, Is.Not.Null.And.Not.Empty);

            Crime crime = crimes.First();
            Assert.IsNotNull(crime.Id);
        }

        #endregion

        #region LastUpdated

        [Test]
        public void LastUpdated_Call_Returns_Result()
        {
            var policeApi = new PoliceUkClient();

            DateTime lastUpdated = policeApi.LastUpdated();

            // Assert
            Assert.That(lastUpdated, Is.Not.Null);
        }

        #endregion
    }
}
