using MongoDB.Bson;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI2014.Scrapping.Entities
{
    public class Publication
    {
        [JsonIgnore]
        public ObjectId Id { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
    }
}
