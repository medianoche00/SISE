USE SiseDB
GO

/* ==================================================================================
   CRUD Egresado
   ================================================================================== */

CREATE OR ALTER PROCEDURE sp_Egresado_Por_Id
    @idEgresado INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        E.idEgresado,
        E.idCarrera,
        C.nombreCarrera,
        E.codigoUniversitario,
        E.anioEgreso,
        E.estado
    FROM dbo.Egresado E
    JOIN dbo.Carrera C ON E.idCarrera = C.idCarrera
    WHERE E.idEgresado = @idEgresado;
END
GO

CREATE OR ALTER PROCEDURE sp_Egresado_Crear
    @idPersona INT,
    @idCarrera INT,
    @codigoUniversitario NVARCHAR(20),
    @anioEgreso INT,
    @idUsuario INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRANSACTION;
    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM Persona WHERE idPersona = @idPersona)
            THROW 51000, 'La persona especificada no existe.', 1;

        IF NOT EXISTS (SELECT 1 FROM Carrera WHERE idCarrera = @idCarrera)
            THROW 51001, 'La carrera especificada no existe.', 1;

        -- Validar que el código universitario no esté duplicado
        IF EXISTS (SELECT 1 FROM Egresado WHERE codigoUniversitario = @codigoUniversitario)
            THROW 51002, 'El código universitario ya está registrado.', 1;
        
        INSERT INTO [dbo].[Egresado] (
            [idPersona],
            [idUsuario],
            [idCarrera],
            [codigoUniversitario],
            [anioEgreso],
            [estado]
        )
        VALUES (
            @idPersona,
            @idUsuario,
            @idCarrera,
            @codigoUniversitario,
            @anioEgreso,
            'Buscando Trabajo'
        );
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
        BEGIN
            ROLLBACK TRANSACTION;
        END

        -- Propagar el error al aplicativo
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
        DECLARE @ErrorState INT = ERROR_STATE();
        RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE sp_Egresado_Actualizar
    @idEgresado INT,
    @idCarrera INT,
    @codigoUniversitario NVARCHAR(20),
    @anioEgreso INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRANSACTION;
    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM Egresado WHERE idEgresado = @idEgresado)
            THROW 51000, 'El egresado especificado no existe.', 1;

        IF NOT EXISTS (SELECT 1 FROM Carrera WHERE idCarrera = @idCarrera)
            THROW 51001, 'La carrera especificada no existe.', 1;

        -- Validar que el código universitario no esté duplicado para otro egresado
        IF EXISTS (SELECT 1 FROM Egresado WHERE codigoUniversitario = @codigoUniversitario AND idEgresado <> @idEgresado)
            THROW 51002, 'El código universitario ya está registrado para otro egresado.', 1;

        UPDATE dbo.Egresado
        SET
            idCarrera = @idCarrera,
            codigoUniversitario = @codigoUniversitario,
            anioEgreso = @anioEgreso
        WHERE idEgresado = @idEgresado;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
        BEGIN
            ROLLBACK TRANSACTION;
        END

        -- Propagar el error al aplicativo
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
        DECLARE @ErrorState INT = ERROR_STATE();
        RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE sp_Egresado_Eliminar
    @idEgresado INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRANSACTION;
    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM Egresado WHERE idEgresado = @idEgresado)
            THROW 51000, 'El egresado especificado no existe.', 1;

        UPDATE dbo.Egresado
        SET estado = 'Eliminado'
        WHERE idEgresado = @idEgresado;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
        BEGIN
            ROLLBACK TRANSACTION;
        END

        -- Propagar el error al aplicativo
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
        DECLARE @ErrorState INT = ERROR_STATE();
        RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END