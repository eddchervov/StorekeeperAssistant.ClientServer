using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace StorekeeperAssistant.DAL.Entities
{
    [Table("WarehouseInventoryItem")]
    public class WarehouseInventoryItem
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public int WarehouseId { get; set; }
        public int NomenclatureId { get; set; }
        public int MovingId { get; set; }
        public int Count { get; set; }
        public bool IsActive { get; set; }
    }
}
