using System.Threading.Tasks;
using AzureClientWebAPI.Auth;
using AzureClientWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AzureClientWebAPI.Controllers
{
    [ApiController]
    [Route("azure")]
    public class AzureUsageController : ControllerBase
    {
        private readonly IAzureService _azureAuth;

        public AzureUsageController(ILogger<AzureUsageController> logger, IAzureService azureAuth)
        {        
            _azureAuth = azureAuth;
        }

        [HttpGet]
        [Route("getusage")]
        public async Task<IActionResult> GetUsage()
        {
            //Using the Microsoft Azure Fluent API
            //var azure = this._azureAuth.Authenticate();
            //var resourceGroup = azure.ResourceGroups.GetByName("cloud-shell-storage-centralindia");

            //Using Azure Billing & Usage API
            UsagePayload usagePayload = _azureAuth.GetUsageDetails();

            return Ok(usagePayload);

            
        }

        [HttpGet]
        [Route("getratecard")]
        public IActionResult GetRateCardDetails()
        {
            RateCardPayload rateCardPayload = _azureAuth.GetRateCardDetails();

            return Ok(rateCardPayload);

        }
    }
}
