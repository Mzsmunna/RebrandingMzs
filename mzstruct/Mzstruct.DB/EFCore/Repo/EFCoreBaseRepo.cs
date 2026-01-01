using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Mzstruct.Base.Entities;
using Mzstruct.Base.Helpers;
using Mzstruct.Base.Models;
using Mzstruct.DB.Contracts.IRepos;
using Mzstruct.DB.EFCore.Context;
using Mzstruct.DB.SQL.Context;
using System;
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

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken token = default)
        {
            return await entities.ToListAsync(token);
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsNoTrackAsync(CancellationToken token = default)
        {
            return await entities.AsNoTracking().ToListAsync(token);
        }

        public virtual async Task<TEntity?> GetByIdAsync(string id, CancellationToken token = default)
        {
            return await entities.FindAsync(id, token);
        }

        public virtual async Task<TEntity?> GetByIdAsNoTrackAsync(string id, CancellationToken token = default)
        {
            //return await entities.Where(x => x.Id == id).AsNoTracking().FirstOrDefaultAsync(token);
            return await entities.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, token);
        }

        public virtual async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken token = default)
        {
            return await entities.Where(predicate).ToListAsync(token);

            //Blogs
            //.IgnoreQueryFilters() // ignore all global filters defined in the model builder
            //.IgnoreQueryFilters(["SoftDeleteFilter"]) // ignore specific filters defined in the model builder
            //.Include(e => e.Posts) // eager loading | joins
            //.Include(e => e.Conttributors) // eager looading | multiple joins
            //.AsSplitQuery() // to avoid cartesian explosion -> multiple queries

            //.Include(e => e.Posts) // eager loading | joins
            //.ThenInclude(e => e.Comments) // eager looading | multiple joins
            // no cartesian explosion

            //var summary = data
            //    .GroupBy(o => new { o.CustomerId, o.Status })
            //    .Select(g => new
            //    {
            //        g.Key.CustomerId,
            //        g.Key.Status,
            //        TotalAmount = g.Sum(x => x.Amount)
            //    }
            //});

            //var specials = db.Products
            //    .Where(p => p.Price > 0)
            //    .AsEnumerable()
            //    .Where(p => IsSpecial(p));

            //var bytes = data.ToLookUp(o => o.CustomerId);

            //int pageIndex = 2, pageSize = 10;
            //var page = db.Users
            //.OrderBy(u => u.Id)
            //.Skip((pageIndex - 1) * pageSize)
            //. Take (pageSize)
            //.ToList();

            //int pageIndex = 2, pageSize = 10;
            //int key =100
            //var page = db. Users
            //.Where(w => w.Id > key)
            //.OrderBy(u => u.Id)
            //. Take (pageSize)
            //.ToList();
        }

        public virtual async Task<IEnumerable<TEntity>> FindAsNoTrackAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken token = default)
        {
            return await entities.Where(predicate).AsNoTracking().ToListAsync(token);
        }

        public virtual async Task<List<TEntity>> ExecuteSPAsync(string spName, List<SqlParameter>? sqlParams)
        {
            if (string.IsNullOrEmpty(spName) || string.IsNullOrWhiteSpace(spName)) return [];
            if (sqlParams is null || sqlParams.Count <= 0)
            {
                var resultNoParams = await entities.FromSqlInterpolated($"exec {spName}").AsNoTracking().ToListAsync();
                return resultNoParams;
            }
            var allSqlParams = string.Join(",", sqlParams);
            var result = await entities.FromSqlInterpolated($"exec {spName} {allSqlParams}").AsNoTracking().ToListAsync();
            return result;
        }

        public virtual async Task<List<TModel>> ExecuteAnySPAsync<TModel>(string spName, List<SqlParameter>? sqlParams) where TModel : class, new()
        {
            if (string.IsNullOrEmpty(spName) || string.IsNullOrWhiteSpace(spName)) return [];
            if (sqlParams is null || sqlParams.Count <= 0)
            {
                var resultNoParams = await dbContext.Set<TModel>().FromSqlInterpolated($"exec {spName}").AsNoTracking().ToListAsync();
                return resultNoParams;
            }
            var allSqlParams = string.Join(",", sqlParams);
            var result = await dbContext.Set<TModel>().FromSqlInterpolated($"exec {spName} {allSqlParams}").AsNoTracking().ToListAsync();
            return result;
        }

        public virtual async Task<List<TEntity>> ExecuteRawSqlAsync(string sql, List<SqlParameter>? sqlParams)
        {
            if (string.IsNullOrEmpty(sql)) return [];
            if (sqlParams is null || sqlParams.Count <= 0)
            {
                var resultNoParams = await entities.FromSqlRaw(sql).AsNoTracking().ToListAsync();
                return resultNoParams;
            }
            var allSqlParams = string.Join(",", sqlParams);
            var result = await entities.FromSqlRaw(sql, sqlParams).AsNoTracking().ToListAsync();
            return result;
        }

        public virtual async Task<List<TModel>> ExecuteAnyRawSqlAsync<TModel>(string sql, List<SqlParameter> sqlParams) where TModel : class, new()
        {
            if (string.IsNullOrEmpty(sql)) return [];
            if (sqlParams is null || sqlParams.Count <= 0)
            {
                var resultNoParams = await dbContext.Set<TModel>().FromSqlRaw(sql).AsNoTracking().ToListAsync();
                return resultNoParams;
            }
            var allSqlParams = string.Join(",", sqlParams);
            var result = await dbContext.Set<TModel>().FromSqlRaw(sql, sqlParams).AsNoTracking().ToListAsync();
            return result;
        }

        public virtual async Task<int> UpdateDbRawSqlAsync(string sql, List<SqlParameter> sqlParams)
        {
            if (string.IsNullOrEmpty(sql)) return 0;
            var resultNoParams = await dbContext.Database.ExecuteSqlRawAsync(sql);
            return resultNoParams;
        }

        public virtual async Task AddAsync(TEntity entity, CancellationToken token = default)
        {
            await entities.AddAsync(entity, token);
        }

        public virtual async Task AddRangeAsync(List<TEntity> entityList, CancellationToken token = default)
        {
            try
            {
                await entities.AddRangeAsync(entityList, token);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual async Task SaveChangesAsync(CancellationToken token = default)
        {
            await dbContext.SaveChangesAsync(token);
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
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            dbContext.Set<TEntity>().AddRange(entities);
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
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            dbContext.Set<TEntity>().RemoveRange(entities);
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
