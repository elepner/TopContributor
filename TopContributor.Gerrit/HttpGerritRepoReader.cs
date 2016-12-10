using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using TopContributor.Common.Crawler;

namespace TopContributor.Gerrit
{
    public class HttpGerritRepoReader : IRequestProvider, IDisposable
    {
        private readonly string _gerritUrl;
        private readonly HttpClient _httpClient;
        private bool _disposed;

        public HttpGerritRepoReader(string gerritUrl, string gerritUser, string gerritPassword)
        {
            _gerritUrl = gerritUrl;

            var credCache = new CredentialCache();
            credCache.Add(new Uri(_gerritUrl), "Digest", new NetworkCredential(gerritUser, gerritPassword));
            _httpClient = new HttpClient(new HttpClientHandler { Credentials = credCache });
            
        }

        public async Task<string> ReadRequest(string apiPath)
        {
            var request = $"{_gerritUrl}/a/{apiPath}";
            var httpResult = await _httpClient.GetAsync(request);

            if (httpResult.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }
            
            return await httpResult.Content.ReadAsStringAsync();
        }

        public string Id => _gerritUrl;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                // If disposing equals true, dispose all managed 
                // and unmanaged resources. 
                if (disposing)
                {
                    // Dispose managed resources.
                    _httpClient.Dispose();
                }
                _disposed = true;
            }
        }
    }
}
