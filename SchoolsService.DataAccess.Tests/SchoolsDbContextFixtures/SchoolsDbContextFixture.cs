using Microsoft.EntityFrameworkCore;
using SchoolService.DataAccess;
using System.Diagnostics.CodeAnalysis;

namespace SchoolService.DataAccess.Tests.SchoolsDbContextFixtures;
[ExcludeFromCodeCoverage]
public class SchoolsDbContextFixture : IAsyncDisposable
{
    public SchoolsDbContext _dbContext { get; set; }
    public SchoolsDbContextFixture()
    {
        _dbContext = new SchoolsDbContext(new DbContextOptionsBuilder().UseSqlite("FileName=:memory:").Options);
        _dbContext.Database.OpenConnection();
        _dbContext.Database.EnsureCreated();
    }
    public ValueTask DisposeAsync()
    {
        Task.Run(async () =>
        {
            await _dbContext.Database.CloseConnectionAsync();
            await _dbContext.DisposeAsync();
        });

        return ValueTask.CompletedTask;
    }
}
