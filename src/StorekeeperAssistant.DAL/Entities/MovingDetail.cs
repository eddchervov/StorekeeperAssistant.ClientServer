using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace StorekeeperAssistant.DAL.Entities
{
    [Table("MovingDetail")]
    public class MovingDetail
    {
        public int Id { get; set; }
        public int MovingId { get; set; }
        /// <summary>
        /// Нуменклатура которая перенесена
        /// </summary>
        public int NomenclatureId { get; set; }
        /// <summary>
        /// Кол-во
        /// </summary>
        public int Count { get; set; }
    }
}
