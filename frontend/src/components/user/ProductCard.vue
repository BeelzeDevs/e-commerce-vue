<template>
    <div v-if="producto.estado == true " class="grid grid-rows-[1fr,auto,auto] text-center gap-5 px-1 py-1">
    
        <div class="flex justify-center w-full">
            <img :src="producto.imagen" alt="img" class="max-h-[150px] object-fill rounded" />
        </div>
        
        <div class="font-mono">
            <h2 class="text-lg font-semibold mt-2 text-white">{{ producto.nombre }} - {{ producto.marca }}</h2>
            <p class="text-gray-500">{{ producto.categoria.nombre }}</p>
            <p class="font-bold text-green-600 mt-1">${{ producto.precio.toLocaleString() }}</p>
            <p class="font-bold text-gray-500 mt-1" >Stock : {{  carrito.getStockDisponible(producto) }}</p>
             
        </div>
        <div class="flex flex-wrap justify-between w-full items-center gap-4">
            <button @click="`${router.push(`producto/${producto.id}`)}`" class="rounded-lg font-semibold text-sm md:text-base text-slate-200 bg-blue-600  hover:bg-blue-700 p-3">
                Ver detalles
            </button>
            <button @click="agregarAlCarrito" class="rounded-lg font-semibold text-sm md:text-base text-slate-200 bg-blue-600 hover:bg-blue-700 p-3" >
                <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="lucide lucide-package-plus-icon lucide-package-plus"><path d="M16 16h6"/><path d="M19 13v6"/><path d="M21 10V8a2 2 0 0 0-1-1.73l-7-4a2 2 0 0 0-2 0l-7 4A2 2 0 0 0 3 8v8a2 2 0 0 0 1 1.73l7 4a2 2 0 0 0 2 0l2-1.14"/><path d="m7.5 4.27 9 5.15"/><polyline points="3.29 7 12 12 20.71 7"/><line x1="12" x2="12" y1="22" y2="12"/></svg>
            </button>
        </div>

    </div>
</template>

<script setup lang="ts">
import type { ProductoReadDTO } from '@/dtos/DTOs';
import router from '@/router';
import { useCartStore } from '@/store/cartStore';

const {producto} = defineProps<{
    producto : ProductoReadDTO
}>();


const carrito = useCartStore();

const agregarAlCarrito = () =>{
    carrito.agregarProducto(producto);
}

</script>