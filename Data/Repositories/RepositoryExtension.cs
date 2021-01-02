using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Data.Helper;
using Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Data.Repositories
{
    public class RepositoryExtension<T> : IRepositoryExtension<T> where T : class
    {
        public async Task<IPagedResult<T>> GetPage(IQueryCollection query, IQueryable<T> results)
        {
            var pageSize = 10;
            if (query.ContainsKey("max"))
                pageSize = int.Parse(query["max"]);
            var offset = 0;
            if (query.ContainsKey("offset"))
                offset = int.Parse(query["offset"]);

            var pageNumber = (offset / pageSize) + 1;
            return await PagedResult<T>.ToPagedResult(results, pageNumber, pageSize);
        }

        public IQueryable<T> Sort(IQueryCollection query, IQueryable<T> results)
        {
            string sort = query["sort"];
            string order = query["order"];
            if (!string.IsNullOrEmpty(sort))
            {
                var property = typeof(T).GetProperty(sort);
                if(property == null)
                    throw new ArgumentException($"Attribute '{sort}' not found in model {typeof(T).Name}");
                if (order == null || order.Equals("asc"))
                    results = results.OrderByDynamic(x => $"x.{sort}");
                else
                    results = results.OrderByDescendingDynamic($"x.{sort}");
;           }
            return results;
        }

        public IQueryable<T> Search(IQueryCollection query, IQueryable<T> results)
        {
            var nonSearchable = new List<string> {"max", "offset", "sort", "order"};
            var searchParams = query.ToList();
            searchParams.RemoveAll(kv => nonSearchable.Contains(kv.Key));

            foreach (var (propName, value) in searchParams)
            {
                var property = typeof(T).GetProperty(propName);
                if (property == null)
                    throw new KeyNotFoundException(
                        $"Attribute '{propName}' not found in entity '{typeof(T).Name}'");
                
                var clause = new ClauseBuilder(typeof(T).Name, propName, value).GetClause(property);

                if (!string.IsNullOrEmpty(clause))
                    results = results.WhereDynamic(x => clause);
            }
            return results;
        }
    }
}
