using Microsoft.EntityFrameworkCore;
using SchoolService.DataAccess;
using SchoolService.Domain.Shared.Pagination;
using SchoolService.Domain.Shared.Repositories;
using SchoolService.Domain.Students;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolService.Infrastructure.Repositories;

public sealed class StudentsRepository : IStudentsRepository
{
    private readonly SchoolsDbContext _dbContext;

    public StudentsRepository(SchoolsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> Create(Student input)
    {
        await _dbContext.Students.AddAsync(input);
        await _dbContext.SaveChangesAsync();
        return input.Id;
    }

    public async Task<bool> Delete(Student input)
    {
        _dbContext.Students.Remove(input);
        return await _dbContext.SaveChangesAsync() > 0;
    }

    public async Task<Student?> Get(int id)
    {
        return await _dbContext.Students.FindAsync(id);
    }

    public async Task<PagedResponse<Student>> Search(string? name, int? schoolId, int? classroomId, int pageNumber, int pageSize)
    {
        var query = _dbContext.Students.AsQueryable();
        if (!string.IsNullOrEmpty(name))
        {
            query = query.Where(w => w.Name!.Contains(name));
        }

        if (schoolId is not null)
        {
            query = query.Where(w => w.SchoolId == schoolId);
        }

        if (classroomId is not null)
        {
            query = query.Where(w => w.ClassRoomId == classroomId);
        }

        return new PagedResponse<Student>
        {
            Data = await query.ToListAsync(),
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = await query.CountAsync()
        };
    }

    public async Task<bool> Update(Student input)
    {
        if (_dbContext.ChangeTracker.Entries<Student>().Any(a => a.State == EntityState.Modified))
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }
        return false;
    }
}
