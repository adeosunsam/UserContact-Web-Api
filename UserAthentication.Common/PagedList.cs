using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserAthentication.Common
{
    public class PagedList<T> : List<T>
    {
        public int CurrentPage { get; private set; }
        public int TotalPage { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public bool HasPrevious => (CurrentPage > 1);
        public bool HasNext => (CurrentPage < TotalPage);

        public PagedList(List<T> item, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            CurrentPage = pageNumber;
            PageSize = pageSize;
            TotalPage = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(item);
        }

        public static PagedList<T> Create(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = source.Count();
            var item = source.Skip(pageSize * (pageNumber - 1))
                    .Take(pageSize)
                    .ToList();
            return new PagedList<T>(item, count, pageNumber, pageSize);
        }
    }
}
