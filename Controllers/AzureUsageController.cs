using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AzureClientWebAPI.Auth;
using Microsoft.AspNetCore.Mvc;


namespace AzureClientWebAPI.Controllers
{
    [ApiController]
    [Route("api/v1/")]
    public class AzureUsageController : ControllerBase
    {
        private readonly IAzureService _azureAuth;

        public AzureUsageController(IAzureService azureAuth)
        {
            _azureAuth = azureAuth;
        }

        private static string subscriptionId = "5f691259-4a3c-4a19-b794-17c2e5449bc0";

        #region URIs
        /**
         * Azure Rest API URIs
         */
        private string baseURI = "https://management.azure.com/";
        //Billing Account URIs
        private string billAccsURI = "providers/Microsoft.Billing/billingAccounts?api-version=2019-10-01-preview";
        private static string billingAccountId = "1234:5678";
        private string billAccsUsgURI = "providers/Microsoft.Billing/BillingAccounts/" + billingAccountId + "/providers/Microsoft.Consumption/usageDetails?api-version=2019-10-01";
        //Usage API URIs
        private string subUsgDtlListURI = "subscriptions/" + subscriptionId + "/providers/Microsoft.Consumption/usageDetails?$filter=properties/usageStart eq '2018-06-01' and properties/usageEnd eq '2020-12-30'&api-version=2019-10-01";
        private string subUsgDtlListTagsURI = "subscriptions/" + subscriptionId + "/providers/Microsoft.Consumption/usageDetails?$filter=properties/usageStart eq '2019-06-01' and properties/usageEnd eq '2020-12-30' and tags eq 'ms-resource-usage:azure-cloud-shell'&api-version=2019-10-01";
        //Cost Management API URIs
        private string costMgmtQrySubURI = "subscriptions/" + subscriptionId + "/providers/Microsoft.CostManagement/query?api-version=2019-11-01";
        private string costMgmtDimSubURI = "subscriptions/" + subscriptionId + "/providers/Microsoft.CostManagement/dimensions?api-version=2019-11-01";
        private string costMgmtDimSubTagsURI = "subscriptions/" + subscriptionId + "/providers/Microsoft.CostManagement/dimensions?$filter=properties/usageStart eq '2019-06-01' and properties/usageEnd eq '2020-12-30' and tags eq 'ms-resource-usage:azure-cloud-shell'&api-version=2019-11-01";
        private string costMgmtExpSubURI = "subscriptions/" + subscriptionId + "/providers/Microsoft.CostManagement/exports?api-version=2020-06-01";
        private string costMgmtExpSubTagsURI = "subscriptions/" + subscriptionId + "/providers/Microsoft.CostManagement/exports?$filter=properties/usageStart eq '2019-06-01' and properties/usageEnd eq '2020-12-30' and tags eq 'ms-resource-usage:azure-cloud-shell'&api-version=2020-06-01";
        #endregion


        #region Azure Usage API
        [HttpGet]
        [Route("usage/subscription")]
        public async Task<IActionResult> GetSubscriptionUsageList()
        {
            string apiURL = baseURI + subUsgDtlListURI;

            var result = await _azureAuth.GetResponse(apiURL, "GET");

            return Ok(result);
        }

        [HttpGet]
        [Route("usage/subscription/tags")]
        public async Task<IActionResult> GetSubscriptionUsageListByTags()
        {
            string apiURL = baseURI + subUsgDtlListTagsURI;

            var result = await _azureAuth.GetResponse(apiURL, "GET");
            //HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            //response.Content = new StringContent(result);
            //response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/csv");
            //response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = "export.csv" };
            return Ok(result);
        }


        [HttpGet]
        [Route("usage/billingaccount")]
        public async Task<IActionResult> GetUsageByBilingAccounts()
        {
            string apiURL = baseURI + billAccsUsgURI;

            var result = await _azureAuth.GetResponse(apiURL, "GET");

            return Ok(result);
        }
        #endregion

        #region Azure Cost Management API
        [HttpGet]
        [Route("costmgmt/query/subscription")]
        public async Task<IActionResult> QueryUsageBySubscription()
        {
            string apiURL = baseURI + costMgmtQrySubURI;

            var result = await _azureAuth.GetResponse(apiURL, "POST");

            return Ok(result);
        }

        [HttpGet]
        [Route("costmgmt/dimensions/subscription")]
        public async Task<IActionResult> DimensionsBySubscription()
        {
            string apiURL = baseURI + costMgmtDimSubURI;

            var result = await _azureAuth.GetResponse(apiURL, "GET");

            return Ok(result);
        }

        [HttpGet]
        [Route("costmgmt/dimensions/subscription/tags")]
        public async Task<IActionResult> DimensionsByTags()
        {
            string apiURL = baseURI + costMgmtDimSubTagsURI;

            var result = await _azureAuth.GetResponse(apiURL, "GET");

            return Ok(result);
        }

        [HttpGet]
        [Route("costmgmt/exports/subscription")]
        public async Task<IActionResult> ExportsBySubscription()
        {
            string apiURL = baseURI + costMgmtExpSubURI;

            var result = await _azureAuth.GetResponse(apiURL, "GET");

            return Ok(result);
        }

        [HttpGet]
        [Route("costmgmt/exports/subscription/tags")]
        public async Task<IActionResult> ExportsByTags()
        {
            string apiURL = baseURI + costMgmtExpSubTagsURI;

            var result = await _azureAuth.GetResponse(apiURL, "GET");

            return Ok(result);
        }

        #endregion

        #region Azure Billing Accounts API
        [HttpGet]
        [Route("billingaccounts")]
        public async Task<IActionResult> GetBillingAccounts()
        {
            string apiURL = baseURI + billAccsURI;

            var result = await _azureAuth.GetResponse(apiURL, "GET");

            return Ok(result);
        }
        #endregion


        //GET
        //https://localhost:44305/api/v1/usage/subscription
        //https://localhost:44305/api/v1/usage/subscription/tags

        //https://localhost:44305/api/v1/costmgmt/query/subscription
        //https://localhost:44305/api/v1/costmgmt/dimensions/subscription
        //https://localhost:44305/api/v1/costmgmt/dimensions/subscription/tags        
        //https://localhost:44305/api/v1/costmgmt/exports/subscription
        //https://localhost:44305/api/v1/costmgmt/exports/subscription/tags   

        //https://localhost:44305/api/v1/billingaccounts
        //https://localhost:44305/api/v1/usage/billingaccount  
    }
}
