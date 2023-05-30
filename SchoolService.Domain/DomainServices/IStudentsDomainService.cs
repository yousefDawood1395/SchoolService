namespace SchoolService.Domain.DomainServices
{
    public interface IStudentsDomainService
    {
        Task<bool> SetClass(int studentId, int? schoolId, int? classroomId);
    }
}