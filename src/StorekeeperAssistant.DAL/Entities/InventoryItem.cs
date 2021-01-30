using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace StorekeeperAssistant.DAL.Entities
{
    /// <summary>
    /// Товар(или ТМЦ) товарно-материальные ценности
    /// </summary>
    [Table("InventoryItem")]
    public class InventoryItem
    {
        public int Id { get; set; }
        public int NomenclatureId { get; set; }
        public int WarehouseId { get; set; }
        public int Count { get; set; }
    }
}
