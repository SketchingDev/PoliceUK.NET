namespace PoliceUk.Exceptions
{
    using System;

    /// <summary>
    /// Thrown when the a problem occurs requesting the data from the Police API
    /// </summary>
    public class DataRequestException : Exception
    {
        public DataRequestException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
