using Microsoft.EntityFrameworkCore;

namespace ContosoAutoOpsWeb.Data
{
    public class ContosoAutoDbReadOnlyContext : DbContext
    {
        public ContosoAutoDbReadOnlyContext(DbContextOptions<ContosoAutoDbReadOnlyContext> options) : base(options)
        {

        }

        public DbSet<Models.Product> Products { get; set; }
        public DbSet<Models.SalesOrderDetail> SalesOrderDetails { get; set; }
        public DbSet<Models.SalesOrderHeader> SalesOrderHeaders { get; set; }
        public DbSet<Models.SalesPerson> SalesPeople { get; set; }
        public DbSet<Models.SalesTerritory> SalesTerritories { get; set; }
        public DbSet<Models.Store> Stores { get; set; }

        public DbQuery<Models.UpdateabilityMessage> Updateability { get; set; }
    }
}