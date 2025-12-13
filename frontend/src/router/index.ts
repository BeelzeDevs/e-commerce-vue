import { createRouter, createWebHistory } from "vue-router";
import HomeView from "../views/Home/HomeView.vue";
import LoginView from "../views/Auth/LoginView.vue";
import AdminDashboard from "../views/Admin/AdminDashboard.vue";
import { useAuthStore } from "../store/authStore";
import { storeToRefs } from "pinia";
import AdminProducts from "@/views/Admin/AdminProducts.vue";
import AdminOrders from "@/views/Admin/AdminOrders.vue";
import AdminUsers from "@/views/Admin/AdminUsers.vue";
import AdminStats from "@/views/Admin/AdminStats.vue";

const router = createRouter({
  history: createWebHistory(),
  routes: [
    { path: "/", component: HomeView },
    { path: "/login", component: LoginView },
    { 
      path: "/admin", 
      component: AdminDashboard, 
      meta: { requiresAdmin: true } 
    },
    {
      path : "/admin/productos",
      component : AdminProducts,
      meta: {requiresAdmin : true }
    },
    {
      path : "/admin/ordenes",
      component : AdminOrders,
      meta : {requiresAdmin : true }
    },
    {
      path : "/admin/usuarios",
      component : AdminUsers,
      meta : {requiresAdmin : true}
    },
    {
      path : "/admin/stats",
      component : AdminStats,
      meta : {requiresAdmin : true}
    }

  ],
});

router.beforeEach((to, _from, next) => {  
  const auth = useAuthStore();
  const {esAdmin, getExp} = storeToRefs(auth);
  const exp = getExp.value;

  if(exp && exp < new Date() ){
    auth.logout();
    return next("/");
  }

  if (to.meta.requiresAdmin && !esAdmin.value && !exp) return next("/");
  if(to.path == "/" && esAdmin.value) return next("/admin");
  else next();
});

export default router;
