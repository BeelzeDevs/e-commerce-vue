<template>
  <div class="p-6 bg-bgContent w-full text-white border border-x-0 border-y-adminborders overflow-y-auto overscroll-contain custom-scrollbar">
      <h1 class="text-2xl mt-4 mb-10 font-medium font-sans">Productos</h1>
      
      <ProductCreate @toogleReload = "reload = !reload" />

      <div v-if="loading" class="loading-container">
        <div  class="loading"></div>
        <div class="loading-text">Cargando...</div>
      </div>

      <div v-if="!loading" class="grid gap-4 grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 mx-10 ">
        <div
            v-for="p in productos"
            :key="p.id"
            class="border p-4 rounded-lg shadow hover:shadow-lg transition "
        >
          <ProductCardAdmin :producto="p"/>
            
        </div>
      </div>

  </div>
</template>

<script setup lang="ts">
import type { ProductoReadDTO } from '@/dtos/DTOs';
import ProductCreate from '@/components/admin/ProductCreate.vue';
import {ref,onMounted, watch} from 'vue';
import ProductCardAdmin from '@/components/admin/ProductCardAdmin.vue';
import fetchApi from '@/api/fetchApi';
import { useAuthStore } from '@/store/authStore';

const productos = ref<ProductoReadDTO[]>([]);
const loading = ref(true);
const errorFetching = ref("");
const reload = ref(false);

const auth = useAuthStore();

const fetchProductos = async () =>{
  const resp = await fetchApi<ProductoReadDTO>('Producto', {
      headers : auth.getAuthHeader,
      
    });
    if(resp.errorMessage) errorFetching.value= "❌ Error: " + resp.errorMessage; 
    else{
        productos.value = resp.results || [];
        loading.value = false;
    }
};


onMounted(async () =>{
    await fetchProductos();
    
});

watch(reload, async () => {
  await fetchProductos();
});


/**
  props → hijo
  emit → padre
 */
</script>