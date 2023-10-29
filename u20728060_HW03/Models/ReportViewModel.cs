using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace u20728060_HW03.Models
{
    public class ReportViewModel
    {
        public List<ChartViewModel> ChartData { get; set; }
        public List<TableViewModel> TableData { get; set; }
    }
}