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
    public class UsersController : BaseController
    {
        public UsersController(RepoDataContext context) : base(context)
        {
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
        public User GetUser(int id)
        {
            return Context.Users.Find(id);
        }
    }
}
