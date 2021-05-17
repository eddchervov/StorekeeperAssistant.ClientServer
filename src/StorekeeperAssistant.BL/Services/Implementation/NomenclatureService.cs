using StorekeeperAssistant.Api.Models.Nomenclature;
using StorekeeperAssistant.DAL.Entities;
using StorekeeperAssistant.DAL.Repositories;
using System.Linq;
using System.Threading.Tasks;

namespace StorekeeperAssistant.BL.Services.Implementation
{
    internal class NomenclatureService : INomenclatureService
    {
        private readonly INomenclatureRepository _nomenclatureRepository;

        public NomenclatureService(INomenclatureRepository nomenclatureRepository)
        {
            _nomenclatureRepository = nomenclatureRepository;
        }

        public async Task<GetNomenclaturesResponse> GetNomenclaturesAsync(GetNomenclaturesRequest request)
        {
            var response = new GetNomenclaturesResponse { };

            var nomenclatures = await _nomenclatureRepository.GetListAsync();
            var nomenclatureModels = nomenclatures.Select(ConvertModel).ToList();

            response.NomenclatureModels = nomenclatureModels;

            return response;
        }

        private NomenclatureModel ConvertModel(Nomenclature nomenclature)
        {
            return new NomenclatureModel
            {
                Id = nomenclature.Id,
                Name = nomenclature.Name
            };
        }

    }
}
