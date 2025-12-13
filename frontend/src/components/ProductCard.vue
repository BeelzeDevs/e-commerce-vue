<template>
    <div v-if="producto.estado == true" class="grid grid-rows-auto text-center gap-5 justify-items-center">
    
        <div>
            <img :src="producto.imagen" alt="img" class="w-full max-h-[150px] object-fill rounded" />
        </div>
        
        <div>
            <h2 class="text-lg font-semibold mt-2">{{ producto.nombre }}</h2>
            <p class="text-gray-500">{{ producto.categoria.nombre }}</p>
            <p class="font-bold text-green-600 mt-1">${{ producto.precio }}</p>
            <p class="font-bold text-gray-500 mt-1">Stock : {{ producto.stock }}</p> 
        </div>
        
        <button class="rounded-lg font-semibold text-lg text-slate-200 bg-blue-600 py-1 w-[150px]">Comprar</button>
    
    </div>
</template>

<script setup lang="ts">
import type { ProductoReadDTO } from '@/dtos/DTOs';

const {producto} = defineProps<{
    producto : ProductoReadDTO
}>();

</script>