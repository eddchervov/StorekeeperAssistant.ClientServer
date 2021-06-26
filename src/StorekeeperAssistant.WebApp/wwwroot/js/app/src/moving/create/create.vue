
<template>

    <div>

        <template>
            <type-operation-component />
        </template>

        <template v-if="isMovingOrConsumption">
            <departure-warehouse-component />
        </template>

        <template v-if="isComing">

            <div class="row mb-2">

                <div class="col-12 mb-2 text-right">
                    <b>Добавить на склад извне</b>
                </div>
                <template v-for="n in nomenclatureModels">
                    <div class="col-6 mb-2 text-right">
                        <span class="line-h-text-by-input">{{n.name}}</span>
                    </div>
                    <div class="col-6 mb-2">
                        <input type="number" class="form-control" v-model="n.newCount" />
                    </div>
                </template>
            </div>

        </template>

        <template v-if="isInventoryItemsDepartureWarehouse">

            <div class="row mb-2">

                <div class="col-4 text-right">
                    <span class="line-h-text-by-input">ТМЦ склада</span>
                </div>
                <div class="col-8 text-primary" v-if="isLoaddepartureWarehouseInventoryItemModels">
                    Загрузка...
                </div>
                <div class="col-8" v-if="isLoaddepartureWarehouseInventoryItemModels == false && departureWarehouseInventoryItemModels.length > 0">
                    <div class="row mb-2"
                         v-for="ii in departureWarehouseInventoryItemModels">

                        <template v-if="isMovingOrConsumption">
                            <div class="col-6 text-right">
                                <span class="line-h-text-by-input">{{ii.nomenclatureModel.name}} ({{ii.count}})</span>
                            </div>
                            <div class="col-6">
                                <input type="number" class="form-control" v-model="ii.newCount" />
                            </div>
                        </template>

                    </div>
                </div>
                <div class="col-8" v-if="isLoaddepartureWarehouseInventoryItemModels == false && departureWarehouseInventoryItemModels.length == 0">
                    У склада нет ТМЦ
                </div>
            </div>

        </template>

        <template v-if="isMovingOrComing">

            <div class="row mb-2">
                <div class="col-4 text-right">
                    <span class="line-h-text-by-input">Склад прибытия (Приход)</span>
                </div>
                <div class="col-8">
                    <v-select :options="arrivalWarehouses"
                              :reduce="m => m.id"
                              placeholder="Выберите склад прибытия"
                              label="name"
                              v-model="selectArrivalWarehouseId">
                        <template slot="no-options">
                            Не найдено
                        </template>
                    </v-select>
                </div>
            </div>

        </template>

        <template v-if="isInventoryItemsArrivalWarehouse">

            <div class="row mb-2">
                <div class="col-4 text-right">
                    <span class="line-h-text-by-input">ТМЦ склада</span>
                </div>
                <div class="col-8 text-primary" v-if="isLoadarrivalWarehouseInventoryItemModels">
                    Загрузка...
                </div>
                <div class="col-8" v-if="isLoadarrivalWarehouseInventoryItemModels == false && arrivalWarehouseInventoryItemModels.length > 0">
                    <div class="row mb-2"
                         v-for="ii in arrivalWarehouseInventoryItemModels">

                        <template v-if="isMovingOrComing">
                            <div class="col-12">
                                <span class="line-h-text-by-input">{{ii.nomenclatureModel.name}} ({{ii.count}})</span>
                            </div>
                        </template>
                    </div>
                </div>

                <div class="col-8" v-if="isLoadarrivalWarehouseInventoryItemModels == false && arrivalWarehouseInventoryItemModels.length == 0">
                    У склада нет ТМЦ
                </div>
            </div>

        </template>

        <div class="row mb-4">
            <div class="col-12 text-center">
                <template v-if="isMovingAndSelectDepartureAndSelectArrival">
                    <button class="btn btn-primary" @click="click_moving">Переместить</button>
                </template>
                <template v-if="isConsumptionAndSelectDeparture">
                    <button class="btn btn-primary" @click="click_del">Убрать со склада</button>
                </template>
                <template v-if="isComingAndSelectArrival">
                    <button class="btn btn-primary" @click="click_add">Добавить на склад</button>
                </template>
            </div>
        </div>

    </div>

</template>


