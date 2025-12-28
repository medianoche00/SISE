import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ExperienciaDialogComponent } from '../experiencia-dialog/experiencia-dialog.component';
import { ExperienciaLaboral, ExperienciasService } from '../../../../core/services/experiencias.service';

@Component({
  selector: 'app-experiencia-laboral',
  templateUrl: './experiencia-laboral.component.html',
  styleUrls: ['./experiencia-laboral.component.css']
})
export class ExperienciaLaboralComponent implements OnInit {
  
  experiencias: ExperienciaLaboral[] = [];
  cargando: boolean = true;

  constructor(
    private experienciaService: ExperienciasService,
    private dialog: MatDialog
  ) {}

  ngOnInit(): void {
    this.cargarExperiencias();
  }

  cargarExperiencias() {
    this.cargando = true;
    this.experienciaService.obtenerExperiencias().subscribe({
      next: (data) => {
        this.experiencias = data;
        this.cargando = false;
      },
      error: () => this.cargando = false
    });
  }

  abrirModal(experiencia?: ExperienciaLaboral) {
    const dialogRef = this.dialog.open(ExperienciaDialogComponent, {
      width: '600px',
      data: experiencia || null // Si es null, el modal sabe que es "Crear"
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        if (experiencia) {
          // MODO EDICIÓN
          this.experienciaService.editarExperiencia(experiencia.idExperiencia!, result).subscribe(() => {
            this.cargarExperiencias(); // Recargar lista
          });
        } else {
          // MODO CREACIÓN
          this.experienciaService.crearExperiencia(result).subscribe(() => {
            this.cargarExperiencias(); // Recargar lista
          });
        }
      }
    });
  }

  eliminar(experiencia: ExperienciaLaboral) {
    if (confirm(`¿Estás seguro de eliminar tu experiencia en ${experiencia.empresa}?`)) {
      this.experienciaService.eliminarExperiencia(experiencia.idExperiencia!).subscribe(() => {
        this.cargarExperiencias();
      });
    }
  }
}