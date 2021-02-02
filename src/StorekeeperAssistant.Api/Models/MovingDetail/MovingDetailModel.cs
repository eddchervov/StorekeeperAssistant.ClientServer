using StorekeeperAssistant.Api.Models.Nomenclature;
using System;
using System.Collections.Generic;
using System.Text;

namespace StorekeeperAssistant.Api.Models.MovingDetail
{
    public class MovingDetailModel
    {
        public int Id { get; set; }
        /// <summary>
        /// Нуменклатура которая перенесена
        /// </summary>
        public NomenclatureModel NomenclatureModel { get; set; }
        /// <summary>
        /// Кол-во
        /// </summary>
        public int Count { get; set; }
    }
}
