﻿namespace Odin.Baseline.Domain.DTO.Common
{
    public class PaginatedListOutput<T> where T : class
    {
        public int PageNumber { get; private set; }
        public int PageSize { get; private set; }
        public int TotalPages { get; private set; }
        public int TotalItems { get; private set; }
        public IEnumerable<T> Items { get; private set; }

        public PaginatedListOutput(int pageNumber, int pageSize, int totalPages, int totalItems, IEnumerable<T> items)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalPages = totalPages;
            TotalItems = totalItems;
            Items = items;
        }

        public PaginatedListOutput(int totalItems, IEnumerable<T> items)
        {
            TotalItems = totalItems;
            Items = items;
        }
    }
}
