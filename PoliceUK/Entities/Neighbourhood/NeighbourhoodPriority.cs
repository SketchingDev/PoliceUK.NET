namespace PoliceUk.Entities.Neighbourhood
{
    using System.Runtime.Serialization;

    [DataContract]
    public class NeighbourhoodPriority
    {
        /// <summary>
        /// An issue raised with the police.
        /// </summary>
        [DataMember(Name = "issue")]
        public string Issue { get; set; }

        /// <summary>
        /// When the priority was agreed upon.
        /// </summary>
        [DataMember(Name = "issue-date")]
        public string IssueDate { get; set; } // TODO use DateTime instead of String

        /// <summary>
        /// Action taken to address the priority.
        /// </summary>
        [DataMember(Name = "action")]
        public string Action { get; set; }

        /// <summary>
        /// When action was last taken.
        /// </summary>
        [DataMember(Name = "action-date")]
        public string ActionDate { get; set; } // TODO use DateTime instead of String
    }
}
