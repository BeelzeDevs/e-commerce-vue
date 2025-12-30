import { createRouter, createWebHistory } from "vue-router";
import HomeView from "@/views/Home/HomeView.vue";
import LoginView from "@/views/Auth/LoginView.vue";
import AdminDashboard from "@/views/Admin/AdminDashboard.vue";
import { useAuthStore } from "@/store/authStore";
import { storeToRefs } from "pinia";
import AdminProducts from "@/views/Admin/AdminProducts.vue";
import AdminOrders from "@/views/Admin/AdminOrders.vue";
import AdminUsers from "@/views/Admin/AdminUsers.vue";
import AdminStats from "@/views/Admin/AdminStats.vue";
import CartView from "@/views/Home/CartView.vue";
import ProductDetails from "@/views/Home/ProductDetails.vue";
import CheckoutView from "@/views/Home/CheckoutView.vue";
import RegisterView from "@/views/Auth/RegisterView.vue";
import CreateAdmin from "@/views/Admin/CreateAdmin.vue";
import AboutUs from "@/views/Home/AboutUs.vue";
import ProductsView from "@/views/Home/ProductsView.vue";

const router = createRouter({
  history: createWebHistory(),
  routes: [
    { path: "/", component: HomeView },
    { path: "/cart", component : CartView},
    { path: "/login", component: LoginView },
    { path: "/login/crear-usuario", component : RegisterView},
    // User
    { path : "/producto/:id", 
      name : "product-detail",
      component: ProductDetails
    },
    {
      path:"/checkout",
      component : CheckoutView,
      meta : {requiresAuth : true}
    },
    {
      path : "/about",
      component : AboutUs,
    },
    {
      path : "/productos",
      component : ProductsView,
    },
    // Admin
    {
      path: "/crear-admin",
      component : CreateAdmin,
      meta : {requiresAuth: true}
    },
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
  if(to.path == "/login" && auth.getUsuario) return next("/");
  if(to.path == "/login/crear-usuario" && auth.getUsuario) return next("/");
  else next();
});



export default router;
