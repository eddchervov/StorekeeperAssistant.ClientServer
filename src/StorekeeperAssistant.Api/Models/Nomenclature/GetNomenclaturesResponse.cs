using System;
using System.Collections.Generic;
using System.Text;

namespace StorekeeperAssistant.Api.Models.Nomenclature
{
    public class GetNomenclaturesResponse
    {
        public List<NomenclatureModel> NomenclatureModels { get; set; } = new List<NomenclatureModel>();
    }
}
