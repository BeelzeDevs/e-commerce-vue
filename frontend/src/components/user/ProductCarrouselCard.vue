<template>
    <div v-if="producto.estado == true " class="grid grid-row-[1fr] grid-cols-[260px] gap-5 px-3 py-1 justify-center text-center w-full h-full ">
        <article class="bg-blue-600/80 rounded-xl hover:bg-blue-600 cursor-default flex flex-col justify-between px-2">
                <div class="flex justify-center w-full ">
                    <img :src="producto.imagen" alt="img" class="max-h-[150px] object-fill rounded p-2" />
                </div>
                
                <div class="font-mono flex flex-col">
                    <h2 class="text-lg font-semibold mt-2 text-white">{{ producto.nombre }} - {{ producto.marca }}</h2>
                    <p class="text-slate-300">{{ producto.categoria.nombre }}</p>
                    <p class="font-bold text-green-400 mt-1">${{ producto.precio.toLocaleString() }}</p>
                    <p class="font-bold text-slate-300 mt-1" >Stock : {{  carrito.getStockDisponible(producto) }}</p>
                    
                </div>
                <div class="px-5 pt-5 pb-3">
                    <button @click="`${router.push(`producto/${producto.id}`)}`" class="rounded-lg font-semibold text-sm md:text-base text-slate-200 bg-green-600  hover:bg-green-700 p-3">
                        Ver detalles
                    </button>
                </div>

        </article>
         

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

</script>