using System;
using System.Collections.Generic;
using System.Text;

namespace StorekeeperAssistant.DAL.Models
{
    public class InventoryItemsDALModel
    {
        public int Id { get; set; }
        public NomenclatureDALModel NomenclatureDALModel { get; set; }
    }
}
