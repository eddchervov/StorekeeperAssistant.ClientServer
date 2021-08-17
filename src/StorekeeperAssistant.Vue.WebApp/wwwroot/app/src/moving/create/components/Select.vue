

<template>

    <v-select :options="options"
              :reduce="m => m.id"
              :placeholder="placeholder"
              label="name"
              v-model="valueLocal">
        <template slot="no-options">
            Не найдено
        </template>
    </v-select>

</template>

<script>

    import vSelect from 'vue-select'
    import "vue-select/dist/vue-select.css";

    export default {
        props: {
            value: {
                type: Number,
                required: false
            },
            options: {
                type: Array,
                required: true
            },
            placeholder: {
                type: String,
                required: false
            }
        },
        model: { prop: 'value', event: 'change-value' },
        computed: {
            valueLocal: {
                get: function () {
                    return this.value
                },
                set: function (value) {
                    this.$emit('change-value', value)
                }
            },
        },
        components: {
            'v-select': vSelect
        },
    }

</script>