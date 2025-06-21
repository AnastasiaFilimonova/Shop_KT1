using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop_KT1.models
{
    public class InventoryItem
    {
        public int InventoryItemID { get; set; }
        public int AuditID { get; set; }
        public InventoryAudit Audit { get; set; }
        public int MaterialID { get; set; }
        public Material Material { get; set; }
        public double SystemAmount { get; set; }
        public double ActualAmount { get; set; }
        public decimal DifferenceValue { get; set; }
    }
}
