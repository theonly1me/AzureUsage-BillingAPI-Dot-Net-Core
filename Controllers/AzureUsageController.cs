using System.Threading.Tasks;
using AzureClientWebAPI.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AzureClientWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AzureUsageController : ControllerBase
    {
        private readonly IAzureAuth _azureAuth;

        public AzureUsageController(ILogger<AzureUsageController> logger, IAzureAuth azureAuth)
        {        
            _azureAuth = azureAuth;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsage()
        {
            //Using the Microsoft Azure Fluent API
            //var azure = this._azureAuth.Authenticate();
            //var resourceGroup = azure.ResourceGroups.GetByName("cloud-shell-storage-centralindia");

            //Using Azure Billing & Usage API
            UsagePayload usagePayload = _azureAuth.GetUsageDetails();

            return Ok(usagePayload);
        }
    }
}
