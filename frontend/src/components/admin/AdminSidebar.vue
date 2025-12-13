<template>
    <aside  class="bg-admin min-h-[100vh] flex text-white flex-col justify-between items-center text-center pt-5 px-5 border border-adminborders min-w-[180px] ">
        
        <div>
            <div @click="router.push('/admin')" class="flex gap-3 items-center hover:cursor-pointer font-medium font-sans ">
                <img src="/img/logo.webp" class="w-10 "/>
                <h1 class="text-sm md:text-base lg:text-lg text-left text-blue-500 ">Ecommerce <span class="flex justify-center">Vue</span></h1>
            </div>
            <div class="flex flex-col pt-5 font-mono">
                <button @click="redirecProd" class="px-10 py-2 text-sm lg:text-base hover:bg-adminhover rounded-xl">Productos</button>
                <button @click="redirectOrd" class="px-10 py-2 text-sm lg:text-base hover:bg-adminhover rounded-xl">Ordenes</button>
                <button @click="redirectUser" class="px-10 py-2 text-sm lg:text-base hover:bg-adminhover rounded-xl">Usuarios</button>
            </div>
        </div>
        
        <div class="flex gap-4 items-center justify-center">
            <div class="flex flex-col gap-2 px-2">
                <span>{{ getUsuario?.email }}</span>
                <button @click="auth.logout()" class="bg-red-600 px-3 py-2 rounded hover:bg-red-500">
                Logout
                </button>
            </div>
        </div>
    </aside>
</template>

<script setup lang="ts">
import { useAuthStore } from '@/store/authStore';
import { storeToRefs } from 'pinia';
import { useRouter } from 'vue-router';

const router = useRouter();


const redirecProd = () => router.push("/admin/productos");
const redirectOrd = () => router.push("/admin/ordenes");
const redirectUser = () => router.push("/admin/usuarios");

const auth = useAuthStore();

const {getUsuario} = storeToRefs(auth);

</script>