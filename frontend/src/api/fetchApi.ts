
import type {ApiResponse} from '@/dtos/DTOs';
const BASE_URL = "http://localhost:5554/api/";


async function fetchApi<T>(endpoint: string, options: RequestInit = {}) : Promise<ApiResponse<T>> {

  const token = localStorage.getItem("token");

  const headers: HeadersInit = {
    "Content-Type": "application/json",
    ...(token ? { Authorization: `Bearer ${token}` } : {}),
    ...options.headers,
  };

  try {
    const res = await fetch(`${BASE_URL}${endpoint}`, {
      ...options,
      headers,
    });

    const contentType = res.headers.get('content-type');
    const json = contentType?.includes('application/json') ? await res.json() : null;

    return json as ApiResponse<T>;


  } catch (err: any) {
    // Error de red | servidor caído | JSON inválido.
    return {
      results: {
      errorMessage: err?.message || "Error de conexión",
      }
    };
  }

}

export default fetchApi;

// RequestInit
// fetch(URL,{
//     method: "GET",
//     headers: {Authorization: `Bearer Key`},
//     body: "...",
//     mode: "cors",
//     cache: "no-cache",
//     credentials:"include",
// })

