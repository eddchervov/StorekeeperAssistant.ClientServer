using StorekeeperAssistant.Api.Models.InventoryItems;
using System.Collections.Generic;

namespace StorekeeperAssistant.Api.Models.Movings
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
        public List<CreateInventoryItemDTO> InventoryItems { get; set; } = new List<CreateInventoryItemDTO>();
        /// <summary>
        /// Отметка удаления перемещения
        /// </summary>
        public bool IsDeleted { get; set; }

        public bool IsMovingDepartureWarehouse => DepartureWarehouseId != null;
        public bool IsMovingArrivalWarehouse => ArrivalWarehouseId != null;
        public IEnumerable<int> WarehouseIds => GetWarehouseIds();
        private IEnumerable<int> GetWarehouseIds()
        {
            var warehouseIds = new List<int>();

            if (DepartureWarehouseId != null)
                warehouseIds.Add(DepartureWarehouseId.Value);

            if (ArrivalWarehouseId != null)
                warehouseIds.Add(ArrivalWarehouseId.Value);

            return warehouseIds;
        }
    }
}
