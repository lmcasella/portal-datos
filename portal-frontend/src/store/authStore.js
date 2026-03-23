import { create } from "zustand";
import { persist } from "zustand/middleware";

export const useAuthStore = create(
    persist(
        (set) => ({
            // Estado inicial
            token: null,
            isAuthenticated: false,

            // Acciones para modificar el estado
            setLogin: (token) => set({ token: token, isAuthenticated: true }),
            setLogout: () => set({ token: null, isAuthenticated: false }),
        }),
        {
            name: "auth-storage", // Nombre con el que se guarda en localStorage
        },
    ),
);
