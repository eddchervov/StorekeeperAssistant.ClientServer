using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace StorekeeperAssistant.DAL.Entities
{
    [Table("WarehouseInventoryItem")]
    public class WarehouseInventoryItem
    {
        public int Id { get; set; }
        public int Count { get; set; }
        public DateTime DateTime { get; set; }

        public int WarehouseId { get; set; }
        [ForeignKey("WarehouseId")]
        public Warehouse Warehouse { get; set; }
        public int NomenclatureId { get; set; }
        [ForeignKey("NomenclatureId")]
        public Nomenclature Nomenclature { get; set; }
        public int MovingId { get; set; }
        [ForeignKey("MovingId")]
        public Moving Moving { get; set; }

        public bool IsActive { get; set; }
    }
}
