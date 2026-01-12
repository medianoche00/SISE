
USE SiseDB
GO
-- select * from persona

/* ==================================================================================
   1. CREAR (INSERTAR) PERSONA
   ================================================================================== */
CREATE OR ALTER PROCEDURE sp_Persona_Insertar
    @Nombres nvarchar(100),
    @ApellidoPaterno nvarchar(100),
    @ApellidoMaterno nvarchar(100),
    @IdTipoDocumento int,
    @NumeroDocumento varchar(20),
    @Telefono nvarchar(15) = NULL,
    @CorreoPersonal nvarchar(150) = NULL,
    @IdPersonaGenerado int OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRANSACTION;

    BEGIN TRY
        -- Validaciones de Negocio
        
        -- Validar Duplicidad
        IF EXISTS (SELECT 1 FROM dbo.Persona 
                   WHERE idTipoDocumento = @IdTipoDocumento 
                   AND numeroDocumento = @NumeroDocumento)
        BEGIN
            RAISERROR('El número de documento ya existe para el tipo de documento seleccionado.', 16, 1);
        END

        -- Validar Formato DNI
        IF @IdTipoDocumento = 1 -- DNI
        BEGIN
            IF LEN(@NumeroDocumento) <> 8 OR @NumeroDocumento LIKE '%[^0-9]%'
            BEGIN
                RAISERROR('El DNI debe contener exactamente 8 dígitos numéricos.', 16, 1);
            END
        END
        ELSE -- Otros documentos
        BEGIN
            IF LEN(@NumeroDocumento) < 3
            BEGIN
                RAISERROR('El número de documento debe tener al menos 3 caracteres.', 16, 1);
            END
        END

        -- Inserción
        INSERT INTO dbo.Persona (
            nombres, 
            apellidoPaterno, 
            apellidoMaterno, 
            idTipoDocumento,
            numeroDocumento, 
            telefono, 
            correoPersonal, 
            estado
        )
        VALUES (
            @Nombres, 
            @ApellidoPaterno, 
            @ApellidoMaterno, 
            @IdTipoDocumento,
            @NumeroDocumento, 
            @Telefono, 
            @CorreoPersonal, 
            'Activo'
        );

        -- Obtener el ID generado
        SET @IdPersonaGenerado = SCOPE_IDENTITY();

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

/* ==================================================================================
   2. ACTUALIZAR PERSONA
   ================================================================================== */
CREATE OR ALTER PROCEDURE sp_Persona_Actualizar
    @IdPersona int,
    @Nombres nvarchar(100),
    @ApellidoPaterno nvarchar(100),
    @ApellidoMaterno nvarchar(100),
    @IdTipoDocumento int,
    @NumeroDocumento varchar(20),
    @Telefono nvarchar(15) = NULL,
    @CorreoPersonal nvarchar(150) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRANSACTION;

    BEGIN TRY
        -- Validar existencia
        IF NOT EXISTS (SELECT 1 FROM dbo.Persona WHERE idPersona = @IdPersona)
        BEGIN
            RAISERROR('La persona no existe.', 16, 1);
        END

        -- Validar que el nuevo documento no pertenezca a otra persona
        IF EXISTS (SELECT 1 FROM dbo.Persona 
                   WHERE idTipoDocumento = @IdTipoDocumento 
                   AND numeroDocumento = @NumeroDocumento 
                   AND idPersona != @IdPersona)
        BEGIN
            RAISERROR('El número de documento ya está registrado por otra persona.', 16, 1);
        END

        -- Validar Formato DNI
        IF @IdTipoDocumento = 1 
        BEGIN
            IF LEN(@NumeroDocumento) <> 8 OR @NumeroDocumento LIKE '%[^0-9]%'
            BEGIN
                RAISERROR('El DNI debe contener exactamente 8 dígitos numéricos.', 16, 1);
            END
        END

        -- Actualización
        UPDATE dbo.Persona
        SET 
            nombres = @Nombres,
            apellidoPaterno = @ApellidoPaterno,
            apellidoMaterno = @ApellidoMaterno,
            idTipoDocumento = @IdTipoDocumento,
            numeroDocumento = @NumeroDocumento,
            telefono = @Telefono,
            correoPersonal = @CorreoPersonal
        WHERE idPersona = @IdPersona;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
        BEGIN
            ROLLBACK TRANSACTION;
        END

        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
        DECLARE @ErrorState INT = ERROR_STATE();
        RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END
GO

/* ==================================================================================
   3. ELIMINAR PERSONA (LÓGICO)
   ================================================================================== */
CREATE OR ALTER PROCEDURE sp_Persona_Eliminar
    @IdPersona int
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRANSACTION;

    BEGIN TRY
        -- Validar existencia antes de intentar borrar
        IF NOT EXISTS (SELECT 1 FROM dbo.Persona WHERE idPersona = @IdPersona)
        BEGIN
             RAISERROR('La persona a eliminar no existe.', 16, 1);
        END

        UPDATE dbo.Persona
        SET estado = 'Eliminado'
        WHERE idPersona = @IdPersona;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
        BEGIN
            ROLLBACK TRANSACTION;
        END

        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMessage, 16, 1);
    END CATCH
END
GO

/* ==================================================================================
   4. LISTAR PERSONAS
   ================================================================================== */
CREATE OR ALTER PROCEDURE sp_Persona_Listar
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        P.idPersona,
        P.nombres,
        P.apellidoPaterno,
        P.apellidoMaterno,
        TD.idTipoDocumento,
        TD.nombreTipo AS nombreTipoDocumento, -- Nombre descriptivo (ej. DNI)
        P.numeroDocumento,
        P.telefono,
        P.correoPersonal,
        P.estado
    FROM dbo.Persona P
    INNER JOIN dbo.TipoDocumento TD ON P.idTipoDocumento = TD.idTipoDocumento
    WHERE P.estado = 'Activo';
END
GO

/* ==================================================================================
   5. OBTENER PERSONA POR ID
   ================================================================================== */
CREATE OR ALTER PROCEDURE sp_Persona_ObtenerPorId
    @IdPersona int
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        P.idPersona,
        P.nombres,
        P.apellidoPaterno,
        P.apellidoMaterno,
        TD.idTipoDocumento,
        TD.nombreTipo AS nombreTipoDocumento,
        P.numeroDocumento,
        P.telefono,
        P.correoPersonal,
        P.estado
    FROM dbo.Persona P
    INNER JOIN dbo.TipoDocumento TD ON P.idTipoDocumento = TD.idTipoDocumento
    WHERE P.idPersona = @IdPersona;
END
GO

/* ==================================================================================
   6. OBTENER PERSONA POR DOCUMENTO
   ================================================================================== */
CREATE OR ALTER PROCEDURE sp_Persona_ObtenerPorDocumento
    @IdTipoDocumento int,
    @NumeroDocumento varchar(20)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        P.idPersona,
        P.nombres,
        P.apellidoPaterno,
        P.apellidoMaterno,
        TD.idTipoDocumento,
        TD.nombreTipo AS nombreTipoDocumento,
        P.numeroDocumento,
        P.telefono,
        P.correoPersonal,
        P.estado
    FROM dbo.Persona P
    INNER JOIN dbo.TipoDocumento TD ON P.idTipoDocumento = TD.idTipoDocumento
    WHERE P.idTipoDocumento = @IdTipoDocumento 
      AND P.numeroDocumento = @NumeroDocumento;
END
GO