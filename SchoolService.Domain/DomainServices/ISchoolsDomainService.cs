namespace SchoolService.Domain.DomainServices
{
    public interface ISchoolsDomainService
    {
        Task<bool> DeleteClass(int schoolId, int classId);
        Task<bool> DeleteSchool(int id);
    }
}