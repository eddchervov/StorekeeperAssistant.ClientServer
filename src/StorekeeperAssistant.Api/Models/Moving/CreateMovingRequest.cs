using StorekeeperAssistant.Api.Models.InventoryItem;
using System.Collections.Generic;

namespace StorekeeperAssistant.Api.Models.Moving
{
    public class CreateMovingRequest
    {
        /// <summary>
        /// Склад отправления
        /// </summary>
        public int? DepartureWarehouseId { get; set; }
        /// <summary>
        /// Склад прибытия
        /// </summary>
        public int? ArrivalWarehouseId { get; set; }
        /// <summary>
        /// Список перемещаемых ТМЦ
        /// </summary>
        public List<CreateInventoryItemModel> CreateInventoryItemModels { get; set; } = new List<CreateInventoryItemModel>();
        /// <summary>
        /// Отметка удаления перемещения
        /// </summary>
        public bool IsActive { get; set; } = true;
    }
}
