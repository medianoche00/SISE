import { Component, Inject, Pipe } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { OfertaLaboral } from '../../core/models/oferta.model';
import { MatDivider } from '@angular/material/divider';
import { FormsModule } from '@angular/forms';
import { CommonModule, DatePipe, DecimalPipe } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';

export type ModoVista = 'POSTULAR' | 'CANCELAR_POSTULACION' | 'ADMIN';

export interface OfertaDetailData {
  oferta: OfertaLaboral;
  modo: ModoVista;
  estadoPostulacion?: string; // Opcional, para mostrar "Pendiente", "Rechazado"
}

@Component({
  selector: 'app-oferta-detail',
  templateUrl: './oferta-detail.component.html',
  styleUrls: ['./oferta-detail.component.css'],
  standalone: true,
  imports: [MatDialogModule, MatDivider, FormsModule, DatePipe, DecimalPipe, MatIconModule, CommonModule, MatButtonModule]
})
export class OfertaDetailComponent {
  constructor(
    public dialogRef: MatDialogRef<OfertaDetailComponent>,
    @Inject(MAT_DIALOG_DATA) public data: OfertaDetailData
  ) {}

  cerrar() {
    this.dialogRef.close();
  }

  // Al dar click, solo devolvemos la acción al padre, no llamamos a la API aquí
  ejecutarAccionPrincipal() {
    this.dialogRef.close(true); // true significa "El usuario confirmó la acción"
  }
}