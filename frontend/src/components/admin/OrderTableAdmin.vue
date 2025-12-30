<template>

        <div v-if="loading" class="loading-container">
            <div  class="loading"></div>
            <div class="loading-text">Cargando...</div>
        </div>
        
        <OrderFilterAdmin v-model:filtros="filter" />

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
                            ${{ o.total.toLocaleString() }}
                        </td>
                        
                        <td class="px-4 py-2 font-semibold">
                            <div class="relative">
                                <select v-on:change="handleUpdateOrden(o)" v-model="o.estado"  :class="`w-full bg-bgContent text-sm rounded pl-3 pr-8 py-2 transition duration-300 ease focus:outline-none shadow-sm focus:shadow-md appearance-none cursor-pointer ${o.estado == 'Pendiente' ? 'text-yellow-400' : o.estado == 'Cancelado' ? 'text-red-600' : o.estado == 'Pagado' ? 'text-green-500' : o.estado == 'Enviado' ? 'text-blue-500' : 'text-white'}`" >
                                    <option :value="'Pendiente'" class="text-white  " >Pendiente</option>
                                    <option :value="'Pagado'" class="text-white">Pagado</option>
                                    <option :value="'Enviado'" class="text-white ">Enviado</option>
                                    <option :value="'Cancelado'" class="text-white ">Cancelado</option>
                                </select>
                                <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.2" stroke="currentColor" :class="`h-5 w-5 ml-1 absolute top-2.5 right-2.5  ${o.estado == 'Pendiente' ? 'text-yellow-400' : o.estado == 'Cancelado' ? 'text-red-600' : o.estado == 'Pagado' ? 'text-green-500' : o.estado == 'Enviado' ? 'text-blue-500' : 'text-white'}`">
                                <path stroke-linecap="round" stroke-linejoin="round" d="M8.25 15 12 18.75 15.75 15m-7.5-6L12 5.25 15.75 9" />
                                </svg>
                            </div>
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
        <PagerComp v-model:page="page" :totalPages="totalPages" />

        <DetailModal v-if="seeDetail" :orden="ordenToDetail" @HandlerDetail="HandlerDetail"/>
        


</template>


<script setup lang="ts">

import fetchApi from '@/api/fetchApi';
import { esResultError, esResultSuccess,  type OrdenReadDTO, type ResultadoPaginado } from '@/dtos/DTOs';
import { useAuthStore } from '@/store/authStore';
import { storeToRefs } from 'pinia';
import { onMounted, ref, watch } from 'vue';
import  DetailModal from '@/components/admin/DetailModal.vue';
import { isNullOrUndef } from 'chart.js/helpers';
import PagerComp from '@/components/Pagers/PagerComp.vue';
import OrderFilterAdmin from '../Filters/OrderFilterAdmin.vue';


const loading = ref(false);
const errorFetchOrdenes = ref("");
const auth = useAuthStore();
const {getAuthHeader} = storeToRefs(auth);
const ordenes = ref<OrdenReadDTO[]>([]);

// filters y pager
const page = ref(1);
const pageSize = ref(10);
const totalPages = ref(0);
const filter = ref({
    fecha : null as string | null,
    searchUsuario : "" as string | null,
    estado : null as boolean | null,
    rolId : null as number | null,
});

const fetchOrdenes = async () =>{
    loading.value = true;

    const params = new URLSearchParams({
        page : page.value.toString(),
        pageSize : pageSize.value.toString(),
    });

    if(!isNullOrUndef(filter.value.rolId)) params.append("rolId", filter.value.rolId.toString());
    if(filter.value.searchUsuario) params.append("searchUsuario",filter.value.searchUsuario.toString());
    if(!isNullOrUndef(filter.value.estado)) params.append("estado", filter.value.estado.toString());
    if(!isNullOrUndef(filter.value.fecha)) params.append("fecha",filter.value.fecha);
    console.log(params.toString());
    const resp = await fetchApi<ResultadoPaginado<OrdenReadDTO>>(`Ordenes?${params.toString()}`);

    if(esResultError(resp.results)){
        errorFetchOrdenes.value= resp.results.errorMessage;
        loading.value = false;
    } 
    else{
        ordenes.value = resp.results.items;
        totalPages.value = resp.results.totalPages;
        loading.value = false;
    }
};

onMounted(async ()=>{
    await fetchOrdenes();
});

watch([page,pageSize],fetchOrdenes);
watch(
    () => filter,
    () =>{
        page.value = 1;
        fetchOrdenes();
    },
    {deep : true}
);

const ordenToDetail = ref<OrdenReadDTO>({
    id : 1,
    total : 10,
    fecha : new Date(Date.now()),
    estado : 'Pendiente',
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

// Update del estado de la orden 
const successMessage = ref<string>("");

const handleUpdateOrden = async (o : OrdenReadDTO)=>{
    const resp  = await fetchApi<string>(`Ordenes/${o.id}`,{
        method : "PUT",
        body : JSON.stringify(o)
    });
    if(esResultError(resp.results)) errorFetchOrdenes.value= resp.results.errorMessage;
    if(esResultSuccess(resp.results)) successMessage.value = resp.results.successMessage || "";
    
};

</script>