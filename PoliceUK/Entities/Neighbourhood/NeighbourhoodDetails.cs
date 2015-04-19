namespace PoliceUk.Entities.Neighbourhood
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class NeighbourhoodDetails
    {
        /// <summary>
        /// URL for the neighbourhood on the Force's website.
        /// </summary>
        [DataMember(Name = "url_force")]
        public string UrlForce { get; set; }

        /// <summary>
        /// URL for the neighbourhood's boundary in KML format.
        /// </summary>
        [DataMember(Name = "url_boundary")]
        public string UrlBoundary { get; set; }

        /// <summary>
        /// Ways to get in touch with the neighbourhood officers.
        /// </summary>
        [DataMember(Name = "contact_details")]
        public ContactDetails Contact { get; set; }

        /// <summary>
        /// Name of the neighbourhood.
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; set; }

        /// <summary>
        /// An introduction message for the neighbourhood.
        /// </summary>
        [DataMember(Name = "welcome_message")]
        public string WelcomeMessage { get; set; }

        [DataMember(Name = "links")]
        public IEnumerable<Link> Links { get; set; }

        /// <summary>
        /// Centre point locator for the neighbourhood.
        /// </summary>
        /// <remarks>This may not be exactly in the centre of the neighbourhood.</remarks>
        [DataMember(Name = "centre")]
        public IGeoposition Centre { get; set; }

        /// <summary>
        /// Any associated locations with the neighbourhood, e.g. police stations.
        /// </summary>
        [DataMember(Name = "locations")]
        public IEnumerable<NeighbourhoodLocation> Locations { get; set; }

        /// <summary>
        /// Population of the neighbourhood.
        /// </summary>
        [DataMember(Name = "population")]
        public int Population { get; set; }

        /// <summary>
        /// Police force specific team identifier.
        /// </summary>
        /// <remarks>This identifier is not unique and may also be used by a different force.</remarks>
        [DataMember(Name = "id")]
        public string Id { get; set; }

        /// <summary>
        /// Description (if available).
        /// </summary>
        [DataMember(Name = "description")]
        public string Description { get; set; }
    }
}