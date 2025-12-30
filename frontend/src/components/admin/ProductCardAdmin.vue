<template>
    <div class="grid grid-rows-auto text-center gap-5 justify-items-center ">
        <div v-if="!modificando">
            <div>
                <img :src="producto.imagen" alt="img" class="w-full max-h-[150px] object-fill rounded" />
            </div>
            
            <div >
                <h2 class="text-lg font-semibold mt-2">{{ producto.nombre }}</h2>
                <p class="text-gray-300">{{ producto.categoria.nombre }}</p>
                <p class="font-bold text-green-500 mt-1">${{ producto.precio }}</p>
                <p class="font-bold text-gray-300 mt-1">Stock : {{ producto.stock }}</p> 
                <p class="font-bold text-gray-300 mt-1">Estado: <span v-if="producto.estado" class="text-green-500">Activo</span><span v-if="!producto.estado" class="text-red-500">Inactivo</span></p>
            </div>
        </div>
        <div v-if="modificando" class="w-full flex justify-center">
            <form @submit.prevent="handleSubmitMod" class="flex flex-col gap-4 w-full p-4 rounded-xl shadow-md items-center">
                <h3 class="text-slate-200 font-semibold text-lg text-center mb-2">
                Modificando {{ producto.nombre.split(" ")[0] }}
                </h3>
                <input type="text" v-model="productoModificado.imagen" 
                class="w-full px-3 py-2 rounded-lg bg-slate-700 text-slate-200 focus:outline-none focus:ring-2 focus:ring-blue-500"/>
                <input type="text" v-model="productoModificado.nombre" 
                class="w-full px-3 py-2 rounded-lg bg-slate-700 text-slate-200 focus:outline-none focus:ring-2 focus:ring-blue-500"/>
                <input type="number" step="0.01" v-model="productoModificado.precio" 
                class="w-full px-3 py-2 rounded-lg bg-slate-700 text-slate-200 focus:outline-none focus:ring-2 focus:ring-blue-500"/>
                <input type="number" v-model="productoModificado.stock" 
                class="w-full px-3 py-2 rounded-lg bg-slate-700 text-slate-200 focus:outline-none focus:ring-2 focus:ring-blue-500"/>
                <button type="submit" class="rounded-lg font-semibold text-lg text-slate-200 bg-blue-600 py-1 w-[120px] md:w-[150px]" >Aceptar</button>
                <button class="rounded-lg font-semibold text-lg text-slate-200 bg-red-600 py-1 w-[120px] md:w-[150px]" @click="handleModificar" >Cancelar</button>
            </form>
        </div>
        
        
        <button class="rounded-lg font-semibold text-lg text-slate-200 bg-blue-600 py-1 w-[120px] md:w-[150px]" @click="handleModificar" v-if="!modificando">Modificar</button>
        <button class="rounded-lg font-semibold text-lg text-slate-200 bg-red-600 py-1 w-[120px] md:w-[150px]" @click="handleEliminar" v-if="producto.estado && !modificando">Eliminar</button>
        <button class="rounded-lg font-semibold text-lg text-slate-200 bg-green-600 py-1 w-[120px] md:w-[150px]" @click="handleEliminar" v-if="!producto.estado && !modificando">Activar</button>
        <p>{{ errorEliminando }}</p>
    </div>
</template>

<script setup lang="ts">
import { esResultError, esResultSuccess, type ProductoReadDTO, type ProductoUpdateDTO } from '@/dtos/DTOs';

// desestructuro producto como ProductoActual
const props = defineProps<{ producto : ProductoReadDTO }>();

import { ref, toRef, watch } from 'vue';
import fetchApi from '@/api/fetchApi';
import { useAuthStore } from '@/store/authStore';

const producto = toRef(props,"producto"); // propReactiva
const modificando = ref(false);
const errorEliminando = ref("");

const productoModificado = ref<ProductoUpdateDTO>({
    nombre: producto.value.nombre,
    descripcion: producto.value.descripcion,
    precio: producto.value.precio,
    stock: producto.value.stock,
    marca: producto.value.marca,
    imagen: producto.value.imagen,
    estado: producto.value.estado,
    categoriaId: producto.value.categoria.id,
});

const cargarBody = () => {
  productoModificado.value = {
    nombre: producto.value.nombre,
    descripcion: producto.value.descripcion,
    precio: producto.value.precio,
    stock: producto.value.stock,
    marca: producto.value.marca,
    imagen: producto.value.imagen,
    estado: producto.value.estado,
    categoriaId: producto.value.categoria.id,
  };
};

const handleModificar = ()=>{
    modificando.value = !modificando.value;
};

const handleEliminar = async () =>{
    producto.value.estado = !producto.value.estado;
    cargarBody();
    const resp = await fetchApi(`Producto/${producto.value.id}`,{
        method: "PUT",
        body: JSON.stringify(productoModificado.value),
            
    });
    if(esResultError(resp.results)) errorEliminando.value= "❌ " +  resp.results.errorMessage;
    if(esResultSuccess(resp.results)) errorEliminando.value = "✔ " + resp.results.successMessage || "";
    
}
const handleSubmitMod = async () =>{
    const resp = await fetchApi(`Producto/${producto.value.id}`,{
        method : "PUT",
        body : JSON.stringify(productoModificado.value)
    });
    if(esResultError(resp.results)) errorEliminando.value = "❌ " + resp.results.errorMessage;
    if(esResultSuccess(resp.results)) errorEliminando.value = "✔ " + resp.results.successMessage || "";

    producto.value.nombre = productoModificado.value.nombre;
    producto.value.imagen = productoModificado.value.imagen;
    producto.value.precio = productoModificado.value.precio;
    producto.value.stock = productoModificado.value.stock;
    
    modificando.value = false;

};


const timeout = ref(0);

watch(errorEliminando,()=>{
    clearTimeout(timeout.value);
    timeout.value = setTimeout(()=> {errorEliminando.value=""},1900);
})
</script>