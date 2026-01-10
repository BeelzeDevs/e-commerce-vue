<template>
  
    <section class="p-6 w-full overflow-y-auto overscroll-contain custom-scrollbar">
        <div class="bg-slate-800 rounded-xl p-6 shadow h-[350px]">
            <h3 class="text-lg font-semibold mb-4 text-white font-mono">
              Ventas por mes
            </h3>
            
            <LoaderOne :loading="loading" />
    
            <Line
            v-if="!loading"
            :data="chartData"
            :options="chartOptions"
            />
            
        </div>

    </section>
</template>

<script setup lang="ts">
import fetchApi from '@/api/fetchApi';
import { esResultError, type VentasPorMesDTO } from '@/dtos/DTOs';
import { useAuthStore } from '@/store/authStore';
import { computed, onMounted, ref } from 'vue';
import type { ChartOptions } from "chart.js";
import LoaderOne from '@/components/loaderOne.vue';

const auth = useAuthStore();
const errorFetch = ref("");
const loading = ref(true);
const ventasMensuales = ref<VentasPorMesDTO[]>([]);


const fetchVentasMensuales = async () =>{
    const resp = await fetchApi<VentasPorMesDTO>("Stats/ventas-mensuales");
    if(esResultError(resp.results)) errorFetch.value = resp.results.errorMessage || "Error al hacer fetch de Ventas Mensuales";
    else{
        ventasMensuales.value = resp.results as VentasPorMesDTO[];
        loading.value = false;
    }
}

onMounted(async () => {
    await fetchVentasMensuales(); 
});

// Gráfico
import { Line } from "vue-chartjs";
import {
  Chart as ChartJS,
  CategoryScale,
  LinearScale,
  PointElement,
  LineElement,
  Tooltip,
  Legend
} from "chart.js";

ChartJS.register(
  CategoryScale,
  LinearScale,
  PointElement,
  LineElement,
  Tooltip,
  Legend
);

const labels = computed(() =>
  ventasMensuales.value.map(v =>
    `${String(v.mes)}/${v.año}`
  )
);
const chartData = computed(() => ({
  labels: labels.value,
  datasets: [
    {
      label: "Ventas ($)",
      data: ventasMensuales.value.map(v => v.totalVentas),
      borderColor: "oklch(62.3% 0.214 259.815)",  // blue-500 tailwind
      backgroundColor: "oklch(92.9% 0.013 255.508)",
      tension: 0.3,
      fill: true,
      yAxisID: "yVentas"
    },
    {
      label: "Cantidad de órdenes",
      data: ventasMensuales.value.map(v => v.cantidadOrdenes),
      borderColor: "oklch(69% 0.17 146)", // verde
      backgroundColor: "oklch(92.9% 0.013 255.508)",
      tension: 0.3,
      fill: false,
      yAxisID: "yOrdenes"
    }
  ]
}));
const chartOptions : ChartOptions<"line"> = {
  responsive: true,
  maintainAspectRatio: false,
  plugins: {
    legend: {
      labels: {
        color: "#e5e7eb"
      }
    },
    tooltip: {
      callbacks: {
        label: (ctx: any) => {
          if (ctx.dataset.label === "Ventas ($)") {
            return `Ventas: $${ctx.raw.toLocaleString("es-AR")}`;
          }
          return `Órdenes: ${ctx.raw}`;
        }
      }
    }
  },
  scales: {
    x: {
      ticks: { color: "#93c5fd" },
      grid: { color: "rgba(255,255,255,0.05)" }
    },
    yVentas: {                                // total en eje Y 
      type: "linear" ,
      position: "left",
      ticks: {
        color: "oklch(62.3% 0.214 259.815)", // blue 500-tailwind - total ventas
        callback: (v) => `$${Number(v).toLocaleString("es-AR")}`
      },
      grid: { color: "rgba(255,255,255,0.05)" }
    },
    yOrdenes: {                               // Cantidad ordenes en eje Y
      type: "linear" ,
      position: "left",
      ticks: { color: "oklch(69% 0.17 146)" }, // green - cantidad ordenes
      grid: { drawOnChartArea: false }
    }
  }
};

</script>