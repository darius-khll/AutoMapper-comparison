using EntityFramework.BulkInsert.Extensions;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;

namespace AutoMapperComparison.Models
{
    public class AppDbContextInitializer : DropCreateDatabaseAlways<AppDbContext>
    {
        protected override void Seed(AppDbContext context)
        {
            User user = context.Users.Add(new User { Name = "Test" });

            context.SaveChanges();

            List<Address> addresses = new List<Address>();

            for (int i = 0; i < 500000; i++)
            {
                addresses.Add(new Address { Id = i, Code = 1, Title = "Test", UserId = user.Id });
            }

            context.BulkInsert(addresses);

            base.Seed(context);
        }
    }

    public class AppDbContext : DbContext
    {
        static AppDbContext()
        {
            Database.SetInitializer(new AppDbContextInitializer());

            //Database.SetInitializer<AppDbContext>(null);
        }
        
        //"Data Source=.;Initial Catalog=AppDbContext;User Id=Sa;Password=password";
        public AppDbContext()
            : base(new SqlConnection(@"Data Source=.;Initial Catalog=AppDbContext;Integrated Security=True"), contextOwnsConnection: true)
        {
            Configuration.AutoDetectChangesEnabled = false;
            Configuration.EnsureTransactionsForFunctionsAndCommands = false;
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
            Configuration.ValidateOnSaveEnabled = false;
            Configuration.UseDatabaseNullSemantics = false;
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Address> Addresses { get; set; }
    }
}
