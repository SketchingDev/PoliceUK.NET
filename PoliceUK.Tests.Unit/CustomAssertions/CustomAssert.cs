namespace PoliceUK.Tests.Unit.CustomAssertions
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;

    public static class CustomAssert
    {
        private const string AreEqualFailureMessage = "CustomAssert.AreEqual failed. Expected {0}. Actual {1}";

        /// <summary>
        /// Varifies that two generic data types are equal based on the result of an equality comparer.
        /// The assertion fails if they are not equal.
        /// </summary>
        /// <param name="expected">The first generic type data to compare. This is the generic type data the</param>
        /// <param name="actual">The second generic type data to compare. This is the generic type data the unit test produced.</param>
        /// <param name="equalityComparer">The EqualityComparer used to determine 
        /// if <paramref name="expected"/> and <paramref name="actual"/> are equal.</param>
        /// <exception cref="Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException">
        /// <paramref name="expected"/> is not equal to <paramref name="actual"/>.
        /// </exception>
        public static void AreEqual<T>(T expected, T actual, IEqualityComparer<T> equalityComparer)
        {
            if (equalityComparer == null)
            {
                throw new ArgumentNullException("equalityComparer");
            }

            var areEqual = equalityComparer.Equals(expected, actual);
            if (areEqual) return;

            throw new AssertFailedException(string.Format(AreEqualFailureMessage, expected, actual));
        }
    }
}
