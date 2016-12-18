using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using TopContributor.Common.Crawler;
using TopContributor.Common.DataAccess;
using TopContributor.Gerrit;
using TopContributor.Github;
using User = TopContributor.Common.User;
namespace TopContributor.Console
{
    public class Program
    {
        
        private static string _gerritUrl;
        private static string _gerritUser;
        private static string _gerritPwd;
        private static string _githubAccessToken;

        public static void Main(string[] args)
        {
            //if (args.Length < 3)
            //{
            //    System.Console.WriteLine("Specify gerrit URL, gerrit user and gerrit HTTP password in the parameters.");
            //    System.Console.ReadKey();
            //    return;
            //}

            //_gerritUrl = args[0];
            //_gerritUser = args[1];
            //_gerritPwd = args[2];

            _githubAccessToken = args[3];
            
            DoRequest().Wait();

            
            System.Console.ReadKey();
        }

        private static async Task DoRequest()
        {

            var githubreader = new GithubApiProvider(_githubAccessToken);
            var githubRepoReader = new GithubRepoReader(githubreader, "PowelAS");
            //await githubRepoReader.QueryCommits(DateTime.Now.Subtract(new TimeSpan(15,0,0,0)), DateTime.Now);
            //var gerritRepoReader = new HttpGerritRepoReader(_gerritUrl, _gerritUser, _gerritPwd);
            //var gerritReader = new GerritRepoReader(gerritRepoReader);


            var optionsBuilder = new DbContextOptionsBuilder<RepoDataContext>();

            var connection =
                @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=TopContributor2;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            optionsBuilder.UseSqlServer(connection);

            var context = new RepoDataContext(optionsBuilder.Options);

            var crawler = new RepoCrawler(context, githubRepoReader);
            await crawler.SyncData();
        }

    }
}
