using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;

namespace UserApp.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
             base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS01;Database=master;User Id=UserApp;Password=12345678;Trusted_Connection=True;TrustServerCertificate=true;") ;
        }

        public DbSet<User> Users { get; set; }
    }
}
