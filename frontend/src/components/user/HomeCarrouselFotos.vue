<template>
  <div class="relative w-full h-[60vh] overflow-hidden rounded-md ">
    
    <img
        :src="fondo"
        class="absolute inset-0 w-full h-full object-cover"
        alt="fondo"
    />

    <div
      class="flex h-full transition-transform duration-700 ease-in-out"
      :style="{ transform: `translateX(-${current * 100}vw)` }"
    >
      <div
        v-for="(img, i) in imagenes"
        :key="i"
        class="relative min-w-[100vw] h-[60vh] flex items-center justify-center overflow-hidden  "
      >
        

        <div class="absolute inset-0 bg-slate-600/60"></div>

       
        <img
          :src="img"
          class="relative z-10 w-[80vw] max-w-[1200px] h-full object-contain px-10 opacity-90 rounded-[90px] p-4"
          alt="banner"
        />
      </div>
    </div>

   

    
    <button
      @click="prev"
      class="absolute left-5 top-1/2 -translate-y-1/2 z-30 bg-black/50 hover:bg-black/70 rounded-full p-3"
    >
      <svg
        class="w-10 h-10 text-white"
        xmlns="http://www.w3.org/2000/svg"
        viewBox="0 0 24 24"
        fill="none"
        stroke="currentColor"
        stroke-width="2"
        stroke-linecap="round"
        stroke-linejoin="round"
      >
        <path d="M13.971 4.285A2 2 0 0 1 17 6v12a2 2 0 0 1-3.029 1.715l-9.997-5.998a2 2 0 0 1-.003-3.432z"/>
        <path d="M21 20V4"/>
      </svg>
    </button>


    <button
      @click="next"
      class="absolute right-5 top-1/2 -translate-y-1/2 z-30 bg-black/50 hover:bg-black/70 rounded-full p-3"
    >
      <svg
        class="w-10 h-10 text-white"
        xmlns="http://www.w3.org/2000/svg"
        viewBox="0 0 24 24"
        fill="none"
        stroke="currentColor"
        stroke-width="2"
        stroke-linecap="round"
        stroke-linejoin="round"
      >
        <path d="M10.029 4.285A2 2 0 0 0 7 6v12a2 2 0 0 0 3.029 1.715l9.997-5.998a2 2 0 0 0 .003-3.432z"/>
        <path d="M3 4v16"/>
      </svg>
    </button>

   
    <div class="absolute bottom-5 w-full flex justify-center gap-2 z-30">
      <button
        v-for="(_, i) in imagenes"
        :key="i"
        @click="current = i"
        class="w-3 h-3 rounded-full transition"
        :class="current === i ? 'bg-blue-500' : 'bg-blue-500/50'"
      />
    </div>

  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, onUnmounted } from 'vue'

const props = defineProps<{
  imagenes: string[]
  fondo: string
}>()

const current = ref(0)
let interval: number

const next = () => {
  current.value = (current.value + 1) % props.imagenes.length
}

const prev = () => {
  current.value = current.value === 0 ? props.imagenes.length - 1 : current.value - 1
}

onMounted(() => {
  interval = window.setInterval(next, 5000)
})

onUnmounted(() => {
  clearInterval(interval)
})
</script>