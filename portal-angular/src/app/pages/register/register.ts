import { Component, inject } from '@angular/core';
import { Router, RouterLink } from '@angular/router'; // Importamos RouterLink para volver al login
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Auth } from '../../services/auth';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule, CommonModule, RouterLink],
  templateUrl: './register.html',
  styleUrls: [],
})
export class Register {
  private authService = inject(Auth);
  private router = inject(Router);

  // Variables para enlazar con el formulario
  username = '';
  password = '';

  // Variables para gestionar el estado de la UI
  errorMessage = '';
  successMessage = '';
  isLoading = false; // Para deshabilitar el botón mientras carga

  onSubmit() {
    // Limpiamos mensajes previos y activamos carga
    this.errorMessage = '';
    this.successMessage = '';
    this.isLoading = true;

    const credentials = { username: this.username, password: this.password };

    // Llamamos al método que creamos en el auth service
    this.authService.registrar(credentials).subscribe({
      // 1. Éxito (HTTP 200 OK)
      next: (respuesta) => {
        console.log('Registro exitoso', respuesta);
        this.isLoading = false;
        this.successMessage = 'Usuario creado con éxito. Redirigiendo al Login...';

        // Esperamos 2 segundos para que el usuario lea el mensaje y redirigimos
        setTimeout(() => {
          this.router.navigate(['/']);
        }, 2000);
      },

      // 2. Error (HTTP 400 Bad Request u otros)
      error: (err) => {
        console.error('Falló el registro', err);
        this.isLoading = false;

        // Si el backend nos mandó el mensaje de "usuario en uso" (el BadRequest que programamos)
        if (err.status === 400 && err.error && err.error.mensaje) {
          this.errorMessage = err.error.mensaje;
        } else {
          this.errorMessage = 'Ocurrió un error inesperado en el servidor.';
        }
      },
    });
  }
}
