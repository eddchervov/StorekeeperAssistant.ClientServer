using StorekeeperAssistant.Api.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace StorekeeperAssistant.DAL.Entities
{
    public class Moving
    {
        public int Id { get; set; }
        /// <summary>
        /// Тип операции - приход/расход
        /// </summary>
        public TypeTransactionEnum TypeTransaction { get; set; }
        /// <summary>
        /// Склад отправления
        /// </summary>
        public int DepartureWarehouseId { get; set; }
        /// <summary>
        /// Склад прибытия
        /// </summary>
        public int ArrivalWarehouseId { get; set; }
        public DateTime DateTime { get; set; }
    }
}
