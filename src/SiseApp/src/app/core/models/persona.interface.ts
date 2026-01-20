export interface Persona {
  idPersona: number;
  nombres: string;
  apellidoPaterno: string;
  apellidoMaterno: string;
  tipoDocumento: string;
  numeroDocumento: string;
  telefono?: string;
  correoPersonal?: string;
  departamento?: string;
  provincia?: string;
  distrito?: string;
  direccionEspecifica?: string;
  estado: string;
  rol?: string;
}
