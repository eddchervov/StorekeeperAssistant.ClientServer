import axios from "axios"
import api from "./api"

/**
 * Клиент взаимодействия с сервером
 */
const client = {
    /**
     * Получить склады
    */
    getWarehouses: () => axios.get(api.GetWarehouses),
    /**
    * Получить нуменклатуры
    */
    getNomenclatures: () => axios.get(api.GetNomenclatures),
    /**
    * Получить предметы инвентаризации склада по идентификатору склада
    */
    getWarehouseInventoryItemByWarehouseId: (warehouseId) => axios.get(api.GetWarehouseInventoryItemByWarehouseId + '/' + warehouseId),
    /**
    * Создать перемещение
    */
    createMoving: (data) => axios.post(api.CreateMoving, data)
}

export default client