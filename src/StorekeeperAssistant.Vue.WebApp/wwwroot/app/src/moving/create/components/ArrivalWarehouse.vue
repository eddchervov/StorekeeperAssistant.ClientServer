
<template>

    <div class="row mb-2">
        <div class="col-md-4 mb-2">
            <span class="line-h-text-by-input">Склад прибытия (Приход)</span>
        </div>
        <div class="col-md-8">
            <v-select v-model="selectArrivalWarehouseId"
                      :options="arrivalWarehouses"
                      :placeholder="'Выберите склад прибытия'" />
        </div>
    </div>

</template>

<script>
    import Select from "../components/Select.vue"
    import mutations from "../store/mutations"
    export default {
        components: {
            'v-select': Select
        },
        computed: {
            selectArrivalWarehouseId: {
                get: function () {
                    return this.$store.state.selectArrivalWarehouseId
                },
                set: function (value) {
                    this.$store.commit(mutations.setData, { name: "selectArrivalWarehouseId", value: value })

                    var warehouses = [];
                    if (value)
                        this.$store.state.warehouses.forEach((wd) => {
                            if (wd.id != this.$store.state.selectArrivalWarehouseId) warehouses.push(wd);
                        });
                    else
                        warehouses = this.$store.state.warehouses;

                    this.$store.commit(mutations.setData, { name: "departureWarehouses", value: warehouses })
                }
            },
            arrivalWarehouses() {
                return this.$store.getters.arrivalWarehouses
            }
        }
    }
</script>