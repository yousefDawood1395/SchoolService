using SchoolService.Application.Schools;
using SchoolService.Application.Students;
using SchoolService.Application.Teachers;

namespace SchoolService.Presentation.ServiceCollectionExtentions;
public static class ApplicationServiceCollectionExtentions
{
    public static IServiceCollection AddApplicationLayerServices(this IServiceCollection services)
    {
        services.AddScoped<ISchoolsService, SchoolsService>();
        services.AddScoped<ISchoolsQueryService, SchoolsQueryService>();
        services.AddScoped<IStudentsService, StudentsService>();
        services.AddScoped<IStudentsQueryService, StudentsQueryService>();
        services.AddScoped<ITeachersService, TeachersService>();
        services.AddScoped<ITeachersQueryService, TeachersQueryService>();
        return services;
    }
}
