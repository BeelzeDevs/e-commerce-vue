<template>
  <div class="p-6 bg-bgContent w-full text-white border border-x-0 border-y-adminborders overflow-y-auto overscroll-contain custom-scrollbar">
      <h1 class="text-2xl mt-4 mb-10 font-medium font-sans">Productos</h1>
      
      <ProductCreate @toogleReload = "reload = !reload" />

      <div v-if="loading" class="loading-container">
        <div  class="loading"></div>
        <div class="loading-text">Cargando...</div>
      </div>

      <ProductFilterAdmin  v-model:filtros="filtros" />

      <div v-if="!loading" class="grid gap-4 grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 mx-10 ">
        <div
            v-for="p in productos"
            :key="p.id"
            class="border p-4 rounded-xl shadow hover:shadow-lg transition bg-slate-900 hover:bg-blue-900/30 "
        >
          <ProductCardAdmin :producto="p"/>
            
        </div>
      </div>

      <div >
        <PagerComp v-model:page="page" :totalPages="totalPages" />
      </div>

  </div>
</template>

<script setup lang="ts">
import { esResultError, type ProductoReadDTO, type ResultadoPaginado } from '@/dtos/DTOs';
import ProductCreate from '@/components/admin/ProductCreate.vue';
import {ref,onMounted, watch} from 'vue';
import ProductCardAdmin from '@/components/admin/ProductCardAdmin.vue';
import fetchApi from '@/api/fetchApi';
import { isNullOrUndef } from 'chart.js/helpers';
import PagerComp from '@/components/Pagers/PagerComp.vue';
import ProductFilterAdmin from '@/components/Filters/ProductFilterAdmin.vue';

const productos = ref<ProductoReadDTO[]>([]);
const loading = ref(true);
const errorFetching = ref("");
const reload = ref(false);




const page = ref(1);
const pageSize = ref(8);
const totalPages = ref(0);

const filtros =  ref({
    search : "" as string,
    categoriaId : null as number | null,
    estado : null as boolean | null,
});

const fetchProductos = async () =>{
  loading.value = true;
  errorFetching.value = "";

  const params = new URLSearchParams({
    page : page.value.toString(),
    pageSize : pageSize.value.toString()
  })

  if(filtros.value.search) params.append("search",filtros.value.search.toString());
  if(filtros.value.categoriaId) params.append("categoriaId",filtros.value.categoriaId.toString());
  if(!isNullOrUndef(filtros.value.estado)) params.append("estado",filtros.value.estado.toString());


  const resp = await fetchApi<ResultadoPaginado<ProductoReadDTO>>(`Producto?${params.toString()}`);

    if(esResultError(resp.results)){ 
      errorFetching.value= resp.results.errorMessage; 
      loading.value = false;
      return;
    }
    else{
      if("items" in resp.results && "totalPages" in resp.results){
        productos.value = resp.results.items;
        totalPages.value = resp.results.totalPages;
        loading.value = false;
      }
    }
};


onMounted(async () =>{
    await fetchProductos();
    
});

watch(reload, async () => {
  await fetchProductos();
});
watch([page,pageSize], fetchProductos);

watch( 
  () => filtros, // observá lo que devuelve la función. Un objeto. El disparador es cuando cambie cualquier propiedad interna del objeto
  () => {
    page.value = 1;  
    fetchProductos();
  },
  {deep : true} // observa cambios internos
);


</script>