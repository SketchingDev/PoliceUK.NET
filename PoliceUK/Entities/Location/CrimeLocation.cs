namespace PoliceUk.Entities.Location
{
    using System.Runtime.Serialization;

    [DataContract]
    public class CrimeLocation
    {
        [DataMember(Name = "latitude")]
        public double Latitude { get; set; }

        [DataMember(Name = "longitude")]
        public double Longitude { get; set; }

        /// <summary>
        /// The approximate street the crime occurred.
        /// </summary>
        [DataMember(Name = "street")]
        public Street Street { get; set; }
    }
}
