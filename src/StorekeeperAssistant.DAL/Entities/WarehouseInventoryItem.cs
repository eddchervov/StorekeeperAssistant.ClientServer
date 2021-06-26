using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace StorekeeperAssistant.DAL.Entities
{
    [Table("WarehouseInventoryItem")]
    public class WarehouseInventoryItem
    {
        public int Id { get; set; }
        /// <summary>
        /// Количество
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// Дата
        /// </summary>
        public DateTime DateTime { get; set; }
        /// <summary>
        /// Склад
        /// </summary>
        public int WarehouseId { get; set; }
        [ForeignKey("WarehouseId")]
        public Warehouse Warehouse { get; set; }
        /// <summary>
        /// Номенклатура
        /// </summary>
        public int InventoryItemId { get; set; }
        [ForeignKey("InventoryItemId")]
        public InventoryItem InventoryItem { get; set; }
        /// <summary>
        /// Перемещение
        /// </summary>
        public int MovingId { get; set; }
        [ForeignKey("MovingId")]
        public Moving Moving { get; set; }

        public bool IsDeleted { get; set; }
    }
}
