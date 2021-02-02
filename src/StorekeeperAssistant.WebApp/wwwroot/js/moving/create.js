
Vue.component('v-select', VueSelect.VueSelect);
// transfer vue prototype const in production
const COMING = 1;
const CONSUMPTION = 2;
const MOVING = 3;

var createMovingApp = new Vue({
    el: '#create-moving-app',
    data: {
        typeOperation: null,
        typeOperations: [
            { name: 'Приход', value: COMING },
            { name: 'Расход', value: CONSUMPTION },
            { name: 'Перемещение', value: MOVING }],

        warehouseModels: [],
        nomenclatureModels: [],

        departureWarehouses: [],
        arrivalWarehouses: [],

        selectDepartureWarehouseId: null,
        selectArrivalWarehouseId: null,

        departureWarehouseInventoryItemModels: [],
        isLoaddepartureWarehouseInventoryItemModels: true,
        arrivalWarehouseInventoryItemModels: [],
        isLoadarrivalWarehouseInventoryItemModels: true,
    },
    computed: {

    },
    watch: {
        typeOperation: function () {
            var vue = this;
            vue.selectDepartureWarehouseId = null;
            vue.selectArrivalWarehouseId = null;
        },
        selectDepartureWarehouseId: function () {
            var vue = this;
            var warehouses = [];
            if (vue.selectDepartureWarehouseId) {
                vue.warehouseModels.forEach(function (wd) {
                    if (wd.id != vue.selectDepartureWarehouseId)
                        warehouses.push(wd);
                });

                vue.getByWarehouseIdWarehouseInventoryItem(vue.selectDepartureWarehouseId, 1);
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

            if (vue.typeOperation == MOVING) {
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
                        DepartureWarehouseId: vue.selectDepartureWarehouseId,
                        ArrivalWarehouseId: vue.selectArrivalWarehouseId,
                        CreateInventoryItemModels: createInventoryItemModels
                    };
            }

            if (vue.typeOperation == CONSUMPTION) {
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
                        DepartureWarehouseId: vue.selectDepartureWarehouseId,
                        ArrivalWarehouseId: null,
                        CreateInventoryItemModels: createInventoryItemModels
                    };
            }

            if (vue.typeOperation == COMING) {
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
});