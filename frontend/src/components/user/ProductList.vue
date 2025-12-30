<template>
  <div class="p-6 bg-bgContent min-h-[90dvh]">
      
      <div class="flex justify-center items-center">
          <div v-if="loading" class="loading-container mx-10 my-10">
            <div  class="loading"></div>
            <div class="loading-text text-white">Cargando...</div>
          </div>
      </div>
      
      <ProductFilters v-model:filtros = "filtros" />

      <div v-if="!loading" class="grid gap-4 grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 mx-10">
        <div
            v-for="p in productos"
            :key="p.id"
            class="border p-4 rounded-xl shadow hover:shadow-lg transition bg-slate-900 hover:bg-blue-900/30"
        >
          <ProductCard :producto="p"/>
            
        </div>
      </div>
      <div v-if="!loading" class="flex">
        <PagerComp v-model:page="Page" :totalPages="totalPages" />
      </div>

  </div>
</template>

<script setup lang="ts">
import { esResultError, type ProductoReadDTO, type ResultadoPaginado } from '@/dtos/DTOs';
import {ref,onMounted, watch} from 'vue';
import ProductCard from '@/components/user/ProductCard.vue';
import fetchApi from '@/api/fetchApi';
import PagerComp from '@/components/Pagers/PagerComp.vue';
import ProductFilters from '@/components/Filters/ProductFilters.vue';


const productos = ref<ProductoReadDTO[]>([]);
const loading = ref(true);
const errorFetching = ref("");

const Page = ref(1);
const PageSize = ref(8);
const totalPages = ref(0);

const filtros = ref({
  search : "" as string,
  categoriaId : null as number | null,
  estado : true,
});

onMounted(async () =>{
  await fetchProductos();


});
const fetchProductos = async () =>{
  loading.value = true;
  errorFetching.value = "";

  const parametros = new URLSearchParams({
    page : Page.value.toString(),
    pageSize : PageSize.value.toString(),
  });

  if(filtros.value.search) parametros.append("search", filtros.value.search);
  if(filtros.value.categoriaId) parametros.append("categoriaId", filtros.value.categoriaId.toString());
  

  const resp = await fetchApi<ResultadoPaginado<ProductoReadDTO>>(`Producto/actives?${parametros.toString()}`);
    if(esResultError(resp.results)){
      errorFetching.value = "❌ Error: " + resp.results.errorMessage;
      loading.value = false;
      return;
    }
    if("items" in resp.results && "totalPages" in resp.results){
      productos.value = resp.results.items;
      totalPages.value = resp.results.totalPages;
      loading.value = false;
    }
    
};

watch([Page,PageSize], fetchProductos);

watch(
  () => filtros.value,
  () => {
    Page.value = 1;
    fetchProductos();
  },
  {deep : true}
);

</script>