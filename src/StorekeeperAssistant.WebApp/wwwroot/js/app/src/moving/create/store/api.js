const api = {
    /**
     * Получить склады
     */
    GetWarehouses: "/warehouses/get",

    /**
     * Получить нуменклатуры
     */
    GetNomenclatures: "/nomenclatures/get",

    /**
    * Получить предметы инвентаризации склада по идентификатору склада
    */
    GetWarehouseInventoryItems: "/warehouse-inventory-items/get",

    /**
    * Создать перемещение
    */
    CreateMoving: "/movings/create"
}

export default api