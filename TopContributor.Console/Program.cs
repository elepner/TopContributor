using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace TopContributor.Console
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length < 3)
            {
                System.Console.WriteLine("Specify gerrit URL, gerrit user and gerrit HTTP password in the parameters.");
                System.Console.ReadKey();
                return;
            }

            
                DoRequest(args[0], args[1], args[2]).Wait();
            

        }

        private static async Task DoRequest(string gerritUrl, string gerritUser, string gerritPwd)
        {

            var credCache = new CredentialCache();
            credCache.Add(new Uri(gerritUrl),"Digest",new NetworkCredential(gerritUser, gerritPwd));
            HttpClient httpClient = new HttpClient(new HttpClientHandler {Credentials = credCache});
            
            var requestUri = gerritUrl + "/a/projects/?type=ALL";
            var response = await httpClient.GetAsync(requestUri);
            var result = response.Content.ReadAsStringAsync();
        }
    }
}
