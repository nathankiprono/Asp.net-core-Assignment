using Microsoft.EntityFrameworkCore;

namespace Queue_Management_System.QueueDbContext
{
    public class QueueDbConnectionDbContext : DbContext
    {
        public QueueDbConnectionDbContext(DbContextOptions<QueueDbConnectionDbContext> options) : base(options)
        {
        }
        public DbSet<Models.Services> Services { get; set; }
        public DbSet<Models.serviceproviders> Serviceproviders { get; set; }
        public DbSet<Models.Check_In> Check_Ins { get; set; }

        public DbSet<Models.Users> Users { get; set; }
    }
}
