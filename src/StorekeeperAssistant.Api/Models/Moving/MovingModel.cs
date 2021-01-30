using StorekeeperAssistant.Api.Enum;
using StorekeeperAssistant.Api.Models.InventoryItem;
using StorekeeperAssistant.Api.Models.Warehouse;
using System;
using System.Collections.Generic;
using System.Text;

namespace StorekeeperAssistant.Api.Models.Moving
{
    public class MovingModel
    {
        public int Id { get; set; }
        /// <summary>
        /// Тип операции - приход/расход
        /// </summary>
        public TypeTransactionEnum TypeTransaction { get; set; }
        /// <summary>
        /// Склад отправления
        /// </summary>
        public WarehouseModel DepartureWarehouse { get; set; }
        /// <summary>
        /// Склад прибытия
        /// </summary>
        public WarehouseModel ArrivalWarehouse { get; set; }
        /// <summary>
        /// ТМЦ
        /// </summary>
        public List<InventoryItemModel> InventoryItemModels { get; set; } = new List<InventoryItemModel>();
        public DateTime DateTime { get; set; }
    }
}
