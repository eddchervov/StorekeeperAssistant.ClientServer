
<template>

    <div>

        <button class="btn btn-primary" @click="click_moving" :disabled="isLoadDepartureWarehouseInventoryItems">{{text}}</button>

    </div>
</template>


<script>
    import { validationMoving } from "../store/validation"
    import api from "../store/api"
    import { mapActions } from 'vuex'

    export default {
        props: {
            text: {
                type: String,
                required: true
            }
        },
        computed: {
            isLoadDepartureWarehouseInventoryItems() {
                return this.$store.state.isLoadDepartureWarehouseInventoryItems
            }
        },
        methods: {
            ...mapActions([
                api.CreateMoving
            ]),
            click_moving: function () {
                var departureWarehouseInventoryItems = this.$store.getters.departureWarehouseInventoryItems;
                var result = validationMoving({ departureWarehouseInventoryItems });
                if (result.isError == false) {

                    var inventoryItems = [];
                    departureWarehouseInventoryItems.forEach((d) => {
                        if (d.newCount) inventoryItems.push({ Id: d.inventoryItem.id, Count: d.newCount });
                    });

                    if (inventoryItems.length == 0) return;

                    var data = {
                        DepartureWarehouseId: this.$store.state.selectDepartureWarehouseId,
                        ArrivalWarehouseId: this.$store.state.selectArrivalWarehouseId,
                        InventoryItems: inventoryItems
                    };

                    this[api.CreateMoving](data)
                }
                else
                    alert(result.messages)
            }
        }
    }
</script>