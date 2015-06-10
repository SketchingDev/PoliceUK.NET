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

        [DataMember(Name = "latitude")]
        public double? Latitude { get; set; }

        [DataMember(Name = "longitude")]
        public double? Longitude { get; set; }

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