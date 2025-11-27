import { createRouter, createWebHistory } from "vue-router";
import HomeView from "../views/Home/HomeView.vue";
import LoginView from "../views/Auth/LoginView.vue";
import AdminDashboard from "../views/Admin/AdminDashboard.vue";
import { useAuthStore } from "../store/authStore";

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
  ],
});

router.beforeEach((to, _from, next) => {
  const auth = useAuthStore();
  if (to.meta.requiresAdmin && auth.user?.rol !== "Administrador") next("/");
  if(to.path == "/" && auth.user?.rol == "Administrador") next("/admin");
  else next();
});

export default router;
