using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using MovieRating.Domain;
using MovieRating.Interfaces;
using MovieRating.Services;
using NUnit.Framework;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace MovieRating.Tests.Unit
{
    [TestFixture]
    public class OmdbServiceTests
    {
        private IOmdbService<Movie> _sut;
        private Mock<IConfigurationRoot> _configurationRootMock;
        private Mock<IHttpClientFactory> _httpClientFactoryMock;

        [SetUp]
        public void Setup()
        {
            _configurationRootMock = new Mock<IConfigurationRoot>();
            _configurationRootMock.Setup(x => x.GetSection("Omdb:BaseUrl").Value).Returns("http://fakeurl.com/");
            _configurationRootMock.Setup(x => x.GetSection("Omdb:ApiKey").Value).Returns("http://fakeurl.com/");

            _httpClientFactoryMock = new Mock<IHttpClientFactory>();

            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{ \"Title\":\"Jurassic Park\",\"Year\":\"1993\",\"imdbRating\":\"8.1\" }"),
                })
                .Verifiable();

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("http://fakeurl.com/"),
            };

            _httpClientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

            _sut = new OmdbService(_configurationRootMock.Object, _httpClientFactoryMock.Object);
        }

        [Test]
        public async Task Should_Return_Movie()
        {
            // when
            var result = await _sut.GetRating("Jurassic Park");

            // then
            Assert.AreEqual("Jurassic Park", result.Title);
        }

        [TestCase(" ")]
        [TestCase(null)]
        public void ShouldThrowErrorWhenTitleIsEmpty(string title)
        {
            // when
            var result = Assert.ThrowsAsync<ArgumentException>(() => _sut.GetRating(title));

            // then
            Assert.AreEqual("Title is missing", result.Message);
        }
    }
}