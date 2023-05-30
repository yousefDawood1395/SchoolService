using SchoolService.Application.Schools.Dtos;
using SchoolService.Domain.DomainServices;
using SchoolService.Domain.Schools;
using SchoolService.Domain.Shared.Exceptions;
using SchoolService.Domain.Shared.Repositories;
using SchoolService.Domain.Shared.Validation;

namespace SchoolService.Application.Schools;
public class SchoolsService : ISchoolsService
{
    private readonly ISchoolsRepository _schoolRepository;
    private readonly IValidationEngine _validationEngine;
    private readonly ISchoolsDomainService _schoolDomainService;

    public SchoolsService(ISchoolsRepository schoolRepository, IValidationEngine validationEngine, ISchoolsDomainService schoolDomainService)
    {
        _schoolRepository = schoolRepository;
        _validationEngine = validationEngine;
        _schoolDomainService = schoolDomainService;
    }

    public async Task<SchoolDto> Create(SchoolCreateDto input)
    {
        var school = input.ToSchool();
        await school.Create(_validationEngine, _schoolRepository);
        return SchoolDto.FromSchool(school);
    }

    public async Task<SchoolDto> Update(SchoolUpdateDto input)
    {
        var school = await School.Get(input.Id, _schoolRepository);
        school.SetName(input.Name);
        school.SetOpenDate(input.OpenDate);
        await school.Update(_validationEngine, _schoolRepository);
        return SchoolDto.FromSchool(school);
    }

    public async Task<bool> Delete(int id)
    {
        return await _schoolDomainService.DeleteSchool(id);
    }

    public async Task<SchoolDto> AddClass(ClassRoomAddDto input)
    {
        var school = await School.Get(input.SchoolId, _schoolRepository);
        await school.AddClass(ClassRoom.Create(null, input.Name, input.SchoolId), _schoolRepository, _validationEngine);
        return SchoolDto.FromSchool(school);
    }

    public async Task<bool> DeleteClass(ClassRoomDeleteDto input)
    {
        return await _schoolDomainService.DeleteClass(input.SchoolId, input.ClassRoomId);
    }

    public async Task<SchoolDto> UpdateClassroom(ClassRoomUpdateDto input)
    {
        var school = await School.Get(input.SchoolId, _schoolRepository);

        school.SetClassName(input.ClassRoomId, input.Name);

        var classRoom = school.Classes.FirstOrDefault(w => w.Id == input.ClassRoomId);

        await school.UpdateClass(classRoom!, _schoolRepository, _validationEngine);

        return SchoolDto.FromSchool(school);
    }
}