using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace TransactionGenerator
{
    internal class CosmosDbConnectionString
    {
        // This class parses a Cosmos DB connection string to its Account Key and Account Endpoint components.
        public CosmosDbConnectionString(string connectionString)
        {
            var builder = new DbConnectionStringBuilder
            {
                ConnectionString = connectionString
            };

            if (builder.TryGetValue("AccountKey", out object key))
            {
                AuthKey = key.ToString();
            }

            if (builder.TryGetValue("AccountEndpoint", out object uri))
            {
                ServiceEndpoint = new Uri(uri.ToString());
            }
        }

        public Uri ServiceEndpoint { get; set; }

        public string AuthKey { get; set; }
    }
}
