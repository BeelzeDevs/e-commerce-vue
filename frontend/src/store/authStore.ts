import { defineStore } from "pinia";
import fetchApi from "../api/fetchApi";
import { type LoginDTO } from "@/dtos/DTOs";
import { RouterLink } from "vue-router";
import router from "@/router";

interface User {
  email: string;
  rol: string;
}
export const useAuthStore = defineStore("auth", {

  // accesibles desde toda la app, auth.user auth.token
  state: () => ({
    user: JSON.parse(localStorage.getItem("user") || "null") as User | null,
    token: localStorage.getItem("token") || null,
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

      this.token = resultados.token;
      this.user = user;

      localStorage.setItem("token", resultados.token);
      localStorage.setItem("user", JSON.stringify(user));
    },

    logout() {
      this.user = null;
      this.token = null;
      localStorage.removeItem("token");
      localStorage.removeItem("user");
      router.push("/");
    },
  },
});