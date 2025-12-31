using Microsoft.EntityFrameworkCore;
using Mzstruct.Base.Entities;
using Mzstruct.Base.Helpers;
using Mzstruct.DB.Contracts.IRepos;
using Mzstruct.DB.EFCore.Context;
using Mzstruct.DB.SQL.Context;
using System.Linq.Expressions;

namespace Mzstruct.DB.EFCore.Repo
{
    public abstract class EFCoreBaseRepo<TEntity> : IEFCoreBaseRepo<TEntity> where TEntity : BaseEntity
    {
        protected readonly EFContext dbContext;
        protected readonly DbSet<TEntity> entities;

        public EFCoreBaseRepo(EFContext context)
        {
            dbContext = context;
            entities = dbContext.Set<TEntity>();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await entities.ToListAsync();
        }

        public virtual async Task<TEntity?> GetByIdAsync(object id)
        {
            return await entities.FindAsync(id);
        }

        public virtual async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await entities.Where(predicate).ToListAsync();
        }

        public virtual async Task AddAsync(TEntity entity)
        {
            await entities.AddAsync(entity);
        }

        public virtual async Task SaveChangesAsync()
        {
            await dbContext.SaveChangesAsync();
        }

        public TEntity? GetById(int Id)
        {
            return this.entities.Find(Id);
        }

        public int Insert(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            entities.Add(entity);
            dbContext.Entry(entity).State = EntityState.Added;
            dbContext.SaveChanges();

            //return entity.Id;

            var value = BaseHelper.GetPropValue(entity, "Id");

            return value is int inserted
                ? inserted
                : int.TryParse(value?.ToString(), out var result)
                    ? result
                    : 0;

            //older in .Net console app =>
            //ObjectContext context = ((IObjectContextAdapter)dbContext).ObjectContext;
            //EntityKey key = context.ObjectStateManager.GetObjectStateEntry(entity).EntityKey;
            //return (int)key.EntityKeyValues[0].Value;
        }

        public void Save(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            try
            {
                entities.Add(entity);
                dbContext.Entry(entity).State = EntityState.Added;
                dbContext.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void BulkInsert(List<TEntity> entityList)
        {
            try
            {
                entities.AddRange(entityList);
                dbContext.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void QuickBulkInsert(List<TEntity> entityList, string? tableName = null)
        {
            try
            {
                SqlClientContext<TEntity>.QuickBulkInsert(entityList, tableName);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual void Update(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            entities.Update(entity);
            //dbContext.Entry(entity).State = EntityState.Modified;
            dbContext.SaveChanges();
        }

        public void Delete(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            entities.Attach(entity);
            entities.Remove(entity);
            dbContext.SaveChanges();
        }

        public void Truncate(string? tableName = null)
        {
            SqlClientContext<TEntity>.TruncateTable(tableName); //dbContext.Database.ExecuteSqlCommand($"TRUNCATE TABLE {typeof(TEntity).Name}s");
        }

        public void CreateTable(string? tableName = null)
        {
            SqlClientContext<TEntity>.CreateTable(tableName);
        }

        public void Add(TEntity entity)
        {
            dbContext.Set<TEntity>().Add(entity);
            dbContext.SaveChanges();
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            dbContext.Set<TEntity>().AddRange(entities);
            dbContext.SaveChanges();
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return dbContext.Set<TEntity>().Where(predicate);
        }

        public TEntity? Get(int id)
        {
            return dbContext.Set<TEntity>().Find(id);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return dbContext.Set<TEntity>().ToList();
        }

        public void Remove(TEntity entity)
        {
            dbContext.Set<TEntity>().Remove(entity);
            dbContext.SaveChanges();
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            dbContext.Set<TEntity>().RemoveRange(entities);
            dbContext.SaveChanges();
        }

        public void Commit()
        {
            dbContext.SaveChanges();
        }

        public void ExecuteStoreProcedure(string storeProcedureName)
        {
            try
            {
                SqlClientContext<TEntity>.ExecuteStoreProcedure(storeProcedureName); //dbContext.Database.SqlQuery<TEntity>("exec " + storeProcedureName);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public TEntity? SingleOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return dbContext.Set<TEntity>().SingleOrDefault(predicate);
        }

        public TEntity? FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return dbContext.Set<TEntity>().FirstOrDefault(predicate);
        }
    }
}
