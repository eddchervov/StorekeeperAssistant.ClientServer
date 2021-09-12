
<template>

    <div class="row mb-2">
        <div class="col-md-4 mb-2">
            <span class="line-h-text-by-input">Склад отправления (Расход)</span>
        </div>
        <div class="col-md-8">
            <v-select v-model="selectDepartureWarehouseId"
                      :options="departureWarehouses"
                      :placeholder="'Выберите склад отправления'" />
        </div>
    </div>

</template>

<script>
    import Select from "../components/Select.vue"
    import api from "../store/api"
    import { mapActions } from 'vuex'
    import mutations from "../store/mutations"
    export default {
        components: {
            'v-select': Select
        },
        computed: {
            selectDepartureWarehouseId: {
                get: function () {
                    return this.$store.state.selectDepartureWarehouseId
                },
                set: function (value) {
                    this.$store.commit(mutations.setData, { name: "selectDepartureWarehouseId", value: value })
                    var warehouses = [];
                    if (value) {
                        this.$store.state.warehouses.forEach((wd) => {
                            if (wd.id != this.$store.state.selectDepartureWarehouseId)
                                warehouses.push(wd);
                        });

                        this.$store.commit(mutations.setData, { name: "isLoadDepartureWarehouseInventoryItems", value: true })
                        this[api.GetWarehouseInventoryItems]({ warehouseId: value });
                    }
                    else
                        warehouses = this.$store.state.warehouses;
                    this.$store.commit(mutations.setData, { name: "arrivalWarehouses", value: warehouses })
                }
            },
            departureWarehouses() {
                return this.$store.getters.departureWarehouses
            }
        },
        methods: {
            ...mapActions([
                api.GetWarehouseInventoryItems
            ])
        }
    }
</script>