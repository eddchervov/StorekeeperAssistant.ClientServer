using Microsoft.Extensions.Configuration;
using StorekeeperAssistant.Api.Utils;
using StorekeeperAssistant.Api.Utils.Implementation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace StorekeeperAssistant.Api.Services
{
    public abstract class BaseRemoteCallService
    {
        #region fields

        protected virtual int _defaultCacheTime { get; set; } = 5;

        protected abstract string _apiSchemeAndHostConfigKey { get; set; }

        protected readonly ICacheProvider _cacheProvider;
        private readonly IConfiguration _configuration;

        #endregion

        #region ctor

        public BaseRemoteCallService(IConfiguration configuration, ICacheProvider cacheProvider)
        {
            _configuration = configuration;
            _cacheProvider = cacheProvider;
        }

        #endregion

        protected async Task<TResponse> ExecutePostAsync<TResponse, TRequest>(string path, TRequest request, int? timeoutMiliseconds = null)
        {
            var schemeAndHost = GetSchemeAndHostFromConfigs();

            var url = ApiUtils.BuildUrl(schemeAndHost: schemeAndHost, urlPath: path);

            var result = await ApiUtils.ExecutePostAsync<TResponse, TRequest>(url: url,
                                            data: request,
                                            timeout: timeoutMiliseconds);

            return result;
        }

        protected async Task<byte[]> ExecutePostReturnsByteArrayAsync<TRequest>(string path, TRequest request, int? timeoutMiliseconds = null)
        {
            var schemeAndHost = GetSchemeAndHostFromConfigs();

            var url = ApiUtils.BuildUrl(schemeAndHost: schemeAndHost, urlPath: path);

            var result = await ApiUtils.ExecutePostWithByteArrayResponseAsync<TRequest>(url: url,
                                            parameters: request,
                                            timeout: timeoutMiliseconds);

            return result;
        }

        protected async Task<TResponse> ExecutePutAsync<TResponse, TRequest>(string path, TRequest request, int? timeoutMiliseconds = null)
        {
            var schemeAndHost = GetSchemeAndHostFromConfigs();

            var url = ApiUtils.BuildUrl(schemeAndHost: schemeAndHost, urlPath: path);

            var result = await ApiUtils.ExecutePutAsync<TResponse, TRequest>(url: url,
                                            data: request,
                                            timeout: timeoutMiliseconds);

            return result;
        }

        protected async Task<TResponse> ExecuteDeleteAsync<TResponse, TRequest>(string path, TRequest request, int? timeoutMiliseconds = null)
        {
            var schemeAndHost = GetSchemeAndHostFromConfigs();

            var query = HttpUtility.ParseQueryString(path);

            if (request != null) AddToNameValueCollections<TRequest>(ref query, request);

            var url = UrlNormalization(ApiUtils.BuildUrl(schemeAndHost: schemeAndHost, urlPath: query.ToString()));

            var result = await ApiUtils.ExecuteDeleteAsync<TResponse>(url: url, timeout: timeoutMiliseconds);

            return result;
        }

        protected async Task<TResponse> ExecuteGetAsync<TResponse, TRequest>(string path, TRequest request, string accessToken = null, int? timeoutMiliseconds = null)
        {
            var schemeAndHost = GetSchemeAndHostFromConfigs();

            var query = HttpUtility.ParseQueryString(path);

            if (request != null) AddToNameValueCollections<TRequest>(ref query, request);

            var url = UrlNormalization(ApiUtils.BuildUrl(schemeAndHost: schemeAndHost, urlPath: query.ToString()));

            var result = await ApiUtils.ExecuteGetAsync<TResponse>(url: url, accessToken: accessToken, timeout: timeoutMiliseconds);

            return result;
        }

        private void AddToNameValueCollections<TRequest>(ref NameValueCollection query, TRequest request)
        {
            Type type = typeof(TRequest);
            PropertyInfo[] propertyInfos = type.GetProperties();

            for (int i = 0; i < propertyInfos.Length; i++)
            {
                PropertyInfo el = propertyInfos[i];
                if (typeof(IEnumerable).IsAssignableFrom(el.PropertyType)
                    && el.PropertyType.IsGenericType
                    && el.PropertyType.GetGenericArguments().Length == 1)
                {
                    IList values = el.GetValue(request) as IList;
                    foreach (var value in values)
                        query.Add(propertyInfos[i].Name, value?.ToString());
                }
                else
                {
                    var value = el?.GetValue(request);
                    query.Add(propertyInfos[i].Name, value?.ToString());
                }
            }
        }

        private string UrlNormalization(string url)
        {
            var firstAndIndex = url.IndexOf('&');
            var firstQuestionIndex = url.IndexOf('?');

            if (firstAndIndex > firstQuestionIndex)
            {
                var stringBuilder = new StringBuilder(url);
                stringBuilder.Remove(firstAndIndex, 1);
                stringBuilder.Insert(firstAndIndex, '?');

                url = stringBuilder.ToString();
            }

            url = url.Replace("%2f", "/");

            return url;
        }

        private string GetSchemeAndHostFromConfigs()
        {
            if (string.IsNullOrEmpty(_apiSchemeAndHostConfigKey) == true)
                throw new InvalidOperationException("ApiSchemeAndHostConfigKey is empty");

            var schemeAndHost = _configuration[_apiSchemeAndHostConfigKey];

            if (string.IsNullOrEmpty(schemeAndHost))
                throw new InvalidOperationException($"schemeAndHost is empty by key = {_apiSchemeAndHostConfigKey}");
            return schemeAndHost;
        }
    }
}
