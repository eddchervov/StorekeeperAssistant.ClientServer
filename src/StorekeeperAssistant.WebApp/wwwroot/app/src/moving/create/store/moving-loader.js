import mutations from "./mutations"
import client from "./Client"

export function loadWarehouses({ commit }) {
    client.getWarehouses()
        .then(p => {
            commit(mutations.setData, {
                name: "departureWarehouses",
                value: p.data.warehouses
            })
            commit(mutations.setData, {
                name: "arrivalWarehouses",
                value: p.data.warehouses
            })
            commit(mutations.setData, {
                name: "warehouses",
                value: p.data.warehouses
            })
        })
        .catch(e => { commit(mutations.setError, { msg: e }) });
}

export function loadInventoryItems({ commit }) {
    client.getInventoryItems()
        .then(p => {
            var inventoryItems = [];
            p.data.inventoryItems.forEach((inventoryItem) => {
                inventoryItem.newCount = null;
                inventoryItems.push(inventoryItem);
            });
            commit(mutations.setData, {
                name: "inventoryItems",
                value: inventoryItems
            })
        })
        .catch(e => { commit(mutations.setError, { msg: e }) });
}

export function loadDepartureWarehouseInventoryItems({ commit }, { warehouseId }) {
    client.getWarehouseInventoryItems({ warehouseId })
        .then(p => {
            commit(mutations.setData, {
                name: "isLoadDepartureWarehouseInventoryItems",
                value: false
            })

            var departureWarehouseInventoryItems = [];
            p.data.warehouseInventoryItems.forEach(function (ii) {
                ii.newCount = null;
                departureWarehouseInventoryItems.push(ii);
            })

            commit(mutations.setData, {
                name: "departureWarehouseInventoryItems",
                value: departureWarehouseInventoryItems
            })
        })
        .catch(e => { commit(mutations.setError, { msg: e }) });
}