using Microsoft.EntityFrameworkCore;

namespace WebAPI.Models
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {

        }
        
        
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectWorkItem> ProjectWorkItems { get; set; }
        public DbSet<ProjectCode> ProjectCodes { get; set; }
        public DbSet<WorkItem> WorkItems { get; set; }
    }
}
