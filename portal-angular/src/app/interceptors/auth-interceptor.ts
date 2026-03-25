import { HttpInterceptorFn } from '@angular/common/http';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  // 1. Buscamos el token en el LocalStorage
  const token = localStorage.getItem('token');

  // 2. Si existe un token, clonamos la petición original y le inyectamos el Header
  if (token) {
    const clonedRequest = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`,
      },
    });

    // Dejamos que la petición modificada siga su viaje hacia la API
    return next(clonedRequest);
  }

  // 3. Si no hay token (ej. en el login), la petición sigue intacta
  return next(req);
};
