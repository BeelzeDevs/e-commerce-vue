<template>
  <NavBar />
  <div class="w-full bg-bgContent">
    <section class="max-w-6xl mx-auto p-6 text-white min-h-[90dvh]">
      <div v-if="loading" class="loading-container">
        <div class="loading"></div>
        <div class="loading-text">Cargando producto...</div>
      </div>
  
      <div v-else-if="producto" class="grid grid-cols-1 md:grid-cols-2 gap-10">
        
        <div class="bg-slate-800 rounded-xl p-6 flex justify-center">
          <img
            :src="producto.imagen"
            alt="producto"
            class="max-h-[350px] object-contain"
          />
        </div>
  
        <div class="flex flex-col gap-4 justify-between">
          <h1 class="text-2xl font-bold">
            {{ producto.nombre }} - {{ producto.marca }}
          </h1>
  
          <p class="text-slate-400">
            {{ producto.categoria.nombre }}
          </p>
  
          <p class="text-3xl font-bold text-green-500">
            ${{ producto.precio.toLocaleString() }}
          </p>
  
          <p class="text-sm"  :class="`${carrito.getStockDisponible(producto) > 0 ? 'text-emerald-400' : 'text-red-500'} font-semibold`" >
            {{ carrito.getStockDisponible(producto) > 0 ? `Stock disponible: ${carrito.getStockDisponible(producto)}` : "Sin stock" }}
          </p>

          <div class="flex items-center gap-3">
            <span class="text-sm">Cantidad:</span>
            <button
              @click="decrementar"
              class="px-3 py-1 bg-slate-700 rounded"
              :disabled="producto.stock - carrito.getCantidadEnCarrito(producto) === 0"
            >−</button>
  
            <span>{{ cantidad }}</span>
  
            <button
              @click="incrementar"
              class="px-3 py-1 bg-slate-700 rounded"
              :disabled="producto.stock - carrito.getCantidadEnCarrito(producto) === 0"
            >+</button>
          </div>
  
          <!-- Compra -->
          <button
            @click="agregarAlCarrito"
            :disabled="producto.stock - carrito.getCantidadEnCarrito(producto) === 0"
            class="mt-4 bg-blue-600 hover:bg-blue-500 disabled:bg-slate-600 py-3 rounded-xl font-semibold transition"
          >
            Agregar al carrito
          </button>
        </div>
      </div>
  
      <div
        v-if="producto"
        class="mt-10 bg-slate-800 rounded-xl p-6"
      >
        <h2 class="font-semibold text-lg mb-2">Descripción</h2>
        <p class="text-slate-300">
          {{ producto.descripcion }}
        </p>
      </div>
    </section>
  </div>

</template>


<script setup lang="ts">

import { ref, onMounted } from "vue";
import { useRoute } from "vue-router";
import NavBar from "@/components/Nav-bar.vue";
import fetchApi from "@/api/fetchApi";
import { esResultError, type ProductoReadDTO } from "@/dtos/DTOs";
import { useCartStore } from "@/store/cartStore";

const route = useRoute();
const carrito = useCartStore();

const producto = ref<ProductoReadDTO>();
const loading = ref(true);
const cantidad = ref(1);
const errorMessages = ref("");

const incrementar = () => {
  if(!producto.value) return;
  const cantidadEnStock = carrito.getStockDisponible(producto.value);
  if (producto.value && cantidad.value < cantidadEnStock) {
    cantidad.value++;
  }
};

const decrementar = () => {
  if (cantidad.value > 1) cantidad.value--;
};

const agregarAlCarrito = () => {
  if (!producto.value) return;

  for (let i = 0; i < cantidad.value; i++) {
    carrito.agregarProducto(producto.value);
  }
  cantidad.value = 1;
};

onMounted(async () => {
  const id = route.params.id;

  const resp = await fetchApi<ProductoReadDTO>(`Producto/${id}`);
  if(esResultError(resp.results)){
    errorMessages.value = resp.results.errorMessage;
    return;
  }
  producto.value = resp.results as ProductoReadDTO;
  loading.value = false;
  
});


</script>

