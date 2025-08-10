using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;
namespace MultipleChoicExam.Models
{
    public class EFCoreDbContext : IdentityDbContext<UserAccount>
    {
        public EFCoreDbContext(DbContextOptions<EFCoreDbContext> options) : base(options) { }
        public DbSet<UserAccount> UserAccount { get; set; }
        public DbSet<Subject01> Subject01 { get; set; }
        public DbSet<Question> Question { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<TestHistory> TestHistory { get; set; }
    }
}
