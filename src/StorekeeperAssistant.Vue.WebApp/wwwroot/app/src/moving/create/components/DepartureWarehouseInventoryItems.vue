
<template>

    <div class="row mb-2">
        <div class="col-md-12 mb-3">
            <span class="line-h-text-by-input"><b>Доступные лимиты склада</b></span>
        </div>

        <div class="col-md-12 text-primary" v-if="isLoadDepartureWarehouseInventoryItems">
            Загрузка...
        </div>

        <template v-if="isExistDepartureWarehouseInventoryItems">

            <template v-for="(ii, index) in departureWarehouseInventoryItems">
                <div class="col-md-6 col-xl-4 mb-2">
                    <span class="line-h-text-by-input">{{ ii.inventoryItem.name }} ({{ ii.count }})</span>
                </div>
                <div class="col-md-6 col-xl-8 mb-2">
                    <input type="number" class="form-control" v-model.number="ii.newCount" />
                </div>
            </template>

        </template>

        <template v-if="isNotExistDepartureWarehouseInventoryItems">
            <div class="col-8">
                У склада нет ТМЦ
            </div>
        </template>

    </div>

</template>

<script>
    export default {
        computed: {
            isExistDepartureWarehouseInventoryItems() {
                return this.isMovingOrConsumption && this.isLoadDepartureWarehouseInventoryItems == false && this.departureWarehouseInventoryItems.length > 0
            },
            isNotExistDepartureWarehouseInventoryItems() {
                return this.isLoadDepartureWarehouseInventoryItems == false && this.departureWarehouseInventoryItems.length == 0
            },
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