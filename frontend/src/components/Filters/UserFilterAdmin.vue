<template>
    <div class="flex gap-5 mb-12 mt-4 w-full justify-center items-center">
        
        <div class="inline-block relative">
            <input
                v-model="filtros.search"
                placeholder="Buscar orden..."
                class="bg-slate-700 px-3 py-2 text-white pe-10 rounded-lg border-none outline-none ring-2 focus:ring-blue-600 "
            />
            <div class="absolute end-3 top-0 bottom-0 m-auto flex justify-center items-center cursor-pointer" 
            
            @click="filtros.search=''">
                <svg class="w-6 h-6 lucide lucide-delete-icon lucide-delete text-red-600 " xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" ><path d="M10 5a2 2 0 0 0-1.344.519l-6.328 5.74a1 1 0 0 0 0 1.481l6.328 5.741A2 2 0 0 0 10 19h10a2 2 0 0 0 2-2V7a2 2 0 0 0-2-2z"/><path d="m12 9 6 6"/><path d="m18 9-6 6"/></svg>
            </div>
        </div>
        
        <div class="relative inline-block">
            <select
                v-model="filtros.rolId"
                
                class="bg-slate-700 px-4 py-2 pe-8 rounded text-white outline-none border-none ring-2 focus:ring-blue-600 cursor-pointer appearance-none shadow-xs"
            >
                <option :value="null">Todos los roles</option>
                <option :value="EnumRoles.Administrador" >{{ EnumRoles[1] }}</option>
                <option :value="EnumRoles.Cliente" >{{ EnumRoles[2] }}</option>
            </select>
            <svg class="cursor-pointer absolute m-auto end-1 top-0 bottom-0 w-6 h-6 text-blue-600 lucide lucide-chevron-down-icon lucide-chevron-down" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" ><path d="m6 9 6 6 6-6"/></svg>
        </div>
        
        <div class="inline-block relative">
            <input
                v-model="filtros.fecha"
                type="date"
                min="0000-01-01"
                max="9999-12-30"
                class="bg-slate-700 px-3 py-2 text-white pe-10 rounded-lg border-none outline-none ring-2 focus:ring-blue-600 "
            />
            <div class="absolute end-3 top-0 bottom-0 m-auto flex justify-center items-center cursor-pointer" 
            
            @click="`${filtros.fecha= null}`" >
                <svg class="w-6 h-6 lucide lucide-delete-icon lucide-delete text-red-600 " xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" ><path d="M10 5a2 2 0 0 0-1.344.519l-6.328 5.74a1 1 0 0 0 0 1.481l6.328 5.741A2 2 0 0 0 10 19h10a2 2 0 0 0 2-2V7a2 2 0 0 0-2-2z"/><path d="m12 9 6 6"/><path d="m18 9-6 6"/></svg>
            </div>
        </div>
        

        <div class="inline-block relative">
            <select v-model="filtros.estado" 
            class="bg-slate-700 px-4 py-2 pe-8 rounded text-white outline-none border-none ring-2 focus:ring-blue-600 cursor-pointer appearance-none shadow-xs"
            >
                <option :value="null">Todos los estados</option>
                <option :value="true">Activos</option>
                <option :value="false">Inactivos</option>
            </select>
            <svg class="cursor-pointer absolute m-auto end-1 top-0 bottom-0 w-6 h-6 text-blue-600 lucide lucide-chevron-down-icon lucide-chevron-down" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" ><path d="m6 9 6 6 6-6"/></svg>
        </div>

    </div>

</template>

<script setup lang="ts">
import {  reactive, watch } from 'vue';

enum EnumRoles {
    "Administrador" = 1,
    "Cliente" = 2,
};

const props = defineProps<{
    filtros : {
        rolId : null | number,
        search : null | string,
        fecha : string | null,
        estado : null | boolean
    }
}>();


const filtros = reactive({... props.filtros});

const emit = defineEmits<{
    (e: "update:filtros", value : object) : void
}>();



watch(()=> filtros,()=>{
    emit("update:filtros",{...filtros})
}, {deep:true});


</script>