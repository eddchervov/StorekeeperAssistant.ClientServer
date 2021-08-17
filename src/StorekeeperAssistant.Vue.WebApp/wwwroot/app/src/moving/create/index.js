import Vue from "vue"
import App from "./index.vue"
import store from "./store/vuex"

new Vue({
	el: '#create-moving-app',
	render: h => h(App),
	store: store
});