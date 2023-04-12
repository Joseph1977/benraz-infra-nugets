using Microsoft.Extensions.Caching.Memory;
using Moq;
using System;
using System.Net.Http;

namespace Benraz.Infrastructure.Gateways.Tests
{
    public static class GatewayTestUtils
    {
        public static IHttpClientFactory CreateHttpClientFactory()
        {
            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            httpClientFactoryMock
                .Setup(x => x.CreateClient(It.IsAny<string>()))
                .Returns(new HttpClient());

            return httpClientFactoryMock.Object;
        }

        public static IMemoryCache CreateMemoryCache()
        {
            var memoryCacheMock = new Mock<IMemoryCache>();
            var mockCacheEntry = new Mock<ICacheEntry>();

            memoryCacheMock
                .Setup(x => x.CreateEntry(It.IsAny<object>()))
                .Returns(mockCacheEntry.Object);

            return memoryCacheMock.Object;
        }
    }
}



