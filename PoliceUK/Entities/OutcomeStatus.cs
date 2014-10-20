namespace PoliceUk.Entities
{
    using System.Runtime.Serialization;

    [DataContract]
    public class OutcomeStatus
    {
        /// <summary>
        /// Category of the outcome.
        /// </summary>
        [DataMember(Name = "category")]
        public string Category { get; set; }

        /// <summary>
        /// Date of the outcome.
        /// </summary>
        [DataMember(Name = "date")]
        public string Date { get; set; }
    }
}
