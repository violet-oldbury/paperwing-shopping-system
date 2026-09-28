using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace paperwing
{
    public class product
    {
        public string description { get; set;}
        public double cost { get; set;}
        public int numberordered { get; set;}
        public product(string descriptionnumber, double costnumber)
        {
            description = descriptionnumber;
            cost = costnumber;
        }
    }
}
