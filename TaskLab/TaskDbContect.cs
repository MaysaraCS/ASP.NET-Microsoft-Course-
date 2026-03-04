using Microsoft.EntityFrameworkCore;

namespace TaskLab
{
    public class TaskDbContext : DbContext
    {
        public TaskDbContext(DbContextOptions<TaskDbContext> options) : base(options)
        {
        }
        public TaskDbContext() : base()
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=tasks.db");
            }
        }

        public DbSet<Task> Tasks { get; set; }
    }
}
