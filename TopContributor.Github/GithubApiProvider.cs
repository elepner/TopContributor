using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TopContributor.Common.Crawler;

namespace TopContributor.Github
{
    public class GithubApiProvider : IRequestProvider
    {
        private readonly string _accessToken;
        private readonly string GitHubApi = "https://api.github.com/";
        private readonly HttpClient _httpClient;

        public GithubApiProvider(string accessToken)
        {
            _accessToken = accessToken;
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "TopContributorApp");
        }    

        public async Task<string> ReadRequest(string apiPath)
        {
            if (apiPath.IndexOf('?') == -1)
            {
                apiPath += "?access_token=" + _accessToken;
            }
            else
            {
                apiPath += "&access_token=" + _accessToken;
            }

            var queryString = apiPath.IndexOf(GitHubApi, StringComparison.Ordinal) >= 0 ? apiPath : GitHubApi + apiPath;
            var result = await _httpClient.GetAsync(queryString);
            return await result.Content.ReadAsStringAsync();
        }

        public string Id => "GitHub";
    }
}
