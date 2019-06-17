using CosmosDb.Common.Extensions;
using System;
using System.Data.Common;
using System.Security;

namespace CosmosDb.Common
{
    public class CosmosDbConnectionString
    {
        public CosmosDbConnectionString(string connectionString)
        {
            var builder = new DbConnectionStringBuilder
            {
                ConnectionString = connectionString
            };

            if (builder.TryGetValue("AccountKey", out object key))
            {
                AuthKey = key.ToString().ToSecureString();
            }

            if (builder.TryGetValue("AccountEndpoint", out object uri))
            {
                ServiceEndpoint = new Uri(uri.ToString());
            }
        }

        public Uri ServiceEndpoint { get; set; }

        public SecureString AuthKey { get; set; }
    }
}