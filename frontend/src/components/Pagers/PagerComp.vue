<template>
    <div class="flex justify-center gap-2 mt-4 w-full">
  <button
    :disabled="page === 1"
    @click="setPage(page-1)"
    class="px-3 py-1 rounded-lg disabled:bg-slate-700 bg-blue-600 text-white"
  >
    Anterior
  </button>

  <div v-for="n in totalPages" :key="n" :class="`${page == n ? 'text-white bg-slate-600' : 'text-white'} bg-blue-600 rounded-lg py-2 px-3 cursor-pointer `"
    @click="setPage(n)"
  >
    <button>{{ n }}</button>
  </div>

  <button
    :disabled="page === totalPages"
    @click="setPage(page+1)"
    class="px-3 py-1 rounded-lg disabled:bg-slate-700 bg-blue-600 text-white"
  >
    Siguiente
  </button>
</div>

</template>

<script setup lang="ts">
import { toRefs } from 'vue';

const props = defineProps<{
    page : number,
    totalPages : number,
}>();

const {page, totalPages} = toRefs(props);

const emit = defineEmits<{
    (e: "update:page", cantidad:number) : void,
}>();

const setPage = (cantidad : number) =>{
    if(cantidad < 1 || cantidad > props.totalPages) return;
    emit("update:page",cantidad);
};



</script>