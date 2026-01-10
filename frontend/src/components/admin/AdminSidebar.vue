<template>
    <nav  class="bg-admin min-h-[100vh] flex text-white flex-col justify-between items-center border border-adminborders min-w-[180px] " >
        
        <div class="pt-5 px-5" >
            <div @click="router.push('/admin')" class="flex gap-3 items-center hover:cursor-pointer font-medium font-sans ">
                <img src="/img/logo.webp" class="w-10 "/>
                <h1 class="text-sm md:text-base lg:text-lg text-left text-blue-500 ">Ecommerce <span class="flex justify-center">Vue</span></h1>
            </div>
            <div class="flex flex-col pt-5 font-mono gap-3">
                <div class="relative flex w-full">
                    <div class="absolute inset-y-0 start-4 flex justify-center items-center">
                        <svg class="w-4 h-4 text-body text-blue-500 lucide lucide-gem-icon lucide-gem" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" ><path d="M10.5 3 8 9l4 13 4-13-2.5-6"/><path d="M17 3a2 2 0 0 1 1.6.8l3 4a2 2 0 0 1 .013 2.382l-7.99 10.986a2 2 0 0 1-3.247 0l-7.99-10.986A2 2 0 0 1 2.4 7.8l2.998-3.997A2 2 0 0 1 7 3z"/><path d="M2 9h20"/></svg>
                    </div>
                    <router-link to="/admin/productos" class="px-10 py-2 text-sm lg:text-base hover:bg-adminhover rounded-xl flex w-full" active-class="bg-slate-800">Productos</router-link>
                </div>
                <div class="relative flex w-full">
                    <div class="absolute inset-y-0 start-4 flex justify-center items-center">
                        <svg  class="w-4 h-4 text-body text-blue-500 lucide lucide-notebook-pen-icon lucide-notebook-pen" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" ><path d="M13.4 2H6a2 2 0 0 0-2 2v16a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2v-7.4"/><path d="M2 6h4"/><path d="M2 10h4"/><path d="M2 14h4"/><path d="M2 18h4"/><path d="M21.378 5.626a1 1 0 1 0-3.004-3.004l-5.01 5.012a2 2 0 0 0-.506.854l-.837 2.87a.5.5 0 0 0 .62.62l2.87-.837a2 2 0 0 0 .854-.506z"/></svg>
                    </div>
                    <router-link to="/admin/ordenes" class="px-10 py-2 text-sm lg:text-base hover:bg-adminhover rounded-xl flex w-full" active-class="bg-slate-800">Ordenes</router-link>
                </div>
                <div class="relative flex w-full">
                    <div class="absolute inset-y-0 start-4 flex justify-center items-center">
                        <svg class="w-4 h-4 text-body text-blue-500 lucide lucide-user-icon lucide-user" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M19 21v-2a4 4 0 0 0-4-4H9a4 4 0 0 0-4 4v2"/><circle cx="12" cy="7" r="4"/></svg>
                    </div>
                    <router-link to="/admin/usuarios" class="px-10 py-2 text-sm lg:text-base hover:bg-adminhover rounded-xl flex w-full" active-class="bg-slate-800">Usuarios</router-link>
                </div>
                <div class="relative flex w-full">
                    <div class="absolute inset-y-0 start-4 flex justify-center items-center">
                        <svg class="lucide lucide-chart-spline-icon lucide-chart-spline w-4 h-4 text-body text-blue-500" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" ><path d="M3 3v16a2 2 0 0 0 2 2h16"/><path d="M7 16c.5-2 1.5-7 4-7 2 0 2 3 4 3 2.5 0 4.5-5 5-7"/></svg>
                    </div>
                    <router-link to="/admin/stats" class="px-10 py-2 text-sm lg:text-base hover:bg-adminhover rounded-xl flex w-full" active-class="bg-slate-800">Stats</router-link>
                </div>
            </div>
        </div>

        <div class="w-full flex flex-col gap-5 justify-center items-center">
            <div :class="` bg-adminhover w-40 p-5 ${optionsVisible ? 'flex':'hidden' } justify-center items-center flex-col gap-5 rounded-xl`">
                        <button @click="auth.logout()" class="bg-red-600 px-3 py-2 rounded hover:bg-red-500 w-full">
                                Logout 
                        </button>
                        <button @click="router.push('/crear-admin')" class="bg-blue-600 px-3 py-2 rounded hover:bg-blue-500 w-full">
                                Crear Admin 
                        </button>
            </div>

            <div class="w-full p-5 hover:bg-adminhover"  @click="handleUserOptions()">
                <div class="flex justify-center items-center w-full">
                    <div class="pt-1">
                        <svg class="w-5 h-5 text-body text-blue-500  lucide lucide-circle-user-round-icon lucide-circle-user-round " xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" ><path d="M18 20a6 6 0 0 0-12 0"/><circle cx="12" cy="10" r="4"/><circle cx="12" cy="12" r="10"/></svg>
                    </div>
                    <label class="break-all text-sm lg:text-base flex w-full max-w-[125px] cursor-pointer pl-5">{{ getUsuario?.nombre.split(" ")[0] }}</label>

                    
                    
                </div>
            </div>

        </div>

    </nav>
</template>

<script setup lang="ts">
import { useAuthStore } from '@/store/authStore';
import { storeToRefs } from 'pinia';
import { ref } from 'vue';
import { useRouter } from 'vue-router';

const router = useRouter();


const auth = useAuthStore();

const {getUsuario} = storeToRefs(auth);

// Opciones del usuario al hacer click en su nombre
const optionsVisible = ref(false);

const optionsHandler = () => optionsVisible.value = !optionsVisible.value;

const handleUserOptions = () =>{
    optionsHandler();
}

</script>