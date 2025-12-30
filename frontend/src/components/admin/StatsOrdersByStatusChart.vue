<template>
     <section class="p-6 w-full overflow-y-auto overscroll-contain custom-scrollbar">
         <div class="bg-slate-800 rounded-xl p-6 shadow h-[350px]">
             <h3 class="text-lg font-semibold text-white font-mono">
                 Órdenes por estado
             </h3>
     
             <div v-if="loading" class="loading-container">
                 <div  class="loading"></div>
                 <div class="loading-text">Cargando gráfico</div>
             </div>
     
             <Doughnut
             v-else
             :data="chartData"
             :options="chartOptions"
             />
         </div>
     </section>

</template>

<script setup lang="ts">

import fetchApi from '@/api/fetchApi';
import { esResultError, type OrdenesPorEstadoDTO } from '@/dtos/DTOs';
import { useAuthStore } from '@/store/authStore';
import { onMounted, ref, computed } from 'vue';

const auth = useAuthStore();
const errorFetch = ref("");
const loading = ref(true);
const ordenesPorEstado = ref<OrdenesPorEstadoDTO[]>([]);


const fetchOrdenesporEstado = async () =>{
    const resp = await fetchApi<OrdenesPorEstadoDTO>("Stats/ordenes-por-estado");
    if(esResultError(resp.results)) errorFetch.value = resp.results.errorMessage || "Error al hacer fetch de ordenes por estado";
    else{
        ordenesPorEstado.value = resp.results as OrdenesPorEstadoDTO[];
        loading.value = false;
    }
}

onMounted(async () => {
    await fetchOrdenesporEstado(); 
});

// Gráfico Doughnut
import { Doughnut } from "vue-chartjs";
import {
  Chart as ChartJS,
  ArcElement,
  Tooltip,
  Legend
} from "chart.js";


ChartJS.register(ArcElement, Tooltip, Legend);

const estadoColores: Record<string, string> = {
  Pendiente: "oklch(87.9% 0.169 91.605)",   // yellow-300
  Cancelado: "oklch(57.7% 0.245 27.325)",   // red-600
  Pagado: "#22c55e",                        // verde 
  Enviado: "oklch(54.6% 0.245 262.881)",      // blue-700
};

const chartData = computed(() => ({
  labels: ordenesPorEstado.value.map(ope => ope.estado),
  datasets: [
    {
      data:  ordenesPorEstado.value.map(ope => ope.cantidad),
      backgroundColor: ordenesPorEstado.value.map(
        ope => estadoColores[ope.estado] ?? "#64748b"
      ),
      borderWidth: 1
    }
  ]
}));

const chartOptions = {
  responsive: true,
  maintainAspectRatio: false,
  cutout: "65%", // donut
  plugins: {
    legend: {
      position: "bottom" as const,
      labels: {
        color: "#e5e7eb",
        padding: 20
      },
    },
    tooltip: {
      callbacks: {
        label: (ctx : any ) =>
          `${ctx.label}: ${ctx.raw} órdenes`
      }
    }
  }
};

</script>