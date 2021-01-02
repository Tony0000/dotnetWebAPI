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
                var clause = GetClause(propName, value.ToString());

                if (!string.IsNullOrEmpty(clause))
                    results = results.WhereDynamic(x => clause);
            }
            return results;
        }

        private string GetClause(string propName, string value)
        {
            var property = typeof(T).GetProperty(propName);
            if(property == null)
                throw new KeyNotFoundException(
                    $"Attribute '{propName}' not found in entity '{typeof(T).Name}'");
            var pType = property.PropertyType;
            string clause = null;

            if (pType == typeof(string))
            {
                clause = $"x.{propName}.Contains(\"{value}\")";
            }
            else if (pType == typeof(DateTime))
            {
                if (DateTime.TryParse(value, out var dateTime))
                    clause = $"x.{propName}.Date.Equals((DateTime)(object)\"{dateTime}\")";
                else
                    throw new InvalidCastException(
                        $"Invalid date for attribute '{propName}' in entity '{typeof(T).Name}'");
            }
            else if (pType == typeof(bool) || pType == typeof(long))
            {
                clause = $"x.{propName} == {value}";
            }
            else if (property.PropertyType.IsEnum)
            {
                var enumNames = property.PropertyType.GetEnumNames().ToList();
                var idx = enumNames.IndexOf(value);
                if(idx == -1)
                    throw new InvalidEnumArgumentException(
                        $"Invalid value for attribute '{propName}' in entity '{typeof(T).Name}'");
                clause = $"x.{propName} == {idx + 1}";
            }
            return clause;
        }
    }

    public class Metadata
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PagesSize { get; set; }
        public int TotalCount { get; set; }

        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;
    }

    public class PagedResult<T> : List<T>
    {
        public Metadata Metadata { get; set; }

        public PagedResult(IQueryable<T> items, int count, int pageNumber, int pageSize)
        {
            Metadata = new Metadata
            {
                TotalCount = count,
                PagesSize = pageSize,
                CurrentPage = pageNumber,
                TotalPages = (int) Math.Ceiling(count / (double) pageSize)
            };

            AddRange(items);
        }

        public static PagedResult<T> ToPagedResult(IEnumerable<T> source, int pageNumber, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            
            return new PagedResult<T>(items, count, pageNumber, pageSize);
        }
    }
}
