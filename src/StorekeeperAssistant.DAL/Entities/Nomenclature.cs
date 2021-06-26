using System.ComponentModel.DataAnnotations.Schema;

namespace StorekeeperAssistant.DAL.Entities
{
    /// <summary>
    /// Номенклатуры
    /// </summary>
    [Table("Nomenclature")]
    public class Nomenclature
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public bool IsActive { get; set; }
    }
}
