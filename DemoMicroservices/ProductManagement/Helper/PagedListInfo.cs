using System;
using ProductManagement.Models;

namespace ProductManagement.Helper
{
    public static class PagedListInfo
    {
        public static Object Info(PagedList<ProductModel> pagedLists)
        {
            var metadata = new
            {
                pagedLists.TotalCount,
                pagedLists.PageSize,
                pagedLists.CurrentPage,
                pagedLists.TotalPages,
                pagedLists.HasNext,
                pagedLists.HasPrevious
            };

            return metadata;
        }
    }
}

