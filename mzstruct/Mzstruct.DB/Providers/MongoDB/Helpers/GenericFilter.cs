using MongoDB.Bson;
using MongoDB.Driver;
using Mzstruct.Base.Entities;
using Mzstruct.Base.Helpers;
using Mzstruct.Base.Models;
using System.Text.RegularExpressions;

namespace Mzstruct.DB.Providers.MongoDB.Helpers
{
    public static class GenericFilter<T> where T : BaseEntity
    {
        public static FilterDefinition<T> BuildDynamicFilter(string? id, List<SearchField>? searchQueries = null, FilterDefinition<T>? filter = null)
        {
            if (filter == null)
                filter = Builders<T>.Filter.Empty;
            filter &= Builders<T>.Filter.Eq(x => x.IsActive, true);
            filter &= Builders<T>.Filter.Eq(x => x.IsDeleted, false);

            if (!string.IsNullOrEmpty(id) && id.ToLower() != "undefined")
                filter = filter & Builders<T>.Filter.Eq("Id", id);

            if (searchQueries != null && searchQueries.Count > 0)
            {
                foreach (var query in searchQueries)
                {
                    if (!string.IsNullOrEmpty(query.Key) && !string.IsNullOrEmpty(query.Value))
                    {
                        if (query.IsEncrypted || query.IsId)
                        {
                            filter = filter & Builders<T>.Filter.Eq(query.Key, query.Value);
                        }
                        else if (query.IsBoolean)
                        {
                            if (query.Value.ToLower().Equals("true") || query.Value.ToLower().Equals("1"))
                            {
                                filter = filter & Builders<T>.Filter.Eq(query.Key, true);
                            }
                            else
                            {
                                filter = filter & Builders<T>.Filter.Ne(query.Key, true);
                            }
                        }
                        else if (query.IsDateTime)
                        {
                            var date = BaseHelper.GetDateFromString(query.Value);

                            if (date.HasValue)
                            {
                                date = date.Value.ToLocalTime();
                                filter &= Builders<T>.Filter.Eq(query.Key, date);
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(query.DataSeparator))
                            {
                                List<string> dataList = new List<string>();
                                dataList.AddRange(query.Value.Split(query.DataSeparator).Select(i => i));

                                if (dataList.Count > 0 && query.IsCaseSensitive)
                                {
                                    if (query.IsPartialMatch)
                                    {
                                        dataList = dataList.ConvertAll(x => "/^" + x.Replace("+", @"\+").ToLower() + "$/i");
                                    }

                                    filter = filter & new BsonDocument(query.Key, new BsonDocument("$in", new BsonArray(dataList)));
                                }
                                else if (dataList.Count > 0 && !query.IsCaseSensitive)
                                {
                                    dataList = dataList.ConvertAll(x => x.Replace("(", @"\(").Replace(")", @"\)").ToLower());

                                    if (query.IsPartialMatch)
                                    {
                                        dataList = dataList.ConvertAll(x => "/^" + x.Replace("+", @"\+").ToLower() + "$/i");
                                    }

                                    var regexFilter = "(" + string.Join("|", dataList) + ")";
                                    filter = filter & Builders<T>.Filter.Regex(query.Key, new BsonRegularExpression(new Regex(regexFilter, RegexOptions.IgnoreCase))); // | RegexOptions.IgnorePatternWhitespace
                                }
                            }
                            else
                            {
                                if (query.IsCaseSensitive)
                                {
                                    if (query.IsPartialMatch)
                                    {
                                        filter = filter & Builders<T>.Filter.Regex(query.Key, new BsonRegularExpression(new Regex(query.Value, RegexOptions.None)));
                                    }
                                    else
                                    {
                                        filter = filter & Builders<T>.Filter.Eq(query.Key, query.Value);
                                    }
                                }
                                else
                                {
                                    if (query.IsPartialMatch)
                                    {
                                        query.Value = query.Value.Replace("(", @"\(").Replace(")", @"\)").ToLower();
                                        var regexFilter = "(" + query.Value + ")";
                                        filter = filter & Builders<T>.Filter.Regex(query.Key, new BsonRegularExpression(new Regex(regexFilter, RegexOptions.IgnoreCase)));                 
                                    }
                                    else
                                    {
                                        filter = filter & Builders<T>.Filter.Regex(query.Key, new BsonRegularExpression(new Regex("^" + query.Value + "$", RegexOptions.IgnoreCase)));
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return filter;
        }
    }
}
