using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TopContributor.Common.DataAccess;

namespace TopContributor.Controllers
{
    [Route("api/[controller]")]
    public abstract class BaseController
    {
        protected readonly RepoDataContext Context;

        protected BaseController(RepoDataContext context)
        {
            Context = context;
        }
    }
}
