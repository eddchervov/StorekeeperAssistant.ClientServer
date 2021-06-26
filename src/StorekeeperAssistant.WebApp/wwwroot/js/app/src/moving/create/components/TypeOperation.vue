
<template>

    <div class="row mb-2">
        <div class="col-4 text-right">
            <span class="line-h-text-by-input">Тип операции</span>
        </div>
        <div class="col-8">

            <v-select :options="this.$store.getters.typeOperations"
                      :reduce="m => m.value"
                      placeholder="Выберите тип операции"
                      label="name"
                      v-model="selectOperation">
                <template slot="no-options">
                    Не найдено
                </template>
            </v-select>

        </div>
    </div>

</template>

<script>

    import vSelect from 'vue-select'

    export default {
        computed: {
            selectOperation: {
                get() {
                    return this.$store.state.selectOperation
                },
                set(value) {
                    this.$store.commit('changeSelectOperation', value)
                }
            }
        },
        components: {
            'v-select': vSelect,
        }
    }

</script>