namespace PoliceUk.Request
{
    using System;
    using System.IO;

    public interface IWebResponse : IDisposable
    {
        Stream GetResponseStream();
    }
}
