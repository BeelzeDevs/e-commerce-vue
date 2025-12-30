<template>
  <NavBar />

  <section class="min-h-[90dvh] bg-bgContent text-white py-10">
    <div class="max-w-5xl mx-auto px-4 grid grid-cols-1 lg:grid-cols-3 gap-8">

      <div class="lg:col-span-2 bg-slate-800 rounded-xl p-6">
        <h2 class="text-xl font-semibold mb-4">Resumen de compra</h2>

        <div v-for="item in carrito.items" :key="item.producto.id"
          class="flex justify-between items-center border-b border-slate-700 py-4">

          <div class="flex gap-4 items-center">
            <img :src="item.producto.imagen" class="w-16 h-16 object-cover rounded" />
            <div>
              <p class="font-semibold">{{ item.producto.nombre }}</p>
              <p class="text-sm text-slate-400">
                ${{ item.producto.precio.toLocaleString() }} x {{ item.cantidad }} u
              </p>
            </div>
          </div>

          <p class="font-semibold">
            ${{ (item.producto.precio * item.cantidad).toLocaleString() }}
          </p>
        </div>
      </div>

      
      <div class="bg-slate-800 rounded-xl p-6 flex flex-col gap-4">

        <div class="flex justify-between">
          <span>Productos</span>
          <span>{{ carrito.cantidadItems }}</span>
        </div>

        <div class="flex justify-between font-bold text-lg">
          <span>Total</span>
          <span class="text-green-500">${{ carrito.totalPrecio.toLocaleString() }}</span>
        </div>

        <button @click="confirmarCompra"
          :disabled="loading"
          class="mt-4 bg-blue-600 hover:bg-blue-700 disabled:bg-slate-600 py-3 rounded-xl font-semibold transition"
        >
          {{ loading ? "Procesando..." : "Confirmar compra" }}
        </button>
        <div v-if="error">
            <p class="mt-4 text-red-600 py-3 font-semibold transition">{{ error }}</p>
        </div>
      </div>

    </div>
  </section>
</template>

<script setup lang="ts">
import NavBar from "@/components/Nav-bar.vue";
import { useCartStore } from "@/store/cartStore";
import { useAuthStore } from "@/store/authStore";
import fetchApi from "@/api/fetchApi";
import router from "@/router";
import { ref } from "vue";
import { type OrdenReadDTO,  type OrdenCreateDTO, esResultError } from "@/dtos/DTOs";

const carrito = useCartStore();
const auth = useAuthStore();
const loading = ref(false);
const error = ref("");

const confirmarCompra = async () => {
  if (carrito.items.length === 0) return;
  if(!auth.getUsuario) router.push("/login");

  loading.value = true;

  
  const orden : OrdenCreateDTO = {
    carritoItems: carrito.items.map(i=> ({ productoId: i.producto.id, cantidad : i.cantidad })),
  };

  const resp = await fetchApi<OrdenReadDTO>("Ordenes", {
    method: "POST",
    body: JSON.stringify(orden),
  });

  loading.value = false;
  
  if (esResultError(resp.results)) {
    error.value = resp.results.errorMessage;
    return;
  }
  else{
    carrito.vaciarCarrito();
    // router.push(`/orden/${resp.results.}`);

  }

  

};
</script>

