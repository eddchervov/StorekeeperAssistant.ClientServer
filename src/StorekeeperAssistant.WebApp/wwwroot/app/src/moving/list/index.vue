
<template>

    <div>

        <template v-if="isLoadPage">

            <div class="row">
                <div class="col-12 text-center">
                    <span class="text-primary">Загрузка...</span>
                </div>
            </div>

        </template>

        <template v-if="isMovingAndIsLoadPage">

            <div class="row">
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
                        <tbody class="c-pointer text-center">
                            <tr v-for="(moving, index) in movings">
                                <td class="align-middle">{{moving.dateTime | toLocalFormat}}</td>

                                <td class="align-middle" v-if="moving.departureWarehouse">{{moving.departureWarehouse.name}}</td>
                                <td class="align-middle" v-if="!moving.departureWarehouse">Извне</td>

                                <td class="align-middle" v-if="moving.arrivalWarehouse">{{moving.arrivalWarehouse.name}}</td>
                                <td class="align-middle" v-if="!moving.arrivalWarehouse">Убрано со складов</td>

                                <td class="align-middle">
                                    <p class="mb-0" v-for="md in moving.movingDetails">{{md.inventoryItem.name}}: {{md.count}} шт.</p>
                                </td>

                                <td class="text-center align-middle">
                                    <button class="btn btn-sm btn-primary" v-on:click="deleteMovings(moving.id)">Удалить</button>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>

        </template>

        <div class="row">
            <div class="col-12">
                <paging :click-handler="getMovings" :totalCount="totalCount" />
            </div>
        </div>

        <template v-if="isNotMovingAndIsLoadPage">

            <div class="row">
                <div class="col-12 text-center">
                    <h5>Нет перемещений</h5>
                </div>
            </div>

        </template>


    </div>
</template>

<script>
    import axios from "axios"
    import moment from 'moment'
    import Paging from '../../components/Paging.vue'

    var getMovingsUrl = "/movings/get";
    var deleteMovingsUrl = "/movings/delete";
    export default {
        data() {
            return {
                isLoadPage: true,
                totalCount: 0,

                movings: []
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
            isMovingAndIsLoadPage() {
                return this.movings.length > 0 && this.isLoadPage == false
            },
            isNotMovingAndIsLoadPage() {
                return this.movings.length == 0 && this.isLoadPage == false
            }
        },
        methods: {
            deleteMovings(movingId) {
                axios.post(deleteMovingsUrl, { MovingId: movingId })
                    .then((response) => {
                        if (response.data.isSuccess) {
                            alert('Успешно удалено')
                            location.reload();
                        }
                        else {
                            if (response.data.message) alert(response.data.message);
                            else alert('Произошла ошибка, обратитесь к администратору');
                        }
                    })
                    .catch(response => {
                        alert('Произошла ошибка, обратитесь к администратору');
                        location.reload();
                    })
                    .finally(() => { });
            },
            getMovings(skipCount = 0, takeCount = 20) {
                var vue = this;
                vue.movings = [];
                vue.isLoadPage = true;
                axios.get(getMovingsUrl + "?skipcount=" + skipCount + "&takecount=" + takeCount)
                    .then((response) => {
                        vue.totalCount = response.data.totalCount;
                        vue.movings = response.data.movings;
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