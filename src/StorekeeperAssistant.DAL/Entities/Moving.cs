using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace StorekeeperAssistant.DAL.Entities
{
    [Table("Moving")]
    public class Moving
    {
        public int Id { get; set; }
        /// <summary>
        /// Склад отправления
        /// </summary>
        public int? DepartureWarehouseId { get; set; }
        /// <summary>
        /// Склад прибытия
        /// </summary>
        public int? ArrivalWarehouseId { get; set; }
        /// <summary>
        /// Дата переноса
        /// </summary>
        public DateTime DateTime { get; set; }
        public bool IsActive { get; set; }
    }
}
