using Microsoft.EntityFrameworkCore;
using SchoolService.DataAccess;
using SchoolService.Domain.Shared.Pagination;
using SchoolService.Domain.Shared.Repositories;
using SchoolService.Domain.Teachers;

namespace SchoolService.Infrastructure.Repositories;
public class TeachersRepository : ITeachersRepository
{
    private readonly SchoolsDbContext _dbContext;

    public TeachersRepository(SchoolsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> Create(Teacher input)
    {
        await _dbContext.Teachers.AddAsync(input);
        await _dbContext.SaveChangesAsync();
        return input.Id;
    }

    public async Task<bool> Delete(Teacher input)
    {
        _dbContext.Teachers.Remove(input);
        return await _dbContext.SaveChangesAsync() > 0;
    }

    public async Task<Teacher?> Get(int id)
    {
        return await _dbContext.Teachers.Include(i => i.Classrooms).FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task<Teacher?> GetRow(int id)
    {
        return await _dbContext.Teachers.FindAsync(id);
    }

    public async Task<PagedResponse<Teacher>> Search(string? name, int? schoolId, int? classroomId, int pageNumber, int pageSize)
    {
        var query = _dbContext.Teachers.Include(i => i.Classrooms).AsQueryable();
        if (!string.IsNullOrEmpty(name))
        {
            query = query.Where(w => w.Name!.Contains(name));
        }

        if (schoolId is not null)
        {
            query = query.Where(w => w.Classrooms.Any(a => a.SchoolId == schoolId));
        }


        if (classroomId is not null)
        {
            query = query.Where(w => w.Classrooms.Any(a => a.ClassroomId == classroomId));
        }

        query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

        return new PagedResponse<Teacher>
        {
            Data = await query.ToListAsync(),
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = await query.CountAsync()
        };
    }

    public async Task<bool> Update(Teacher input)
    {
        if (_dbContext.ChangeTracker.Entries<Teacher>().Any(a => a.State == EntityState.Modified))
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }
        if (_dbContext.ChangeTracker.Entries<TeacherClassrooms>().Any(a => (a.CurrentValues.ToObject() as TeacherClassrooms)!.TeacherId == input.Id))
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }
        return false;
    }
}
