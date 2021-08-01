
<template>

    <div>
        <div class="row">
            <div class="col-6">

                <!--Paging https://github.com/lokyoung/vuejs-paginate-->
                <template v-if="totalPage > 1">
                    <paginate v-model="currentPage"
                              :page-count="totalPage"
                              :page-range="5"
                              :margin-pages="1"
                              :click-handler="clickPaging"
                              :prev-text="'Предыдущая'"
                              :next-text="'Следующая'"
                              :container-class="'pagination'"
                              :page-link-class="'page-link'"
                              :page-class="'page-item'"
                              :prev-link-class="'page-link'"
                              :prev-class="'page-item'"
                              :next-link-class="'page-link'"
                              :next-class="'page-item'">
                    </paginate>
                </template>
            </div>
            <div class="col-6">

                <ul class="pagination float-right">
                    <li class="pt-1 mr-3">Кол-во элементов на страницу: </li>
                    <li class="page-item"
                        v-bind:class="{ active: pageSize === 20 }"
                        v-on:click="changePageSize(20)">
                        <button class="page-link">
                            20
                        </button>
                    </li>
                    <li class="page-item"
                        v-bind:class="{ active: pageSize === 40 }"
                        v-on:click="changePageSize(40)">
                        <button class="page-link">
                            40
                        </button>
                    </li>
                    <li class="page-item"
                        v-bind:class="{ active: pageSize === 60 }"
                        v-on:click="changePageSize(60)">
                        <button class="page-link">
                            60
                        </button>
                    </li>
                </ul>
            </div>
        </div>
        <template v-if="totalPage > 1">
            <div class="row">
                <div class="col-6">
                    <h7>{{message}}</h7>
                </div>
            </div>
        </template>

    </div>

</template>

<script>
    import Paginate from 'vuejs-paginate'

    export default {
        components: {
            "paginate": Paginate
        },
        props: {
            clickHandler: {
                type: Function,
                default() {
                    return function () { };
                }
            },
            totalCount: {
                type: Number
            }
        },
        data() {
            return {
                pageSize: 20,
                currentPage: 1
            }
        },
        computed: {
            skipCount() {
                return (this.currentPage - 1) * this.pageSize
            },
            takeCount() {
                return this.pageSize
            },
            message() {
                if (this.totalPage == 0) return '';

                var from = (this.currentPage - 1) * this.pageSize + 1;
                var to = from + this.pageSize - 1;
                to = to >= this.totalCount ? this.totalCount : to;

                return "Показаны с " + from + " по " + to + " из " + this.totalCount + " записей";
            },
            totalPage() {
                return Math.floor((this.totalCount + this.pageSize - 1) / this.pageSize);
            }
        },
        methods: {
            clickPaging(e) {
                this.currentPage = e;
                this.reloadEntityTable();
            },
            reloadEntityTable() {
                this.clickHandler(this.skipCount, this.takeCount);
            },
            reloadAndGoFirstPageEntityTable() {
                this.currentPage = 1;
                this.reloadEntityTable();
            },
            changePageSize(count) {
                this.pageSize = count;
                this.reloadAndGoFirstPageEntityTable();
            }
        }
    }
</script>