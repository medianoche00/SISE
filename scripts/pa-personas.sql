PROCEDIMIENTOS ALMACENADOS (Gestión de Personas)
Implementación completa del caso de uso solicitado por el docente.

-- 1. CREAR PERSONA Y EGRESADO (Transaccional)
CREATE OR ALTER PROCEDURE [dbo].[sp_RegistrarPersonaEgresado]
    @Nombres NVARCHAR(100),
    @ApellidoPaterno NVARCHAR(100),
    @ApellidoMaterno NVARCHAR(100),
    @DNI NVARCHAR(20),
    @Telefono NVARCHAR(20),
    @CorreoPersonal NVARCHAR(100),
    @IdCarrera INT,
    @CodigoUniversitario NVARCHAR(20),
    @AnioEgreso INT,
    @IdUsuarioNet NVARCHAR(450) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
            DECLARE @IdPersonaGenerado INT;
            
            INSERT INTO [dbo].[Persona] (nombres, apellidoPaterno, apellidoMaterno, documentoIdentidad, telefono, correoPersonal)
            VALUES (@Nombres, @ApellidoPaterno, @ApellidoMaterno, @DNI, @Telefono, @CorreoPersonal);
            
            SET @IdPersonaGenerado = SCOPE_IDENTITY();

            INSERT INTO [dbo].[Egresado] (idPersona, idCarrera, idUsuario, codigoUniversitario, anioEgreso)
            VALUES (@IdPersonaGenerado, @IdCarrera, @IdUsuarioNet, @CodigoUniversitario, @AnioEgreso);

        COMMIT TRANSACTION;
        SELECT 1 AS Resultado, 'Registro exitoso' AS Mensaje;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        SELECT 0 AS Resultado, ERROR_MESSAGE() AS Mensaje;
    END CATCH
END;
GO

-- 2. LEER / DETALLE (Por ID)
CREATE OR ALTER PROCEDURE [dbo].[sp_ObtenerPersonaPorId]
    @IdPersona INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT P.*, E.codigoUniversitario, E.anioEgreso, E.idCarrera, E.estado AS EstadoLaboral
    FROM [dbo].[Persona] P
    LEFT JOIN [dbo].[Egresado] E ON P.idPersona = E.idPersona
    WHERE P.idPersona = @IdPersona;
END;
GO

-- 3. ACTUALIZAR (Transaccional)
CREATE OR ALTER PROCEDURE [dbo].[sp_ActualizarPersonaEgresado]
    @IdPersona INT,
    @Nombres NVARCHAR(100),
    @ApellidoPaterno NVARCHAR(100),
    @ApellidoMaterno NVARCHAR(100),
    @DNI NVARCHAR(20),
    @Telefono NVARCHAR(20),
    @CorreoPersonal NVARCHAR(100),
    @IdCarrera INT,
    @CodigoUniversitario NVARCHAR(20),
    @AnioEgreso INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
            UPDATE [dbo].[Persona]
            SET nombres = @Nombres, apellidoPaterno = @ApellidoPaterno, apellidoMaterno = @ApellidoMaterno,
                documentoIdentidad = @DNI, telefono = @Telefono, correoPersonal = @CorreoPersonal
            WHERE idPersona = @IdPersona;

            UPDATE [dbo].[Egresado]
            SET idCarrera = @IdCarrera, codigoUniversitario = @CodigoUniversitario, anioEgreso = @AnioEgreso
            WHERE idPersona = @IdPersona;
        COMMIT TRANSACTION;
        SELECT 1 AS Resultado, 'Actualización exitosa' AS Mensaje;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        SELECT 0 AS Resultado, ERROR_MESSAGE() AS Mensaje;
    END CATCH
END;
GO

-- 4. CAMBIAR ESTADO (Borrado Lógico)
CREATE OR ALTER PROCEDURE [dbo].[sp_CambiarEstadoPersona]
    @IdPersona INT,
    @NuevoEstado NVARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE [dbo].[Persona] SET estado = @NuevoEstado WHERE idPersona = @IdPersona;
    SELECT 1 AS Resultado, 'Estado actualizado' AS Mensaje;
END;
GO