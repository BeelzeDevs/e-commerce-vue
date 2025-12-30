<template>
  <div class="max-h-[100vh] flex bg-bgContent ">
    <AdminSidebar />
    <div class="p-6 bg-bgContent w-full text-white border border-x-0 border-y-adminborders overflow-y-auto overscroll-contain custom-scrollbar">
      <StatsMensualSales />
      <StatsOrdersByStatusChart />
      <StatsTopProducts />
      <StatsTotalSales />
    </div>
  </div>
</template>

<script setup lang="ts">
import  AdminSidebar from '@/components/admin/AdminSidebar.vue';
import StatsMensualSales from '@/components/admin/StatsMensualSales.vue';
import StatsOrdersByStatusChart from '@/components/admin/StatsOrdersByStatusChart.vue';
import  StatsTopProducts from '@/components/admin/StatsTopProducts.vue';
import  StatsTotalSales from '@/components/admin/StatsTotalSales.vue';


</script>

<style scoped>

</style>