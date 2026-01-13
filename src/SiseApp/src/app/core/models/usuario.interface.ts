export interface Usuario {
  id: number;
  nombreUsuario: string;
  rol: string;
  personaId: number;
  nombreCompleto?: string;
  activo: string;
  dni?: string;
  nombres?: string;
  apellidoPaterno?: string;
  apellidoMaterno?: string;
  email?: string;
}
