import { Component, ViewChild, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatDialog } from '@angular/material/dialog';
import { Persona } from '../../../core/models/persona.interface';
import { PersonasDetailComponent } from '../../../shared/persona-detail/persona-detail.component';

@Component({
  selector: 'app-personas-list',
  templateUrl: './personas-list.component.html',
  styleUrls: ['./personas-list.component.scss'],
})
export class PersonasListComponent implements OnInit {
  displayedColumns: string[] = [
    'nombre',
    'apellidoPaterno',
    'apellidoMaterno',
    'tipoDocumento',
    'numeroDocumento',
    'acciones',
  ];
  dataSource = new MatTableDataSource<Persona>([]);

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  // Datos dummy
  datosIniciales: Persona[] = [
    {
      idPersona: 1,
      nombres: 'Juan',
      apellidoPaterno: 'Perez',
      apellidoMaterno: 'Lopez',
      tipoDocumento: 'DNI',
      numeroDocumento: '12345678',
      correoPersonal: 'juan@mail.com',
      telefono: '999888777',
      estado: 'Buscando Trabajo',
    },
    {
      idPersona: 2,
      nombres: 'Ana',
      apellidoPaterno: 'Gomez',
      apellidoMaterno: 'Diaz',
      tipoDocumento: 'Pasaporte',
      numeroDocumento: 'A1234567',
      correoPersonal: 'ana@mail.com',
      telefono: '999111222',
      estado: 'Buscando Trabajo',
    },
  ];

  constructor(private dialog: MatDialog) {}

  ngOnInit(): void {
    this.dataSource.data = this.datosIniciales;
  }

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
  }

  aplicarFiltro(event: Event) {
    const valor = (event.target as HTMLInputElement).value;
    this.dataSource.filter = valor.trim().toLowerCase();
  }

  abrirModal(accion: 'crear' | 'editar' | 'ver', persona?: Persona) {
    const dialogRef = this.dialog.open(PersonasDetailComponent, {
      width: '800px',
      disableClose: true, // El usuario debe dar click en cerrar
      data: {
        accion: accion,
        persona: persona ? { ...persona } : null,
      },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        if (accion === 'crear') {
          result.id = new Date().getTime(); // ID Simulado
          this.dataSource.data = [...this.dataSource.data, result];
        } else if (accion === 'editar') {
          const index = this.dataSource.data.findIndex(
            (p) => p.idPersona === result.idPersona,
          );
          const dataActualizada = [...this.dataSource.data];
          dataActualizada[index] = result;
          this.dataSource.data = dataActualizada;
        }
      }
    });
  }

  eliminar(id: number) {
    if (confirm('¿Estás seguro de eliminar este registro?')) {
      this.dataSource.data = this.dataSource.data.filter((p) => p.idPersona !== id);
    }
  }
}
