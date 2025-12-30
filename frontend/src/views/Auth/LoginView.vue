<template>
  <NavBar />
  <div class="flex items-center justify-center  bg-bgContent w-full h-full min-h-screen">
    <form
      @submit.prevent="handleLogin"
      class="bg-slate-800 p-8 rounded-2xl shadow space-y-5 text-white w-[450px] border-2 border-blue-600 "
    >
       
      <h1 class="text-xl font-semibold text-center font-mono block">Iniciar Sesión</h1>

      <label class="block mb-2.5 text-sm font-medium">Email</label>
      <div class="relative">
        <div class="absolute inset-y-0 start-0 flex items-center ps-3 pointer-events-none">
          <svg class="w-4 h-4 text-body text-blue-500" aria-hidden="true" xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="m3.5 5.5 7.893 6.036a1 1 0 0 0 1.214 0L20.5 5.5M4 19h16a1 1 0 0 0 1-1V6a1 1 0 0 0-1-1H4a1 1 0 0 0-1 1v12a1 1 0 0 0 1 1Z"/></svg>
        </div>
        <input v-model="email" type="email" autocomplete="username"
        class="block w-full ps-9 pe-3 py-2.5 
        bg-slate-700  
        text-heading text-sm rounded-lg border-none outline-none ring-2 focus:ring-blue-600
        shadow-xs placeholder:text-body" 
        placeholder="Email"
        required 
        />
      </div>

      <label class="block mb-2.5 text-sm font-medium ">Contraseña</label>
      <div class="relative">
        <div class="absolute inset-y-0 start-0 flex items-center ps-3 pointer-events-none">
          <svg class="w-4 h-4 text-body lucide lucide-lock-keyhole-icon lucide-lock-keyhole text-blue-500" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" ><circle cx="12" cy="16" r="1"/><rect x="3" y="10" width="18" height="12" rx="2"/><path d="M7 10V7a5 5 0 0 1 10 0v3"/></svg>
        </div>
        <input v-model="password" type="password" autocomplete="current-password"
        class="block w-full ps-9 pe-3 py-2.5 
        bg-slate-700  
        text-heading text-sm rounded-lg border-none outline-none ring-2 focus:ring-blue-600
        shadow-xs placeholder:text-body " 
        placeholder="Email"
        required 
        />
      </div>

      <div class="flex justify-center items-center w-full">
        <button type="button" class="text-sm text-white md:text-base block bg-amber-600 hover:bg-amber-600/60 w-full p-2 transition rounded-lg " @click="router.push('login/crear-usuario')">Crear Usuario</button>
      </div>

      <button
        type="submit"
        class="block bg-blue-600 hover:bg-blue-600/60 text-white w-full p-2 transition rounded-lg "
      >
        Entrar
      </button>
      <p v-if="error" class="text-red-500 text-center text-sm">{{ error }}</p>
    </form>
  </div>
</template>

<script setup lang="ts">
import { ref } from "vue";
import { useAuthStore } from "../../store/authStore";
import { useRouter } from "vue-router";
import NavBar from "@/components/Nav-bar.vue";

const email = ref("");
const password = ref("");
const error = ref("");
const router = useRouter();
const auth = useAuthStore();

async function handleLogin() {
  try {
    await auth.login(email.value, password.value);
    router.push("/");
  } catch (err: any) {
    error.value = err.message;
  }
}
</script>