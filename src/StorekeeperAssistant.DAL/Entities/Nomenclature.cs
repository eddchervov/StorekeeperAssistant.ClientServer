using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

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
    }
}
