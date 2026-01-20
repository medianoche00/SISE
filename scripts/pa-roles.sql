USE SiseDB
GO

/* ==================================================================================
   LISTAR ROLES DEL SISTEMA
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
