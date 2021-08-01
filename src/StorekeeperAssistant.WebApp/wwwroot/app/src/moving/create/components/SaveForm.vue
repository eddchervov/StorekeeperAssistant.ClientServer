

<template>

    <div>

        <div class="row mb-4">
            <div class="col-12 text-center">

                <template v-if="isMovingAndSelectDepartureAndArrival">
                    <save-moving :text="'Переместить'" />
                </template>

                <template v-if="isConsumptionAndSelectDeparture">
                    <save-moving :text="'Убрать со склада'" />
                </template>

                <template v-if="isComingAndSelectArrival">
                    <save-coming :text="'Добавить на склад'" />
                </template>

            </div>
        </div>

    </div>
</template>


<script>
    import SaveMoving from "../components/SaveMoving.vue"
    import SaveComing from "../components/SaveComing.vue"

    export default {
        components: {
            "save-moving": SaveMoving,
            "save-coming": SaveComing
        },
        computed: {
            isMovingAndSelectDepartureAndArrival() {
                return (
                    this.$store.state.selectOperation == this.$store.state.operation.MOVING &&
                    this.$store.state.selectDepartureWarehouseId && this.$store.state.selectArrivalWarehouseId
                );
            },
            isConsumptionAndSelectDeparture() {
                return (
                    this.$store.state.selectOperation == this.$store.state.operation.CONSUMPTION && this.$store.state.selectDepartureWarehouseId
                );
            },
            isComingAndSelectArrival() {
                return (
                    this.$store.state.selectOperation == this.$store.state.operation.COMING && this.$store.state.selectArrivalWarehouseId
                );
            }
        }
    }
</script>