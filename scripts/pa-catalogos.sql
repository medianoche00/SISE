USE SiseDB
GO

/* ==================================================================================
   Listar datos de tablas consideradas catalogos
   ================================================================================== */

CREATE OR ALTER PROCEDURE sp_Rol_Listar
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        Id   AS idRol,
        Name AS nombreRol
    FROM dbo.AspNetRoles
    ORDER BY Name;
END
GO

CREATE OR ALTER PROCEDURE sp_Carrera_Listar
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        idCarrera,
        nombreCarrera,
        estado
    FROM dbo.Carrera
    WHERE estado = 'Activo'
    ORDER BY nombreCarrera;
END
GO

CREATE OR ALTER PROCEDURE sp_CargoAdministrativo_Listar
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        idCargoAdministrativo,
        nombreCargo,
        estado
    FROM dbo.CargoAdministrativo
    WHERE estado = 'Activo'
    ORDER BY nombreCargo;
END
GO

CREATE OR ALTER PROCEDURE sp_Empresa_Listar
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        e.idEmpresa,
        e.idDireccion,
        d.idDistrito,
        di.nombreDistrito,
        d.calle,
        d.numero,
        e.ruc,
        e.razonSocial,
        e.telefono,
        e.correo,
        e.descripcion,
        e.estado
    FROM dbo.Empresa e
    LEFT JOIN Direccion d ON e.idDireccion = d.idDireccion
    LEFT JOIN Distrito di ON d.idDistrito = di.idDistrito
    WHERE e.estado = 'Activa'
    ORDER BY e.razonSocial;
END
GO