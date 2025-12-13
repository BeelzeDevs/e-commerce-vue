import { defineStore } from "pinia";
import fetchApi from "../api/fetchApi";
import { type LoginDTO } from "@/dtos/DTOs";
import router from "@/router";

interface User {
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
      
      if (data.errorMessage) {
        throw new Error(data.errorMessage);
      }

      const resultados = data.results?.[0];
      if (!resultados) {
          throw new Error("Respuesta inesperada del servidor");
      }
      
      const user: User = { email: resultados.email, rol: resultados.rol };
      const expiracion = resultados.expiration;


      this.token = resultados.token;
      this.user = user;
      this.exp = expiracion.toString();

      localStorage.setItem("token", resultados.token);
      localStorage.setItem("user", JSON.stringify(user));
      localStorage.setItem("exp", expiracion.toString() );
      
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
      return { Authorization : `Bearer ${state.token}`};
    },
    getExp(state){
      return state.exp ? new Date(state.exp) : null;
    }
  },
});