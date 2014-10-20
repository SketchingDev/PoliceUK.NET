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

        public object abc;

        /// <summary>
        /// Tests equality based on member variables being equal.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object's properties are equal to the current object's; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (obj is Category)
            {
                var testCategory = (Category)obj;

                return testCategory.Url == this.Url 
                    && testCategory.Name == this.Name;
            }

            return false;
        }
    }
}
