namespace PoliceUk.Entities.Neighbourhood
{
    using System.Runtime.Serialization;

    [DataContract]
    public class NeighbourhoodLocation
    {
        /// <summary>
        /// Name (if available).
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; set; }

        public IGeoposition Position { get; set; } // XXX Derive from same level lat/lng properties

        [DataMember(Name = "address")]
        public string Address { get; set; }

        [DataMember(Name = "postcode")]
        public string PostCode { get; set; }

        [DataMember(Name = "telephone")]
        public string Telephone { get; set; }

        /// <summary>
        /// Type of location, e.g. 'station' (police station)
        /// </summary>
        [DataMember(Name = "type")]
        public string Type { get; set; } // TODO Use Enum?

        [DataMember(Name = "description")]
        public string Description { get; set; }
    }
}