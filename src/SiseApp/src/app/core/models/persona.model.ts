export interface Persona {
  idPersona: number;
  nombres: string;
  apellidoPaterno: string;
  apellidoMaterno: string;
  numeroDocumento: string;
  idTipoDocumento: number;
  nombreTipoDocumento: string;
  telefono: string | null;
  correoPersonal: string;
  idDireccion: number;
  idDistrito: number;
  calle: string;
  numero: string;
  pisoDepartamento: string | null;
  referencia: string | null;
  estado: string;
}
