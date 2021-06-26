using StorekeeperAssistant.Api.Models.InventoryItems;

namespace StorekeeperAssistant.Api.Models.MovingDetails
{
    public class MovingDetailDTO
    {
        public int Id { get; set; }
        /// <summary>
        /// Нуменклатура которая перенесена
        /// </summary>
        public InventoryItemDTO InventoryItem { get; set; }
        /// <summary>
        /// Количество
        /// </summary>
        public int Count { get; set; }
    }
}
