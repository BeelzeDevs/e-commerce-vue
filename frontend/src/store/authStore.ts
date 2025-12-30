import { defineStore } from "pinia";
import fetchApi from "../api/fetchApi";
import { esResultError, type LoginDTO } from "@/dtos/DTOs";
import router from "@/router";

interface User {
  nombre : string,
  email: string;
  rol: string;
}
export const useAuthStore = defineStore("auth", {

  // accesibles desde toda la app, auth.user auth.token. Reactivos.
  state: () => ({
    user: JSON.parse(localStorage.getItem("user") || "null") as User | null,
    token: localStorage.getItem("token") || null,
    exp : localStorage.getItem("exp") || null,
  }),

  // funciones desde toda la app, app.login() app.logout()
  actions: {
    async login(email: string, password: string) {

      const data = await fetchApi<LoginDTO>("auth/login", {
        method: "POST",
        body: JSON.stringify({ email, password }),
      });
      
      if (!data.results || "token" in data.results === false) {
        throw new Error("Respuesta inesperada del servidor");
      }

      if (esResultError(data.results)) {
        throw new Error(data.results.errorMessage);
      }

      const resultados = data.results;
      
      
      const user: User = { nombre : resultados.nombre , email: resultados.email, rol: resultados.rol };
      const expiracion = resultados.expiration;


      this.token = resultados.token;
      this.user = user;
      this.exp = new Date(expiracion).toISOString();

      localStorage.setItem("token", resultados.token);
      localStorage.setItem("user", JSON.stringify(user));
      localStorage.setItem("exp", this.exp );
      
    },

    logout() {
      this.user = null;
      this.token = null;
      this.exp = null;
      localStorage.removeItem("token");
      localStorage.removeItem("user");
      localStorage.removeItem("exp");
      router.push("/");
    },
  },
  // Los getters son computed internamente, Reactivos , sino debería importarlos con computed(()=> );
  getters: {
    esAdmin(state) {
      return state.user?.rol === "Administrador";
    },
    getToken(state){
      return state.token;
    },
    getUsuario(state){
      return state.user;
    },
    getAuthHeader(state){
      return state.token ? { Authorization : `Bearer ${state.token}`} : {};
    },
    estaAuthenticado(state) {
      return !!state.token && !!state.user;
    },
    getExp(state){
      return state.exp ? new Date(state.exp) : null;
    }
  },
});