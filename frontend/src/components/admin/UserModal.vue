<template>
    <section class="fixed inset-0 flex justify-center items-center bg-black/70 z-30">
        <article class="bg-slate-800 w-full max-w-2xl rounded-xl p-6 flex flex-col gap-6 shadow-xl relative  overflow-y-auto">
            <div class="flex justify-between items-center mr-20 mb-5">
                <p class="font-semibold text-lg md:text-xl px-5 ">{{ usuario.nombre }}</p>
                <p class="font-semibold text-lg md:text-xl px-5 ">Ordenes</p>
                <p class="font-semibold text-lg md:text-xl px-5 ">ID : {{ usuario.id }}</p>
            </div>
            <div class="w-full max-h-[65vh] overflow-y-auto custom-scrollbar">
                <table class="min-w-full divide-y divide-white text-sm md:text-base">
                    <thead class="bg-blue-800 ">
                            <tr>
                                <th class="px-4 py-2 text-left font-semibold">Fecha</th>
                                <th class="px-4 py-2 text-center font-semibold">Estado</th>
                                <th class="px-4 py-2 text-center font-semibold">Total</th>
                                <th class="px-4 py-2 text-center font-semibold"></th>
                            </tr>
                    </thead>
                    <tbody class="divide-y divide-slate-700 ">
                            <tr  v-for="(or ) in ordenes" v-bind:key="or.id" class="hover:bg-slate-700 transition">
                                <td class="px-4 py-2 text-left font-semibold">{{ new Date(or.fecha).toLocaleDateString()  }}</td>
                                <td class="px-4 py-2 text-center font-semibold">{{ or.estado }}</td>
                                <td class="px-4 py-2 text-center font-semibold" >${{ or.total.toLocaleString()  }}</td>
                                <td class="px-4 py-2 text-right whitespace-nowrap"><button @click="mostrarModalOrden(or)" class="px-4 py-2 bg-blue-700 hover:bg-blue-600 rounded-lg text-sm md:text-base shadow">Ver Detalles</button></td>
                            </tr>
                            
                    </tbody>
                </table>
            </div>
            
            <button @click="$emit('toogleModal')" class="absolute top-0 right-0 py-3 px-3 bg-red-700 hover:bg-red-700/70">
                ❌
            </button>

            
            <DetailModal v-if="seeDetail" :orden="ordenToDetail" @HandlerDetail="HandlerDetail"/>
            
        </article>
    </section>


</template>

<script setup lang="ts">
import fetchApi from '@/api/fetchApi';
import DetailModal from '@/components/admin/DetailModal.vue';
import { type UsuarioReadDTO, type OrdenReadDTO, esResultError } from '@/dtos/DTOs';
import { useAuthStore } from '@/store/authStore';

import { onMounted, ref, toRef } from 'vue';


const props = defineProps<{
    usuario :  UsuarioReadDTO
}>();
const usuario = toRef(props,"usuario");

const auth = useAuthStore();
const ordenes = ref<OrdenReadDTO[]>([]);
const errorLoading = ref("");

const fetchOrdenes = async ()=>{
    const resp = await fetchApi<OrdenReadDTO>(`Ordenes/usuario/${usuario.value.id}/all`);
    if(esResultError(resp.results)) errorLoading.value = resp.results.errorMessage; 
    else{
        ordenes.value = resp.results as OrdenReadDTO[];
    }
    
};

onMounted(async()=>{
    await fetchOrdenes();
});
// Modal Detalle de Orden
const ordenToDetail = ref<OrdenReadDTO>({
    id : 1,
    usuario : {
        id : 1,
        rol : {
            id : 2,
            nombre : "Cliente",
        },
        nombre : "",
        email : "",
        fechaRegistro : new Date("00/00/0000"), 
        estado : true,
    },
    fecha : new Date("00/00/0000"),
    total : 0,
    estado : "Pendiente"
});
const seeDetail = ref(false);
const HandlerDetail = () =>{
    seeDetail.value = !seeDetail.value;
};

const mostrarModalOrden = (or : OrdenReadDTO) =>{
    ordenToDetail.value.id = or.id;
    ordenToDetail.value.fecha = or.fecha;
    ordenToDetail.value.total = or.total;
    ordenToDetail.value.usuario = or.usuario;
    console.log(ordenToDetail.value.usuario.rol.nombre);
    seeDetail.value = true;
};

</script>