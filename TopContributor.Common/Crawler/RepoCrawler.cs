using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TopContributor.Common.DataAccess;
using TopContributor.Common.Model;

namespace TopContributor.Common.Crawler
{
    public class RepoCrawler
    {
        private readonly RepoDataContext _repoDataContext;
        private readonly IRepositoryReader _repoReader;
        private const int DAYS_PER_QUERY = 15;

        public RepoCrawler(RepoDataContext repoDataContext, IRepositoryReader repositoryReader)
        {
            _repoDataContext = repoDataContext;
            _repoReader = repositoryReader;
        }

        public async void SyncData()
        {
            

            var repository = _repoDataContext.VCSRepositories.FirstOrDefault(x => x.Id == _repoReader.Id);
            if (repository == null)
            {
                repository = new VCSRepository
                {
                    CrawlerProviderType = _repoReader.GetType().FullName,
                    Id = _repoReader.Id
                };
                _repoDataContext.VCSRepositories.Add(repository);
            }
            int totalCount = 0;
            DateTime dateBefore = DateTime.Now;
            while (true)
            {
                var dateAfter = dateBefore.Subtract(new TimeSpan(DAYS_PER_QUERY, 0, 0, 0));
                var commits = (await _repoReader.QueryCommits(dateAfter, dateBefore)).Commits;

                if (commits == null)
                {
                    break;
                }

                int count = 0;
                foreach (var crawlerCommit in commits)
                {
                    var authorId = crawlerCommit.AuthorId;
                    var account = _repoDataContext.RepositoryAccounts.Find(repository.Id, authorId);
                    Person person;

                    var c = _repoDataContext.Commits.Find(crawlerCommit.Id);
                    if (c != null)
                    {
                        continue;
                    }
                    if (account == null)
                    {
                        var accountInfo = await _repoReader.GetAuthorDetail(authorId);
                        if (accountInfo == null) continue;

                        account = new RepoAccount
                        {
                            AccountId = authorId,
                            SourceRepoId = repository.Id,
                            Email = accountInfo.Email,
                            Name = accountInfo.Name,
                            SourceRepository = repository
                        };
                        _repoDataContext.RepositoryAccounts.Add(account);

                        person = new Person
                        {
                            Accounts = new List<RepoAccount> {account},
                            FullName = account.Name
                        };
                        _repoDataContext.Persons.Add(person);
                    }
                    else
                    {
                        person = _repoDataContext.Persons.Find(account.PersonId);
                    }
                    if (person == null)
                    {
                        Console.WriteLine("Error, cannot find person");
                        continue;
                    }


                    var commit = new Model.Commit
                    {
                        Id = crawlerCommit.Id,
                        VSCAuthorAccountId = account.AccountId,
                        VSCRepositoryId = repository.Id,
                        AuthorRepoAccount = account,
                        Created = crawlerCommit.Created,
                        Deletions = crawlerCommit.Deletions,
                        Insertions = crawlerCommit.Insertions,
                        Source = repository,
                        ProjectId = crawlerCommit.ProjectName,
                        Message = crawlerCommit.Message
                    };
                    _repoDataContext.Commits.Add(commit);
                    
                    count++;
                    totalCount++;
                }

                _repoDataContext.SaveChanges();
                if (count == 0) break;
                Console.WriteLine($"Total: {totalCount}, Last insert: {count}, Currert period: {dateBefore.ToString("d")}");
                dateBefore = dateAfter;
            }

            //_repoDataContext.SaveChanges();
        }
    }
}
