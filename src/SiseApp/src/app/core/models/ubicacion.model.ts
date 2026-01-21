export interface DistritoDto {
  id: number;
  nombre: string;
}

export interface ProvinciaDto {
  id: number;
  nombre: string;
  distritos: DistritoDto[];
}

export interface DepartamentoDto {
  id: number;
  nombre: string;
  provincias: ProvinciaDto[];
}
