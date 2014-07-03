using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI2014.Operational.Entities
{
    public class DimFailure
    {
        [Key]
        public int Pk { get; set; }
        public string Id { get; set; }
        public string study_name { get; set; }
        public string program_code { get; set; }
        public string level { get; set; }
        public int? start_year { get; set; }
        public int? end_year { get; set; }
        public bool? propedeuse { get; set; }

    }
}
