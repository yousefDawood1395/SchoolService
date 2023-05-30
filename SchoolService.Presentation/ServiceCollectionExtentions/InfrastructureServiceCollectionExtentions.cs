using SchoolService.Domain.Shared.Repositories;
using SchoolService.Infrastructure.Repositories;

namespace SchoolService.Presentation.ServiceCollectionExtentions;

public static class InfrastructureServiceCollectionExtentions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<ITeachersRepository, TeachersRepository>();
        services.AddScoped<IStudentsRepository, StudentsRepository>();
        services.AddScoped<ISchoolsRepository, SchoolsRepository>();
        return services;
    }
}
