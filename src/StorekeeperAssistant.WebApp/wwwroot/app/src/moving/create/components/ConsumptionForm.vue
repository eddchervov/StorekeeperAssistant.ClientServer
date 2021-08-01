
<template>

    <div>

        <departure-warehouse />

        <template v-if="isDepartureWarehouseInventoryItems">
            <departure-warehouse-inventory-items />
        </template>

    </div>
</template>


<script>
    import DepartureWarehouse from "../components/DepartureWarehouse.vue"
    import DepartureWarehouseInventoryItems from "../components/DepartureWarehouseInventoryItems.vue"

    export default {
        components: {
            "departure-warehouse": DepartureWarehouse,
            "departure-warehouse-inventory-items": DepartureWarehouseInventoryItems
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