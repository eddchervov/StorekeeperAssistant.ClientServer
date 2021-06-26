using System.ComponentModel.DataAnnotations.Schema;

namespace StorekeeperAssistant.DAL.Entities
{
    /// <summary>
    /// Номенклатуры
    /// </summary>
    public class InventoryItem
    {
        public int Id { get; set; }
        /// <summary>
        /// Название
        /// </summary>
        public string Name { get; set; }

        public bool IsDeleted { get; set; }
    }
}
