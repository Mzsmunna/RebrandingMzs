using Microsoft.EntityFrameworkCore;
using Mzstruct.DB.EFCore.Context;
using System.Linq.Expressions;

namespace Mzstruct.DB.EFCore.Repo
{
    public abstract class BaseSqlRepo<TContext, TEntity> where TContext : DbContext where TEntity : class
    {
        protected readonly AppDBContext<TContext> _context;

        public BaseSqlRepo(AppDBContext<TContext> context)
        {
            _context = context;
        }

        public async Task AddAsync(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
        }

        public void Add(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return _context.Set<TEntity>().Where(predicate);
            
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
