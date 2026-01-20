USE SiseDB
GO

/*
buscar usuarios de la persona que existan en AspNetUsers y 
en alguna de las tablas Administrativo, Egresado, Representante 
devolviendo datos relevantes de cada perfil
*/
CREATE OR ALTER PROCEDURE sp_Usuarios_De_Persona_Listar 
    @idPersona INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Bloque 1: Obtener perfiles de ADMINISTRATIVOS
    SELECT 
        ROL.Name AS rol,
        CA.nombreCargo AS contexto,
        U.UserName AS usuario,
        ROL.Id AS idRol,
        U.Id as idUsuario,
        A.estado AS estadoRol
    FROM Persona P
    INNER JOIN Administrativo A ON P.IdPersona = A.IdPersona
    INNER JOIN CargoAdministrativo CA ON A.IdCargoAdministrativo = CA.IdCargoAdministrativo
    INNER JOIN AspNetUsers U ON A.IdUsuario = U.Id
    INNER JOIN AspNetUserRoles UR ON U.Id = UR.UserId
    INNER JOIN AspNetRoles ROL ON UR.RoleId = ROL.Id
    WHERE P.IdPersona = @idPersona

    UNION ALL

    -- Bloque 2: Obtener perfiles de EGRESADOS
    SELECT 
        ROL.Name AS rol,
        CAR.nombreCarrera AS contexto,
        U.UserName AS usuario,
        ROL.Id AS idRol,
        U.Id AS idUsuario,
        E.estado AS estadoRol
    FROM Persona P
    INNER JOIN Egresado E ON P.IdPersona = E.IdPersona
    INNER JOIN Carrera CAR ON E.IdCarrera = CAR.IdCarrera
    INNER JOIN AspNetUsers U ON E.IdUsuario = U.Id
    INNER JOIN AspNetUserRoles UR ON U.Id = UR.UserId
    INNER JOIN AspNetRoles ROL ON UR.RoleId = ROL.Id
    WHERE P.IdPersona = @idPersona

    UNION ALL

    -- Bloque 3: Obtener perfiles de REPRESENTANTES
    SELECT 
        ROL.Name AS rol,
        EMP.razonSocial AS contexto,
        U.UserName AS usuario,
        ROL.Id AS idRol,
        U.Id AS idUsuario,
        R.estado AS estadoRol
    FROM Persona P
    INNER JOIN Representante R ON P.IdPersona = R.IdPersona
    INNER JOIN Empresa EMP ON R.IdEmpresa = EMP.IdEmpresa
    INNER JOIN AspNetUsers U ON R.IdUsuario = U.Id
    INNER JOIN AspNetUserRoles UR ON U.Id = UR.UserId
    INNER JOIN AspNetRoles ROL ON UR.RoleId = ROL.Id
    WHERE P.IdPersona = @idPersona;

END