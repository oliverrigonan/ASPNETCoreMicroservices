using System;

namespace CustomerManagement.Helper
{
    public class PagedList<T> : List<T>
    {
        public Int32 CurrentPage { get; private set; }
        public Int32 TotalPages { get; private set; }
        public Int32 PageSize { get; private set; }
        public Int32 TotalCount { get; private set; }

        public Boolean HasPrevious => CurrentPage > 1;
        public Boolean HasNext => CurrentPage < TotalPages;

        public PagedList(List<T> items, Int32 count, Int32 pageNumber, Int32 pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (Int32)Math.Ceiling(count / (Double)pageSize);

            AddRange(items);
        }

        public static PagedList<T> ToPagedList(List<T> source, int pageNumber, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }
}

