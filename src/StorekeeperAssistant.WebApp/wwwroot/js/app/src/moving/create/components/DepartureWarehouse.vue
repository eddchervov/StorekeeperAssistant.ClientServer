

<template>

    <div class="row mb-2">
        <div class="col-4 text-right">
            <span class="line-h-text-by-input">Склад отправления (Расход)</span>
        </div>
        <div class="col-8">
            <v-select :options="this.$store.getters.departureWarehouses"
                      :reduce="m => m.id"
                      placeholder="Выберите склад отправления"
                      label="name"
                      v-model="this.$store.state.selectDepartureWarehouseId">
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