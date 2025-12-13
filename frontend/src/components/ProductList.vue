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
          <ProductCard :producto="p"/>
            
        </div>
      </div>

  </div>
</template>

<script setup lang="ts">
import type { ProductoReadDTO } from '@/dtos/DTOs';
import {ref,onMounted} from 'vue';
import ProductCard from '@/components/ProductCard.vue';
import fetchApi from '@/api/fetchApi';


const productos = ref<ProductoReadDTO[]>([]);
const loading = ref(true);
const errorFetching = ref("");

onMounted(async () =>{
    const resp = await fetchApi<ProductoReadDTO>('Producto/actives');
    if(resp.errorMessage){ errorFetching.value= "❌ Error: " + resp.errorMessage; console.log(errorFetching.value);}
    else{
        productos.value = resp.results || [];
        loading.value = false;
    }
});


</script>