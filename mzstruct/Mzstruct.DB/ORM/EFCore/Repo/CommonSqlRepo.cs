using InsightinCloud.Domain.Interface;
using Microsoft.EntityFrameworkCore;
using Mzstruct.Base.Helpers;
using Mzstruct.DB.ORM.EFCore.Context;
using Mzstruct.DB.SQL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace InsightinCloud.Infrastructure.SQLRepository
{
    public class CommonSqlRepo<TEntity> : ISqlRepository<TEntity> where TEntity : class
    {
        private readonly SqlDbContext dbContext;
        private readonly DbSet<TEntity> entities;

        public CommonSqlRepo(SqlDbContext context)
        {
            dbContext = context;
            entities = dbContext.Set<TEntity>();
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

        public void Update(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            dbContext.Entry(entity).State = EntityState.Modified;
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
