namespace PoliceUk.Entities.Neighbourhood
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Description of a member of a Neighbourhood Team
    /// </summary>
    [DataContract]
    public class NeighbourhoodTeamMember
    {
        /// <summary>
        /// Name of the Neighbourhood Team Member.
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Rank of the Neighbourhood Team Member (i.e. "Sgt" or "PC").
        /// </summary>
        [DataMember(Name = "rank")]
        public string Rank { get; set; }

        /// <summary>
        /// Bio of the Neighbourhood Team Member (if available).
        /// </summary>
        [DataMember(Name = "bio")]
        public string Bio { get; set; }

        /// <summary>
        /// Contact details of the Neighbourhood Team Member.
        /// </summary>
        [DataMember(Name = "contact_details")]
        public ContactDetails ContactDetails { get; set; }

    }
}
