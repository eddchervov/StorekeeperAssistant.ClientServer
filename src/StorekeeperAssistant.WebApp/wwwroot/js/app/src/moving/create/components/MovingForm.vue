
<template>

    <div>

        <departure-warehouse />
        <template v-if="isDepartureWarehouseInventoryItems">
            <departure-warehouse-inventory-items />
        </template>

        <arrival-warehouse />

    </div>
</template>


<script>
    import DepartureWarehouse from "../components/DepartureWarehouse.vue"
    import DepartureWarehouseInventoryItems from "../components/DepartureWarehouseInventoryItems.vue"
    import ArrivalWarehouse from "../components/ArrivalWarehouse.vue"

    export default {
        components: {
            "departure-warehouse": DepartureWarehouse,
            "departure-warehouse-inventory-items": DepartureWarehouseInventoryItems,
            "arrival-warehouse": ArrivalWarehouse
        },
        computed: {
            isDepartureWarehouseInventoryItems() {
                return (
                    this.$store.state.selectDepartureWarehouseId && this.$store.getters.departureWarehouses.length > 0
                );
            }
        }
    }
</script>