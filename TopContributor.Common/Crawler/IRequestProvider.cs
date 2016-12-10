using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TopContributor.Common.Crawler
{
    public interface IRequestProvider
    {
        Task<string> ReadRequest(string apiPath);
    }
}
