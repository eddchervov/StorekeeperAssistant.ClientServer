using System.ComponentModel.DataAnnotations.Schema;

namespace StorekeeperAssistant.DAL.Entities
{
    /// <summary>
    /// Склады компании
    /// </summary>
    [Table("Warehouse")]
    public class Warehouse
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public bool IsActive { get; set; }
    }
}
