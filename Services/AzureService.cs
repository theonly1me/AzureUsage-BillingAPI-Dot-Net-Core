using System;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Net.Http;
using System.Threading.Tasks;

namespace AzureClientWebAPI.Auth
{
    public interface IAzureService
    {                
        public Task<string> GetResponse(string apiURL);        
    }

    public class AzureService : IAzureService
    {

        #region AzureDetails 
        /**
         * Azure Details         
         */

        private string clientID = "YOUR_CLIENT_ID";
        private string clientSecret = "YOUR_CLIENT_SECRET";
        private string tenantID = "YOUR_TENANT_ID";               
        #endregion

        #region Generate Auth Token
        private static string GetAuthToken(string clientID, string clientSecret, string tenantID)
        {
            ClientCredential credentials = new ClientCredential(clientID, clientSecret);
            var context = new AuthenticationContext("https://login.microsoftonline.com/" + tenantID);
            var result = context.AcquireTokenAsync("https://management.azure.com", credentials);

            if (result == null)
            {
                throw new InvalidOperationException("Error while getting the access token");
            }

            return result.Result.AccessToken;
        }    
        #endregion

        #region Azure Rest API Helpers      
        public async Task<string> GetResponse(string apiURL)
        {
            string token = GetAuthToken(clientID, clientSecret, tenantID);
            HttpClient client = new HttpClient();
            string response = "";
            client.BaseAddress = new Uri(apiURL);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, client.BaseAddress);
            try
            {
                response = await MakeRequestAsync(request, client);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return response;
        }

        public static async Task<string> MakeRequestAsync(HttpRequestMessage getRequest, HttpClient client)
        {
            var response = await client.SendAsync(getRequest).ConfigureAwait(false);
            var responseString = string.Empty;
            try
            {
                response.EnsureSuccessStatusCode();
                responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine(e);
            }

            return responseString;
        }
        #endregion        
    }
}
