using Microsoft.EntityFrameworkCore;

namespace ParameterCache
{
    public class ProjectDbContext : DbContext
    {
        public DbSet<Project> Projects { get; set; }
        public DbSet<StaffMember> StaffMembers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Project>().ToTable("Project", "dbo")
               .HasKey(m => m.Id);

            builder.Entity<StaffMember>().ToTable("StaffMember", "dbo")
                .HasKey(m => m.Id);

            builder.Entity<StaffMember>().HasOne(m => m.Project)
                .WithMany(m => m.StaffMembers)
                .HasForeignKey(m => m.ProjectId);
        }

        public ProjectDbContext(DbContextOptions<ProjectDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.LogTo(Console.WriteLine);
            base.OnConfiguring(optionsBuilder);
        }
    }

    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<StaffMember> StaffMembers { get; set; }
    }

    public class StaffMember
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int ProjectId { get; set; }
        public Project Project { get; set; }
    }
}