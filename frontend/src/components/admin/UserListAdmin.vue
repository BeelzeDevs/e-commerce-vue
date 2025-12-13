<template>
    <div class="w-full p-6 bg-bgContent border border-x-0 border-y-adminborders text-white overflow-y-auto overscroll-contain custom-scrollbar">
        <h2 class="text-2xl font-semibold mb-10 mt-5">Usuarios</h2>
        <UserTableAdmin />
    </div>
</template>

<script setup lang="ts">
import UserTableAdmin from '@/components/admin/UserTableAdmin.vue';



</script>