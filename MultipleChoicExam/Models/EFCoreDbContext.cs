using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
namespace MultipleChoicExam.Models
{
    public class EFCoreDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=FC-HAN\SQLEXPRESS; Database=TestProjectDB;
                                    Trusted_Connection=True;TrustServerCertificate=True");
        }
        public DbSet<UserAccount> UserAccount { get; set; }
    }
}
