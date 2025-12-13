<template>
    
    <section class="w-full p-6 bg-bgContent border border-x-0 border-y-adminborders text-white overflow-y-auto overscroll-contain custom-scrollbar">
        <h1 class="text-2xl font-semibold mb-10 mt-5">Ordenes</h1>

        <OrderTableAdmin />
    </section>
</template>

<script setup lang="ts">
import OrderTableAdmin from '@/components/admin/OrderTableAdmin.vue';


</script>