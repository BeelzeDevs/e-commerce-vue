<template>
  <nav class="bg-gradient-cover px-6 py-3 flex justify-between items-center h-20 text-title sticky top-0 ">
    <router-link to="/admin" v-if="esAdmin" class=" text-xl font-bol">EcommerceVue</router-link>
    <router-link to="/" v-if="!esAdmin" class="text-xl font-bold">EcommerceVue</router-link>

    <div class="flex gap-4 items-center">
      <router-link v-if="!auth.user" to="/login" class="rounded bg-emerald-600 hover:bg-green-700/70 transition w-full py-2 px-3 text-white">Login</router-link>

      <div v-else class="flex items-center gap-2">
        <span>{{ auth.user.email }}</span>
        <button @click="auth.logout()" class="bg-red-600 px-3 py-2 rounded hover:bg-red-500">
          Logout
        </button>
      </div>
    </div>
  </nav>
</template>

<script setup lang="ts">
import { useAuthStore } from "../store/authStore";
import {storeToRefs} from 'pinia';

const auth = useAuthStore();
const {esAdmin} = storeToRefs(auth);


</script>