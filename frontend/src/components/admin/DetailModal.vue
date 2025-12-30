<template>
    <section  class="fixed inset-0 flex justify-center items-center bg-black/70 z-50> ">
        <article class="bg-slate-800 w-full max-w-2xl rounded-xl p-6 flex flex-col gap-6 shadow-xl relative  overflow-y-auto">
            <h2 class="font-semibold text-lg md:text-xl text-center">Detalles de OrdenID : {{ orden.id }}</h2>
            
            <div class="w-full max-h-[65vh] overflow-y-auto custom-scrollbar">
                <table class="min-w-full divide-y divide-white text-sm md:text-base">
                    <thead class="bg-blue-800 ">
                            <tr>
                                <th class="px-4 py-2 text-left font-semibold">Nombre</th>
                                <th class="px-4 py-2 text-center font-semibold">Precio</th>
                                <th class="px-4 py-2 text-center font-semibold">Cantidad</th>
                                <th class="px-4 py-2 text-center font-semibold">Subtotal</th>
                            </tr>
                    </thead>
                    <tbody class="divide-y divide-slate-700 " >
                            <tr v-for=" det in detalles" v-bind:key="`${det.ordenId} + ${det.producto.id}`"  class="hover:bg-slate-700 transition">
                                <td class="px-4 py-2 text-left font-semibold">{{ det.producto.nombre  }}</td>
                                <td class="px-4 py-2 text-center font-semibold">${{ det.precio_Producto.toLocaleString() }}</td>
                                <td class="px-4 py-2 text-center font-semibold" >{{ det.cantidad  }}</td>
                                <td class="px-4 py-2 text-center font-semibold">${{ det.subtotal.toLocaleString()  }}</td>
                            </tr>
                            <tr class="text-lg md:text-xl font-semibold">
                                <td class="px-4 py-2 text-left font-semibold">Total</td>
                                <td></td>
                                <td></td>
                                <td class="text-green-600 px-4 py-2 text-center font-semibold">
                                    ${{ orden.total.toLocaleString() }}
                                </td>
                            </tr>
                    </tbody>
                </table>
                <button @click="$emit('HandlerDetail')" class="absolute top-0 right-0 py-3 px-3 bg-red-700 hover:bg-red-700/70">
                    ❌
                </button>
            </div>

        </article>
    </section>
</template>

<script setup lang="ts">

import fetchApi from '@/api/fetchApi';
import { esResultError, type DetalleReadDTO, type OrdenReadDTO } from '@/dtos/DTOs';
import { useAuthStore } from '@/store/authStore';
import { storeToRefs } from 'pinia';
import { onMounted, ref, toRef } from 'vue';


const props = defineProps<{orden : OrdenReadDTO}>();
const orden = toRef(props,"orden");

const auth = useAuthStore();
const {getAuthHeader} = storeToRefs(auth);
const error = ref("");

const detalles = ref<DetalleReadDTO[]>([{
    ordenId : 1,
    cantidad : 0,
    subtotal : 0,
    precio_Producto : 0,
    producto :{
        id: 1,
        categoria : {
            id:1,
            nombre:"",
        },
        nombre : "",
        marca : "",
        descripcion : "",
        precio : 0,
        stock : 0,
        imagen : "@/public/img/sinimagen.webp",
        estado : true,
    }
}]);


const fetchDetalles = async () => { 
    const resp = await fetchApi<DetalleReadDTO>(`Ordenes/${orden.value.id}/detalles`);
    if(esResultError(resp.results)) error.value = resp.results.errorMessage;
    else{
        detalles.value = resp.results as DetalleReadDTO[];
    }
};


onMounted(async()=>{
    await fetchDetalles();
});
</script>