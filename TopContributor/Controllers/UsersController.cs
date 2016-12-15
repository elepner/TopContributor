using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Remotion.Linq.Clauses;
using TopContributor.Common.DataAccess;
using TopContributor.Common.Model;

namespace TopContributor.Controllers
{
    public class UsersController : BaseController
    {
        public UsersController(RepoDataContext context) : base(context)
        {
        }

        [HttpGet("top")]
        public IActionResult TopUsers(int days, int limit = -1)
        {

            var startDate = DateTime.Now.Subtract(new TimeSpan(days, 0, 0, 0));

            var q = from u in Context.Users
                    join account in Context.RepositoryAccounts on u.Id equals account.PersonId
                    join commit in Context.Commits on new { a = account.AccountId, b = account.SourceRepoId }
                    equals new { a = commit.VSCAuthorAccountId, b = commit.VSCRepositoryId }
                    where commit.Created >= startDate group new {u, commit} by u.Id into gr
                    let user = gr.FirstOrDefault().u
                    let count = gr.Count()
                    orderby count descending 
                    select new
                    {
                        user,
                        count
                    };

            if (limit > 0)
            {
                q = q.Take(limit);
            }
            
            return new OkObjectResult(q);
        }

        [HttpGet]
        public IEnumerable<User> GetUsers(int limit = -1)
        {
            if (limit <= 0)
            {
                return Context.Users.Include(x => x.Accounts);
            }
            return Context.Users.Take(limit).Include(x => x.Accounts);
        }

        [HttpGet("{id}")]
        public IActionResult GetUser(int id)
        {
            var user = Context.Users.Include(x => x.Accounts)
                .ThenInclude(x => x.Commits)
                .FirstOrDefault(x => x.Id == id);

            var q = from u in Context.Users
                    join account in Context.RepositoryAccounts on u.Id equals account.PersonId
                    join commit in Context.Commits on new { a = account.AccountId, b = account.SourceRepoId }
                    equals new { a = commit.VSCAuthorAccountId, b = commit.VSCRepositoryId }
                    where u.Id == id
                    select new
                    {
                        u,
                        commit
                    };

            var arr = q.ToArray();

            return new OkObjectResult(user);
        }
    }
}
