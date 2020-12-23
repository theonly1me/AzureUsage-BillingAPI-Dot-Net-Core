using System.Threading.Tasks;
using AzureClientWebAPI.Auth;
using AzureClientWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AzureClientWebAPI.Controllers
{
    [ApiController]    
    public class AzureUsageController : ControllerBase
    {
        private readonly IAzureService _azureService;

        public AzureUsageController(ILogger<AzureUsageController> logger, IAzureService azureService)
        {
            _azureService = azureService;
        }

        [HttpGet]
        [Route("/getusage")]
        public IActionResult GetUsage()
        {
            //Using the Microsoft Azure Fluent API
            //var azure = this._azureAuth.Authenticate();
            //var resourceGroup = azure.ResourceGroups.GetByName("cloud-shell-storage-centralindia");

            //Using Azure Billing & Usage API
            UsagePayload usagePayload = _azureService.GetUsageDetails();

            return Ok(usagePayload);
        }

        [HttpGet]
        [Route("/getratecard")]
        public IActionResult GetRateCardDetails()
        {
            RateCardPayload rateCardPayload = _azureService.GetRateCardDetails();

            return Ok(rateCardPayload);
        }
    }
}
