// DTO para Listar (Lo que devuelve el GET)
export interface Persona {
  idPersona: number;
  nombres: string;
  apellidoPaterno: string;
  apellidoMaterno: string;

  // Datos Documento
  idTipoDocumento: number;
  nombreTipoDocumento: string; // Solo lectura
  numeroDocumento: string;

  // Contacto
  telefono: string | null;
  correoPersonal: string | null;

  // Dirección (Lectura)
  idDireccion: number;
  idDistrito: number;
  nombreDistrito?: string; // Opcional si el backend lo envía
  calle: string;
  numero: string | null;
  pisoDepartamento: string | null;
  referencia: string | null;

  estado: string;
}

// DTO para Crear (Lo que envías en el POST)
export interface PersonaCrearDto {
  nombres: string;
  apellidoPaterno: string;
  apellidoMaterno: string;
  idTipoDocumento: number;
  numeroDocumento: string;
  telefono?: string;
  correoPersonal?: string;

  // Datos de Dirección
  idDistrito: number;
  calle: string;
  numero?: string;
  pisoDepartamento?: string;
  referencia?: string;

  // Auditoría
  documentoRespaldo: string;
}

// DTO para Actualizar (Lo que envías en el PUT)
export interface PersonaActualizarDto {
  idPersona: number; // Obligatorio para identificar
  nombres: string;
  apellidoPaterno: string;
  apellidoMaterno: string;
  idTipoDocumento: number;
  numeroDocumento: string;
  telefono?: string;
  correoPersonal?: string;

  // Datos de Dirección
  idDistrito: number;
  calle: string;
  numero?: string;
  pisoDepartamento?: string;
  referencia?: string;

  // Auditoría
  documentoRespaldo: string;
}
