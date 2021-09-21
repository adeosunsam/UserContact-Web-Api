using System;

namespace UserAthentication.Common
{
    public class Pagination
    {
        const int maxPageSize = 10;
        public int PageNumber { get; set; } = 1;

        private int pageSize = 5;
        public int PageSize 
        {
            get => pageSize;

            set => pageSize = (value > maxPageSize) ? maxPageSize : value;
        }
    }
}
