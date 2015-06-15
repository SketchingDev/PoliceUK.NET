namespace PoliceUk.Entities.StreetLevel
{
    using System.Runtime.Serialization;
    using System;

    [DataContract]
    public class LastUpdated
    {
        /// <summary>
        /// Year and month of last update to crime data in 
        /// <see href="http://en.wikipedia.org/wiki/ISO_8601#Dates">ISO format</see>.
        /// </summary>
        [DataMember(Name = "date")]
        public DateTime Date { get; set; }
    }
}

