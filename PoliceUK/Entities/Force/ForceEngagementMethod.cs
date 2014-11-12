namespace PoliceUk.Entities.Force
{
    using System.Runtime.Serialization;

    /// <summary>
    /// A way of keeping informed.
    /// </summary>
    [DataContract]
    public class ForceEngagementMethod
    {
        /// <summary>
        /// Method website URL.
        /// </summary>
        [DataMember(Name = "url")]
        public string Url { get; set; }

        /// <summary>
        /// Method type.
        /// </summary>
        [DataMember(Name = "type")]
        public string Type { get; set; }

        /// <summary>
        /// Method description.
        /// </summary>
        [DataMember(Name = "description")]
        public string Description { get; set; }

        /// <summary>
        /// Method title.
        /// </summary>
        [DataMember(Name = "title")]
        public string Title { get; set; }
    }
}
