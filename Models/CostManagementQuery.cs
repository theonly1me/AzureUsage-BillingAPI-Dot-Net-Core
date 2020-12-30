using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureClientWebAPI.Models
{
    public class CostManagementQuery
    {
        public static string requestByTagPayload = "{\"type\":\"Usage\",\"timeframe\":\"MonthToDate\",\"dataset\":{\"granularity\":\"Daily\",\"filter\":{\"tags\":{\"name\":\"ms-resource-usage\",\"operator\":\"In\",\"values\":[\"azure-cloud-shell\"]}}}}";
        public static string requestByDimensionPayload = "{\"type\":\"Usage\",\"timeframe\":\"MonthToDate\",\"dataset\":{\"granularity\":\"Daily\",\"filter\":{\"dimensions\":{\"name\":\"ResourceLocation\",\"operator\":\"In\",\"values\":[\"East US\",\"West Europe\"]}}}}";
    }
}