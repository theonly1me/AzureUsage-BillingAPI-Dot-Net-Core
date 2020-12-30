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

        private static string subscriptionId = "YOUR_SUBSCRIPTION_ID";

        #region URIs
        /**
         * Azure Rest API URIs
         */
        private string baseURI = "https://management.azure.com/";
        private string billAccsURI = "providers/Microsoft.Billing/billingAccounts?api-version=2019-10-01-preview";
        private static string billingAccountId = "1234:5678";
        private string billAccsUsgURI = "providers/Microsoft.Billing/BillingAccounts/" + billingAccountId + "/providers/Microsoft.Consumption/usageDetails?api-version=2019-10-01";
        private string subUsgDtlListURI = "subscriptions/" + subscriptionId + "/providers/Microsoft.Consumption/usageDetails?$filter=properties/usageStart eq '2018-06-01' and properties/usageEnd eq '2020-12-30'&api-version=2019-10-01";
        private string subUsgDtlListTagsURI = "subscriptions/" + subscriptionId + "/providers/Microsoft.Consumption/usageDetails?$filter=properties/usageStart eq '2019-06-01' and properties/usageEnd eq '2020-12-30' and tags eq 'ms-resource-usage:azure-cloud-shell'&api-version=2019-10-01";
        #endregion



        [HttpGet]
        [Route("usage/subscription")]
        public async Task<IActionResult> GetSubscriptionUsageList()
        {
            string apiURL = baseURI + subUsgDtlListURI;

            var result = await _azureAuth.GetResponse(apiURL);

            return Ok(result);
        }

        [HttpGet]
        [Route("usage/subscription/tags")]
        public async Task<IActionResult> GetSubscriptionUsageListByTags()
        {
            string apiURL = baseURI + subUsgDtlListTagsURI;

            var result = await _azureAuth.GetResponse(apiURL);

            return Ok(result);
        }

        [HttpGet]
        [Route("billingaccounts")]
        public async Task<IActionResult> GetBillingAccounts()
        {
            string apiURL = baseURI + billAccsURI;

            var result = await _azureAuth.GetResponse(apiURL);

            return Ok(result);
        }

        [HttpGet]
        [Route("usage/billingaccount")]
        public async Task<IActionResult> GetUsageByBilingAccounts()
        {
            string apiURL = baseURI + billAccsUsgURI;

            var result = await _azureAuth.GetResponse(apiURL);

            return Ok(result);
        }

        //https://localhost:44305/api/v1/usage/subscription
        //https://localhost:44305/api/v1/usage/subscription/tags

        //https://localhost:44305/api/v1/billingaccounts
        //https://localhost:44305/api/v1/usage/billingaccount

    }
}
