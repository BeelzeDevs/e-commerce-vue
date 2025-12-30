import "./main.css";
import { createApp } from 'vue';
import { createPinia } from 'pinia';

import App from './App.vue';
import router from './router';
import { useCartStore } from "./store/cartStore";

const app = createApp(App);

app.use(createPinia());
app.use(router);
app.mount('#app');

const carrito = useCartStore();
carrito.cargarCarrito();
