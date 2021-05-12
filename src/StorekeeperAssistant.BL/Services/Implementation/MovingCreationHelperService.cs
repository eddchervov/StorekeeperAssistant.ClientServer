using StorekeeperAssistant.Api.Models.InventoryItem;
using StorekeeperAssistant.Api.Models.Nomenclature;
using StorekeeperAssistant.DAL.Entities;
using System;

namespace StorekeeperAssistant.BL.Services.Implementation
{
    public static class MovingCreationHelperService
    {
        public static Moving CreateMoving(int? departureWarehouseId, int? arrivalWarehouseId, DateTime utcNow)
        {
            var moving = new Moving
            {
                DepartureWarehouseId = departureWarehouseId,
                ArrivalWarehouseId = arrivalWarehouseId,
                IsActive = true,
                DateTime = utcNow
            };

            return moving;
        }

        public static MovingDetail CreateMovingDetail(int movingId, int nomenclatureId, int count)
        {
            var movingDetail = new MovingDetail
            {
                MovingId = movingId,
                NomenclatureId = nomenclatureId,
                Count = count
            };

            return movingDetail;
        }

        public static WarehouseInventoryItem CreateWarehouseInventoryItem(int nomenclatureId, DateTime utcNow, int count, int departureWarehouseId, int movingId)
        {
            var warehouseInventoryItem = new WarehouseInventoryItem
            {
                NomenclatureId = nomenclatureId,
                DateTime = utcNow,
                Count = count,
                WarehouseId = departureWarehouseId,
                MovingId = movingId,
                IsActive = true
            };

            return warehouseInventoryItem;
        }
    }
}
