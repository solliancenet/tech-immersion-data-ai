using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Newtonsoft.Json;

namespace TechImmersion.CarEventProcessor.Helpers
{
    public static class CosmosDbHelper
    {
        // Converts a Cosmos DB Document to the passed in type.
        public static async Task<T> ReadAsAsync<T>(this Document d)
        {
            using (var ms = new MemoryStream())
            using (var reader = new StreamReader(ms))
            {
                d.SaveTo(ms);
                ms.Position = 0;
                return JsonConvert.DeserializeObject<T>(await reader.ReadToEndAsync());
            }
        }
    }
}
