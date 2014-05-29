using MongoDB.Bson;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI2014.Scrapping.Entities
{
    public class StudentDrop
    {
        //[JsonIgnore]
        //public ObjectId Id { get; set; }
        public string Name { get; set; }
        public string study { get; set; }
        public string level { get; set; }
        public string classic_level { get; set; }
        public string start_year {get; set;}
        public string prop_date { get; set; }

    }
}
