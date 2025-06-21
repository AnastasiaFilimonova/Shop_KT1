using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop_KT1.models
{
    public class ProductMaterial
    {
        public int ProductMaterialId { get; set; }
        public int ProductID { get; set; }
        public Product Product { get; set; }
        public int MaterialID { get; set; }
        public Material Material { get; set; }
        public double Quantity { get; set; } 
    }
}
