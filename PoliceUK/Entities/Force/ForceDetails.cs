namespace PoliceUK.Entities.Force
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class ForceDetails
    {
        [DataMember(Name = "description")]
        public string Description { get; set; }

        /// <summary>
        /// Force website URL.
        /// </summary>
        [DataMember(Name = "url")]
        public string Url { get; set; }

        /// <summary>
        /// Ways to keep informed.
        /// </summary>
        [DataMember(Name = "engagement_methods")]
        public IEnumerable<ForceEngagementMethod> EngagementMethods { get; set; }

        /// <summary>
        /// Force telephone number.
        /// </summary>
        [DataMember(Name = "telephone")]
        public string Telephone { get; set; }

        /// <summary>
        /// Unique force identifier.
        /// </summary>
        [DataMember(Name = "id")]
        public string ID { get; set; }

        /// <summary>
        /// Force name.
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; set; }
    }
}
