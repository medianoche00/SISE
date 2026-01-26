USE SiseDB
GO

/* ==================================================================================
   CRUD Representante
   ================================================================================== */

CREATE OR ALTER PROCEDURE sp_Representante_Por_Id
    @idRepresentante INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        R.idRepresentante,
        R.idEmpresa,
        E.razonSocial AS nombreEmpresa, 
        R.idPersona,
        R.idUsuario,
        R.cargo,
        R.estado
    FROM dbo.Representante R
    JOIN dbo.Empresa E ON R.idEmpresa = E.idEmpresa
    WHERE R.idRepresentante = @idRepresentante;
END
GO

CREATE OR ALTER PROCEDURE sp_Representante_Crear
    @idEmpresa INT,
    @idPersona INT,
    @idUsuario INT,
    @cargo NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRANSACTION;
    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM Empresa WHERE idEmpresa = @idEmpresa)
            THROW 51000, 'La empresa especificada no existe.', 1;

        IF NOT EXISTS (SELECT 1 FROM Persona WHERE idPersona = @idPersona)
            THROW 51001, 'La persona especificada no existe.', 1;

        -- Validar que la persona no sea ya representante de esa misma empresa
        IF EXISTS (SELECT 1 FROM Representante WHERE idEmpresa = @idEmpresa AND idPersona = @idPersona AND estado <> 'Eliminado')
            THROW 51002, 'Esta persona ya está registrada como representante de la empresa seleccionada.', 1;
        
        INSERT INTO [dbo].[Representante] (
            [idEmpresa],
            [idPersona],
            [idUsuario],
            [cargo],
            [estado]
        )
        VALUES (
            @idEmpresa,
            @idPersona,
            @idUsuario,
            @cargo,
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

CREATE OR ALTER PROCEDURE sp_Representante_Actualizar
    @idRepresentante INT,
    @idEmpresa INT,
    @cargo NVARCHAR(100),
    @estado NVARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRANSACTION;
    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM Representante WHERE idRepresentante = @idRepresentante)
            THROW 51000, 'El representante especificado no existe.', 1;

        IF NOT EXISTS (SELECT 1 FROM Empresa WHERE idEmpresa = @idEmpresa)
            THROW 51001, 'La empresa especificada no existe.', 1;

        -- Obtener el idPersona del representante actual para validar duplicidad
        DECLARE @idPersonaActual INT;
        SELECT @idPersonaActual = idPersona FROM Representante WHERE idRepresentante = @idRepresentante;

        -- Validar que no exista otro registro activo con la misma Empresa y Persona (excluyendo el actual)
        IF EXISTS (SELECT 1 FROM Representante WHERE idEmpresa = @idEmpresa AND idPersona = @idPersonaActual AND idRepresentante <> @idRepresentante AND estado <> 'Eliminado')
            THROW 51002, 'Esta persona ya figura como representante en la empresa seleccionada en otro registro.', 1;

        UPDATE dbo.Representante
        SET
            idEmpresa = @idEmpresa,
            cargo = @cargo,
            estado = @estado
        WHERE idRepresentante = @idRepresentante;

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

CREATE OR ALTER PROCEDURE sp_Representante_Eliminar
    @idRepresentante INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRANSACTION;
    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM Representante WHERE idRepresentante = @idRepresentante)
            THROW 51000, 'El representante especificado no existe.', 1;

        UPDATE dbo.Representante
        SET estado = 'Eliminado'
        WHERE idRepresentante = @idRepresentante;

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