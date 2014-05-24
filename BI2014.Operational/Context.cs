using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI2014.Operational
{
    public class AzureContext:DbContext
    {

        public AzureContext()
            : base("BIDW")
        {

        }
    }
}
