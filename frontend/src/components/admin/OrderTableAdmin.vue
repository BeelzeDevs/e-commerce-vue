<template>

        <div v-if="loading" class="loading-container">
            <div  class="loading"></div>
            <div class="loading-text">Cargando...</div>
        </div>
        
        <div class="w-full overflow-x-auto custom-scrollbar">
            <table class="min-w-full divide-y divide-white text-sm md:text-base">
                <thead class="bg-blue-800 text-slate-200">
                    <tr>
                        <th class="px-4 py-2 text-left font-semibold">Nombre</th>
                        <th class="px-4 py-2 font-semibold text-center">Email</th>
                        <th class="px-4 py-2 font-semibold text-center">Fecha</th>
                        <th class="px-4 py-2 font-semibold text-center">Total</th>
                        <th class="px-4 py-2 font-semibold text-center">Estado</th>
                        <th class="px-4 py-2"></th>
                    </tr>
                </thead>

                <tbody class="divide-y divide-slate-700">
                    <tr
                        v-for="o in ordenes"
                        :key="o.id"
                        class="hover:bg-slate-800 transition"
                    >
                        <td class="px-4 py-2 font-medium text-white">
                            {{ o.usuario.nombre }}
                        </td>

                        <td class="px-4 py-2 text-slate-300 text-center break-all">
                            {{ o.usuario.email }}
                        </td>

                        <td class="px-4 py-2 text-slate-300 text-center">
                            {{ new Date(o.fecha).toLocaleDateString() }}
                        </td>

                        <td class="px-4 py-2 font-semibold text-center">
                            ${{ o.total }}
                        </td>
                        
                        <td class="px-4 py-2 font-semibold text-center">
                            {{ o.estado }}
                        </td>


                        <td class="px-4 py-2 text-right whitespace-nowrap">
                            <button
                                @click="clickDetalle(o)"
                                class="px-4 py-2 bg-blue-700 hover:bg-blue-600 rounded-lg text-sm md:text-base shadow"
                            >
                                Ver detalles
                            </button>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>

        <DetailModal v-if="seeDetail" :orden="ordenToDetail" @HandlerDetail="HandlerDetail"/>
        


</template>


<script setup lang="ts">

import fetchApi from '@/api/fetchApi';
import type { OrdenReadDTO } from '@/dtos/DTOs';
import { useAuthStore } from '@/store/authStore';
import { storeToRefs } from 'pinia';
import { onMounted, ref, watch } from 'vue';
import  DetailModal from '@/components/admin/DetailModal.vue';


const loading = ref(false);
const errorFetchOrdenes = ref("");
const auth = useAuthStore();
const {getAuthHeader} = storeToRefs(auth);
const ordenes = ref<OrdenReadDTO[]>([]);

const fetchOrdenes = async () =>{
    const resp = await fetchApi<OrdenReadDTO>('Ordenes',{
        headers : getAuthHeader.value,
    });
    if(resp.errorMessage) errorFetchOrdenes.value = "❌ Error: " + resp.errorMessage ;
    else{
        ordenes.value = resp.results || [];
        loading.value = false;
    }
};

onMounted(async ()=>{
    await fetchOrdenes();
});

const ordenToDetail = ref<OrdenReadDTO>({
    id : 1,
    total : 10,
    fecha : new Date(Date.now()),
    estado : 'Carrito',
    usuario : {
        id : 1,
        nombre : "ninguno",
        email : "vacio",
        estado : true,
        fechaRegistro : new Date(Date.now()),
        rol : {
            id : 1,
            nombre : "cliente"
        }
    }

});
const seeDetail = ref(false);
const clickDetalle = (o : OrdenReadDTO) =>{
    ordenToDetail.value = o;
    HandlerDetail();
};
const HandlerDetail = () =>{
    seeDetail.value = !seeDetail.value;
};
watch(seeDetail,()=>{

});

</script>