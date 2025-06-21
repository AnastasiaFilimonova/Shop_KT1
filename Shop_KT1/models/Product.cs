using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop_KT1.models
{
    public class Product
    {
        public int ProductID { get; set; }
        public string Name { get; set; }
        public string Size { get; set; }
        public ICollection<ProductMaterial> Materials { get; set; }
    }
}
