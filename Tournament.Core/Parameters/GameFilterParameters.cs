using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tournament.Core.Parameters
{
    public class GameFilterParameters
    {
        /*[FromQuery]*/
        public DateTime? StartDate { get; set; }
        /*[FromQuery]*/
        public DateTime? EndDate { get; set; }
        /*[FromQuery]*/
        public string? Title { get; set; }
        /*[FromQuery]*/
        public string? SortBy { get; set; }

        public int PageNumber { get; set; } = 1;

        private int _pageSize = 10;
        private readonly int maxPageSize = 50;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > maxPageSize) ? maxPageSize : value;
        }
    }
}
