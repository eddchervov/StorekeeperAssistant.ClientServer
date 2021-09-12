
<template>

    <div class="row mb-2">
        <div class="col-md-3 mb-2">
            <span class="line-h-text-by-input">Тип операции</span>
        </div>
        <div class="col-md-9">
            <v-select v-model="selectOperation"
                      :options="$store.getters.typeOperations"
                      :placeholder="'Выберите тип операции'" />
        </div>
    </div>

</template>

<script>
    import Select from "../components/Select.vue"
    export default {
        components: {
            'v-select': Select
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