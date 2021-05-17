using StorekeeperAssistant.Api.Models.Nomenclature;

namespace StorekeeperAssistant.Api.Models.InventoryItem
{
    public class InventoryItemModel
    {
        public int Id { get; set; }
        public int Count { get; set; }
        public NomenclatureModel NomenclatureModel { get; set; }
    }
}
