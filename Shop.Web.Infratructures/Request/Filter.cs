using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Web.Infratructures.Request
{
    public class Filter
    {
        public string? FieldName { get; set; }
        public string? Value { get; set; }
        public string? Operation {  get; set; }
    }
}
