
<template>

    <div>

        <template>
            <error-form />
        </template>

        <template>
            <select-operation />
        </template>

        <template v-if="isComing">
            <coming-form />
        </template>

        <template v-if="isConsumption">
            <consumption-form />
        </template>

        <template v-if="isMoving">
            <moving-form />
        </template>

        <template>
            <save-form />
        </template>

    </div>
</template>

<script>
    import api from "./store/api"
    import { mapActions } from 'vuex'

    import SelectOperation from "./components/SelectOperation.vue"
    import ComingForm from "./components/ComingForm.vue"
    import ConsumptionForm from "./components/ConsumptionForm.vue"
    import MovingForm from "./components/MovingForm.vue"
    import SaveForm from "./components/SaveForm.vue"
    import ErrorForm from "./components/ErrorForm.vue"

    export default {
        components: {
            "select-operation": SelectOperation,
            "coming-form": ComingForm,
            "consumption-form": ConsumptionForm,
            "moving-form": MovingForm,
            "save-form": SaveForm,
            "error-form": ErrorForm
        },
        computed: {
            isMoving() {
                return this.$store.state.selectOperation == this.$store.state.operation.MOVING
            },
            isComing() {
                return this.$store.state.selectOperation == this.$store.state.operation.COMING
            },
            isConsumption() {
                return this.$store.state.selectOperation == this.$store.state.operation.CONSUMPTION
            }
        },
        mounted() {
            this[api.GetWarehouses](),
                this[api.GetInventoryItems]()
        },
        methods: {
            ...mapActions([
                api.GetWarehouses,
                api.GetInventoryItems
            ])
        }
    };
</script>