using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Helper
{
    public class Metadata
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PagesSize { get; set; }
        public int TotalCount { get; set; }
        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;
    }

    public class PagedResult<T> : List<T>, IPagedResult<T> where T : class
    {
        public Metadata Metadata { get; set; }

        public PagedResult(IEnumerable<T> items, int count, int pageNumber, int pageSize)
        {
            Metadata = new Metadata
            {
                TotalCount = count,
                PagesSize = pageSize,
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling(count / (double)pageSize)
            };

            AddRange(items);
        }

        public static async Task<PagedResult<T>> ToPagedResult(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = source.Count();
            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PagedResult<T>(items, count, pageNumber, pageSize);
        }

        async Task<IPagedResult<T>> IPagedResult<T>.ToPagedResult(IQueryable<T> source, int pageNumber, int pageSize)
        {
            return await ToPagedResult(source, pageNumber, pageSize);
        }
    }
}
