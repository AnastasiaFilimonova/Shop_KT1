using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop_KT1.models
{
    public class InventoryAudit
    {
        public int InventoryAuditId { get; set; }
        public DateTime AuditDate { get; set; }
        public bool ApprovedByDirector { get; set; }
        public ICollection<InventoryItem> Items { get; set; }
    }
}
