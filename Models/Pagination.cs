﻿namespace Models
{
    public class Pagination<T>
    {
        public int TotalItemsCount { get; set; }
        public int PageSize { get; set; }
        public int TotalPagesCount
        {
            get
            {
                var temp = TotalItemsCount / PageSize;
                if (TotalItemsCount % PageSize == 0)
                {
                    return temp;
                }
                return temp;
            }
        }
        public int PageIndex { get; set; }

        /// <summary>
        /// page number start from 0
        /// </summary>
        public bool Next => PageIndex + 1 < TotalPagesCount;
        public bool Previous => PageIndex > 0;
        public ICollection<T>? Items { get; set; }
    }
}