import axios from "axios";
import { useAuthStore } from "../store/authStore";

// Creamos una instancia base de Axios
const api = axios.create({
    baseURL: "http://localhost:5011/api",
});

// El Interceptor: Antes de que cualquier petición salga de React hacia .NET...
api.interceptors.request.use(
    (config) => {
        // Sacar el token del store de Zustand
        const token = useAuthStore.getState().token;

        // Si hay un token se inyecta en el Header de la peticion
        if (token) {
            config.headers.Authorization = `Bearer ${token}`;
        }

        return config;
    },
    (error) => {
        return Promise.reject(error);
    },
);

export default api;
