
<template>

    <div class="error-box">

        <div class="alert alert-danger" role="alert"
             v-for="error in serverErrors">
            {{error}}
        </div>

    </div>

</template>

<style>
    .error-box {
        z-index: 1;
        position: absolute;
        margin-left: auto;
        margin-right: 15px;
        left: 0;
        right: 0;
        text-align: center;
        width: 100%;
        max-width: 500px;
    }

    .alert-danger {
        padding: 10px;
    }
</style>

<script>
    export default {
        computed: {
            serverErrors() {
                return this.$store.getters.serverErrors
            }
        }
    }
</script>