using AutoFixture;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using Microsoft.Extensions.DependencyInjection;
using SchoolService.DataAccess;

namespace SchoolService.BDD.Tests.Collections;
public class SchoolsWebService : IDisposable
{
    private SqliteConnection? _sqliteConnection;
    public readonly HttpClient _httpClient;
    private readonly WebApplicationFactory<Program> _webApplicationFactory;
    private readonly Fixture _fixture;
    public SchoolsWebService()
    {
        _fixture = new Fixture();
        _webApplicationFactory = new WebApplicationFactory<Program>().WithWebHostBuilder(whb =>
        {
            whb.ConfigureServices(services =>
            {
                var sdesc = services.SingleOrDefault(w => w.ServiceType == typeof(DbContextOptions<SchoolsDbContext>));
                if (sdesc is not null)
                {
                    services.Remove(sdesc);
                }

                string databaseName = _fixture.Create<string>();

                string connectionString = $"datasource={databaseName};mode=memory;cache=shared;";
                _sqliteConnection = new SqliteConnection(connectionString);
                _sqliteConnection.Open();

                services.AddDbContext<SchoolsDbContext>(x => x.UseSqlite(connectionString));

                PrepareDatabase(services.BuildServiceProvider());
            });
        });

        _httpClient = _webApplicationFactory.CreateClient();
    }

    private static void PrepareDatabase(IServiceProvider sp)
    {
        using (var scope = sp.CreateScope())
        {
            var dbcontext = scope.ServiceProvider.GetRequiredService<SchoolsDbContext>();
            dbcontext.Database.OpenConnection();
            dbcontext.Database.EnsureCreated();
            dbcontext.Database.CloseConnection();
        }
    }
    public void Dispose()
    {
        if (_sqliteConnection is not null && _sqliteConnection.State == System.Data.ConnectionState.Open)
        {
            _sqliteConnection.Close();
            _sqliteConnection.Dispose();
        }
        _webApplicationFactory.Dispose();
    }
}
