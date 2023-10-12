using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models
{
    public class FilterModel
    {
        public string? Attribute { get; set; }
        public string? Order { get; set; }
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }

        public override string ToString()
        {
            return string.Format("Attribute: {0}, Order: {1}, PageNumber: {2}, PageSize: {3}", Attribute, Order, PageNumber, PageSize);
        }
    }
}
