using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using WebApplicationVKTest.Model.Entities;

namespace WebApplicationVKTest.Controllers
{
    public class PostgreContext: DbContext
    {
        public DbSet<User> Users { get; set; }= null;
        public IConfiguration Configuration { get; }


        public PostgreContext(string settingsFile)
        {
            Configuration = new ConfigurationBuilder().AddJsonFile(settingsFile)
                .AddEnvironmentVariables()
                .Build();
            Database.EnsureCreated();
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(Configuration.GetConnectionString("ConnectionStringDB"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) 
            => modelBuilder.Entity<User>()
            .Property(b => b.Id)
            .UseIdentityAlwaysColumn();

    }

}
