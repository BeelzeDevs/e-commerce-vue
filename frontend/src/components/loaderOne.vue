<template>
    <div v-if="activo" class="block w-full my-14">
        <div class="flex justify-center items-center w-full relative h-full">
            <div class="w-[200px] h-[200px] border-8 border-bgloader rounded-full relative">
                <div  class="loading"></div>
                <div class="loading-text text-white">Cargando...</div>
            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
import { toRef } from 'vue';


const props = defineProps<{
    loading : boolean
}>();

const activo = toRef(props,"loading");

    
</script>