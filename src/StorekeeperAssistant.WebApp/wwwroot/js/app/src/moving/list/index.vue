
<template>

    <div>

        <div class="row" v-if="isLoadPage == true">
            <div class="col-12 text-center">
                <span class="text-primary">Загрузка...</span>
            </div>
        </div>
        <div class="row" v-if="movingModels.length == 0 && isLoadPage == false">
            <div class="col-12 text-center">
                <h5>Нет перемещений</h5>
            </div>
        </div>
        <div class="row" v-if="movingModels.length > 0 && isLoadPage == false">
            <div class="col-12">

                <table class="table table-bordered table-hover table-sm">
                    <thead class="thead-light thead-hermes">
                        <tr class="text-center bg-light">
                            <th class="align-middle"><b>Время</b></th>
                            <th class="align-middle"><b>Откуда</b></th>
                            <th class="align-middle"><b>Куда</b></th>
                            <th class="align-middle"><b>Перенесено</b></th>
                            <th class="align-middle"></th>
                        </tr>
                    </thead>
                    <tbody class="body-hermes c-pointer text-center">
                        <tr v-for="(d, index) in movingModels">
                            <td class="align-middle">{{d.dateTime | toLocalFormat}}</td>

                            <td class="align-middle" v-if="d.departureWarehouse">{{d.departureWarehouse.name}}</td>
                            <td class="align-middle" v-if="!d.departureWarehouse">Извне</td>

                            <td class="align-middle" v-if="d.arrivalWarehouse">{{d.arrivalWarehouse.name}}</td>
                            <td class="align-middle" v-if="!d.arrivalWarehouse">Убрано со складов</td>

                            <td class="align-middle">
                                <p class="mb-0" v-for="md in d.movingDetails">{{md.inventoryItem.name}}: {{md.count}} шт.</p>
                            </td>

                            <td class="text-center align-middle">
                                <button class="btn btn-sm btn-primary" @@click="deleteMovings(d.id)">Удалить</button>
                            </td>
                        </tr>
                    </tbody>
                </table>

                <paging :click-handler="getMovings" :totalCount="totalCount" />

            </div>
        </div>

    </div>
</template>

<script>
    import axios from "axios"
    import moment from 'moment'
    import Paging from './components/Paging.vue'

    var getMovingsUrl = "/movings/get";
    var deleteMovingsUrl = "/movings/delete";
    export default {
        data() {
            return {
                isLoadPage: true,
                totalCount: 0,

                movingModels: []
            }
        },
        filters: {
            toLocalFormat: function (value) {
                var stillUtc = moment.utc(value).toDate();
                return moment(stillUtc).local().format('DD.MM.YYYY HH:mm:ss');
            }
        },
        components: {
            "paging": Paging
        },
        computed: {

        },
        mounted() {

        },
        methods: {
            deleteMovings: function (movingId) {
                var vue = this;
                axios.post(deleteMovingsUrl, { MovingId: movingId })
                    .then((response) => {
                        if (response.data.isSuccess) {
                            alert('Успешно удалено')
                            location.reload();
                        }
                        else {
                            if (response.data.message) {
                                alert(response.data.message);
                            }
                            else {
                                alert('Произошла ошибка, обратитесь к администратору');
                            }

                        }
                    })
                    .catch(response => {
                        alert('Произошла ошибка, обратитесь к администратору');
                        location.reload();
                    })
                    .finally(() => { });
            },
            getMovings: function (skipCount = 0, takeCount = 20) {
                var vue = this;
                axios.get(getMovingsUrl + "?skipcount=" + skipCount + "&takecount=" + takeCount)
                    .then((response) => {
                        vue.totalCount = response.data.totalCount;
                        vue.movingModels = response.data.movings;
                        vue.isLoadPage = false;
                    })
                    .catch(response => {
                        console.log(response);
                    })
                    .finally(() => { });
            },
        },
        created: function () {

            this.getMovings();
        }
    };
</script>