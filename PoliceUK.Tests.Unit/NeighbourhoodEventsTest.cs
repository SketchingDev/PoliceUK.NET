using NUnit.Framework;
using PoliceUk;
using PoliceUk.Entities.Neighbourhood;
using PoliceUk.Tests.Unit;
using PoliceUk.Tests.Unit.CustomAssertions;
using PoliceUK.Tests.Unit.CustomAssertions.Equality.NeighbourhoodDetails;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PoliceUK.Tests.Unit
{
    [TestFixture]
    public class NeighbourhoodEventsTest : BaseMethodTests
    {
        private static readonly object[] DummyNeighbourhoodEvents =
        {
            new object[]
            {
                "PoliceUK.Tests.Unit.TestData.NeighbourhoodEvents.Multiple.json",
                new[]
                { 
                        new NeighbourhoodEvent
                        {
                            Title = "Aikman Avenue beat surgery",
                            Address = "Library, Aikman Avenue",
                            Type = "meeting",
                            Description = "desc1",
                            StartDate = new DateTime(2011,11,2,10,0,0),
                            ContactDetails = new ContactDetails
                                            {
                                                Mobile = "07884 117276",
                                                Email = "ChadwellHeath.snt@met.police.uk",
                                                Telephone = "020 8721 2902",
                                                Website = "http://www.example.com"
                                            }
                        } ,
                        new NeighbourhoodEvent
                        {
                            Title = "Bateman Road beat surgery",
                            Address = "Hand in Hand, Bateman Road",
                            Type = "meeting",
                            Description = "desc2",
                            StartDate = new DateTime(2011,11,2,18,30,0),
                            ContactDetails = new ContactDetails
                                            {
                                                Mobile = "07884 117276",
                                                Email = "ChadwellHeath.snt@met.police.uk",
                                                Telephone = "020 8721 2902",
                                                Website = "http://www.example.com"
                                            }
                        }            
                }
            }
        };

        [Test, TestCaseSource("DummyNeighbourhoodEvents")]
        public void Call_Parses_Elements_From_Json_Repsonse(string jsonResourceName,
            NeighbourhoodEvent[] expectedNeighbourhoodsEvents)
        {
            using (Stream stream = GetTestDataFromResource(jsonResourceName))
            {
                var policeApi = new PoliceUkClient
                {
                    RequestFactory = CreateRequestFactory(stream)
                };
                IEnumerable<NeighbourhoodEvent> events = policeApi.NeighbourhoodEvents("", "");

                // Assert
                Assert.That(events, Is.Not.Null.And.Length.EqualTo(expectedNeighbourhoodsEvents.Length));

                int total = events.Count();
                for (int i = 0; i < total; i++)
                {
                    NeighbourhoodEvent expected = expectedNeighbourhoodsEvents[i];
                    NeighbourhoodEvent actual = events.ElementAtOrDefault(i);

                    CustomAssert.AreEqual(expected, actual, new NeighbourhoodEventEqualityComparer());
                }
            }
        }

    }
}
