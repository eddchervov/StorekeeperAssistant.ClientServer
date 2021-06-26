import Vue from "vue"
import Vuex from "vuex"

Vue.use(Vuex)

export default new Vuex.Store({
    state: {
        operation: {
            COMING: 'COMING',
            CONSUMPTION: 'CONSUMPTION',
            MOVING: 'MOVING'
        },
        typeOperations: [
            { name: 'Приход', value: 'COMING' },
            { name: 'Расход', value: 'CONSUMPTION' },
            { name: 'Перемещение', value: 'MOVING' }],
        selectOperation: null,
        selectDepartureWarehouseId: null,

        //lists
        departureWarehouses: [],
    },
    mutations: {

        changeSelectOperation(state, value) {
            state.selectOperation = value;
        },
        changeDepartureWarehouses(state, values) {
            state.departureWarehouses = values;
        },


    },
    getters: {
        typeOperations: state => state.typeOperations,
        departureWarehouses: state => state.departureWarehouses,
    }

})
