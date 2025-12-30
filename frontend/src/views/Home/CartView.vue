<template>
  <NavBar />

  <section class="px-6 py-10 text-white bg-bgContent w-full min-h-[90dvh]">
    <h1 class="text-2xl font-bold mb-6">Tu carrito</h1>

    <div v-if="carrito.items.length === 0" class="text-center py-20">
      <p class="text-slate-400 mb-4">Tu carrito está vacío</p>
      <router-link to="/" class="bg-blue-600 hover:bg-blue-500 px-6 py-3 rounded-lg font-semibold">
        Ver productos
      </router-link>
    </div>


    <div v-else class="grid grid-cols-1 lg:grid-cols-3 gap-8">
      <div class="lg:col-span-2 space-y-4">
        <div
          v-for="item in carrito.items"
          :key="item.producto.id"
          class="flex gap-4 bg-slate-800 p-4 rounded-xl shadow"
        >
          <img :src="item.producto.imagen" class="w-24 h-24 object-cover rounded-lg" />

          <div class="flex-1">
            <h2 class="font-semibold text-lg"> {{ item.producto.nombre }}</h2>

            <p class="text-slate-400">
              ${{ item.producto.precio.toLocaleString() }}
            </p>

            <div class="flex items-center gap-3 mt-3">
              <button @click="carrito.bajarCantidadAComprar(item.producto)" class="px-3 py-1 bg-slate-700 rounded hover:bg-slate-600" >
                −
              </button>

              <span class="font-semibold">
                {{ item.cantidad }}
              </span>

              <button @click="carrito.aumentarCantidadAComprar(item.producto)" class="px-3 py-1 bg-slate-700 rounded hover:bg-slate-600">
                +
              </button>
            </div>

          </div>

          <div class="flex flex-col justify-between items-end">
            <p class="font-semibold text-green-400">${{ (item.producto.precio * item.cantidad).toLocaleString() }}</p>
            <svg class="lucide lucide-circle-x-icon lucide-circle-x bg-red-600 hover:bg-red-500 py-2 rounded-lg transition text-sm w-10 h-10"
            @click="carrito.eliminarProducto(item.producto)"
            xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" ><circle cx="12" cy="12" r="10"/><path d="m15 9-6 6"/><path d="m9 9 6 6"/></svg>
          </div>
        </div>
      </div>

      
      <aside class="bg-slate-800 p-6 rounded-xl shadow h-fit">
        <h2 class="text-lg font-semibold mb-4">Resumen</h2>

        <div class="flex justify-between text-slate-300 mb-2">
          <span>Productos</span>
          <span>{{ carrito.cantidadItems }}</span>
        </div>

        <div class="flex justify-between font-bold text-lg mb-6">
          <span>Total</span>
          <span class="text-green-400"> ${{ carrito.totalPrecio.toLocaleString() }} </span>
        </div>

        <button @click="handleContinuarAlPago" class="w-full bg-blue-600 hover:bg-blue-500 py-3 rounded-lg font-semibold transition" >Continuar al pago</button>

        <button @click="carrito.vaciarCarrito" class="w-full bg-red-600 hover:bg-red-500 mt-3 font-semibold py-3 rounded-lg transition">Vaciar carrito</button>
      </aside>

    </div>
  </section>
</template>

<script setup lang="ts">
import NavBar from "@/components/Nav-bar.vue";
import router from "@/router";
import { useAuthStore } from "@/store/authStore";
import { useCartStore } from "@/store/cartStore";

const carrito = useCartStore();
const auth = useAuthStore();

const handleContinuarAlPago = () =>{
  if(!auth.getUsuario){
    router.push("/login");
  }else{
    router.push("/checkout");
  }
}

</script>

