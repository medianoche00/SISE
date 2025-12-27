import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog'; 
import { ExperienciaDialogComponent } from '../experiencia-dialog/experiencia-dialog.component'; 
import { EgresadoService } from '../../../../core/services/egresado.service';

@Component({
  selector: 'app-experiencia-laboral',
  templateUrl: './experiencia-laboral.component.html',
  styleUrls: ['./experiencia-laboral.component.css']
})
export class ExperienciaLaboralComponent implements OnInit {

  listaExperiencias: any[] = [];
  cargando: boolean = false;

  constructor(
    private dialog: MatDialog,
    private egresadoService: EgresadoService
  ) { }

  ngOnInit(): void {
    // TODO: Aquí deberías llamar al servicio para obtener las experiencias guardadas en BD
  }

  agregarNueva() {
    const dialogRef = this.dialog.open(ExperienciaDialogComponent, {
      width: '700px', 
      disableClose: true, 
      data: null 
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.listaExperiencias.push(result);
        this.guardarCambiosEnBackend();
      }
    });
  }

  editar(index: number, item: any) {
    const dialogRef = this.dialog.open(ExperienciaDialogComponent, {
      width: '700px',
      disableClose: true,
      data: item 
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.listaExperiencias[index] = result;
        this.guardarCambiosEnBackend();
      }
    });
  }

  eliminar(index: number) {
    if (confirm('¿Estás seguro de que deseas eliminar este registro de experiencia laboral?')) {
      this.listaExperiencias.splice(index, 1);
      this.guardarCambiosEnBackend();
    }
  }

  guardarCambiosEnBackend() {
    this.cargando = true;

    const payload = {
      experienciaLaboral: this.listaExperiencias
    };

    this.egresadoService.actualizarPerfil(payload).subscribe({
      next: () => {
        this.cargando = false;
        console.log('Sincronización exitosa');
      },
      error: (err) => {
        this.cargando = false;
        console.error(err);
        alert('Error al guardar los cambios en el servidor.');
      }
    });
  }
}
