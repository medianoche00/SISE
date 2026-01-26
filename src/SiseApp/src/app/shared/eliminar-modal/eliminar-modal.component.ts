import { Component, Inject, NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';

@Component({
  selector: 'app-eliminar-modal',
  standalone: true,
  imports: [MatDialogModule, MatButtonModule, MatFormFieldModule, FormsModule],
  templateUrl: './eliminar-modal.component.html',
  styleUrl: './eliminar-modal.component.css',
})
export class EliminarModalComponent {
  documentoRespaldo: string = '';

  constructor(
    public dialogRef: MatDialogRef<EliminarModalComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { mensaje: string },
  ) {}

  onNoClick(): void {
    this.dialogRef.close();
  }
}
