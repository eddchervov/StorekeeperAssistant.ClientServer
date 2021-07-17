
<template>

    <div>

        <button class="btn btn-primary" @click="click_moving">{{text}}</button>

    </div>
</template>


<script>
    import api from "../store/api"
    import { mapActions } from 'vuex'

    export default {
        props: {
            text: {
                type: String,
                required: true
            },
        },
        methods: {
            ...mapActions([
                api.CreateMoving
            ]),
            click_moving: function () {
                var inventoryItems = [];
                this.$store.getters.inventoryItems.forEach((d) => {
                    if (d.newCount)
                        inventoryItems.push({
                            Id: d.id,
                            Count: d.newCount
                        });
                });

                if (inventoryItems.length == 0) return;

                var data = {
                    DepartureWarehouseId: null,
                    ArrivalWarehouseId: this.$store.state.selectArrivalWarehouseId,
                    InventoryItems: inventoryItems
                };

                this[api.CreateMoving](data)
            }
        }
    }
</script>