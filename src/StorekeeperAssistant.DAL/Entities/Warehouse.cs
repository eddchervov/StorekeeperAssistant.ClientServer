using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

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
    }
}
