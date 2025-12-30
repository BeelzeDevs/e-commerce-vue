<template>
  <NavBar />
  <div class="min-h-[90dvh] bg-bgContent">

    <HomeCarrouselFotos :imagenes = "[
      '/img-carrousel/carrousel1.webp',
      '/img-carrousel/carrousel2.webp',
      '/img-carrousel/carrousel3.webp',
      '/img-carrousel/carrousel4.webp',
      '/img-carrousel/carrousel5.webp',
      '/img-carrousel/carrousel6.webp',
    ]" 
    :fondo = "'/img-carrousel/photogrid.webp'"
     /> 

    <HomeCarrouselProd />

    <HomeServ />

  </div>

</template>

<script setup lang="ts">
import NavBar from '@/components/Nav-bar.vue';
import HomeCarrouselFotos from '@/components/user/HomeCarrouselFotos.vue';
import HomeCarrouselProd from '@/components/user/HomeCarrouselProd.vue';
import HomeServ from '@/components/user/HomeServ.vue';

</script>