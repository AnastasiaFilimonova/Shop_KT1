using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop_KT1.models
{
    public class Material
    {
        public int MaterialID { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; } 
        public double StockAmount { get; set; } 
        public decimal Price { get; set; } 
    }
}
