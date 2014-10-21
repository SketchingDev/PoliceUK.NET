namespace PoliceUK.Entities
{
    using System.Runtime.Serialization;

    [DataContract]
    public class ForceShortDescription
    {
        /// <summary>
        /// Unique force identifier
        /// </summary>
        [DataMember(Name = "id")]
        public string ID { get; set; } // TODO Turn into enum?

        /// <summary>
        /// Force name
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; set; }
    }
}
