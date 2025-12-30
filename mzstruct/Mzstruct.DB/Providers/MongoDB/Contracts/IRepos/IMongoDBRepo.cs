using MongoDB.Driver;
using Mzstruct.Base.Entities;
using Mzstruct.Base.Models;
using Mzstruct.DB.Providers.MongoDB.Models;

namespace Mzstruct.DB.Providers.MongoDB.Contracts.IRepos
{
    public interface IMongoDBRepo <T> where T : BaseEntity //class
    {
        FilterDefinition<T> BuildFilter(string? id, List<SearchField>? searchQueries = null);
        Task<T?> Get(MongoDBParam query);
        Task<T?> GetById(string id);
        Task<List<T>> GetByFieldValue(string fieldName, string fieldValue);
        Task<List<T>> GetAll(string sortField = "", string sortDirection = "");
        Task<List<T>> GetAll(MongoDBParam query);
        Task<List<T>> GetAll(int currentPage, int pageSize, string sortField, string sortDirection, List<SearchField>? searchQueries = null);
        Task<T?> SortAndGet(MongoDBParam query, string sortingFieldName);
        Task<long> GetTotalCount();
        Task<long> GetCount(List<SearchField>? searchQueries = null);
        Task<T?> SaveAsync(T entity);
        Task<long> SaveManyAsync(List<T> entities);
        Task<T?> Save(T entity);
        Task<long> SaveMany(IEnumerable<T> records);
        Task<T?> DeleteAsync(T entity, bool isSoftDelete = true);
        Task<T?> DeleteById(string id, bool isSoftDelete = true);
        Task<bool> DeleteManyAsync(string id, bool isSoftDelete = true);
    }
}
