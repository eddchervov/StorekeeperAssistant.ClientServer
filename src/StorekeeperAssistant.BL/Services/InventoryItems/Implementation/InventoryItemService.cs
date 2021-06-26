using StorekeeperAssistant.Api.Models.InventoryItems;
using StorekeeperAssistant.DAL.Entities;
using StorekeeperAssistant.DAL.Repositories;
using System.Linq;
using System.Threading.Tasks;

namespace StorekeeperAssistant.BL.Services.InventoryItems.Implementation
{
    internal class InventoryItemService : IInventoryItemService
    {
        private readonly IInventoryItemRepository _inventoryItemRepository;

        public InventoryItemService(IInventoryItemRepository inventoryItemRepository)
        {
            _inventoryItemRepository = inventoryItemRepository;
        }

        public async Task<GetInventoryItemsResponse> GetAsync(GetInventoryItemsRequest request)
        {
            var response = new GetInventoryItemsResponse();

            var inventoryItems = await _inventoryItemRepository.GetAsync();

            response.InventoryItems = inventoryItems.Select(MapToDTO);

            return response;
        }

        #region private
        private InventoryItemDTO MapToDTO(InventoryItem nomenclature)
        {
            return new InventoryItemDTO
            {
                Id = nomenclature.Id,
                Name = nomenclature.Name
            };
        }
        #endregion
    }
}
