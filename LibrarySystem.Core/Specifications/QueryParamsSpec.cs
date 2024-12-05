using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.Core.Specifications
{
    public class QueryParamsSpec
    {
        public string? Sort {  get; set; }
       // public string? Title { get; set; }

        private int pageSize = 2;

        public int? BookId { get; set; }

        public int PageSize
        {
            get { return pageSize; }
            set { pageSize  = value > 3  ? 3 : value; }
        }
        public int PageIndex { get; set; } = 1;

        public string? AuthorName { get; set; }

        public string? Role {  get; set; }

        private string? search;

        public string? Search
        {
            get { return search; }
            set { search = value.ToLower(); }
        }



    }
}
