import Vue from "vue"
import Vuex from "vuex"
import api from "./api"
import mutations from "./mutations"
import { loadWarehouses, loadInventoryItems, loadDepartureWarehouseInventoryItems, loadArrivalWarehouseInventoryItems } from "./moving-loader"
import { saveMoving } from "./moving-save"

Vue.use(Vuex)

export default new Vuex.Store({
    state: {
        maxValueInventoryItem: 10000,
        operation: {
            COMING: 1,
            CONSUMPTION: 2,
            MOVING: 3
        },
        typeOperations: [
            { name: 'Приход', id: 1 },
            { name: 'Расход', id: 2 },
            { name: 'Перемещение', id: 3 }],

        selectOperation: null,
        selectDepartureWarehouseId: null,
        selectArrivalWarehouseId: null,

        isLoadDepartureWarehouseInventoryItems: true,
        isCreateMoving: false,

        //lists
        warehouses: [],
        departureWarehouses: [],
        arrivalWarehouses: [],
        inventoryItems: [],
        departureWarehouseInventoryItems: [],
        arrivalWarehouseInventoryItems: [],

        serverErrors: []
    },
    getters: {
        typeOperations: state => state.typeOperations,
        departureWarehouses: state => state.departureWarehouses,
        arrivalWarehouses: state => state.arrivalWarehouses,
        inventoryItems: state => state.inventoryItems,
        departureWarehouseInventoryItems: state => state.departureWarehouseInventoryItems,
        arrivalWarehouseInventoryItems: state => state.arrivalWarehouseInventoryItems,
        serverErrors: state => state.serverErrors
    },
    mutations: {
        [mutations.setData]: (state, { name, value }) => state[name] = value,
        [mutations.setError]: (state, { msg }) => {
            state.serverErrors.push(msg);
            setTimeout(() => {
                var index = state.serverErrors.indexOf(msg);
                state.serverErrors.splice(index, 1);
            }, 3000);
        },

        [mutations.changeSelectOperation]: (state, value) => {
            state.selectOperation = value;
            state.selectDepartureWarehouseId = null;
            state.selectArrivalWarehouseId = null;
            state.departureWarehouses = state.warehouses;
            state.arrivalWarehouses = state.warehouses;
        }
    },
    actions: {
        [api.GetWarehouses]({ commit }) {
            loadWarehouses({ commit })
        },
        [api.GetInventoryItems]({ commit }) {
            loadInventoryItems({ commit })
        },
        [api.GetWarehouseInventoryItems]({ commit }, { warehouseId }) {
            loadDepartureWarehouseInventoryItems({ commit }, { warehouseId })
        },
        [api.CreateMoving]({ commit }, data) {
            this.state.isCreateMoving = true;
            saveMoving({ commit }, data)
        }
    }

})
