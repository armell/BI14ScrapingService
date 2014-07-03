using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI2014.Operational.Entities
{
    public class DimDate
    {
        public int Id { get; set; }
        public string Period { get; set; }
        public string Timeslot { get; set; }
        public string Year { get; set; }
    }
}
