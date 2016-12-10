using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TopContributor.Common.DataAccess;

namespace TopContributor.Common.Crawler
{
    public class RepoCrawler
    {
        private readonly RepoDataContext _repoDataContext;
        private readonly IRepositoryReader _repoRequestProvider;
        private const int DAYS_PER_QUERY = 10;

        public RepoCrawler(RepoDataContext repoDataContext, IRepositoryReader repositoryReader)
        {
            _repoDataContext = repoDataContext;
            _repoRequestProvider = repositoryReader;
        }

        public async void SyncData()
        {
            DateTime dateBefore = DateTime.Now;
            var count = 0;
            while (count < 10)
            {
                var dateAfter = dateBefore.Subtract(new TimeSpan(DAYS_PER_QUERY, 0, 0, 0));
                var commits = (await _repoRequestProvider.QueryCommits(dateAfter, dateBefore)).Commits;

                if (commits == null)
                {
                    break;
                }
                foreach (var commit in commits)
                {
                    Console.WriteLine(commit.ToString());
                }
                count++;
            }
        }
    }
}
