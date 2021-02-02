using StorekeeperAssistant.Api.Models.InventoryItem;
using StorekeeperAssistant.Api.Models.MovingDetail;
using StorekeeperAssistant.Api.Models.Nomenclature;
using StorekeeperAssistant.Api.Models.Warehouse;
using System;
using System.Collections.Generic;
using System.Text;

namespace StorekeeperAssistant.Api.Models.Moving
{
    public class MovingModel
    {
        public int Id { get; set; }
        public List<MovingDetailModel> MovingDetailModels { get; set; } = new List<MovingDetailModel>();

        /// <summary>
        /// Склад отправления
        /// </summary>
        public WarehouseModel DepartureWarehouse { get; set; }
        /// <summary>
        /// Склад прибытия
        /// </summary>
        public WarehouseModel ArrivalWarehouse { get; set; }
        public DateTime DateTime { get; set; }
    }
}
