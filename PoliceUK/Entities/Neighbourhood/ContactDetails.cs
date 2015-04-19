namespace PoliceUk.Entities.Neighbourhood
{
    using System.Runtime.Serialization;

    [DataContract]
    public class ContactDetails // TODO Use Enum for each contact type. Then only need to use a Has(ContactType) and Get(ContactType)
    {
        /// <summary>
        /// E-Mail address.
        /// </summary>
        [DataMember(Name = "email")]
        public string Email { get; set; }

        /// <summary>
        /// Telephone number.
        /// </summary>
        [DataMember(Name = "telephone")]
        public string Telephone { get; set; }

        /// <summary>
        /// Mobile number.
        /// </summary>
        [DataMember(Name = "mobile")]
        public string Mobile { get; set; }

        /// <summary>
        /// Fax number.
        /// </summary>
        [DataMember(Name = "fax")]
        public string Fax { get; set; }

        /// <summary>
        /// Website address.
        /// </summary>
        [DataMember(Name = "web")]
        public string Web { get; set; }

        /// <summary>
        /// Street address.
        /// </summary>
        [DataMember(Name = "address")]
        public string Address { get; set; }

        /// <summary>
        /// Facebook profile URL.
        /// </summary>
        [DataMember(Name = "facebook")]
        public string Facebook { get; set; }

        /// <summary>
        /// Twitter profile URL.
        /// </summary>
        [DataMember(Name = "twitter")]
        public string Twitter { get; set; }

        /// <summary>
        /// Youtube profile URL.
        /// </summary>
        [DataMember(Name = "youtube")]
        public string YouTube { get; set; }

        /// <summary>
        /// MySpace profile URL.
        /// </summary>
        [DataMember(Name = "myspace")]
        public string MySpace { get; set; }

        /// <summary>
        /// Bebo profile URL.
        /// </summary>
        [DataMember(Name = "bebo")]
        public string Bebo { get; set; }

        /// <summary>
        /// Flickr profile URL.
        /// </summary>
        [DataMember(Name = "flickr")]
        public string Flickr { get; set; }

        /// <summary>
        /// Google+ profile URL.
        /// </summary>
        [DataMember(Name = "google-plus")]
        public string GooglePlus { get; set; }

        /// <summary>
        /// Forum URL.
        /// </summary>
        [DataMember(Name = "forum")]
        public string Forum { get; set; }

        /// <summary>
        /// E-messaging URL.
        /// </summary>
        [DataMember(Name = "e-messaging")]
        public string Emessaging { get; set; }

        /// <summary>
        /// Blog URL.
        /// </summary>
        [DataMember(Name = "blog")]
        public string Blog { get; set; }

        /// <summary>
        /// RSS URL.
        /// </summary>
        [DataMember(Name = "rss")]
        public string Rss { get; set; }
    }
}