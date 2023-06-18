using System;
namespace APIGateway.Helper
{
    public class PagedListParams
    {
        private const Int32 _maxPageSize = 50;
        private Int32 _pageSize = 10;

        public Int32 PageNumber { get; set; } = 1;
        public Int32 PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value > _maxPageSize) ? _maxPageSize : value;
            }
        }
    }
}

