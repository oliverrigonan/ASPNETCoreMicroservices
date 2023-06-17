using System;
using CustomerManagement.Models;

namespace CustomerManagement.Helper
{
    public static class PagedListInfo
    {
        public static Object Info(PagedList<CustomerModel> pagedLists)
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

