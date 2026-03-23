# Portal Datos - Sistema de Gestión de Boletas Digitales

Aplicación Full-Stack diseñada para el procesamiento, gestión y visualización de datos de contribuyentes y pagos municipales. Este proyecto demuestra la implementación de buenas prácticas de desarrollo, arquitecturas escalables y seguridad moderna.

## 🚀 Arquitectura y Patrones de Diseño

El backend fue construido bajo los lineamientos de **Clean Architecture**, separando estrictamente las responsabilidades en capas (Domain, Application, Infrastructure, API).

- **Principios SOLID:** Aplicados en todo el ciclo de vida del código.
- **Patrón Repositorio:** Abstracción de la capa de acceso a datos para facilitar el testing y el mantenimiento.
- **Inyección de Dependencias (DI):** Gestión centralizada de servicios en .NET.
- **Stateless Authentication:** Uso de JWT para mantener una API RESTful pura y escalable.

## 🛠️ Stack Tecnológico

**Backend:**

- **.NET 9** (Web API & Worker Service)
- **C#**
- **Dapper:** Micro-ORM utilizado para maximizar el rendimiento en consultas SQL complejas (JOINs).
- **SQL Server:** Motor de base de datos relacional.
- **BCrypt.Net:** Hashing criptográfico robusto para contraseñas.
- **JWT (JSON Web Tokens):** Seguridad y autorización de endpoints.

**Frontend:**

- **React 18+** (Empaquetado con Vite).
- **Zustand:** Manejo de estado global ligero y directo para la sesión del usuario.
- **Axios:** Cliente HTTP configurado con Interceptors para la inyección automática de tokens Bearer.
- **React Router DOM:** Enrutamiento y protección de rutas privadas (Guards).

## ⚙️ Características Principales

1.  **Background Worker:** Servicio en segundo plano encargado de procesar archivos de texto (simulando lotes de pagos de boletas) e insertarlos en la base de datos de forma asíncrona.
2.  **Seguridad y Autenticación:** Sistema de Login con contraseñas encriptadas. Generación y validación de tokens JWT para proteger el acceso a los datos sensibles.
3.  **Consultas Relacionales Optimizadas:** Cruce de datos (JOIN) entre pagos registrados y padrón de contribuyentes, expuesto a través de un endpoint protegido.
4.  **Frontend Reactivo:** Interfaz SPA (Single Page Application) que consume la API de forma segura, con manejo de estado persistente (LocalStorage).

## 🏃‍♂️ Cómo ejecutar el proyecto localmente

### 1. Base de Datos

Ejecutar los scripts SQL proporcionados en la carpeta `/database` para crear las tablas `Usuarios`, `Contribuyentes` y `PagosBoletaDigital`.

### 2. Backend (API & Worker)

Navegar a la carpeta del proyecto API y configurar el `appsettings.json` con la cadena de conexión local y la clave JWT.

```bash
cd PortalDatos.Api
dotnet build
dotnet run
```

### 3. Frontend (React)

En una terminal separada, navegar a la carpeta del frontend, instalar las dependencias y levantar el servidor de desarrollo.

```bash
cd portal-frontend
npm install
npm run dev
```

### 4. Simulación del Worker (Procesamiento de Lotes)

El sistema incluye un `Background Worker` independiente que monitorea y procesa archivos de pagos en segundo plano. Para que el procesamiento de los lotes funcione, **es obligatorio mantener este servicio en ejecución**.

**Pasos para probarlo localmente:**

1. Asegurate de crear la siguiente estructura de carpetas en tu disco C:
    - `C:\BoletasDigitales\Input` (Directorio de lectura)
    - `C:\BoletasDigitales\Processed` (Archivos procesados con éxito)
    - `C:\BoletasDigitales\Failed` (Archivos con errores de formato)

2. Abrí una nueva terminal en la raíz de la solución, navegá al proyecto del Worker y ejecutalo:

    ```bash
    cd PortalDatos.Worker
    dotnet build
    dotnet run
    ```

3. Mientras el Worker está escuchando, andá a la carpeta Input y creá un archivo de texto (ej. lote_pagos.txt). El archivo debe contener la siguiente cabecera y estructura de datos:

    ```bash
    NumeroBoleta, MontoCobrado, FechaCobro
    1, 13000.00, 2/3/2026
    12345, 5000.50, 15/3/2026
    ```

4. El Worker detectará el archivo rapidamente. Procesará cada línea insertándola en la tabla PagosBoletaDigital de SQL Server y, al finalizar, moverá el archivo a la carpeta Processed (o a Failed si hay un error de parseo).

5. Desde el Dashboard en React, actualizá la página para ver los nuevos pagos reflejados y cruzados con el padrón de forma inmediata.
