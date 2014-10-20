namespace PoliceUk.Exceptions
{
    using System;

    /// <summary>
    /// Thrown when the a problem occurs deserialising the data returned by 
    /// the Police API.
    /// </summary>
    public class InvalidDataException : Exception
    {
        public InvalidDataException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
