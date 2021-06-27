using System.ComponentModel.DataAnnotations.Schema;

namespace StorekeeperAssistant.DAL.Entities
{
    /// <summary>
    /// Подробности перемещения
    /// </summary>
    public class MovingDetail
    {
        public int Id { get; set; }
        /// <summary>
        /// Количество
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Перемещение
        /// </summary>
        public int MovingId { get; set; }
        [ForeignKey("MovingId")]
        public Moving Moving { get; set; }
        /// <summary>
        /// Нуменклатура
        /// </summary>
        public int InventoryItemId { get; set; }
        [ForeignKey("InventoryItemId")]
        public InventoryItem InventoryItem { get; set; }
    }
}
