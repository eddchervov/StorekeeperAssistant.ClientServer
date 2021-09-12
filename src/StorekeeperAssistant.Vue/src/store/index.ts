import { createStore } from 'vuex'
import api from './api'
import mutations from './mutation-types'
import OperationDto from 'src/model/dto/OperationDto'
import TypeOperation from 'src/model/dto/TypeOperation'

export interface State {
  maxValueInventoryItem: number
  typeOperations: Array<TypeOperation>
  operation: OperationDto
  selectOperation: number | null
  selectDepartureWarehouseId: number | null
  selectArrivalWarehouseId: number | null
  isLoadDepartureWarehouseInventoryItems: boolean
  isCreateMoving: boolean
}

export default createStore<State>({
  state: {
    maxValueInventoryItem: 10000,
    typeOperations: [
      { name: 'Приход', id: 1 },
      { name: 'Расход', id: 2 },
      { name: 'Перемещение', id: 3 }
    ],
    operation: {
      coming: 1,
      consumption: 2,
      moving: 3
    },
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
    [mutations.setData]: (state, { name, value }) => (state[name] = value),
    [mutations.setError]: (state, { msg }) => {
      ;(state.serverErrors as Array<string>).push(msg)
      setTimeout(() => {
        const index = (state.serverErrors as Array<string>).indexOf(msg)
        state.serverErrors.splice(index, 1)
      }, 3000)
    },
    [mutations.changeSelectOperation]: (state, value: Number) => {
      state.selectOperation = value
      state.selectDepartureWarehouseId = null
      state.selectArrivalWarehouseId = null
      state.departureWarehouses = state.warehouses
      state.arrivalWarehouses = state.warehouses
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
      this.state.isCreateMoving = true
      saveMoving({ commit }, data)
    }
  }
})
