
Vue.component('v-select', VueSelect.VueSelect);
Vue.component('vue-ctk-date-time-picker', window['vue-ctk-date-time-picker']);

var reportApp = new Vue({
    el: '#report-app',
    data: {
        isLoadForm: null,
        inventoryItemModels: [],

        warehouseModels: [],
        selectWarehouseId: null,

        maxSelectDate: moment().format("YYYY-MM-DD"),
        selectDateTime: moment().format("YYYY-MM-DD HH:mm"),
    },
    computed: {

    },
    watch: {
    },
    filters: {

    },
    methods: {
        click_get_report: function () {
            var vue = this;
            if (!vue.selectWarehouseId) {
                alert('Выберите склад');
            }
            else {
                vue.getByWarehouseIdWarehouseInventoryItem();
            }

        },
        getByWarehouseIdWarehouseInventoryItem: function () {
            var vue = this;
            vue.isLoadForm = true;
            axios.get(getByWarehouseIdWarehouseInventoryItemUrl + '/' + vue.selectWarehouseId + '?DateTime=' + moment(vue.selectDateTime).utc().format('YYYY-MM-DD HH:mm:ss'))
                .then((response) => {
                    vue.inventoryItemModels = response.data.inventoryItemModels;
                })
                .catch(response => {
                    console.log(response);
                })
                .finally(() => {
                    vue.isLoadForm = false;
                });
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
    },
    created: function () {
        var vue = this;
        vue.getWarehouses();
    }
});
