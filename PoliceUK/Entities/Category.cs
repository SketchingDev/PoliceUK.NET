namespace PoliceUk.Entities
{
    using System.Runtime.Serialization;

    [DataContract]
    public class Category
    {
        /// <summary>
        /// Unique identifier
        /// </summary>
        [DataMember(Name = "url")]
        public string Url { get; set; }

        /// <summary>
        /// Name of the crime category
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; set; }
    }
}
