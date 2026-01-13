import { Component, OnInit } from '@angular/core';
// Usamos rutas relativas (../../..) para llegar a la carpeta services y core
import { PersonaService } from '../../../core/services/persona.service';
import { Persona } from '../../../core/models/persona.interface';

@Component({
  selector: 'app-personas-list',
  templateUrl: './personas-list.component.html',
  styleUrls: ['./personas-list.component.css']
})
export class PersonasListComponent implements OnInit {
  personas: Persona[] = [];

  constructor(private personaService: PersonaService) { }

  ngOnInit(): void {
    this.cargarPersonas();
  }

  cargarPersonas() {
    this.personaService.getAll().subscribe({
      next: (data: Persona[]) => {
        this.personas = data;
      },
      error: (err: any) => {
        console.error('Error al cargar personas', err);
      }
    });
  }

  eliminarPersona(id: number) {
    if (confirm('¿Estás seguro de eliminar esta persona?')) {
      this.personaService.delete(id).subscribe(() => {
        this.cargarPersonas();
      });
    }
  }
}
