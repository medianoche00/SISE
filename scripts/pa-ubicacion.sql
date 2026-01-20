USE SiseDB
GO

/* ==================================================================================
   1. LISTAR DEPARTAMENTOS ACTIVOS
   ================================================================================== */
CREATE OR ALTER PROCEDURE sp_Departamento_ListarActivos
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        idDepartamento,
        nombreDepartamento
    FROM dbo.Departamento
    WHERE estado = 'Activo'
    ORDER BY nombreDepartamento;
END
GO

/* ==================================================================================
   2. LISTAR PROVINCIAS ACTIVAS POR DEPARTAMENTO
   ================================================================================== */
CREATE OR ALTER PROCEDURE sp_Provincia_ListarPorDepartamento
    @IdDepartamento INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        idProvincia,
        idDepartamento,
        nombreProvincia
    FROM dbo.Provincia
    WHERE estado = 'Activo'
      AND idDepartamento = @IdDepartamento
    ORDER BY nombreProvincia;
END
GO

/* ==================================================================================
   3. LISTAR DISTRITOS ACTIVOS POR PROVINCIA
   ================================================================================== */
CREATE OR ALTER PROCEDURE sp_Distrito_ListarPorProvincia
    @IdProvincia INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        idDistrito,
        idProvincia,
        nombreDistrito
    FROM dbo.Distrito
    WHERE estado = 'Activo'
      AND idProvincia = @IdProvincia
    ORDER BY nombreDistrito;
END
GO
