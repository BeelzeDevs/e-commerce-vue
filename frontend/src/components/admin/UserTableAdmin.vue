<template>
    
    <div v-if="loading" class="loading-container">
        <div  class="loading"></div>
        <div class="loading-text">Cargando...</div>
    </div>
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
                    <td :class="`px-4 py-2 text-center font-semibold ${u.estado ? 'text-green-500': 'text-red-500'}` " >{{ u.estado }}</td>
                    <td class="px-4 py-2 text-right"><button class="rounded-xl bg-blue-800 py-2 px-3 hover:bg-blue-700" @click="clickVerOrden(u)">Ver Ordenes</button></td>
                </tr>
            </tbody>
        </table>

    </div>
    
    <UserModal v-if="modal" @toogleModal = "handleModal" :usuario = "userToDetail"/>


</template>

<script setup lang="ts">
import fetchApi from '@/api/fetchApi';
import type { UsuarioReadDTO } from '@/dtos/DTOs';
import { useAuthStore } from '@/store/authStore';
import { storeToRefs } from 'pinia';
import { onMounted, ref, watch } from 'vue';
import UserModal from '@/components/admin/UserModal.vue';


const loading = ref(true);
const auth = useAuthStore();
const {getAuthHeader} = storeToRefs(auth);
const errorLoadingUsuarios = ref("");
const usuarios = ref<UsuarioReadDTO[]>([]);

const FetchUsuarios = async () =>{
    const resp = await fetchApi<UsuarioReadDTO>("Usuario",{
        headers : getAuthHeader.value
    });
    if(resp.errorMessage) errorLoadingUsuarios.value = `❌ error : ${resp.errorMessage}`;
    else{
        usuarios.value = resp.results || [];
        loading.value = false;
    }

};

onMounted(async()=>{
    await FetchUsuarios();
});

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

</script>