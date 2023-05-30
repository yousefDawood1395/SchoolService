using FluentValidation;
using SchoolService.Domain.Shared.Aggregates;
using SchoolService.Domain.Shared.Exceptions;
using SchoolService.Domain.Shared.Pagination;
using SchoolService.Domain.Shared.Repositories;
using SchoolService.Domain.Shared.Validation;

namespace SchoolService.Domain.Schools;
public sealed class School : IEntity<int>, IValidationModel<School>
{
    private School()
    {

    }
    public int Id { get; private set; }
    public string? Name { get; private set; }
    public DateTime? OpenDate { get; private set; }
    public int ClassesCount => Classes.Count;
    public IList<ClassRoom> Classes { get; private set; } = new List<ClassRoom>();
    public AbstractValidator<School> Validator => new SchoolValidator();

    public async Task<int> Create(IValidationEngine validationEngine, ISchoolsRepository repository)
    {
        validationEngine.Validate(this);
        await EnsureNoDuplicate(repository);
        return await repository.CreateSchool(this);
    }

    public async Task<bool> Update(IValidationEngine validationEngine, ISchoolsRepository repository)
    {
        validationEngine.Validate(this);
        await EnsureNoDuplicate(repository);
        return await repository.UpdateSchool(this);
    }


    public async Task<int> AddClass(ClassRoom input, ISchoolsRepository repository, IValidationEngine validationEngine)
    {
        validationEngine.Validate(input);

        EnsureNoClassRoomDuplicates(input);
        Classes.Add(input);

        await repository.UpdateSchool(this);

        return input.Id;
    }

    public async Task<bool> UpdateClass(ClassRoom input, ISchoolsRepository repository, IValidationEngine validationEngine)
    {
        validationEngine.Validate(input);

        var classRoom = Classes.FirstOrDefault(w => w.Id == input.Id);

        if (classRoom is null)
        {
            throw new DataNotFoundException();
        }

        EnsureNoClassRoomDuplicates(input);

        return await repository.UpdateClass(input);
    }

    public void SetName(string? name)
    {
        if (Name == name) return;
        Name = name;
    }

    public void SetClassName(int classroomId, string? name)
    {
        var classRoom = Classes.FirstOrDefault(a => a.Id == classroomId);
        if (classRoom is null)
        {
            throw new DataNotFoundException();
        }

        if (classRoom.Name == name) return;

        classRoom.SetName(name);
    }

    public void SetOpenDate(DateTime? openDate)
    {
        if (OpenDate == openDate) return;
        OpenDate = openDate;
    }

    internal async Task<bool> Delete(ISchoolsRepository repository)
    {
        return await repository.DeleteSchool(this);
    }

    internal async Task<bool> DeleteClass(int id, ISchoolsRepository repository)
    {
        var dbClass = Classes.FirstOrDefault(w => w.Id == id);

        if (dbClass is null)
        {
            throw new DataNotFoundException();
        }

        Classes.Remove(dbClass);

        return await repository.UpdateSchool(this);
    }

    private async Task<bool> EnsureNoDuplicate(ISchoolsRepository repository, bool throwExcepion = true)
    {
        var dbSchool = await repository.SearchSchoolsByName(Name!, 1, 1);
        if (Id == default)
        {
            if (dbSchool.Data.Any())
            {
                //if (throwExcepion)
                //{
                throw new DataDuplicateException();
                //}
                //return false;
            }
        }
        else
        {
            if (dbSchool.Data.Where(w => w.Id != Id).Any())
            {
                //if (throwExcepion)
                //{
                throw new DataDuplicateException();
                //}
                //return false;
            }
        }
        return true;
    }

    private bool EnsureNoClassRoomDuplicates(ClassRoom classRoom, bool throwExcepion = true)
    {

        var existingClass = Classes.FirstOrDefault(w => w.Name == classRoom.Name);

        if (classRoom.Id == default && existingClass is not null)
        {
            //if (throwExcepion)
            //{
            throw new DataDuplicateException();
            //}
            //else
            //{
            //    return false;
            //}
        }

        if (existingClass is not null && classRoom.Id != existingClass.Id)
        {
            //if (throwExcepion)
            //{
            throw new DataDuplicateException();
            //}
            //else
            //{
            //    return false;
            //}
        }

        return true;
    }

    public static School Create(int? id, string? name, DateTime? OpenDate)
    {
        return new School
        {
            Id = id ?? 0,
            Name = name,
            OpenDate = OpenDate
        };
    }

    public static async Task<School> Get(int id, ISchoolsRepository repository)
    {
        var dbObject = await repository.GetSchool(id);
        if (dbObject is null)
        {
            throw new DataNotFoundException();
        }
        return dbObject;
    }

    public static async Task<PagedResponse<School>> SearchSchools(string? name, int pageNumbe, int pageSize, ISchoolsRepository repository)
    {
        return await repository.SearchSchoolsByName(name, pageNumbe, pageSize);
    }
}
