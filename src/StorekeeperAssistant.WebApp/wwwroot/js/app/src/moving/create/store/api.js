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
    GetWarehouseInventoryItemByWarehouseId: "/warehouse-inventory-item/get-by-warehouse-id",

    /**
    * Создать перемещение
    */
    CreateMoving: "/movings/create"
}

export default api