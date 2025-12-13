<template>
  <NavBar />
  <div class="flex items-center justify-center h-screen bg-background">
    <form
      @submit.prevent="handleLogin"
      class="bg-white/70 p-8 rounded-2xl shadow w-80 space-y-4"
    >
      <h1 class="text-xl font-semibold text-center">Iniciar Sesión</h1>
      <input
        v-model="email"
        type="email"
        placeholder="Email"
        required
        class="w-full border p-2 rounded focus:ring-2 focus:ring-blue-500 bg-white focus-visible:outline-none"
      />
      <input
        v-model="password"
        type="password"
        placeholder="Contraseña"
        required
        class="w-full border p-2 rounded focus:ring-2 focus:ring-blue-500 bg-white/50 focus-visible:outline-none"
      />
      <button
        type="submit"
        class="bg-button hover:bg-buttonhover text-white w-full p-2 rounded transition "
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