<script>

    import axios from "axios"
    import vSelect from 'vue-select'

    import TypeOperationComponent from "./components/TypeOperation.vue"
    import DepartureWarehouseComponent from "./components/DepartureWarehouse.vue"

    export default {
        components: {
            'v-select': vSelect,
            'type-operation-component': TypeOperationComponent,
            'departure-warehouse-component': DepartureWarehouseComponent
        },
        data() {
            return {
                warehouseModels: [],
                nomenclatureModels: [],

                departureWarehouses: [],
                arrivalWarehouses: [],

                selectArrivalWarehouseId: null,

                departureWarehouseInventoryItemModels: [],
                isLoaddepartureWarehouseInventoryItemModels: true,
                arrivalWarehouseInventoryItemModels: [],
                isLoadarrivalWarehouseInventoryItemModels: true,
            };
        },
        computed: {
            isMovingOrConsumption() {
                return this.$store.state.selectOperation == this.$store.state.operation.MOVING || this.$store.state.selectOperation == this.$store.state.operation.CONSUMPTION
            },
            isComing() {
                return this.$store.state.selectOperation == this.$store.state.operation.COMING
            },
            isMovingOrComing() {
                return this.$store.state.selectOperation == this.$store.state.operation.MOVING || this.$store.state.selectOperation == this.$store.state.operation.COMING
            },
            isInventoryItemsDepartureWarehouse() {
                return this.$store.state.selectDepartureWarehouseId && this.departureWarehouses.length > 0
            },
            isInventoryItemsArrivalWarehouse() {
                return this.selectArrivalWarehouseId && this.arrivalWarehouses.length > 0
            },

            isMovingAndSelectDepartureAndSelectArrival() {
                return this.$store.state.selectOperation == this.$store.state.operation.MOVING && this.$store.state.selectDepartureWarehouseId && this.selectArrivalWarehouseId
            },
            isConsumptionAndSelectDeparture() {
                return this.$store.state.selectOperation == this.$store.state.operation.CONSUMPTION && this.$store.state.selectDepartureWarehouseId
            },
            isComingAndSelectArrival() {
                return this.$store.state.selectOperation == this.$store.state.operation.COMING && this.selectArrivalWarehouseId
            },

            selectOperation: {
                get() {
                    return this.$store.state.selectOperation
                },
                set(value) {
                    this.$store.commit('changeSelectOperation', value)
                }
            },
        },
        watch: {
            selectOperation() {
                var vue = this;
                vue.$store.state.selectDepartureWarehouseId = null;
                vue.selectArrivalWarehouseId = null;
            },
            selectDepartureWarehouseId: function () {
                var vue = this;
                var warehouses = [];
                if (vue.$store.state.selectDepartureWarehouseId) {
                    vue.warehouseModels.forEach(function (wd) {
                        if (wd.id != vue.$store.state.selectDepartureWarehouseId)
                            warehouses.push(wd);
                    });

                    vue.getByWarehouseIdWarehouseInventoryItem(vue.$store.state.selectDepartureWarehouseId, 1);
                }
                else
                    warehouses = vue.warehouseModels;
                vue.arrivalWarehouses = warehouses;
            },
            selectArrivalWarehouseId: function () {
                var vue = this;
                var warehouses = [];
                if (vue.selectArrivalWarehouseId) {
                    vue.warehouseModels.forEach(function (wd) {
                        if (wd.id != vue.selectArrivalWarehouseId)
                            warehouses.push(wd);
                    });

                    vue.getByWarehouseIdWarehouseInventoryItem(vue.selectArrivalWarehouseId, 2);
                }
                else
                    warehouses = vue.warehouseModels;
                vue.departureWarehouses = warehouses;
            }
        },
        methods: {
            click_del: function () {
                var vue = this;
                if (vue.validation_moving()) {
                    vue.createMoving();
                }
            },
            click_add: function () {
                var vue = this;
                if (vue.validation_add()) {
                    vue.createMoving();
                }
            },
            click_moving: function () {
                var vue = this;
                if (vue.validation_moving()) {
                    vue.createMoving();
                }
            },
            validation_add: function () {
                var vue = this;
                var countError = 0;
                var messages = [];

                vue.nomenclatureModels.forEach(function (n) {
                    if (n.newCount) {
                        if (n.newCount > 1000) {
                            countError++;
                            messages.push(n.name + ': Нельзя добавлять больше 1000 товаров склад');
                        }
                    }
                });

                if (countError > 0) {
                    var text = '';
                    messages.forEach(function (m) {
                        text += m + '\n';
                    });

                    alert(text)
                }

                return countError == 0;
            },
            validation_moving: function () {
                var vue = this;
                var countError = 0;
                var messages = [];

                vue.departureWarehouseInventoryItemModels.forEach(function (d) {
                    if (d.newCount) {
                        if (d.count < d.newCount) {
                            countError++;
                            messages.push(d.nomenclatureModel.name + ': Вы ввели значения больше чем находится на складе');
                        }
                    }
                });

                if (countError > 0) {
                    var text = '';
                    messages.forEach(function (m) {
                        text += m + '\n';
                    });

                    alert(text)
                }

                return countError == 0;
            },
            getWarehouses: function () {
                var vue = this;
                axios.get(getWarehousesUrl)
                    .then((response) => {
                        vue.warehouseModels = response.data.warehouseModels;
                        vue.departureWarehouses = response.data.warehouseModels;
                        vue.arrivalWarehouses = response.data.warehouseModels;
                    })
                    .catch(response => {
                        console.log(response);
                    })
                    .finally(() => { });
            },
            getNomenclatures: function () {
                var vue = this;
                axios.get(getNomenclaturesUrl)
                    .then((response) => {
                        vue.nomenclatureModels = [];
                        response.data.nomenclatureModels.forEach(function (n) {
                            n.newCount = null;
                            vue.nomenclatureModels.push(n);
                        });
                    })
                    .catch(response => {
                        console.log(response);
                    })
                    .finally(() => { });
            },
            getByWarehouseIdWarehouseInventoryItem: function (WarehouseId, type) {
                var vue = this;
                if (type == 1) {
                    vue.isLoaddepartureWarehouseInventoryItemModels = true;
                }
                if (type == 2) {
                    vue.isLoadarrivalWarehouseInventoryItemModels = true;
                }
                axios.get(getByWarehouseIdWarehouseInventoryItemUrl + '/' + WarehouseId)
                    .then((response) => {
                        if (type == 1) {
                            vue.isLoaddepartureWarehouseInventoryItemModels = false;

                            vue.departureWarehouseInventoryItemModels = [];
                            response.data.inventoryItemModels.forEach(function (ii) {
                                ii.newCount = null;
                                vue.departureWarehouseInventoryItemModels.push(ii);
                            });
                        }

                        if (type == 2) {
                            vue.isLoadarrivalWarehouseInventoryItemModels = false;
                            vue.arrivalWarehouseInventoryItemModels = response.data.inventoryItemModels;
                        }

                    })
                    .catch(response => {
                        console.log(response);
                    })
                    .finally(() => { });
            },
            createMoving: function () {
                var vue = this;

                var data = null;

                if (vue.$store.state.selectOperation == vue.$store.state.operation.MOVING) {
                    var createInventoryItemModels = [];
                    vue.departureWarehouseInventoryItemModels.forEach(function (d) {
                        if (d.newCount)
                            createInventoryItemModels.push({
                                Id: d.nomenclatureModel.id,
                                Count: +d.newCount
                            });
                    });
                    if (createInventoryItemModels.length > 0)
                        data = {
                            DepartureWarehouseId: vue.$store.state.selectDepartureWarehouseId,
                            ArrivalWarehouseId: vue.selectArrivalWarehouseId,
                            CreateInventoryItemModels: createInventoryItemModels
                        };
                }

                if (vue.$store.state.selectOperation == vue.$store.state.operation.CONSUMPTION) {
                    var createInventoryItemModels = [];
                    vue.departureWarehouseInventoryItemModels.forEach(function (d) {
                        if (d.newCount)
                            createInventoryItemModels.push({
                                Id: d.nomenclatureModel.id,
                                Count: +d.newCount
                            });
                    });
                    if (createInventoryItemModels.length > 0)
                        data = {
                            DepartureWarehouseId: vue.$store.state.selectDepartureWarehouseId,
                            ArrivalWarehouseId: null,
                            CreateInventoryItemModels: createInventoryItemModels
                        };
                }

                if (vue.$store.state.selectOperation == vue.$store.state.operation.COMING) {
                    var createInventoryItemModels = [];
                    vue.nomenclatureModels.forEach(function (n) {
                        if (n.newCount)
                            createInventoryItemModels.push({
                                Id: n.id,
                                Count: +n.newCount
                            });
                    });
                    if (createInventoryItemModels.length > 0)
                        data = {
                            DepartureWarehouseId: null,
                            ArrivalWarehouseId: vue.selectArrivalWarehouseId,
                            CreateInventoryItemModels: createInventoryItemModels
                        };
                }

                if (data)
                    axios.post(createMovingUrl, data)
                        .then((response) => {
                            location.reload();

                        })
                        .catch(response => {
                            alert('Произошла ошибка, обратитесь к администратору');
                            location.reload();
                        })
                        .finally(() => { });
            }

        },
        created: function () {
            var vue = this;
            vue.getNomenclatures();
            vue.getWarehouses();
        }
    }
</script>