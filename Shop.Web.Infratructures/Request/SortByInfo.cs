using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Web.Infratructures.Request
{
    public class SortByInfo
    {
        [Required]
        public string? FieldName { get; set; }

        public bool? Accending { get; set; } = true;

        public SortByInfo() { }

        public SortByInfo(string fieldName, bool accending)
        {
            FieldName = fieldName;
            Accending = accending;
        }
    }
}
