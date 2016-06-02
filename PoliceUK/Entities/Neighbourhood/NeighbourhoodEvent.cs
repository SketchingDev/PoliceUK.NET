namespace PoliceUk.Entities.Neighbourhood
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Description of a Neighbourhood Event
    /// </summary>
    [DataContract]
    public class NeighbourhoodEvent
    {
        /// <summary>
        /// Name of the Neighbourhood Event.
        /// </summary>
        [DataMember(Name = "title")]
        public string Title { get; set; }

        /// <summary>
        /// Long Description of the Neighbourhood Event.
        /// </summary>
        [DataMember(Name = "description")]
        public string Description { get; set; }

        /// <summary>
        /// Address of the Neighbourhood Event.
        /// </summary>
        [DataMember(Name = "address")]
        public string Address { get; set; }

        /// <summary>
        /// Start Date of the Neighbourhood Event.
        /// </summary>
        [DataMember(Name = "start_date")]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Type of the Neighbourhood Event (i.e. "meeting").
        /// </summary>
        [DataMember(Name = "type")]
        public string Type { get; set; }

        /// <summary>
        /// Contact Details of the Neighbourhood Event.
        /// </summary>
        [DataMember(Name = "contact_details")]
        public ContactDetails ContactDetails { get; set; }

    }
}
