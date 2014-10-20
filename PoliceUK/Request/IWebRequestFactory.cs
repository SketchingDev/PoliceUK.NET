namespace PoliceUk.Request
{
    public interface IHttpWebRequestFactory
    {
        IHttpWebRequest Create(string uri);
    }
}
