namespace PoliceUK.Tests.Unit.CustomAssertions.Equality
{
    using NUnit.Framework;
    using PoliceUk.Entities;

    public class CategoryEqualityComparer : AbstractEqualityComparer<Category>
    {
        public override bool AreEqual(Category categoryOne, Category categoryTwo)
        {
            Assert.AreEqual(categoryOne.Url, categoryTwo.Url);
            Assert.AreEqual(categoryOne.Name, categoryTwo.Name);

            return true;
        }
    }
}
