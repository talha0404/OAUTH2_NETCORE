using IdentityModel.Client;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ConsoleApplication
{
    class Program
    {
        static async Task Main(string[] args)
        {
            TokenResponse tokenResponse;

            using (var discoveryDocumentHttpClient = new HttpClient())
            {
                var discoveryDocument = await discoveryDocumentHttpClient.GetDiscoveryDocumentAsync(address: "https://localhost:5001");

                Console.WriteLine(discoveryDocument.TokenEndpoint);

                tokenResponse = await discoveryDocumentHttpClient.RequestClientCredentialsTokenAsync(
                   new ClientCredentialsTokenRequest
                   {
                       Address = discoveryDocument.TokenEndpoint,
                       ClientId = "console",
                       ClientSecret = "secret",
                       Scope = "api"
                   });

                Console.WriteLine(tokenResponse.AccessToken);

            }

            using (var apiHttpClient = new HttpClient())
            {
                apiHttpClient.SetBearerToken(tokenResponse.AccessToken);

                //We are supposed to set token before fetching data. 
                //Token is like key to access to function
                var response = await apiHttpClient.GetStringAsync(requestUri: "https://localhost:5003/api/CrudOperation/GetCustomerModel?Id=2");

                Console.WriteLine(response);
            }

            Console.ReadLine();
        }
    }
}
