
<template>

    <div>

        <coming />
        <arrival-warehouse />

    </div>
</template>


<script>
    import Coming from "../components/Coming.vue"
    import ArrivalWarehouse from "../components/ArrivalWarehouse.vue"

    export default {
        components: {
            "coming": Coming,
            "arrival-warehouse": ArrivalWarehouse
        }
    }
</script>