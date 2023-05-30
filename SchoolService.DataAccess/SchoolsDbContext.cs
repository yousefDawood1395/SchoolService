using Microsoft.EntityFrameworkCore;
using SchoolService.Domain.Schools;
using SchoolService.Domain.Students;
using SchoolService.Domain.Teachers;
using System.Diagnostics.CodeAnalysis;

namespace SchoolService.DataAccess;
[ExcludeFromCodeCoverage]
public sealed class SchoolsDbContext : DbContext
{
    public SchoolsDbContext(DbContextOptions options) : base(options) { }

    public DbSet<School> Schools => Set<School>();
    public DbSet<ClassRoom> ClassRooms => Set<ClassRoom>();
    public DbSet<Teacher> Teachers => Set<Teacher>();
    public DbSet<TeacherClassrooms> TeacherClassrooms => Set<TeacherClassrooms>();
    public DbSet<Student> Students => Set<Student>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
    }
}