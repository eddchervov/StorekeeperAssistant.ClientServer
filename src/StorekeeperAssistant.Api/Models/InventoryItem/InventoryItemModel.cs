using StorekeeperAssistant.Api.Models.Nomenclature;
using System;
using System.Collections.Generic;
using System.Text;

namespace StorekeeperAssistant.Api.Models.InventoryItem
{
    public class InventoryItemModel
    {
        public int Id { get; set; }
        public NomenclatureModel NomenclatureModel { get; set; }
    }
}
