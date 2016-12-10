using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public IEnumerable<Commit> GetCommits(int days = 1)
        {
            if (days <= 0)
            {
                return Enumerable.Empty<Commit>();
            }

            var after = DateTime.Now.Subtract(new TimeSpan(days, 0, 0, 0));

            return _context.Commits.Where(x => x.Created > after).Include(x => x.Author).OrderByDescending(x => x.Created).ToArray();
        }
    }
}
