using StorekeeperAssistant.Api.Models.MovingDetails;
using StorekeeperAssistant.Api.Models.Warehouses;
using System;
using System.Collections.Generic;

namespace StorekeeperAssistant.Api.Models.Movings
{
    public class MovingDTO
    {
        public int Id { get; set; }
        public IEnumerable<MovingDetailDTO> MovingDetails { get; set; } = new List<MovingDetailDTO>();

        /// <summary>
        /// Склад отправления
        /// </summary>
        public WarehouseDTO DepartureWarehouse { get; set; }
        /// <summary>
        /// Склад прибытия
        /// </summary>
        public WarehouseDTO ArrivalWarehouse { get; set; }
        public DateTime DateTime { get; set; }
    }
}
