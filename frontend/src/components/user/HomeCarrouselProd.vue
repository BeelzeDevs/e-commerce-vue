<template>
  <div class="block w-full px-40 pt-32 pb-10 " v-if="loading">
    <div class="flex justify-center items-center w-full relative">
      <div class="loading-container ">
        <div  class="loading"></div>
        <div class="loading-text text-white">Cargando...</div>
      </div>
    </div>
  </div>

    <section class="px-40 relative pt-32 pb-10" v-if="!loading">

        <button
            @click="prev"
            class="absolute left-10 top-1/2 -translate-y-1/2 bg-slate-800/80 p-2 rounded-full disabled:opacity-40 w-16 h-16 text-white outline outline-indigo-600  hover:outline-yellow-400 hover:outline flex justify-center items-center"
            >
            <svg class="lucide lucide-step-back-icon lucide-step-back w-10 h-10" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" ><path d="M13.971 4.285A2 2 0 0 1 17 6v12a2 2 0 0 1-3.029 1.715l-9.997-5.998a2 2 0 0 1-.003-3.432z"/><path d="M21 20V4"/></svg>
        </button>

        <div class="w-full overflow-hidden">

            <div
            class="flex flex-nowrap transition-transform duration-500 ease-out"
            :style="{ transform: `translateX(-${offset}px)` }"
            >
            <div
                v-for="producto in productos"
                :key="producto.id"
                ref="cards"
            >
                <ProductCarrouselCard :producto="producto" />
            </div>
            </div>
            
        </div>
        
        <button
        @click="next"
        class="absolute right-10 top-1/2 -translate-y-1/2 bg-slate-800/80  p-2 rounded-full disabled:opacity-40 w-16 h-16 text-white outline outline-indigo-600  hover:outline-yellow-400 flex justify-center items-center"
        >
        <svg class="lucide lucide-step-forward-icon lucide-step-forward  w-10 h-10" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" ><path d="M10.029 4.285A2 2 0 0 0 7 6v12a2 2 0 0 0 3.029 1.715l9.997-5.998a2 2 0 0 0 .003-3.432z"/><path d="M3 4v16"/></svg>
        
        </button>
    </section>
</template>

<script setup lang="ts">
import { computed, onMounted, ref } from 'vue'
import { esResultError, type ResultadoPaginado, type ProductoReadDTO } from '@/dtos/DTOs'
import fetchApi from '@/api/fetchApi';
import ProductCarrouselCard from '@/components/user/ProductCarrouselCard.vue';

const productos = ref<ProductoReadDTO[]>([]);
const loading = ref(true);

const index = ref(0)
// const cardWidth = 260 + 9 // el ancho de la tarjeta + el padding pero dificil calcular el gap
const cards = ref<HTMLElement[]>([]) // Este ref calcula a precisión el tamaño de tarjeta

const cardWidth = computed(() => {
  return cards.value?.[index.value]?.offsetWidth ?? (260+9);
})

const visibleCards = computed(() => {
  if (widthTotalActual.value < 640) return 1
  else if (widthTotalActual.value < 1024) return 2
  else return 4;
})

const maxIndex = computed(() =>
  Math.max(0, productos.value.length - visibleCards.value)
)

const offset = computed(() => index.value * cardWidth.value)

const next = () => {
  if (index.value < maxIndex.value) index.value++;
  else index.value = 0;
}

const prev = () => {
  if (index.value > 0) index.value--;
  else index.value = maxIndex.value;
}
const widthTotalActual = ref(window.innerWidth);

onMounted( async ()=>{
    await fetchProductos();
    window.addEventListener("resize", ()=>{
        widthTotalActual.value = window.innerWidth;
    });
});

const errorFetch = ref("");
const fetchProductos = async ()=>{
    loading.value = true;

    const params = new URLSearchParams({
        page : "1",
        pageSize : "8",
    });

    const resp = await fetchApi<ResultadoPaginado<ProductoReadDTO>>(`Producto/actives?${params.toString()}`);
    if(esResultError(resp.results)){
        errorFetch.value = "❌ " + resp.results.errorMessage;
        loading.value = false;
    }
    if("items" in resp.results && "totalPages" in resp.results){
        productos.value = resp.results.items;
        loading.value = false;
    }
};

</script>