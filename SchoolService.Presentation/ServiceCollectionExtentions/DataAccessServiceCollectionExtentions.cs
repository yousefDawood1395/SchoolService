using Microsoft.EntityFrameworkCore;
using SchoolService.DataAccess;

namespace SchoolService.Presentation.ServiceCollectionExtentions;
public static class DataAccessServiceCollectionExtentions
{
    private const string CONNECTION_STRING = "SchoolsDbConnectionString";
    public static IServiceCollection AddDataAccessServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<SchoolsDbContext>(opt => opt.UseSqlServer(configuration.GetConnectionString(CONNECTION_STRING)).EnableSensitiveDataLogging(true).EnableDetailedErrors(true));
        return services;
    }
}
