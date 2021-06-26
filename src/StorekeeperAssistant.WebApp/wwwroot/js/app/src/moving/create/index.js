import Vue from "vue"
import App from "./create.vue"
import store from "./store/index"

new Vue({
	el: '#create-moving-app',
	render: h => h(App),
	store: store
});