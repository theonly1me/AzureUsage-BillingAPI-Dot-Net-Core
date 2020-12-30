using System.Collections.Generic;


namespace AzureClientWebAPI.Models
{
    public class Properties
    {
        public string displayName { get; set; }
        public string agreementType { get; set; }
        public string customerType { get; set; }
        public string accountType { get; set; }
        public string organizationId { get; set; }
        public object address { get; set; }
    }

    public class BillingAccount
    {
        public string id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public Properties properties { get; set; }
    }

    public class BillingAcounts
    {
        public List<BillingAccount> billingAccounts { get; set; }
    }    
}
