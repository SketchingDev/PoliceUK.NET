namespace PoliceUk.Entities.Location
{
    using System.Runtime.Serialization;

    [DataContract]
    public class Street
    {
        /// <summary>
        /// Unique identifier for the street
        /// </summary>
        [DataMember(Name = "id")]
        public int Id { get; set; }

        /// <summary>
        /// Name of the location.
        /// </summary>
        /// <remarks>This is only an approximation of where the crime happened.</remarks>
        [DataMember(Name = "name")]
        public string Name { get; set; }
    }
}
