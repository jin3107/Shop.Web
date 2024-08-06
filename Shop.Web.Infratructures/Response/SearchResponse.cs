using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Web.Infratructures.Response
{
    public class SearchResponse<T>
    {
        public List<T>? Data { get; set; }
        public long CurrentPage { get; set; }
        public long TotalPages { get; set; }
        public long RowsPerPage { get; set; }
        public long TotalRows {  get; set; }
    }
}
