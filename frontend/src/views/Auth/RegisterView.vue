<template>
  <NavBar />
  <div class="flex flex-col items-center justify-center  bg-bgContent w-full h-full min-h-screen">
    <form @submit.prevent="handleCrearUsuario" action="" class="bg-slate-800 p-8 rounded-2xl shadow space-y-5 text-white w-[450px] border-2 border-blue-600">

        <h2 class="text-xl font-semibold text-center font-mono block ">Crear Usuario</h2>
        
        <div >
          <label class="block mb-2.5 text-sm font-medium">Nombre</label>
          <div class="block w-full relative">
            <input @blur="verificarNombre" v-model="UsuarioACrear.nombre" type="text" :class="` block w-full ps-9 pe-2 py-2.5 
          bg-slate-700 text-sm rounded-lg border-none outline-none ring-2 focus:ring-blue-600
            shadow-xs placeholder:text-body  ${errorNombre ? 'ring-red-600' : 'focus:ring-blue-600'}`" />

            <div class="inset-y-0 absolute start-0 flex items-center ps-3 pointer-events-none">
              
              <svg class="w-4 h-4 text-blue-500 lucide lucide-user-round-pen-icon lucide-user-round-pen"
              xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M2 21a8 8 0 0 1 10.821-7.487"/><path d="M21.378 16.626a1 1 0 0 0-3.004-3.004l-4.01 4.012a2 2 0 0 0-.506.854l-.837 2.87a.5.5 0 0 0 .62.62l2.87-.837a2 2 0 0 0 .854-.506z"/><circle cx="10" cy="8" r="5"/></svg>
            </div>
          </div>

        </div>

        <div >
          <label class="block mb-2.5 text-sm font-medium">Email</label>
          
          <div class="w-full block relative">
            <input @blur="verificarEmail" v-model="UsuarioACrear.email" type="email" class="block w-full ps-9 pe-2 py-2.5
          bg-slate-700 text-sm rounded-lg border-none outline-none ring-2 focus:ring-blue-600
            shadow-xs placeholder:text-body "/>

            <div class="absolute inset-y-0 start-0 flex items-center ps-3 pointer-events-none">
              <svg class="w-4 h-4 text-body text-blue-500" aria-hidden="true" xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="m3.5 5.5 7.893 6.036a1 1 0 0 0 1.214 0L20.5 5.5M4 19h16a1 1 0 0 0 1-1V6a1 1 0 0 0-1-1H4a1 1 0 0 0-1 1v12a1 1 0 0 0 1 1Z"/></svg>
            </div>
          </div>

        </div>
        
        <div class="block w-full">
          <div class="flex flex-nowrap w-full gap-2">
            <label class="inline-block mb-2.5 text-sm font-medium w-full">Constraseña</label>
            <label class="inline-block mb-2.5 text-sm font-medium w-full">Repetir Contraseña</label>
          </div>
          
          <div class="flex flex-nowrap gap-2">
              <div class="inline-block  relative w-full ">
              <input v-model="UsuarioACrear.password" type="text" autocomplete="new-password" @blur="verificarContraseña"
              :class="`block w-full ps-9 pe-2 py-2.5 
            bg-slate-700 text-sm rounded-lg border-none outline-none ring-2 focus:ring-blue-600
              shadow-xs placeholder:text-body  ${errorContraseña ? 'ring-red-600' : 'focus:ring-blue-600'}` " />
              <div class="absolute inset-y-0 start-0 flex items-center ps-3 pointer-events-none">
              <svg class="w-4 h-4 text-body lucide lucide-lock-keyhole-icon lucide-lock-keyhole text-blue-500" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" ><circle cx="12" cy="16" r="1"/><rect x="3" y="10" width="18" height="12" rx="2"/><path d="M7 10V7a5 5 0 0 1 10 0v3"/></svg>
              </div>
            </div>
            
            <div class="inline-block  relative w-full" >
              <input @blur="verificarContraseña" v-model="repetirContraseña" autocomplete="new-password" type="text" :class=" `block w-full ps-2 pe-2 py-2.5 
            bg-slate-700 text-sm rounded-lg border-none outline-none ring-2 focus:ring-blue-600
              shadow-xs placeholder:text-body  ${errorContraseña ? 'ring-red-600' : 'focus:ring-blue-600'}` " />
            </div>

          </div>

        </div>
        
        <div class="block pt-5"> 
          <button type="submit" class="block p-2 transition bg-blue-600 hover:bg-blue-600/60 w-full">
            Crear Usuario
          </button>
        </div>
    </form>
    <div class="flex gap-4 mt-5 flex-col items-stretch p-8 bg-slate-800 border-2 border-blue-600 w-[450px]  rounded-2xl shadow text-white" v-if="errorList.length > 0" >
      <ul v-for="item in errorList" :key="item">
        <li class="text-sm font-medium"><span>❌ </span>{{ item }}</li>
      </ul>  
    </div>
  </div>
</template>

<script setup lang="ts">
import fetchApi from '@/api/fetchApi';
import  NavBar from '@/components/Nav-bar.vue';
import type { UsuarioReadDTO, CreateUsuarioDTO } from '@/dtos/DTOs';
import router from '@/router';
import { useAuthStore } from '@/store/authStore';
import { ref } from 'vue';

const errorList = ref<string[]>([]);
const errorContraseña = ref(false);
const errorNombre = ref(false);
const errorEmail = ref(false);
const repetirContraseña = ref("");

const UsuarioACrear = ref<CreateUsuarioDTO>({
  nombre : "",
  email : "",
  password : ""
});


const handleCrearUsuario = () =>{
  vaciarEstados();
  verificarContraseña();
  verificarNombre();
  verificarEmail();

  if(errorList.value.length == 0 && !errorContraseña.value && !errorNombre.value && !errorEmail.value) CrearUsuario();
};

const verificarContraseña = () => {
  
  const reg = /^(?=.*[A-Z])[A-Za-zñÑ0-9]{6,20}$/;

  if (repetirContraseña.value !== UsuarioACrear.value.password) pushError("Contraseñas deben ser iguales");
  else removeError("Contraseñas deben ser iguales");

  if ( UsuarioACrear.value.password.includes(" ") || repetirContraseña.value.includes(" ")) pushError("La contraseña no puede contener espacios");
  else removeError("La contraseña no puede contener espacios");

  if (!reg.test(UsuarioACrear.value.password)) pushError("Contraseña entre 6 y 20 caracteres. 1 Mayúscula obligatoria. Sin caracteres especiales, solo letras y números.");
  else removeError("Contraseña entre 6 y 20 caracteres. 1 Mayúscula obligatoria. Sin caracteres especiales, solo letras y números.");

  if(errorList.value.includes("Contraseña entre 6 y 20 caracteres") || errorList.value.includes("La contraseña no puede contener espacios") || errorList.value.includes("Contraseñas deben ser iguales") ) errorContraseña.value = true;
  else errorContraseña.value = false;

};


const verificarNombre = () => {
  const reg = /^[A-Za-z ]{5,60}$/;

  if (!reg.test(UsuarioACrear.value.nombre)) {
    errorNombre.value = true;
    pushError("El nombre solo puede contener letras y tener entre [5-60] caracteres");
  }else{
    errorNombre.value = false;
    removeError("El nombre solo puede contener letras y tener entre [5-60] caracteres");
  }
};

const verificarEmail = () => {
  const regEmail = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

  if (!regEmail.test(UsuarioACrear.value.email)) {
    errorEmail.value = true;
    pushError("Email inválido");
  }else{
    errorEmail.value = false;
    removeError("Email inválido");
  }
};

const vaciarEstados = ()=>{
  errorList.value = [];
  errorContraseña.value = false;
  errorNombre.value = false;
}

const auth  = useAuthStore();

const CrearUsuario = async() =>{
  const resp = await fetchApi<UsuarioReadDTO>('Usuario',{
    method : "POST",
    body : JSON.stringify(UsuarioACrear.value)
  });
  if(resp.errorMessage) console.log('Error al crear usuario : ' + resp.results);
  else{
      auth.login(UsuarioACrear.value.email,UsuarioACrear.value.password);
      router.push('/');
  }
};

const pushError = (msg: string) => {
  if (!errorList.value.includes(msg)) {
    errorList.value.push(msg);
  }
};
const removeError = (msg: string)=>{
  if(errorList.value.includes(msg)) errorList.value = errorList.value.filter(e=> e != msg);
}

</script>
