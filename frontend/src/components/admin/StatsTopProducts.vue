<template>
    <section class="p-6 w-full overflow-y-auto">
        <div class="bg-slate-800 rounded-xl p-6 shadow">
            <div class="flex justify-between items-center mb-4">
                <h3 class="text-lg font-semibold text-white font-mono">
                Top {{ cantidadProd }} productos vendidos
                </h3>
                <div class="inline-block relative">
                    <select
                    v-model="cantidadProd"
                    class="bg-slate-700 px-4 py-1 pe-8 rounded text-white outline-none border-none ring-2 focus:ring-blue-600 cursor-pointer appearance-none shadow-xs"
                    >
                    
                    <option :value="3">Top 3</option>
                    <option :value="5">Top 5</option>
                    <option :value="10">Top 10</option>
                    <option :value="20">Top 20</option>
                    </select>
                    <svg class="cursor-pointer absolute m-auto end-1 top-0 bottom-0 w-6 h-6 text-blue-600 lucide lucide-chevron-down-icon lucide-chevron-down" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" ><path d="m6 9 6 6 6-6"/></svg>
                </div>
            </div>

            <div v-if="loading" class="loading-container">
                <div class="loading"></div>
                <div class="loading-text">Cargando productos</div>
            </div>

            <table
                v-else
                class="min-w-full divide-y divide-slate-700 text-sm"
            >
                <thead class="bg-slate-900 text-slate-200">
                    <tr>
                        <th class="px-4 py-2 text-left">#</th>
                        <th class="px-4 py-2 text-left">Producto</th>
                        <th class="px-4 py-2 text-center">Vendidos</th>
                        <th class="px-4 py-2 text-right">Total facturado</th>
                    </tr>
                </thead>

                <tbody class="divide-y divide-slate-800">
                    <tr
                        v-for="(p, index) in topProd"
                        :key="p.productoId"
                        class="hover:bg-slate-700/50 transition"
                    >
                        <td class="px-4 py-2 font-semibold">
                        {{ index + 1 }}
                        </td>

                        <td class="px-4 py-2 text-white">
                        {{ p.nombre }}
                        </td>

                        <td class="px-4 py-2 text-center text-blue-400 font-semibold">
                        {{ p.cantidadVendidad }}
                        </td>

                        <td class="px-4 py-2 text-right font-semibold text-green-400">
                        ${{ p.totalFacturado.toLocaleString() }}
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </section>
</template>


<script setup lang="ts">
import fetchApi from '@/api/fetchApi';
import { esResultError, type TopProductoDTO } from '@/dtos/DTOs';
import { useAuthStore } from '@/store/authStore';
import { onMounted, ref, watch } from 'vue';

const auth = useAuthStore();
const cantidadProd = ref(3);
const errorFetch = ref("");
const topProd = ref<TopProductoDTO[]>([]);
const loading = ref(true);

const FetchTopProd = async ()=>{
    const resp = await fetchApi<TopProductoDTO>(`stats/top-productos?top=${cantidadProd.value}`);
    if(esResultError(resp.results)) errorFetch.value = resp.results.errorMessage || "❌ error fetching top";
    else{
        topProd.value = resp.results as TopProductoDTO[];
        loading.value = false;
    }
};

onMounted(async () => {
    await FetchTopProd();
});

watch(cantidadProd, async ()=>{
    FetchTopProd();
});

</script>