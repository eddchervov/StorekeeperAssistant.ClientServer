
<template>

    <div class="row mb-2">
        <div class="col-4 text-right">
            <span class="line-h-text-by-input">Тип операции</span>
        </div>
        <div class="col-8">
            <v-select v-model="selectOperation"
                      :options="$store.getters.typeOperations"
                      :placeholder="'Выберите тип операции'" />
        </div>
    </div>

</template>

<script>
    import SelectComponent from "./Select.vue";
    export default {
        components: {
            'v-select': SelectComponent
        },
        computed: {
            selectOperation: {
                get: function () {
                    return this.$store.state.selectOperation
                },
                set: function (value) {
                    this.$store.commit('changeSelectOperation', value)
                }
            }
        },
    }
</script>