import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit } from '@angular/core';
import { Auth } from '../../services/auth';
import { Router } from '@angular/router';
import { CommonModule, DatePipe } from '@angular/common';

@Component({
  selector: 'app-dashboard',
  imports: [DatePipe, CommonModule],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css',
})
export class Dashboard implements OnInit {
  private http = inject(HttpClient);
  private authService = inject(Auth);
  private router = inject(Router);

  pagos: any[] = [];
  errorMessage = '';

  ngOnInit(): void {
    this.cargarDatos();
  }

  cargarDatos() {
    this.http.get<any[]>('http://localhost:5011/api/Reportes/pagos').subscribe({
      next: (data) => {
        this.pagos = data;
        // console.log(this.pagos);
      },
      error: (err) => {
        console.error(err);
        this.errorMessage = 'Error al cargar los datos. ¿Expiró el token?';
      },
    });
  }

  cerrarSesion() {
    this.authService.logout();
    this.router.navigate(['/']);
  }
}
