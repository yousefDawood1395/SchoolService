using Microsoft.EntityFrameworkCore;
using SchoolService.DataAccess;
using SchoolService.Domain.Schools;
using SchoolService.Domain.Shared.Pagination;
using SchoolService.Domain.Shared.Repositories;

namespace SchoolService.Infrastructure.Repositories
{
    public sealed class SchoolsRepository : ISchoolsRepository
    {
        private readonly SchoolsDbContext _dbContext;

        public SchoolsRepository(SchoolsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> CreateSchool(School input)
        {
            await _dbContext.Schools.AddAsync(input);
            await _dbContext.SaveChangesAsync();
            return input.Id;
        }

        public async Task<bool> DeleteSchool(School input)
        {
            _dbContext.Schools.Remove(input);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<School?> GetSchool(int id)
        {
            return await _dbContext.Schools.Include(i => i.Classes).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<PagedResponse<School>> SearchSchoolsByName(string? name, int pageNumber, int pageSize)
        {
            var query = _dbContext.Schools.Include(a => a.Classes).AsQueryable();
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(w => w.Name!.Contains(name));
            }
            return new PagedResponse<School>
            {
                TotalCount = await query.CountAsync(),
                PageNumber = pageNumber,
                PageSize = pageSize,
                Data = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync()
            };
        }

        public async Task<bool> UpdateClass(ClassRoom input)
        {
            if (_dbContext.ChangeTracker.Entries<ClassRoom>().Any(a => a.State == EntityState.Modified))
            {
                return await _dbContext.SaveChangesAsync() > 0;
            }
            return false;
        }

        public async Task<bool> UpdateSchool(School input)
        {
            if (_dbContext.ChangeTracker.Entries<School>().Any(a => a.State == EntityState.Modified))
            {
                return await _dbContext.SaveChangesAsync() > 0;
            }

            if (_dbContext.ChangeTracker.Entries<ClassRoom>().Any(a => (a.CurrentValues.ToObject() as ClassRoom)!.SchoolId == input.Id))
            {
                return await _dbContext.SaveChangesAsync() > 0;
            }
            return false;
        }
    }
}
