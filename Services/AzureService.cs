using System;
using System.Net;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using AzureClientWebAPI.Models;

namespace AzureClientWebAPI.Auth
{
    public interface IAzureService
    {
        Microsoft.Azure.Management.Fluent.IAzure Authenticate();
        public UsagePayload GetUsageDetails();
        public RateCardPayload GetRateCardDetails();
    }

    public class AzureService : IAzureService
    {
        string clientID = Environment.GetEnvironmentVariable("Azure_Client_ID", EnvironmentVariableTarget.User);
        string clientSecret = Environment.GetEnvironmentVariable("Azure_Client_Secret", EnvironmentVariableTarget.User);
        string tenantID = Environment.GetEnvironmentVariable("Azure_Tenant_ID", EnvironmentVariableTarget.User);
        string subscriptionID = Environment.GetEnvironmentVariable("Azure_Subscription_ID", EnvironmentVariableTarget.User);

        private readonly System.Net.Http.IHttpClientFactory _clientFactory;
        public AzureService(System.Net.Http.IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public Microsoft.Azure.Management.Fluent.IAzure Authenticate()
        {
            var credentials = SdkContext.AzureCredentialsFactory
                .FromServicePrincipal(
                clientID,
                clientSecret,
                tenantID,
                AzureEnvironment.AzureGlobalCloud);

            var azure = Microsoft.Azure.Management.Fluent.Azure
                .Configure()
                .Authenticate(credentials)
                .WithSubscription(subscriptionID);

            return azure;
        }

        public UsagePayload GetUsageDetails()
        {
            string token = GetAuthToken(clientID, clientSecret, tenantID);

            string requestURL = String.Format("{0}/{1}/{2}/{3}",
                      "https://management.azure.com",
                      "subscriptions",
                      subscriptionID,
                      "providers/Microsoft.Commerce/UsageAggregates?api-version=2015-06-01-preview&reportedstartTime=2015-03-01+00%3a00%3a00Z&reportedEndTime=2015-05-18+00%3a00%3a00Z"
                      );

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestURL);

            // Add the OAuth Authorization header, and Content Type header
            request.Headers.Add(HttpRequestHeader.Authorization, "Bearer " + token);
            request.ContentType = "application/json";
            UsagePayload payload = new UsagePayload();

            try
            {
                //Calling the Rest API
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
                var usageResponse = readStream.ReadToEnd();
                payload = JsonConvert.DeserializeObject<UsagePayload>(usageResponse);
                response.Close();
                readStream.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(String.Format("{0} \n\n{1}", e.Message, e.InnerException != null ? e.InnerException.Message : ""));
                Console.ReadLine();
            }

            return payload;
        }

        public RateCardPayload GetRateCardDetails()
        {
            string token = GetAuthToken(clientID, clientSecret, tenantID);

            string requestURL = String.Format("{0}/{1}/{2}/{3}",
                      "https://management.azure.com",
                      "subscriptions",
                      subscriptionID,
                      "providers/Microsoft.Commerce/RateCard?api-version=2016-08-31-preview&$filter=OfferDurableId eq 'MS-AZR-0044P' and Currency eq 'USD' and Locale eq 'en-US' and RegionInfo eq 'IN'");
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestURL);

            request.Headers.Add(HttpRequestHeader.Authorization, "Bearer " + token);
            request.ContentType = "application/json";
            RateCardPayload payload = new RateCardPayload();
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream receiveStream = response.GetResponseStream();

                StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
                var rateCardResponse = readStream.ReadToEnd();

                payload = JsonConvert.DeserializeObject<RateCardPayload>(rateCardResponse);
                response.Close();
                readStream.Close();
            }
            catch (WebException webEx)
            {
                var resp = webEx.Response;
                var responseDetails = (new System.IO.StreamReader(resp.GetResponseStream(), true)).ReadToEnd();
                Console.WriteLine(responseDetails);
            }

            return payload;
        }

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
    }
}
