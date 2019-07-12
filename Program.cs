using IdentityModel.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BankOfDotNet.ConsoleClient
{
    class Program
    {
        public static void Main(string[] args) => MainAsync().GetAwaiter().GetResult();

        private static async Task MainAsync()
        {

            var discoRO = await DiscoveryClient.GetAsync("http://localhost:5000");
            if (discoRO.IsError)
            {
                Console.WriteLine(discoRO.Error);
                return;
            }

            //Grab a bearer token using ResourceOwnerPassword Grant Type:
            var tokenClientRO = new TokenClient(discoRO.TokenEndpoint, "ro.client", "secret");
            var tokenResponseRO = await tokenClientRO.RequestResourceOwnerPasswordAsync
                ("Michael","password", "bankOfDotNetIdentityServer4Api");

            if (tokenResponseRO.IsError)
            {
                Console.WriteLine(tokenResponseRO.Error);
                return;
            }

            Console.WriteLine(tokenResponseRO.Json);
            Console.WriteLine("\n\n");




            //discover all the endpoints using metadata of identity server:
            var disco = await DiscoveryClient.GetAsync("http://localhost:5000");
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                return;
            }

            //Grab a bearer token using Client Credential Flow grant type:
            var tokenClient = new TokenClient(disco.TokenEndpoint, "client", "secret");
            var tokenResponse = await tokenClient.RequestClientCredentialsAsync("bankOfDotNetIdentityServer4Api");

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }

            Console.WriteLine(tokenResponse.Json);
            Console.WriteLine("\n\n");

            //Consume our Customer Apit:
            var client = new HttpClient();
            client.SetBearerToken(tokenResponse.AccessToken);

            var customerInfo = new StringContent(
                JsonConvert.SerializeObject(
                    new { Id = 10, FirstName = "Michael", LastName = "Ma" }),
                    Encoding.UTF8, "application/json");

            var createCustomerResponse = await client.PostAsync("http://localhost:59337/api/customers", customerInfo);
            if(!createCustomerResponse.IsSuccessStatusCode)
            {
                Console.WriteLine(createCustomerResponse.StatusCode);
            }

            var getCustomerResponse = await client.GetAsync("http://localhost:59337/api/customers");
            if (!getCustomerResponse.IsSuccessStatusCode)
            {
                Console.WriteLine(getCustomerResponse.StatusCode);
            }
            else
            {
                var content = await getCustomerResponse.Content.ReadAsStringAsync();
                Console.WriteLine(JArray.Parse(content));
            }

        }
    }
}
