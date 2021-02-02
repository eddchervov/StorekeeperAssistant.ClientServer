
var movingApp = new Vue({
    el: '#moving-app',
    components: {
        'paginate': Vue.component('paginate', VuejsPaginate)
    },
    data: {
        isLoadPage: true,
        // universal paging
        totalCount: 0, // общее кол-во элементов
        totalPage: 0, // общее кол-во страниц
        pageSize: 20, // кол-во элементов на страницу
        currentPage: 1, // текущая страница
        // -----------------
        totalCount: 0,

        movingModels: []
    },
    computed: {
        pagingMessage: function () { // fixed part paging
            var vue = this;

            if (vue.totalPage > 1) {

                var from = (vue.currentPage - 1) * vue.pageSize + 1;
                var to = from + vue.pageSize - 1;

                to = to >= vue.totalCount ? vue.totalCount : to;

                return "Показаны с"
                    + " "
                    + from
                    + " "
                    + "по"
                    + " "
                    + to
                    + " "
                    + "из"
                    + " "
                    + vue.totalCount
                    + " "
                    + "записей";
            } else {
                return '';
            }
        }
    },
    watch: {
    },
    filters: {
        toLocalFormat: function (value) {
            var stillUtc = moment.utc(value).toDate();
            return moment(stillUtc).local().format('DD.MM.YYYY HH:mm:ss');
        }
    },
    methods: {

        go_to_action: function (id) {

        },
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
        getMovings: function () {
            var vue = this;
            var skipCount = (vue.currentPage - 1) * vue.pageSize;
            var takeCount = vue.pageSize;
            axios.get(getMovingsUrl + "?skipcount=" + skipCount + "&takecount=" + takeCount)
                .then((response) => {
                    vue.totalCount = response.data.totalCount;
                    vue.movingModels = response.data.movingModels;
                    vue.isLoadPage = false;
                })
                .catch(response => {
                    console.log(response);
                })
                .finally(() => { });
        },
        // paging methods
        clickPaging: function (e) { // fixed part paging
            var vue = this;
            vue.currentPage = e;
            vue.reloadEntityTable();
        },
        pagingPreparation: function () { // fixed part paging
            var vue = this;
            vue.totalPage = Math.floor((vue.totalCount + vue.pageSize - 1) / vue.pageSize);
        },
        reloadEntityTable: function () { // variable part paging
            var vue = this;
            vue.getMovings();
        },
        reloadAndGoFirstPageEntityTable: function (isDefault = false) { // variable part paging
            var vue = this;
            vue.currentPage = 1;
            vue.getMovings();
        },
        changePageSize: function (count) { // variable part paging
            var vue = this;
            vue.pageSize = count;
            vue.reloadAndGoFirstPageEntityTable();
        },
    },
    created: function () {

        this.getMovings();
    }
});