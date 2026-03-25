import { Component, inject } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { Auth } from '../../services/auth';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule, CommonModule, RouterLink],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login {
  // Inyectar el servicio y el enrutador
  private authService = inject(Auth);
  private router = inject(Router);

  // Variables para la vista
  username = '';
  password = '';
  errorMessage = '';

  onSubmit() {
    this.errorMessage = '';

    const credentials = { username: this.username, password: this.password };

    // Se suscribe al canal del Observable
    this.authService.login(credentials).subscribe({
      // 1. Si la API devuelve 200 OK y el Token
      next: (respuesta) => {
        console.log('Login exitoso');
        this.router.navigate(['/dashboard']);
      },

      // 2. Si la API devuelve 401 Unauthorized o 500
      error: (err) => {
        console.log('Falló el login', err);
        if (err.status === 401) {
          this.errorMessage = 'Credenciales incorrectas';
        } else {
          this.errorMessage = 'Error de conexion con el servidor';
        }
      },
    });
  }
}
