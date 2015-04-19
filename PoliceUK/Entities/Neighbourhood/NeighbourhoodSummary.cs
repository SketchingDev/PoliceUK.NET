namespace PoliceUk.Entities.Neighbourhood
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Summary of a neighbourhood.
    /// </summary>
    [DataContract]
    public class NeighbourhoodSummary
    {
        /// <summary>
        /// Police force specific team identifier.
        /// </summary>
        /// <remarks>This identifier is not unique and may also be used by a differene force.</remarks>
        [DataMember(Name = "id")]
        public string Id { get; set; }

        /// <summary>
        /// Name of the neighbourhood.
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; set; }
    }
}
