<template>
    
    <div v-if="loading" class="loading-container">
        <div  class="loading"></div>
        <div class="loading-text text-white">Cargando...</div>
    </div>

    

    <UserFilterAdmin v-model:filtros="filter"/>

    <div class="w-full overscroll-x-auto custom-scrollbar">
        <table class="min-w-full divide-y divide-white text-sm md:text-base">
            <thead class="bg-blue-800 text-slate-200">
                <tr >
                    <th class="px-4 py-2 font-semibold text-left">Nombre</th>
                    <th class="px-4 py-2 font-semibold">Email</th>
                    <th class="px-4 py-2 font-semibold">Rol</th>
                    <th class="px-4 py-2 font-semibold">Fecha Registro</th>
                    <th class="px-4 py-2 font-semibold">Estado</th>
                    <th></th>
                </tr>
            </thead>
            <tbody class="divide-y divide-slate-700 ">
                <tr v-for="(u , index) in usuarios" :key="u.id" class="hover:bg-slate-800 transition">
                    <td class="px-4 py-2 font-medium text-white ">{{ u.nombre }}</td>
                    <td class="px-4 py-2 text-slate-300 text-center break-all" >{{ u.email }}</td>
                    <td class="px-4 py-2 text-slate-300 text-center font-semibold" >{{ u.rol.nombre }}</td>
                    <td class="px-4 py-2 text-slate-300 text-center " >{{ new Date(u.fechaRegistro).toLocaleDateString() }}</td>
                    <td class="px-4 py-2 text-center font-semibold" >
                        <div class="relative">
                            <select v-on:change="handleUpdateUsuario(u)" :class="`w-full bg-bgContent text-sm rounded pl-3 pr-8 py-2 transition duration-300 ease focus:outline-none shadow-sm focus:shadow-md appearance-none cursor-pointer  ${u.estado ? 'text-green-500': 'text-red-500'}`" v-model="u.estado">
                                <option :value="true" class="text-white">Activo</option>
                                <option :value="false" class="text-white">Inactivo</option>
                            </select>
                            <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.2" stroke="currentColor" :class="`h-5 w-5 ml-1 absolute top-2.5 right-2.5  ${u.estado ? 'text-green-500':'text-red-600'}`">
                            <path stroke-linecap="round" stroke-linejoin="round" d="M8.25 15 12 18.75 15.75 15m-7.5-6L12 5.25 15.75 9" />
                            </svg>
                        </div>
                    </td>
                    <td class="px-4 py-2 text-right">
                        <button class="rounded-xl bg-blue-800 py-2 px-3 hover:bg-blue-700" @click="clickVerOrden(u)">Ver Ordenes</button>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    
    <PagerComp v-model:page="page" :totalPages="totalPages" />
    
    <UserModal v-if="modal" @toogleModal = "handleModal" :usuario = "userToDetail"/>


</template>

<script setup lang="ts">
import fetchApi from '@/api/fetchApi';
import { esResultError, esResultSuccess, type ResultadoPaginado, type UsuarioReadDTO } from '@/dtos/DTOs';
import { useAuthStore } from '@/store/authStore';
import { onMounted, ref, watch } from 'vue';
import UserModal from '@/components/admin/UserModal.vue';
import { isNullOrUndef } from 'chart.js/helpers';
import PagerComp from '@/components/Pagers/PagerComp.vue';
import UserFilterAdmin from '@/components/Filters/UserFilterAdmin.vue';


const loading = ref(true);
const auth = useAuthStore();
const errorLoadingUsuarios = ref("");
const usuarios = ref<UsuarioReadDTO[]>([]);


// filtros
const page = ref(1);
const pageSize = ref(8);
const totalPages = ref(0);
const filter = ref({
    rolId : null as number | null,
    search : "" as string | null,
    fecha : null as string | null,
    estado : null as boolean | null,
});


const FetchUsuarios = async () =>{
    loading.value = true;

    const params = new URLSearchParams({
        page : page.value.toString(),
        pageSize : pageSize.value.toString(),
    });

    if(!isNullOrUndef(filter.value.rolId)) params.append("rolId",filter.value.rolId.toString());
    if(filter.value.search) params.append("search", filter.value.search);
    if(!isNullOrUndef(filter.value.fecha)) params.append("fecha", filter.value.fecha);
    if(!isNullOrUndef(filter.value.estado)) params.append("estado",filter.value.estado.toString()); 

    const resp = await fetchApi<ResultadoPaginado<UsuarioReadDTO>>(`Usuario?${params.toString()}`);

    if(esResultError(resp.results)){ 
        errorLoadingUsuarios.value = resp.results.errorMessage; 
        loading.value = false;
        return;
    }
    else{
        usuarios.value = resp.results.items;
        totalPages.value = resp.results.totalPages;
        loading.value = false;
    }

};

onMounted(async()=>{
    await FetchUsuarios();
});

watch(()=> filter,
()=>{
    page.value=1;
    FetchUsuarios();
},
{deep : true});

watch([page,pageSize],FetchUsuarios);


// modal

const modal = ref(false);
const userToDetail = ref<UsuarioReadDTO>({
    id : 1,
    rol : {
        id : 2,
        nombre : "Cliente"
    },
    email : "vacio",
    fechaRegistro : new Date("00/00/0000"),
    nombre : "vacio",
    estado : true,

});

const handleModal = ()=>{
    modal.value = ! modal.value;
}

const clickVerOrden = (u : UsuarioReadDTO)=>{
    userToDetail.value.id = u.id;
    userToDetail.value.email = u.email;
    userToDetail.value.rol = u.rol;
    userToDetail.value.estado = u.estado;
    userToDetail.value.nombre = u.nombre;
    userToDetail.value.fechaRegistro = u.fechaRegistro;
    handleModal();
};

watch(modal,()=>{

});

// update
const errorUpdate = ref("");
const succesMessage = ref("");
const handleUpdateUsuario = async(u : UsuarioReadDTO) =>{
    const resp = await fetchApi<string>(`Usuario/${u.id}`,{
        method : "PUT",
        body : JSON.stringify(u)
    });
    if(esResultError(resp.results)) errorUpdate.value =  resp.results.errorMessage;
    if(esResultSuccess(resp.results)) succesMessage.value = resp.results.successMessage;
}



</script>