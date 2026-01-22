USE SiseDB
GO

/* ==================================================================================
   CRUD Administrativo
   ================================================================================== */

CREATE OR ALTER PROCEDURE sp_Administrativo_Por_Id
    @idAdministrativo INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        A.idAdministrativo,
        A.idCargoAdministrativo,
        C.nombreCargo, 
        A.idPersona,
        A.idUsuario,
        A.estado
    FROM dbo.Administrativo A
    JOIN dbo.CargoAdministrativo C ON A.idCargoAdministrativo = C.idCargoAdministrativo
    WHERE A.idAdministrativo = @idAdministrativo;
END
GO

CREATE OR ALTER PROCEDURE sp_Administrativo_Crear
    @idCargoAdministrativo INT,
    @idPersona INT,
    @idUsuario INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRANSACTION;
    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM CargoAdministrativo WHERE idCargoAdministrativo = @idCargoAdministrativo)
            THROW 51000, 'El cargo administrativo especificado no existe.', 1;

        IF NOT EXISTS (SELECT 1 FROM Persona WHERE idPersona = @idPersona)
            THROW 51001, 'La persona especificada no existe.', 1;

        -- Validar que la persona no tenga ya asignado ese mismo cargo administrativo activo
        IF EXISTS (SELECT 1 FROM Administrativo WHERE idCargoAdministrativo = @idCargoAdministrativo AND idPersona = @idPersona AND estado <> 'Eliminado')
            THROW 51002, 'Esta persona ya tiene asignado el cargo administrativo seleccionado.', 1;
        
        INSERT INTO [dbo].[Administrativo] (
            [idCargoAdministrativo],
            [idPersona],
            [idUsuario],
            [estado]
        )
        VALUES (
            @idCargoAdministrativo,
            @idPersona,
            @idUsuario,
            'Activo'
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

CREATE OR ALTER PROCEDURE sp_Administrativo_Actualizar
    @idAdministrativo INT,
    @idCargoAdministrativo INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRANSACTION;
    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM Administrativo WHERE idAdministrativo = @idAdministrativo)
            THROW 51000, 'El registro administrativo especificado no existe.', 1;

        IF NOT EXISTS (SELECT 1 FROM CargoAdministrativo WHERE idCargoAdministrativo = @idCargoAdministrativo)
            THROW 51001, 'El cargo administrativo especificado no existe.', 1;

        -- Obtener el idPersona del registro actual para validar duplicidad
        DECLARE @idPersonaActual INT;
        SELECT @idPersonaActual = idPersona FROM Administrativo WHERE idAdministrativo = @idAdministrativo;

        -- Validar que la persona no tenga ya ese NUEVO cargo en otro registro activo
        IF EXISTS (SELECT 1 FROM Administrativo WHERE idCargoAdministrativo = @idCargoAdministrativo AND idPersona = @idPersonaActual AND idAdministrativo <> @idAdministrativo AND estado <> 'Eliminado')
            THROW 51002, 'La persona ya posee el cargo administrativo seleccionado en otro registro.', 1;

        UPDATE dbo.Administrativo
        SET
            idCargoAdministrativo = @idCargoAdministrativo
        WHERE idAdministrativo = @idAdministrativo;

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

CREATE OR ALTER PROCEDURE sp_Administrativo_Eliminar
    @idAdministrativo INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRANSACTION;
    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM Administrativo WHERE idAdministrativo = @idAdministrativo)
            THROW 51000, 'El registro administrativo especificado no existe.', 1;

        UPDATE dbo.Administrativo
        SET estado = 'Eliminado'
        WHERE idAdministrativo = @idAdministrativo;

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