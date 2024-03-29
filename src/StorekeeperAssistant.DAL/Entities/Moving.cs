﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace StorekeeperAssistant.DAL.Entities
{
    /// <summary>
    /// Перемещения
    /// </summary>
    public class Moving
    {
        public int Id { get; set; }
        /// <summary>
        /// Дата переноса
        /// </summary>
        public DateTime DateTime { get; set; }

        /// <summary>
        /// Склад отправления
        /// </summary>
        public int? DepartureWarehouseId { get; set; }
        [ForeignKey("DepartureWarehouseId")]
        public Warehouse DepartureWarehouse { get; set; }
        /// <summary>
        /// Склад прибытия
        /// </summary>
        public int? ArrivalWarehouseId { get; set; }
        [ForeignKey("ArrivalWarehouseId")]
        public Warehouse ArrivalWarehouse { get; set; }

        public bool IsDeleted { get; set; }

        public ICollection<MovingDetail> MovingDetails { get; set; } = new List<MovingDetail>();
    }
}
