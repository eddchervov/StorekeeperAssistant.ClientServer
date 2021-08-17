
<template>

    <div class="row mb-2">
        <div class="col-4 text-right">
            <span class="line-h-text-by-input">ТМЦ склада</span>
        </div>

        <div class="col-8 text-primary" v-if="isLoadDepartureWarehouseInventoryItems">
            Загрузка...
        </div>

        <template v-if="isLoadDepartureWarehouseInventoryItems == false && departureWarehouseInventoryItems.length > 0">

            <div class="col-8" >
                <div class="row mb-2" v-for="ii in departureWarehouseInventoryItems">

                    <template v-if="isMovingOrConsumption">

                        <div class="col-6 text-right">
                            <span class="line-h-text-by-input">{{ ii.inventoryItem.name }} ({{ ii.count }})</span>
                        </div>
                        <div class="col-6">
                            <input type="number" class="form-control" v-model.number="ii.newCount" />
                        </div>

                    </template>

                </div>
            </div>

        </template>
        
        <template v-if="isLoadDepartureWarehouseInventoryItems == false && departureWarehouseInventoryItems.length == 0">

            <div class="col-8">
                У склада нет ТМЦ
            </div>

        </template>
       
    </div>

</template>

<script>
    export default {
        computed: {
            isLoadDepartureWarehouseInventoryItems() {
                return this.$store.state.isLoadDepartureWarehouseInventoryItems
            },
            isMovingOrConsumption() {
                return (
                    this.$store.state.selectOperation == this.$store.state.operation.MOVING ||
                    this.$store.state.selectOperation == this.$store.state.operation.CONSUMPTION
                );
            },
            departureWarehouseInventoryItems() {
                return this.$store.getters.departureWarehouseInventoryItems
            }
        }
    }
</script>