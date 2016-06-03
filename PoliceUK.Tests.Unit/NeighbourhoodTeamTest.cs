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
    public class NeighbourhoodTeamTest : BaseMethodTests
    {
        private static readonly object[] DummyNeighbourhoodTeam =
        {
            new object[]
            {
                "PoliceUK.Tests.Unit.TestData.NeighbourhoodTeam.Multiple.json",
                new[]
                { 
                        new NeighbourhoodTeamMember
                        {
                            Name = "Adam Wardle",
                            Rank = "Sgt",
                            Bio = "bio1",
                            ContactDetails = new ContactDetails
                                            {
                                                Mobile = "07884 117276",
                                                Email = "ChadwellHeath.snt@met.police.uk",
                                                Telephone = "020 8721 2902",
                                                Website = "http://www.example.com"
                                            }
                        } ,
                        new NeighbourhoodTeamMember
                        {
                            Name = "Dave Marlow",
                            Rank = "PC",
                            Bio = "bio2",
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

        [Test, TestCaseSource("DummyNeighbourhoodTeam")]
        public void Call_Parses_Elements_From_Json_Repsonse(string jsonResourceName,
            NeighbourhoodTeamMember[] expectedNeighbourhoodTeam)
        {
            using (Stream stream = GetTestDataFromResource(jsonResourceName))
            {
                var policeApi = new PoliceUkClient
                {
                    RequestFactory = CreateRequestFactory(stream)
                };
                IEnumerable<NeighbourhoodTeamMember> team = policeApi.NeighbourhoodTeam("", "");

                // Assert
                Assert.That(team, Is.Not.Null.And.Length.EqualTo(expectedNeighbourhoodTeam.Length));

                int total = team.Count();
                for (int i = 0; i < total; i++)
                {
                    NeighbourhoodTeamMember expected = expectedNeighbourhoodTeam[i];
                    NeighbourhoodTeamMember actual = team.ElementAtOrDefault(i);

                    CustomAssert.AreEqual(expected, actual, new NeighbourhoodTeamMemberEqualityComparer());
                }
            }
        }

    }
}
