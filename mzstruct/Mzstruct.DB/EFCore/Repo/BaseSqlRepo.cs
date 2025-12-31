using Mzstruct.DB.EFCore.Context;
using System.Linq.Expressions;

namespace Mzstruct.DB.EFCore.Repo
{
    public class BaseSqlRepo<T> where T : class
    {
        protected readonly EFContext _context;

        public BaseSqlRepo(EFContext context)
        {
            _context = context;
        }
        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }
        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().Where(predicate);
            
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
