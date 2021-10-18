
<template>

    <div>
        <h4>Отчет по остаткам</h4>
        <hr class="mb-4" />

        <div class="row mb-2">
            <div class="col-4 text-right">
                <span class="line-h-text-by-input">Склад</span>
            </div>
            <div class="col-8">
                <v-select :options="warehouses"
                          :reduce="m => m.id"
                          placeholder="Выберите склад"
                          label="name"
                          v-model="selectWarehouseId">
                    <template slot="no-options">
                        Не найдено
                    </template>
                </v-select>

            </div>
        </div>

        <div class="row mb-2">
            <div class="col-4 text-right">
                <span class="line-h-text-by-input">Дата</span>
            </div>
            <div class="col-8">

                <template v-if="isCurrentTime == false">
                    <vue-ctk-date-time-picker class="date-input-vue-ctk"
                                              format="DD-MM-YYYY HH:mm"
                                              button-now-translation="Сейчас"
                                              output-format="YYYY-MM-DD HH:mm"
                                              :max-date="maxSelectDate"
                                              :no-clear-button=true
                                              label=""
                                              v-model="selectDateTime">
                    </vue-ctk-date-time-picker>
                </template>

                Текущее время
                <input type="checkbox" v-model="isCurrentTime" />

            </div>
        </div>

        <div class="row mb-4">
            <div class="col-12 text-center">
                <button class="btn btn-primary" v-on:click="click_get_report">
                    Получить
                </button>
            </div>
        </div>

        <template v-if="isNotWarehouseInventoryItemsAndIsNotLoadForm">
            <h4 class="text-center">У склада нет ТМЦ</h4>
        </template>


        <template v-if="isWarehouseInventoryItemsAndIsNotLoadForm">
            <h4 class="text-center">Отчет остатков по дате и времени</h4>

            <table class="table table-bordered table-hover table-sm">
                <thead class="thead-light thead-hermes">
                    <tr class="text-center bg-light">
                        <th class="align-middle"><b>Нуменклатура</b></th>
                        <th class="align-middle"><b>Остаток на складе</b></th>
                    </tr>
                </thead>
                <tbody class="body-hermes c-pointer text-center">
                    <tr v-for="(inventoryItem, index) in warehouseInventoryItems">
                        <td class="align-middle">{{inventoryItem.inventoryItem.name}}</td>
                        <td class="align-middle">{{inventoryItem.count}}</td>
                    </tr>
                </tbody>
            </table>

        </template>
        <template v-if="isLoadForm">

            <div class="row">
                <div class="col-12 text-center text-primary">
                    Загрузка...
                </div>
            </div>

        </template>


    </div>

</template>

<script>
    var getWarehousesUrl = "/warehouses/get";
    var getWarehouseInventoryItemByWarehouseIdUrl = "/warehouse-inventory-items/get";

    import moment from 'moment'
    import vSelect from "vue-select"
    import "vue-select/dist/vue-select.css";
    import axios from "axios"
    import VueCtkDateTimePicker from 'vue-ctk-date-time-picker'
    import 'vue-ctk-date-time-picker/dist/vue-ctk-date-time-picker.css';

    export default {
        data() {
            return {
                isCurrentTime: true,

                isLoadForm: null,
                warehouseInventoryItems: [],

                warehouses: [],
                selectWarehouseId: null,

                maxSelectDate: moment().format("YYYY-MM-DD"),
                selectDateTime: moment().format("YYYY-MM-DD HH:mm")
            }
        },
        components: {
            "v-select": vSelect,
            "vue-ctk-date-time-picker": VueCtkDateTimePicker
        },
        computed: {
            isWarehouseInventoryItemsAndIsNotLoadForm() {
                return this.warehouseInventoryItems.length > 0 && this.isLoadForm == false
            },
            isNotWarehouseInventoryItemsAndIsNotLoadForm() {
                return this.warehouseInventoryItems.length == 0 && this.isLoadForm == false
            }
        },
        methods: {
            click_get_report: function () {
                if (!this.selectWarehouseId) alert('Выберите склад');
                else this.getByWarehouseIdWarehouseInventoryItem();
            },
            getByWarehouseIdWarehouseInventoryItem: function () {
                var vue = this;
                vue.isLoadForm = true;

                var date = null;
                if (vue.isCurrentTime == false) date = moment(vue.selectDateTime);
                else date = moment();

                var dateUtc = date.utc();
                var textDate = dateUtc.format('YYYY-DD-MM HH:mm:ss')
                axios.get(getWarehouseInventoryItemByWarehouseIdUrl + '/' + vue.selectWarehouseId + '?DateTime=' + textDate)
                    .then((response) => {
                        vue.warehouseInventoryItems = response.data.warehouseInventoryItems;
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
                        vue.warehouses = response.data.warehouses;
                        vue.departureWarehouses = response.data.warehouses;
                        vue.arrivalWarehouses = response.data.warehouses;
                    })
                    .catch(response => {
                        console.log(response);
                    })
                    .finally(() => { });
            },
        },
        created: function () {
            this.getWarehouses();
        }
    };

</script>