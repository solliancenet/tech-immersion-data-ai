using Microsoft.EntityFrameworkCore;

namespace ContosoAutoOpsWeb.Data
{
    public class ContosoAutoDbContext : DbContext
    {
        public ContosoAutoDbContext(DbContextOptions<ContosoAutoDbContext> options): base(options)
        {

        }

        public DbSet<Models.Product> Products { get; set; }
        public DbQuery<Models.UpdateabilityMessage> Updateability { get; set; }
    }
}
