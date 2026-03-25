import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, tap } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class Auth {
  // 1. Inyección de dependencias
  // Reemplaza el constructor(private http: HttpClient)
  private http = inject(HttpClient);

  private apiUrl = 'http://localhost:5011/api/Auth';

  // 2. Estado global
  // BehaviorSubject es una variable reactiva que guarda un valor y avisa cuando cambia
  private tokenSubject = new BehaviorSubject<string | null>(localStorage.getItem('token'));

  // Exponemos el token como un Observable de solo lectura
  public token$ = this.tokenSubject.asObservable();

  // 3. Peticion HTTP
  registrar(credentials: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/registrar`, credentials);
  }

  // Devuelve un Observable
  login(credentials: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/login`, credentials).pipe(
      // tap permite hacer algo con la respuesta antes de darsela al componente, solo si no hay error
      tap((response: any) => {
        const token = response.token;
        localStorage.setItem('token', token);
        this.tokenSubject.next(token); // Avisa a toda la aplicacion que se inicio sesion
      }),
    );
  }

  logout(): void {
    localStorage.removeItem('token');
    this.tokenSubject.next(null); // Destruir sesion
  }

  isAuthenticated(): boolean {
    return !!this.tokenSubject.value; // Devuelve true si hay un token
  }
}
