<template>
    <div class="flex gap-5 mb-12 mt-4 w-full justify-center items-center flex-wrap">
        <div class="inline-block relative">
            <input
                v-model="localFiltros.search"
                placeholder="Buscar producto..."
                class="bg-slate-700 px-3 py-2 text-white pe-10 rounded-lg border-none outline-none ring-2 focus:ring-blue-600 "
            />
            <div class="absolute end-3 top-0 bottom-0 m-auto flex justify-center items-center cursor-pointer" 
            
            @click="localFiltros.search=''">
                <svg class="w-6 h-6 lucide lucide-delete-icon lucide-delete text-red-600 " xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" ><path d="M10 5a2 2 0 0 0-1.344.519l-6.328 5.74a1 1 0 0 0 0 1.481l6.328 5.741A2 2 0 0 0 10 19h10a2 2 0 0 0 2-2V7a2 2 0 0 0-2-2z"/><path d="m12 9 6 6"/><path d="m18 9-6 6"/></svg>
            </div>
        </div>
        <div class="relative">
            <select
                v-model="localFiltros.categoriaId"
                
                class="bg-slate-700 px-4 py-2 pe-8 rounded text-white outline-none border-none ring-2 focus:ring-blue-600 cursor-pointer appearance-none shadow-xs"
            >
                <option :value="null">Todas las categorias</option>
                <option v-for="c in categorias" :key="c.id" :value="c.id" :class="``">
                {{ c.nombre }}
                </option>
            </select>
            <svg class="cursor-pointer absolute m-auto end-1 top-0 bottom-0 w-6 h-6 text-blue-600 lucide lucide-chevron-down-icon lucide-chevron-down" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" ><path d="m6 9 6 6 6-6"/></svg>
            
        </div>
    </div>

</template>

<script setup lang="ts">
import fetchApi from '@/api/fetchApi';
import { esResultError, type CategoriaReadDTO } from '@/dtos/DTOs';
import { onMounted, reactive, ref, watch } from 'vue';


const props = defineProps<{
    filtros : {
        search : string,
        categoriaId : number | null,
        estado : boolean
    }
}>();

const categorias = ref<CategoriaReadDTO[]>([]);
const errorFetchingCat = ref("");

const emit = defineEmits<{
    (e: "update:filtros", value : object ) : void
}>();

const localFiltros = reactive({...props.filtros});




onMounted(async()=>{
    const resp = await fetchApi<CategoriaReadDTO>('Categoria');
    
    if(esResultError(resp.results)) errorFetchingCat.value = "❌ error : " + resp.results.errorMessage;
    
    categorias.value = resp.results as CategoriaReadDTO[];

    
});

watch(localFiltros,()=>{
    emit("update:filtros", {...localFiltros})
}
);
</script>