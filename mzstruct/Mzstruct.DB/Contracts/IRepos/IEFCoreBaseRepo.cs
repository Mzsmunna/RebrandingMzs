using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Mzstruct.DB.Contracts.IRepos
{
    public interface IEFCoreBaseRepo<TEntity> where TEntity : class
    {
        //asynchronous methods
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity?> GetByIdAsync(object id);
        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
        Task AddAsync(TEntity entity);
        Task SaveChangesAsync();

        //synchronous methods
        TEntity? Get(int id);
        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);
        void Remove(TEntity entity);
        void CreateTable(string? tableName = null);
        void Commit();
        void ExecuteStoreProcedure(string storeProcedureName);
        void Truncate(string? tableName = null);
        void RemoveRange(IEnumerable<TEntity> entities);
        TEntity? SingleOrDefault(Expression<Func<TEntity, bool>> predicate);
        TEntity? FirstOrDefault(Expression<Func<TEntity, bool>> predicate);
        TEntity? GetById(int Id);
        int Insert(TEntity entity);
        void Save(TEntity entity);
        void BulkInsert(List<TEntity> entityList);
        void QuickBulkInsert(List<TEntity> entityList, string? tableName = null);
        void Update(TEntity entity);
        void Delete(TEntity entity);
    }
}
