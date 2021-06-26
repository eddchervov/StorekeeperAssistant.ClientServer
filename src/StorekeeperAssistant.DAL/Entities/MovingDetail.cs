using System.ComponentModel.DataAnnotations.Schema;

namespace StorekeeperAssistant.DAL.Entities
{
    /// <summary>
    /// Подробности перемещения
    /// </summary>
    [Table("MovingDetail")]
    public class MovingDetail
    {
        public int Id { get; set; }
        /// <summary>
        /// Кол-во
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Передвиженеие
        /// </summary>
        public int MovingId { get; set; }
        [ForeignKey("MovingId")]
        public Moving Moving { get; set; }
        /// <summary>
        /// Нуменклатура которая перенесена
        /// </summary>
        public int NomenclatureId { get; set; }
        [ForeignKey("NomenclatureId")]
        public Nomenclature Nomenclature { get; set; }

        public bool IsActive { get; set; }
    }
}
