
<template>

    <div>

        <button class="btn btn-secondary" @click="click_moving" :disabled="isCreateMoving">{{text}}</button>

    </div>
</template>


<script>
    import api from "../store/api"
    import { validationComing } from "../store/validation"
    import { mapActions } from 'vuex'
    import mutations from "../store/mutations"

    export default {
        props: {
            text: {
                type: String,
                required: true
            },
        },
        computed: {
            isCreateMoving() {
                return this.$store.state.isCreateMoving
            }
        },
        methods: {
            ...mapActions([
                api.CreateMoving
            ]),
            click_moving() {
                var inventoryItems = this.getNewInventoryItems();
                var result = validationComing(this, { inventoryItems });
                if (result.isError == false) {
                    var createdInventoryItems = this.getCreatedInventoryItems(inventoryItems);
                    var data = {
                        DepartureWarehouseId: null,
                        ArrivalWarehouseId: this.$store.state.selectArrivalWarehouseId,
                        InventoryItems: createdInventoryItems
                    };

                    this[api.CreateMoving](data)
                }
                else
                    result.messages.forEach(x => { this.$store.commit(mutations.setError, { msg: x }) });
            },
            getNewInventoryItems() {
                var inventoryItems = this.$store.getters.inventoryItems;
                var newInventoryItems = [];

                inventoryItems.forEach((d) => {
                    if (d.newCount) newInventoryItems.push(d);
                });
                return newInventoryItems;
            },
            getCreatedInventoryItems(inventoryItems) {
                var createdInventoryItems = [];
                inventoryItems.forEach((d) => {
                    if (d.newCount) createdInventoryItems.push({ Id: d.id, Count: d.newCount });
                });
                return createdInventoryItems;
            }
        }
    }
</script>