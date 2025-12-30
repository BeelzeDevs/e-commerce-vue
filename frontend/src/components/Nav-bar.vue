<template>
  <nav
    class="bg-slate-900 px-6 md:px-6 pt-4 grid lg:h-[10dvh] text-white sticky top-0 z-50 shadow grid-cols-2 grid-rows-2  lg:grid-cols-3 lg:grid-rows-1"
  >
    <router-link
      :to="esAdmin ? '/admin' : '/'"
      class="text-sm md:text-xl font-bold tracking-wide hover:text-cyan-400 transition text-blue-400 w-full break-words pr-14 lg:pr-0 place-self-center col-start-1 row-start-1"
    >
      EcommerceVue - Bazar el hispano
    </router-link>

    <div class="flex w-full justify-center gap-2 md:gap-5 place-self-end row-start-2 col-span-2 lg:col-span-1 lg:row-start-1">
      <router-link
        :to="esAdmin ? '/admin' : '/'"
        class="hover:bg-blue-600 0 transition px-4 py-2 rounded-lg font-semibold navitem-anim"
        active-class="navitem-activo bg-slate-700"
        >
        Home
      </router-link>
      <router-link
          to="/productos"
          class="hover:bg-cyan-500 0 transition px-4 py-2 rounded-lg font-semibold navitem-anim"
          active-class="navitem-activo bg-slate-700"
        >
        Productos
      </router-link>

      <router-link
          to="/about"
          class="hover:bg-indigo-600 transition px-4 py-2 rounded-lg font-semibold navitem-anim"
          active-class="navitem-activo  bg-slate-700"
        >
        About
      </router-link>

    </div>

    

    <div class="flex gap-2 lg:gap-6 justify-end items-center w-full col-start-2 row-start-1 lg:col-start-3 pt-4 lg:pt-0">
      <div
        class="relative cursor-pointer p-2 rounded-full hover:bg-slate-800 transition" @click="pushCarrito"
      >
        <svg
          class="w-6 h-6"
          xmlns="http://www.w3.org/2000/svg"
          viewBox="0 0 24 24"
          fill="none"
          stroke="currentColor"
          stroke-width="2"
          stroke-linecap="round"
          stroke-linejoin="round"
        >
          <circle cx="8" cy="21" r="1" />
          <circle cx="19" cy="21" r="1" />
          <path d="M2.05 2.05h2l2.66 12.42a2 2 0 0 0 2 1.58h9.78a2 2 0 0 0 1.95-1.57l1.65-7.43H5.12" />
        </svg>

        <span
          v-if="carrito.cantidadItems > 0"
          :class="badge1"
        >
          {{ carrito.cantidadItems > 99 ? '99+' : carrito.cantidadItems }}
        </span>
      </div>

      <router-link
        v-if="!auth.user"
        to="/login"
        class="bg-blue-600 hover:bg-blue-500 transition px-4 py-2 rounded-lg font-semibold "
      >
        Login
      </router-link>

      <div v-else class="flex items-center gap-3">
        <span class="text-sm text-slate-300">
          {{ auth.user.email }}
        </span>
        <button
          @click="auth.logout()"
          class="bg-red-600 hover:bg-red-500 px-3 py-2 rounded-lg transition"
        >
          Logout
        </button>
      </div>
    </div>
  </nav>
</template>


<script setup lang="ts">
import { useCartStore } from "@/store/cartStore";
import { useAuthStore } from "../store/authStore";
import {storeToRefs} from 'pinia';
import { computed } from "vue";
import router from "@/router";

const auth = useAuthStore();
const {esAdmin} = storeToRefs(auth);
const carrito = useCartStore();


const badge1 = computed(() =>
  `
  absolute -top-1 -right-1
  min-w-[20px] h-[20px]
  px-1
  flex items-center justify-center
  text-xs font-bold
  rounded-full
  bg-cyan-500 text-slate-900
  `
);

const pushCarrito = () => router.push("/cart");



</script>