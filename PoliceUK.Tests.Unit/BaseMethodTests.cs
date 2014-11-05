namespace PoliceUk.Tests.Unit
{
    using FakeItEasy;
    using NUnit.Framework;
    using PoliceUk.Request;
    using System.IO;
    using System.Net;
    using System.Reflection;

    public abstract class BaseMethodTests
    {
        protected const string EmptyArrayTestDataResource = "PoliceUK.Tests.Unit.TestData.EmptyArray.json";

        protected const string MalformedTestDataResource = "PoliceUK.Tests.Unit.TestData.Malformed.json";

        protected static IHttpWebRequestFactory CreateRequestFactory(Stream streamResponse, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            var response = A.Fake<IHttpWebResponse>();
            A.CallTo(() => response.GetResponseStream()).Returns(streamResponse);
            A.CallTo(() => response.StatusCode).Returns(statusCode);

            var request = A.Fake<IHttpWebRequest>();
            A.CallTo(() => request.GetResponse()).Returns(response);

            var requestFactory = A.Fake<IHttpWebRequestFactory>();
            A.CallTo(() => requestFactory.Create(A<string>.Ignored)).Returns(request);

            return requestFactory;
        }

        protected static Stream GetTestDataFromResource(string name)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Stream stream = assembly.GetManifestResourceStream(name);

            if (stream == null)
            {
                Assert.Fail("Failed to get resource '{0}' from the calling assembly", name);
            }

            return stream;
        }
    }
}
