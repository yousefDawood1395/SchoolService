using SchoolService.Application.Schools.Dtos;

namespace SchoolService.Application.Schools;
public interface ISchoolsService
{
    Task<SchoolDto> AddClass(ClassRoomAddDto input);
    Task<SchoolDto> Create(SchoolCreateDto input);
    Task<bool> Delete(int id);
    Task<bool> DeleteClass(ClassRoomDeleteDto input);
    Task<SchoolDto> Update(SchoolUpdateDto input);
    Task<SchoolDto> UpdateClassroom(ClassRoomUpdateDto input);
}