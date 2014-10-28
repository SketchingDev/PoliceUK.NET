namespace PoliceUK.Entities.Force
{
    using System.Runtime.Serialization;

    [DataContract]
    public class ForceSummary
    {
        /// <summary>
        /// Unique force identifier
        /// </summary>
        [DataMember(Name = "id")]
        public string Id { get; set; } // TODO Turn into enum?

        /// <summary>
        /// Force name
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; set; }
    }
}
