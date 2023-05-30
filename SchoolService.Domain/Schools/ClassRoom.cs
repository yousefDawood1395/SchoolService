using FluentValidation;
using SchoolService.Domain.Shared.Aggregates;
using SchoolService.Domain.Shared.Validation;
using System.Diagnostics.CodeAnalysis;

namespace SchoolService.Domain.Schools;

public class ClassRoom : IEntity<int>, IValidationModel<ClassRoom>
{
    private ClassRoom()
    {

    }
    public int Id { get; private set; }
    public int SchoolId { get; private set; }
    public string? Name { get; private set; }
    [ExcludeFromCodeCoverage]
    public School? School { get; private set; }
    public AbstractValidator<ClassRoom> Validator => new ClassroomValidator();

    public void SetName(string? name)
    {
        if (Name == name) return;
        Name = name;
    }

    public static ClassRoom Create(int? id, string? name, int? schoolId)
    {
        return new ClassRoom
        {
            Id = id ?? 0,
            Name = name,
            SchoolId = schoolId ?? 0
        };
    }
}