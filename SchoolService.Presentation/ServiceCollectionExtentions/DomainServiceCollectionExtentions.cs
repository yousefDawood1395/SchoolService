using SchoolService.Domain.DomainServices;
using SchoolService.Domain.Shared.Validation;

namespace SchoolService.Presentation.ServiceCollectionExtentions;
public static class DomainServiceCollectionExtentions
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services.AddScoped<ISchoolsDomainService, SchoolsDomainService>();
        services.AddScoped<ITeachersDomainService, TeachersDomainService>();
        services.AddScoped<IStudentsDomainService, StudentsDomainService>();
        services.AddScoped<IValidationEngine, ValidationEngine>();
        return services;
    }
}
