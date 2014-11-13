namespace PoliceUk.Entities.StreetLevel
{
    using System.Runtime.Serialization;
    using Location;

    [DataContract]
    public class Crime
    {
        /// <summary>
        /// Category of the crime.
        /// <see cref="PoliceUkClient.CrimeCategories"/>
        /// </summary>
        [DataMember(Name = "category")]
        public string Category { get; set; }

        /// <summary>
        /// 64-character unique identifier for that crime.
        /// </summary>
        /// <remarks>This is different to the existing 'id' attribute, which is not guaranteed to always stay the same for each crime.</remarks>
        [DataMember(Name = "persistent_id")]
        public string PersistentId { get; set; }

        /// <summary>
        /// The type of the location. 
        /// Either Force or BTP: Force indicates a normal police force location; BTP indicates a British Transport Police location. 
        /// BTP locations fall within normal police force boundaries.
        /// </summary>
        [DataMember(Name = "location_type")]
        public string LocationType { get; set; }

        /// <summary>
        /// For BTP locations, the type of location at which this crime was recorded.
        /// </summary>
        [DataMember(Name = "location_subtype")]
        public string LocationSubtype { get; set; }

        /// <summary>
        /// ID of the crime.
        /// </summary>
        /// <remarks>This ID only relates to the API, it is NOT a police identifier.</remarks>
        [DataMember(Name = "id")]
        public string Id { get; set; }

        /// <summary>
        /// Approximate location of the crime.
        /// </summary>
        [DataMember(Name = "location")]
        public CrimeLocation Location { get; set; }

        /// <summary>
        /// Extra information about the crime (if applicable).
        /// </summary>
        [DataMember(Name = "context")]
        public string Context { get; set; }

        /// <summary>
        /// Month of the crime
        /// </summary>
        [DataMember(Name = "month")]
        public string Month { get; set; }

        /// <summary>
        /// The category and date of the latest recorded outcome for the crime.
        /// </summary>
        [DataMember(Name = "outcome_status")]
        public OutcomeStatus OutcomeStatus { get; set; }
    }
}
