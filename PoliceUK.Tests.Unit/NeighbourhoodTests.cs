namespace PoliceUk.Tests.Unit
{
    using CustomAssertions;
    using CustomAssertions.Equality.NeighbourhoodDetails;
    using Entities.Neighbourhood;
    using NUnit.Framework;
    using PoliceUk;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    [TestFixture]
    public class NeighbourhoodTests : BaseMethodTests
    {
        #region Dummy data

        private static readonly NeighbourhoodDetails DummyNeighbourhoodDetails = new NeighbourhoodDetails
            {
                UrlForce = "http://www.example.com/",
                Contact = new ContactDetails
                {
                    Mobile = "07884 117276",
                    Email = "ChadwellHeath.snt@met.police.uk",
                    Telephone = "020 8721 2902",
                    Website = "http://www.example.com"
                },
                Name = "Chadwell Heath",
                Links = new Link[]
                {
                    new Link
                    {
                        Url = "http://www.example.com/2",
                        Description = "This is an example description",
                        Title = "Chadwell Heath Saferneighbourhoods Team"
                    },
                    new Link
                    {
                        Url = "http://www.example.com/3",
                        Description = "This is another example description",
                        Title = "Local Crime"
                    }
                },
                Centre = new Geoposition(51.5851, 0.138774),
                Locations = new NeighbourhoodLocation[] {
                    new NeighbourhoodLocation
                    {
                        Name = "Example Station",
                        Latitude = 51.6,
                        Longitude = .1,
                        PostCode = "RM6 5JU",
                        Address = "Marks Gate Police Station, 78 Rose Lane, Romford",
                        Type = "station",
                        Description = "This is yet another example description"
                    }
                },
                Description = "Welcome to the Chadwell Heath Safer Neighbourhoods Team page.<br/>Our team is comprised of Police Officers and Police Community Support Officers (PCSOs) and we are dedicated to making your neighbourhood a safer place to live in, work in and visit. We listen and talk to you, and find out what affects your daily life and feelings of security. Our priorities are then set by the local community and we work with you and other agencies to find a lasting solution.<br/>You can contact us via e-mail or phone. As part of the Policing Pledge we will respond to every message directed to us within 24 hours. However, we are not a 24-hour response team, so if it is an emergency please call 999.<br/>For more information about local policing visit the main <a href=\"http://www.met.police.uk/saferneighbourhoods/\" title=\"Safer Neighbourhoods Website\">Safer Neighbourhoods website</a>.<br/>For more information about local crime statistics in your ward, please visit the <a href=\"http://maps.met.police.uk/index.php?areacode=00ABGA\" title=\"local crime maps website\">local crime maps website</a> for Chadwell Heath Safer Neighbourhoods Team.",
                Id = "00ABGA",
                Population = 10021
            };

        private static readonly object[] DummyNeighbourhood = 
            {
                new object[]
                {
                    "PoliceUK.Tests.Unit.TestData.Neighbourhood.json", 
                    DummyNeighbourhoodDetails
                }
            };

        #endregion

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Call_With_Null_ForceId_Throws_ArgumentNullException()
        {
            (new PoliceUkClient()).Neighbourhood(null, "ABC");
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Call_With_Null_Id_Throws_ArgumentNullException()
        {
            (new PoliceUkClient()).Neighbourhood("ABC", null);
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

                policeApi.Neighbourhood("", "");
            }
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

                NeighbourhoodDetails Neighbourhood = policeApi.Neighbourhood("", "");

                // Assert
                Assert.IsNull(Neighbourhood);
            }
        }

        [Test, TestCaseSource("DummyNeighbourhood")]
        public void Call_Parses_Element_From_Json_Repsonse(string jsonResourceName, NeighbourhoodDetails expectedNeighbourhoodDetails)
        {
            using (Stream stream = GetTestDataFromResource(jsonResourceName))
            {
                var policeApi = new PoliceUkClient
                {
                    RequestFactory = CreateRequestFactory(stream)
                };
                NeighbourhoodDetails neighbourhood = policeApi.Neighbourhood("", "");

                // Assert
                Assert.IsNotNull(neighbourhood);

                CustomAssert.AreEqual(expectedNeighbourhoodDetails, neighbourhood, new NeighbourhoodDetailsEqualityComparer());

                CustomAssert.AreEqual(expectedNeighbourhoodDetails.Contact, neighbourhood.Contact, new ContactDetailsEqualityComparer());

                IEnumerable<Link> actualLinks = neighbourhood.Links;
                Link[] expectedLinks = expectedNeighbourhoodDetails.Links.ToArray();

                Assert.That(actualLinks, Is.Not.Null.And.Count.EqualTo(expectedLinks.Length));

                for (int i = 0; i < actualLinks.Count(); i++)
                {
                    Link expected = expectedLinks[i];
                    Link actual = actualLinks.ElementAtOrDefault(i);

                    CustomAssert.AreEqual(expected, actual, new LinkEqualityComparer());
                }

                IEnumerable<NeighbourhoodLocation> actualLocations = neighbourhood.Locations;
                NeighbourhoodLocation[] expectedLocations = expectedNeighbourhoodDetails.Locations.ToArray();

                Assert.That(actualLocations, Is.Not.Null.And.Count.EqualTo(expectedLocations.Length));

                for (int i = 0; i < actualLocations.Count(); i++)
                {
                    NeighbourhoodLocation expected = expectedLocations[i];
                    NeighbourhoodLocation actual = actualLocations.ElementAtOrDefault(i);

                    CustomAssert.AreEqual(expected, actual, new NeighbourhoodLocationEqualityComparer());
                }
            }
        }
    }
}
