using Microsoft.EntityFrameworkCore;
using WebApplicationVKTest.Model.Entities;

namespace WebApplicationVKTest.Controllers
{
    public class ApplicationContext: DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<UserState> UserStates { get; set; }
        

        public ApplicationContext()
        {
            Database.EnsureCreated();
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = "appsettings.json";

            IConfiguration Configuration = new ConfigurationBuilder().AddJsonFile(connectionString)
                .AddEnvironmentVariables()
                .Build();

            optionsBuilder.UseNpgsql(Configuration.GetConnectionString("ConnectionStringDB"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) 
            => modelBuilder.Entity<User>()
            .Property(b => b.Id)
            .UseIdentityAlwaysColumn();

    }
}