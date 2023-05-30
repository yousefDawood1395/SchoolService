using FluentValidation;
using SchoolService.Domain.Shared.Aggregates;
using SchoolService.Domain.Shared.Exceptions;
using SchoolService.Domain.Shared.Pagination;
using SchoolService.Domain.Shared.Repositories;
using SchoolService.Domain.Shared.Validation;

namespace SchoolService.Domain.Teachers;
public class Teacher : IEntity<int>, IValidationModel<Teacher>
{
    public int Id { get; private set; }
    public string? Name { get; private set; }
    public IList<TeacherClassrooms> Classrooms { get; private set; } = new List<TeacherClassrooms>();

    public AbstractValidator<Teacher> Validator => new TeacherValidator();

    private Teacher()
    {

    }

    public async Task<int> Create(IValidationEngine validationEngine, ITeachersRepository repository)
    {
        validationEngine.Validate(this);
        return await repository.Create(this);
    }

    public async Task<bool> Update(IValidationEngine validationEngine, ITeachersRepository repository)
    {
        validationEngine.Validate(this);
        return await repository.Update(this);
    }

    public static async Task<PagedResponse<Teacher>> Search(string? name, int? schoolId, int? classroomId, int pageNumber, int pageSize, ITeachersRepository repository)
    {
        return await repository.Search(name, schoolId, classroomId, pageNumber, pageSize);
    }

    public void SetName(string? name)
    {
        if (Name == name) return;
        Name = name;
    }

    internal async Task<bool> AssignTeacherToClass(int schoolId, int classroomId, ITeachersRepository repository)
    {
        if (Classrooms.Any(a => a.ClassroomId == classroomId))
        {
            throw new DataDuplicateException();
        }

        Classrooms.Add(TeacherClassrooms.Create(0, Id, classroomId, schoolId));
        return await repository.Update(this);
    }

    internal async Task<bool> UnAssignTecherToClass(int classroomId, ITeachersRepository repository)
    {
        var existingClass = Classrooms.FirstOrDefault(w => w.ClassroomId == classroomId);

        Classrooms.Remove(existingClass);
        return await repository.Update(this);
    }

    public static Teacher Create(int? id, string? name)
    {
        return new Teacher
        {
            Id = id ?? 0,
            Name = name
        };
    }

    public static async Task<Teacher> Get(int id, ITeachersRepository repository)
    {
        var dbTeacher = await repository.Get(id);
        if (dbTeacher is null)
        {
            throw new DataNotFoundException();
        }
        return dbTeacher;
    }

    public static async Task<bool> Delete(int id, ITeachersRepository repository)
    {
        var dbTeacher = await repository.Get(id);
        if (dbTeacher is null)
        {
            throw new DataNotFoundException();
        }

        if (dbTeacher.Classrooms.Any())
        {
            throw new BusinessException("Teacher has classrooms");
        }
        return await repository.Delete(dbTeacher);
    }
}