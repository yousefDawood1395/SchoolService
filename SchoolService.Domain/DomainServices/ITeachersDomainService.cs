namespace SchoolService.Domain.DomainServices
{
    public interface ITeachersDomainService
    {
        Task<bool> AssignTeacherToClass(int teacherId, int classroomId, int schoolId);
        Task<bool> UnAssignTeacherFromClass(int teacherId, int classroomId);
    }
}