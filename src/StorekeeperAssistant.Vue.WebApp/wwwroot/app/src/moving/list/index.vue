
<template>

    <div>
        <h4>Список перемещений</h4>
        <hr class="mb-4" />

        <template v-if="isLoadPage">

            <div class="row">
                <div class="col-12 text-center">
                    <span class="text-primary">Загрузка...</span>
                </div>
            </div>

        </template>

        <template v-if="isMovingAndIsLoadPage">

            <div class="row">
                <div class="col-12 mb-2" v-for="(moving, index) in movings">
                    <div class="card">
                        <div class="card-body">
                            <h6 class="card-title mb-3">Дата: {{moving.dateTime | toLocalFormat}}</h6>
                            <h6 class="card-subtitle mb-2">
                                <template v-if="moving.departureWarehouse">
                                    {{moving.departureWarehouse.name}}
                                </template>
                                <template v-else>
                                    Извне
                                </template>
                                <img src="57116.png" class="right-btn" />
                                <template v-if="moving.arrivalWarehouse">
                                    {{moving.arrivalWarehouse.name}}
                                </template>
                                <template v-else>
                                    Убрано со складов
                                </template>
                            </h6>
                            <div class="card-text">
                                <p class="mb-0" v-for="md in moving.movingDetails">{{md.inventoryItem.name}}: {{md.count}} шт.</p>
                            </div>
                            <img class="del-btn"
                                 src="trash.png"
                                 v-on:click="deleteMovings(moving.id)" />
                        </div>
                    </div>
                </div>
            </div>

        </template>

        <paging v-show="isMovingAndIsLoadPage" :click-handler="getMovings" :totalCount="totalCount" />


        <template v-if="isNotMovingAndIsLoadPage">

            <div class="row">
                <div class="col-12 text-center">
                    <h5>Нет перемещений</h5>
                </div>
            </div>

        </template>


    </div>
</template>

<style>
    .del-btn {
        position: absolute;
        right: 15px;
        top: 10px;
        width: 16px;
        height: 16px;
        cursor: pointer;
    }
    .right-btn {
        width: 19px;
        height: 17px;
    }
</style>

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
                var result = confirm("Действительно хотите удалить перемещение?");
                if (!result) return
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