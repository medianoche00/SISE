import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { FormacionDialogComponent } from '../formacion-dialog/formacion-dialog.component'; // Importa tu modal
import { EgresadoService } from '../../../../core/services/egresado.service';

@Component({
  selector: 'app-formacion-complementaria',
  templateUrl: './formacion-complementaria.component.html',
  styleUrls: ['./formacion-complementaria.component.css']
})
export class FormacionComplementariaComponent implements OnInit {

  listaFormacion: any[] = [];
  cargando = false;

  constructor(
    private dialog: MatDialog,
    private egresadoService: EgresadoService
  ) { }

  ngOnInit(): void {
    // Aquí cargarías los datos reales:
    // this.egresadoService.obtenerPerfil().subscribe(...)
  }

  agregarNueva() {
    const dialogRef = this.dialog.open(FormacionDialogComponent, {
      width: '600px',
      disableClose: true,
      data: null
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.listaFormacion.push(result);
        this.guardarCambios();
      }
    });
  }

  editar(index: number, item: any) {
    const dialogRef = this.dialog.open(FormacionDialogComponent, {
      width: '600px',
      disableClose: true,
      data: item
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.listaFormacion[index] = result;
        this.guardarCambios();
      }
    });
  }

  eliminar(index: number) {
    if (confirm('¿Deseas eliminar este registro de formación?')) {
      this.listaFormacion.splice(index, 1);
      this.guardarCambios();
    }
  }

  guardarCambios() {
    this.cargando = true;
    const payload = {
      // Asegúrate que tu DTO en Backend se llame igual
      formacionComplementaria: this.listaFormacion
    };

    this.egresadoService.actualizarPerfil(payload).subscribe({
      next: () => {
        this.cargando = false;
      },
      error: (err) => {
        this.cargando = false;
        alert('Error al guardar formación');
      }
    });
  }
}
