using FluentValidation;
using SchoolService.Domain.Shared.Aggregates;
using SchoolService.Domain.Shared.Exceptions;
using SchoolService.Domain.Shared.Pagination;
using SchoolService.Domain.Shared.Repositories;
using SchoolService.Domain.Shared.Validation;

namespace SchoolService.Domain.Students;

public class Student : IEntity<int>, IValidationModel<Student>
{
    private Student()
    {

    }
    public int Id { get; private set; }
    public string? Name { get; private set; }
    public int? ClassRoomId { get; private set; }
    public int? SchoolId { get; private set; }

    public AbstractValidator<Student> Validator => new StudentValidator();

    public async Task<int> Create(IStudentsRepository repository, IValidationEngine validationEngine)
    {
        validationEngine.Validate(this);
        await EnsureNoDuplicates(repository);
        return await repository.Create(this);
    }

    public async Task<bool> Update(IStudentsRepository repository, IValidationEngine validationEngine)
    {
        validationEngine.Validate(this);
        await EnsureNoDuplicates(repository);
        return await repository.Update(this);
    }

    public async Task<bool> Delete(IStudentsRepository repository)
    {
        return await repository.Delete(this);
    }

    public static async Task<PagedResponse<Student>> Search(IStudentsRepository repository, string? name, int? schoolId, int? classroomId, int pageNumber, int pageSize)
    {
        return await repository.Search(name, schoolId, classroomId, pageNumber, pageSize);
    }

    public static async Task<Student> Get(int id, IStudentsRepository repository)
    {
        var dbStudent = await repository.Get(id);

        if (dbStudent is null)
        {
            throw new DataNotFoundException();
        }

        return dbStudent;
    }

    public static Student Create(int? id, string? name, int? classroomId, int? schoolId)
    {
        return new Student
        {
            Id = id ?? 0,
            Name = name,
            ClassRoomId = classroomId,
            SchoolId = schoolId
        };
    }

    public void SetName(string? name)
    {
        if (Name == name) return;
        Name = name;
    }

    internal async Task<bool> SetClass(int? classroomId, int? schoolId, IValidationEngine validationEngine, IStudentsRepository repository)
    {
        SchoolId = schoolId;
        ClassRoomId = classroomId;
        validationEngine.Validate(this);
        return await repository.Update(this);
    }

    private async Task<bool> EnsureNoDuplicates(IStudentsRepository studentsRepository, bool throwException = true)
    {
        var dbStudent = await studentsRepository.Search(Name, SchoolId, ClassRoomId, 1, 1);
        if (Id == default)
        {
            if (dbStudent.Data.Any())
            {
                //if (throwException)
                //{
                throw new DataDuplicateException();
                //}
                //else
                //{
                //    return false;
                //}
            }
        }
        else
        {
            if (dbStudent.Data.Any(x => x.Id != Id))
            {
                //if (throwException)
                //{
                throw new DataDuplicateException();
                //}
                //else
                //{
                //    return false;
                //}
            }
        }
        return true;
    }
}