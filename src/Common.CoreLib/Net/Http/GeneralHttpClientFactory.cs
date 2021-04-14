﻿using Microsoft.Extensions.Logging;

namespace System.Net.Http
{
    /// <summary>
    /// 通用 <see cref="HttpClient"/> 工厂
    /// </summary>
    public abstract class GeneralHttpClientFactory
    {
        protected readonly ILogger logger;
        protected readonly IHttpPlatformHelper http_helper;
        readonly IHttpClientFactory _clientFactory;

        public GeneralHttpClientFactory(
            ILogger logger,
            IHttpPlatformHelper http_helper,
            IHttpClientFactory clientFactory)
        {
            this.logger = logger;
            this.http_helper = http_helper;
            _clientFactory = clientFactory;
        }

        /// <summary>
        /// 用于 <see cref="IHttpClientFactory.CreateClient(string)"/> 中传递的 name
        /// <para>如果为 <see langword="null"/> 则调用 <see cref="HttpClientFactoryExtensions.CreateClient(IHttpClientFactory)"/></para>
        /// <para>默认值为 <see langword="null"/></para>
        /// </summary>
        protected virtual string? ClientName { get; }

        /// <summary>
        /// 默认超时时间，19秒
        /// </summary>
        public static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(19);

        /// <inheritdoc cref="DefaultTimeout"/>
        protected virtual TimeSpan Timeout { get; } = DefaultTimeout;

        protected virtual HttpClient CreateClient()
        {
            var client = CreateClient_();
            client.Timeout = Timeout;
            return client;
        }

        HttpClient CreateClient_()
        {
#if DEBUG
            logger.LogDebug("CreateClient, name: {0}", ClientName);
#endif
            if (ClientName == null)
            {
                return _clientFactory.CreateClient();
            }
            else
            {
                return _clientFactory.CreateClient(ClientName);
            }
        }
    }
}