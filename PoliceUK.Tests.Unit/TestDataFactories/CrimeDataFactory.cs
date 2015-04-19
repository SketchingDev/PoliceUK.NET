namespace PoliceUk.Tests.Unit.TestDataFactories
{
    using Entities;
    using Entities.Location;
    using Entities.StreetLevel;

    public class CrimeDataFactory
    {
        #region Dummy data

        protected static readonly Crime DummyCrimeOne = new Crime
        {
            Category = "anti-social-behaviour",
            PersistentId = "",
            LocationType = "Force",
            LocationSubtype = "",
            Id = "20599642",
            Location = new CrimeLocation
            {
                Latitude = 52.6269479,
                Longitude = -1.1121716,
                Street = new Street
                {
                    Id = 882380,
                    Name = "On or near Cedar Road"
                }
            },
            Context = "",
            Month = "2013-01",
            OutcomeStatus = null
        };

        protected static readonly Crime DummyCrimeTwo = new Crime
        {
            Category = "burglary",
            PersistentId = "aebd220e869a235ba92cde43f7e0df29001573b3df1b094bb952820b2b8f44b0",
            LocationType = "Force",
            LocationSubtype = "",
            Id = "20604632",
            Location = new CrimeLocation
            {
                Latitude = 52.6271606,
                Longitude = -1.1485111,
                Street = new Street
                {
                    Id = 882208,
                    Name = "On or near Norman Street"
                }
            },
            Context = "Example context",
            Month = "2013-01",
            OutcomeStatus = new OutcomeStatus
            {
                Category = "Under investigation",
                Date = "2013-01"
            }
        };

        #endregion

        protected static readonly object[] NoCrime = 
        {
            new object[]
            {
                "PoliceUK.Tests.Unit.TestData.EmptyArray.json", 
                new Crime[]{}
            }
        };

        protected static readonly object[] DummyCrime = 
        {
            new object[]
            {
                "PoliceUK.Tests.Unit.TestData.Crimes.Single.json", 
                new Crime[]
                {
                    DummyCrimeOne
                }
            }
        };

        protected static readonly object[] DummyCrimes = 
        {
            new object[]
            {
                "PoliceUK.Tests.Unit.TestData.Crimes.Multiple.json", 
                new Crime[]
                {
                    DummyCrimeOne, 
                    DummyCrimeTwo
                }
            }
        };
    }
}
