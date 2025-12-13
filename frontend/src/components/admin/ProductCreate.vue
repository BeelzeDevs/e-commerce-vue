<template>
    <button
    @click="handleModalCrear"
    class="flex items-center gap-2 mb-6 bg-blue-600 hover:bg-blue-500 transition px-4 py-2 rounded-lg text-slate-100 font-semibold shadow"
    >
        <span class="text-xl">+</span>
        Crear producto
    </button>

    <section
    v-if="modalCrear"
    class="fixed inset-0 z-50 bg-black/70 flex items-center justify-center"
    >
        <form
            @submit.prevent="handleSubmitCrear"
            class="bg-slate-800 w-full max-w-2xl max-h-[85vh] overflow-y-auto rounded-xl p-6 flex flex-col gap-6 shadow-xl custom-scrollbar"
        >
            <h1 class="text-2xl font-bold text-slate-100 text-center">
            Crear producto
            </h1>

           
            <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
                <div>
                    <label class="text-sm text-slate-300">Imagen</label>
                    <input v-model="productoACrear.imagen" class="input-admin" />
                </div>

                <div>
                    <label class="text-sm text-slate-300">Nombre</label>
                    <input v-model="productoACrear.nombre" class="input-admin" />
                </div>

                <div>
                    <label class="text-sm text-slate-300">Categoría ID</label>
                    <select v-model.number="productoACrear.categoriaId" class="input-admin">
                    <option disabled value="0">Seleccione una categoría</option>
                    <option
                        v-for="i in categorias"
                        :key="i.id"
                        :value="i.id"
                    >
                        {{ i.nombre }}
                    </option>
                    </select>
                </div>

                <div>
                    <label class="text-sm text-slate-300">Marca</label>
                    <input v-model="productoACrear.marca" class="input-admin" />
                </div>

                <div class="md:col-span-2">
                    <label class="text-sm text-slate-300">Descripción</label>
                    <textarea
                    v-model="productoACrear.descripcion"
                    class="input-admin resize-none"
                    rows="3"
                    />
                </div>

                <div>
                    <label class="text-sm text-slate-300">Precio</label>
                    <input type="number" step="0.01" v-model.number="productoACrear.precio" class="input-admin" />
                </div>

                <div>
                    <label class="text-sm text-slate-300">Stock</label>
                    <input type="number" v-model.number="productoACrear.stock" class="input-admin" />
                </div>

                <div>
                    <label class="text-sm text-slate-300">Estado</label>
                    <select v-model="productoACrear.estado" class="input-admin">
                    <option :value="true">Activo</option>
                    <option :value="false">Inactivo</option>
                    </select>
                </div>
            </div>

            <div class="flex justify-end gap-4 pt-4 border-t border-slate-700">
                <button
                    type="button"
                    @click="handleModalCrear"
                    class="px-4 py-2 rounded-lg bg-slate-600 hover:bg-slate-500 text-slate-100"
                >
                    Cancelar
                </button>

                <button
                    type="submit"
                    class="px-4 py-2 rounded-lg bg-blue-600 hover:bg-blue-500 text-slate-100 font-semibold"
                >
                    Crear
                </button>
            </div>
            <div class="flex  flex-col justify-center items-center">
                <p class="text-slate-100 font-semibold">{{ errorCategorias }}</p>
                <p v-for="i in errorList" :key="i">{{ i }}</p>
            </div>
        </form>
    </section>
</template>

<script setup lang="ts">
    
import fetchApi from '@/api/fetchApi';
import type { CategoriaReadDTO, ProductoCreateDTO } from '@/dtos/DTOs';
import { useAuthStore } from '@/store/authStore';
import { onMounted, ref } from 'vue';

const emit = defineEmits<{ (e: 'toogleReload') : void} >();

const auth = useAuthStore();
const errorCategorias = ref("");
const errorList = ref<string[]>([]);
const categorias = ref<CategoriaReadDTO[]>([]);

const productoACrear = ref<ProductoCreateDTO>({
    nombre : "",
    categoriaId : 0,
    marca : "",
    descripcion : "",
    precio : 0.0,
    stock : 0,
    imagen : "/img/sinimagen.webp",
    estado : true,
});

const modalCrear = ref(false);

const handleModalCrear = () =>{
    modalCrear.value = ! modalCrear.value;
    productoACrear.value = {
    nombre : "",
    categoriaId : 0,
    marca : "",
    descripcion : "",
    precio : 0.0,
    stock : 0,
    imagen : "/img/sinimagen.webp",
    estado : true,
    };
};

const handleSubmitCrear = async () =>{
    verificarDatos();
    if(errorList.value.length > 0 ) return;

    const resp = await fetchApi('Producto',{
        method : "POST",
        headers : auth.getAuthHeader,
        body : JSON.stringify(productoACrear.value)
    });
    if(resp.errorMessage) errorCategorias.value = "❌ " + resp.errorMessage;
    else{
        errorCategorias.value = resp.successMessage || "";
        errorList.value = [];
        handleModalCrear();
        emit('toogleReload');
    }

};
const verificarDatos = () =>{
    errorList.value = [];

    if (productoACrear.value.categoriaId === 0) errorList.value.push("❌ Seleccione una categoría");
    if(productoACrear.value.precio < 0) errorList.value.push("❌ Precio menor a 0");
    if(productoACrear.value.stock < 0 ) errorList.value.push("❌ Stock menor a 0");
    if (!Number.isInteger(productoACrear.value.stock)) errorList.value.push("❌ Stock debe ser entero");
    if(productoACrear.value.nombre.length == 0) errorList.value.push("❌ Ingrese nombre");
    if(productoACrear.value.marca.length == 0) errorList.value.push("❌ Ingrese marca");
    if(productoACrear.value.descripcion.length == 0) errorList.value.push("❌ Ingrese marca");
    if(productoACrear.value.precio == 0) errorList.value.push("❌ Ingrese precio");
    if(productoACrear.value.stock == 0 ) errorList.value.push("❌ Ingrese stock");

};

onMounted( async () =>{

    const resp = await fetchApi<CategoriaReadDTO>('Categoria');
    if (resp.errorMessage) errorCategorias.value = "❌ "+ resp.errorMessage;
    else{
        categorias.value = resp.results || [];
        errorCategorias.value = "";
    }

});

    

</script>