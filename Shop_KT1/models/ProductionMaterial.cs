using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop_KT1.models
{
    public class ProductionMaterial
    {
        public int ProductionMaterialID { get; set; }
        public int ProductionID { get; set; }
        public Production Production { get; set; }
        public int MaterialID { get; set; }
        public Material Material { get; set; }
        public double PlannedAmount { get; set; }
        public double ActualAmount { get; set; }
    }
}
