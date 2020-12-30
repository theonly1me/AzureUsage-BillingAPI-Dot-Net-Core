using System;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;
using AzureClientWebAPI.Models;
using System.Data;
using Newtonsoft.Json;
using System.IO;
using CsvHelper;
using System.Net;
using System.Net.Http.Headers;

namespace AzureClientWebAPI.Auth
{
    public interface IAzureService
    {
        public Task<string> GetResponse(string apiURL, string method);
    }

    public class AzureService : IAzureService
    {

        #region AzureDetails 
        /**
         * Azure Details         
         */

        private string clientID = "CLIENT_ID";
        private string clientSecret = "CLIENT_SECRET";
        private string tenantID = "TENANT_ID";
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
        public async Task<string> GetResponse(string apiURL, string method)
        {
            string token = GetAuthToken(clientID, clientSecret, tenantID);
            HttpClient client = new HttpClient();
            string response = "";
            client.BaseAddress = new Uri(apiURL);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            if (method == "GET")
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, client.BaseAddress);
                response = await GetRequestAsync(request, client);
            }
            else if (method == "POST")
            {
                var payload = CostManagementQuery.requestByTagPayload;
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, client.BaseAddress);
                request.Content = new StringContent(payload, Encoding.UTF8, "application/json");
                response = await PostRequestAsync(request, client);
            }
            
            //string csv = jsonToCSV(response, ",");

            return response;
        }

        public static async Task<string> GetRequestAsync(HttpRequestMessage getRequest, HttpClient client)
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

        public static async Task<string> PostRequestAsync(HttpRequestMessage postRequest, HttpClient client)
        {
            var response = await client.SendAsync(postRequest);
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

        #region JSON to csv Helper Methods
        public static DataTable jsonStringToTable(string jsonContent)
        {
            DataTable dt = JsonConvert.DeserializeObject<DataTable>(jsonContent);
            return dt;
        }

        public static string jsonToCSV(string jsonContent, string delimiter)
        {
            StringWriter csvString = new StringWriter();
            using (var csv = new CsvWriter(csvString))
            {
                //csv.Configuration.SkipEmptyRecords = true;
                //csv.Configuration.WillThrowOnMissingField = false;
                csv.Configuration.Delimiter = delimiter;

                using (var dt = jsonStringToTable(jsonContent))
                {
                    foreach (DataColumn column in dt.Columns)
                    {
                        csv.WriteField(column.ColumnName);
                    }
                    csv.NextRecord();

                    foreach (DataRow row in dt.Rows)
                    {
                        for (var i = 0; i < dt.Columns.Count; i++)
                        {
                            csv.WriteField(row[i]);
                        }
                        csv.NextRecord();
                    }
                }
            }
            return csvString.ToString();
        }

        #endregion
    }
}
