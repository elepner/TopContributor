using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.v3;
using TopContributor.Common.DataAccess;
using TopContributor.Common.Model;

namespace TopContributor.Controllers
{
    [Route("api/[controller]")]
    public class CommitsController
    {
        private readonly RepoDataContext _context;

        public CommitsController(RepoDataContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        public IActionResult GetCommits(int days = 1)
        {
            if (days <= 0)
            {
                return null;
            }

            var after = DateTime.Now.Subtract(new TimeSpan(days, 0, 0, 0));

            var result = _context.Commits.Where(x => x.Created > after)
                .OrderByDescending(x => x.Created)
                .Include(x => x.AuthorRepoAccount)
                .Include(x => x.AuthorRepoAccount.Person)
                .ToArray().Select(commit =>
                {
                    commit.AuthorRepoAccount.Person.Accounts = null;
                    return new
                    {
                        CommitId = commit.Id,
                        SourceRepoId = commit.VSCRepositoryId,
                        commit.Message,
                        commit.Created,
                        commit.Insertions,
                        commit.Deletions,
                        Author = commit.AuthorRepoAccount.Person,
                        Project = commit.ProjectId
                    };
                }).ToArray();

            return new OkObjectResult(result);
        }
    }
}
