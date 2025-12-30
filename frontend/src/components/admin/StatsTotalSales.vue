<template>
    <section class="p-6 w-full">
        <div class="bg-slate-800 rounded-xl p-6 shadow flex flex-col gap-6">
            <h3 class="text-lg font-semibold text-white font-mono">
                Ventas totales
            </h3>

            <div v-if="loading" class="loading-container">
                <div class="loading"></div>
                <div class="loading-text">Cargando métricas</div>
            </div>

            <div v-if="ventasTotales && !loading" class="grid grid-cols-1 md:grid-cols-3 gap-4" >
                <div class="bg-slate-900 rounded-lg p-4 flex flex-col gap-1">
                    <span class="text-slate-400 text-sm">Órdenes</span>
                    <span class="text-2xl font-bold text-white">
                        {{ ventasTotales.cantidadVentas }}
                    </span>
                </div>

                <div class="bg-slate-900 rounded-lg p-4 flex flex-col gap-1">
                    <span class="text-slate-400 text-sm">Total facturado</span>
                    <span class="text-2xl font-bold text-green-400">
                        ${{ ventasTotales.ventasTotales.toLocaleString() }}
                    </span>
                </div>

                <div class="bg-slate-900 rounded-lg p-4 flex flex-col gap-1">
                    <span class="text-slate-400 text-sm">Ticket promedio</span>
                    <span class="text-2xl font-bold text-blue-400">
                        ${{ ventasTotales.ticketPromedio.toLocaleString() }}
                    </span>
                </div>
            </div>
        </div>
    </section>
</template>


<script setup lang="ts">
import fetchApi from '@/api/fetchApi';
import { esResultError, type VentasTotalesDTO } from '@/dtos/DTOs';
import { useAuthStore } from '@/store/authStore';
import { onMounted, ref } from 'vue';


const ventasTotales = ref<VentasTotalesDTO>({
    cantidadVentas : 0,
    ticketPromedio : 0,
    ventasTotales : 0,
});
const loading = ref(true);
const errorFetchVentas = ref("");
const auth = useAuthStore();



const FetchVentasTotales = async ()=>{
    loading.value = true;
    const resp = await fetchApi<VentasTotalesDTO>('Stats/ventas-totales');
    if(esResultError(resp.results)) errorFetchVentas.value = resp.results.errorMessage || "❌ error fetching ventas totales";
    else{
        ventasTotales.value = resp.results as VentasTotalesDTO;
    }
    loading.value = false;
}

onMounted(async()=>{
    await FetchVentasTotales();
});

</script>