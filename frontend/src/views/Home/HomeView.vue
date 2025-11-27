<template>
<div class="p-6">
    <h1 class="text-2xl font-bold mb-4">Productos Activos</h1>
    
    <div class="flex justify-center items-center">
        <div v-if="loading" class="loading-container mx-10 my-10">
          <div  class="loading"></div>
          <div class="loading-text">Cargando...</div>
        </div>
    </div>

    <div v-if="!loading" class="grid gap-4 grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 mx-10">
      <div
          v-for="p in productos"
          :key="p.id"
          class="border p-4 rounded-lg shadow hover:shadow-lg transition "
      >
          
          <div v-if="p.estado == true" class="grid grid-rows-auto text-center gap-5 justify-items-center">
            <div>
              <img :src="p.imagen" alt="img" class="w-full max-h-[150px] object-fill rounded" />
            </div>
            
            <div>
              <h2 class="text-lg font-semibold mt-2">{{ p.nombre }}</h2>
              <p class="text-gray-500">{{ p.categoria.nombre }}</p>
              <p class="font-bold text-green-600 mt-1">${{ p.precio }}</p>
              <p class="font-bold text-gray-500 mt-1">Stock : {{ p.stock }}</p> 
            </div>
            
            <button class="rounded-lg font-semibold text-lg text-slate-200 bg-blue-600 py-1 w-[150px]">Comprar</button>
          </div>
      </div>
    </div>

</div>
</template>

<script setup lang="ts">
import { ref, onMounted } from "vue";
import fetchApi from "../../api/fetchApi";
import type {ProductoReadDTO} from "@/dtos/DTOs";

const productos = ref<ProductoReadDTO[]>([]);
const loading = ref(true);
const errorFetching = ref("");

onMounted(async () => {
  const resp = await fetchApi<ProductoReadDTO>("Producto/actives");
  if(resp.errorMessage){ errorFetching.value= "❌ Error: " + resp.errorMessage; console.log(errorFetching.value);}
  else{
    productos.value = resp.results || [];
    loading.value = false;
  }
});

</script>