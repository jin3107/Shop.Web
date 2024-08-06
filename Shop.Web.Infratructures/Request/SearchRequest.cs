using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Web.Infratructures.Request
{
    public class SearchRequest
    {
        public List<Filter>? Filters { get; set; }
        public SortByInfo? SortBy { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int? PageNumber { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int? PageSize { get; set; }
    }
}
