using Microsoft.EntityFrameworkCore;
using TimeTracker.Data.Entities;
using TimeTracker.Data.Interfaces;

namespace TimeTracker.Data.EFRepositories
{
    public class EFRepository<T> : IRepository<T> where T : BaseEntity
    {
        protected TimeTrackerDbContext _context;

        protected DbSet<T> _dbSet;

        public EFRepository(TimeTrackerDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void Delete(T entity)
        {
            if(_context.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }
            _dbSet.Remove(entity);
        }

        public async Task DeleteByIdAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            Delete(entity);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public void Update(T entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }
    }
}
