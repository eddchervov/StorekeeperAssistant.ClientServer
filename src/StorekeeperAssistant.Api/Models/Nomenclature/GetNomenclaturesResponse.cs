using System.Collections.Generic;

namespace StorekeeperAssistant.Api.Models.Nomenclature
{
    public class GetNomenclaturesResponse
    {
        public List<NomenclatureModel> NomenclatureModels { get; set; } = new List<NomenclatureModel>();
    }
}
