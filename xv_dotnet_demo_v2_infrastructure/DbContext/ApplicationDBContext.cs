using Microsoft.EntityFrameworkCore;
using xv_dotnet_demo_v2_domain.Entities;

namespace xv_dotnet_demo_v2_infrastructure.DbContext
{
    public class ApplicationDBContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public DbSet<Message> Message { get; set; }

        public DbSet<Names> Names { get; set; }

        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }
    }
}
