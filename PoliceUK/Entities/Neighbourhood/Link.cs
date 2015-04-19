namespace PoliceUk.Entities.Neighbourhood
{
    using System.Runtime.Serialization;

    [DataContract]
    public class Link
    {
        [DataMember(Name = "url")]
        public string Url { get; set; } // TODO Uri data-type

        /// <summary>
        /// Description of the link (if available).
        /// </summary>
        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "title")]
        public string Title { get; set; }
    }
}