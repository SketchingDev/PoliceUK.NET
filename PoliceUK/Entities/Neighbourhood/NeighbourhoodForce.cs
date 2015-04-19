namespace PoliceUk.Entities.Neighbourhood
{
    using System.Runtime.Serialization;

    [DataContract]
    public class NeighbourhoodForce
    {
        /// <summary>
        /// Unique force identifier
        /// </summary>
        [DataMember(Name = "force")]
        public string ForceId { get; set; }

        /// <summary>
        /// Force specific team identifier.
        /// </summary>
        [DataMember(Name = "neighbourhood")]
        public string NeighbourhoodId { get; set; }
    }
}
