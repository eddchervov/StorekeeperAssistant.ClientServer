﻿const api = {
    /**
     * Получить склады
     */
    GetWarehouses: "/warehouses/get",

    /**
     * Получить номенклатуры
     */
    GetInventoryItems: "/inventory-items/get",

    /**
    * Получить остатки склада
    */
    GetWarehouseInventoryItems: "/warehouse-inventory-items/get",

    /**
    * Создать перемещение
    */
    CreateMoving: "/movings/create"
}

export default api