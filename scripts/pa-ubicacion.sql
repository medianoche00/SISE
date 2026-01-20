USE SiseDB
GO

/* ==================================================================================
   1. LISTAR JOIN DE LAS 3 TABLAS
   ================================================================================== */
CREATE OR ALTER PROCEDURE [dbo].[sp_Ubicacion_ListarCompleto]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        d.idDepartamento,
        d.nombreDepartamento,
        p.idProvincia,
        p.nombreProvincia,
        dis.idDistrito,
        dis.nombreDistrito
    FROM [dbo].[Departamento] d
    INNER JOIN [dbo].[Provincia] p ON d.idDepartamento = p.idDepartamento
    INNER JOIN [dbo].[Distrito] dis ON p.idProvincia = dis.idProvincia
    WHERE d.estado = 'Activo' 
      AND p.estado = 'Activo' 
      AND dis.estado = 'Activo'
    ORDER BY d.nombreDepartamento, p.nombreProvincia, dis.nombreDistrito;
END
GO
