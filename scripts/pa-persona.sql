USE SiseDB
GO

/* ==================================================================================
   1. INSERTAR PERSONA
   ================================================================================== */
CREATE OR ALTER PROCEDURE sp_Persona_Insertar
    -- Datos Persona
    @Nombres NVARCHAR(100),
    @ApellidoPaterno NVARCHAR(100),
    @ApellidoMaterno NVARCHAR(100),
    @IdTipoDocumento INT,
    @NumeroDocumento VARCHAR(20),
    @Telefono NVARCHAR(15) = NULL,
    @CorreoPersonal NVARCHAR(150) = NULL,
    
    -- Datos Dirección
    @IdDistrito INT,
    @Calle NVARCHAR(150),
    @Numero VARCHAR(20) = NULL,
    @PisoDepartamento VARCHAR(20) = NULL,
    @Referencia NVARCHAR(200) = NULL,
    
    -- Salida
    @IdPersonaGenerado INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION;

    BEGIN TRY
        IF EXISTS (SELECT 1 FROM dbo.Persona WHERE idTipoDocumento = @IdTipoDocumento AND numeroDocumento = @NumeroDocumento)
        BEGIN
            RAISERROR('El documento ya existe registrado.', 16, 1);
        END

        DECLARE @NewIdDireccion INT;

        INSERT INTO dbo.Direccion (
            idDistrito, calle, numero, pisoDepartamento, referencia, estado, fechaRegistro
        )
        VALUES (
            @IdDistrito, @Calle, @Numero, @PisoDepartamento, @Referencia, 'Activo', GETDATE()
        );

        SET @NewIdDireccion = SCOPE_IDENTITY();

        INSERT INTO dbo.Persona (
            nombres, apellidoPaterno, apellidoMaterno, numeroDocumento, 
            idTipoDocumento, idDireccion, telefono, correoPersonal, estado
        )
        VALUES (
            TRIM(@Nombres), TRIM(@ApellidoPaterno), TRIM(@ApellidoMaterno), @NumeroDocumento,
            @IdTipoDocumento, @NewIdDireccion, 
            @Telefono, LOWER(@CorreoPersonal), 'Activo'
        );

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
    @IdPersona INT,
    -- Datos Persona
    @Nombres NVARCHAR(100),
    @ApellidoPaterno NVARCHAR(100),
    @ApellidoMaterno NVARCHAR(100),
    @IdTipoDocumento INT,
    @NumeroDocumento VARCHAR(20),
    @Telefono NVARCHAR(15) = NULL,
    @CorreoPersonal NVARCHAR(150) = NULL,
    
    -- Datos Dirección
    @IdDistrito INT,
    @Calle NVARCHAR(150),
    @Numero VARCHAR(20) = NULL,
    @PisoDepartamento VARCHAR(20) = NULL,
    @Referencia NVARCHAR(200) = NULL,

    @estado NVARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION;

    BEGIN TRY
        -- 1. Obtener ID Dirección actual
        DECLARE @IdDireccionActual INT;

        SELECT @IdDireccionActual = idDireccion
        FROM dbo.Persona
        WHERE idPersona = @IdPersona;

        IF @IdDireccionActual IS NULL
        BEGIN
            RAISERROR('La persona no existe.', 16, 1);
        END

        -- 2. Validar Documento
        IF EXISTS (
            SELECT 1 FROM dbo.Persona
            WHERE idTipoDocumento = @IdTipoDocumento 
              AND numeroDocumento = @NumeroDocumento 
              AND idPersona <> @IdPersona
        )
        BEGIN
            RAISERROR('El documento ya pertenece a otra persona.', 16, 1);
        END

        -- 3. ACTUALIZAR DIRECCION
        UPDATE dbo.Direccion
        SET 
            idDistrito = @IdDistrito,
            calle = @Calle,
            numero = @Numero,
            pisoDepartamento = @PisoDepartamento,
            referencia = @Referencia
        WHERE idDireccion = @IdDireccionActual;

        -- 4. ACTUALIZAR PERSONA
        UPDATE dbo.Persona
        SET
            nombres = TRIM(@Nombres),
            apellidoPaterno = TRIM(@ApellidoPaterno),
            apellidoMaterno = TRIM(@ApellidoMaterno),
            numeroDocumento = @NumeroDocumento,
            idTipoDocumento = @IdTipoDocumento,
            telefono = @Telefono,
            correoPersonal = LOWER(@CorreoPersonal),
            estado = @estado
        WHERE idPersona = @IdPersona;

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
   3. ELIMINAR PERSONA (LÓGICO)
   ================================================================================== */
CREATE OR ALTER PROCEDURE sp_Persona_Eliminar
    @IdPersona INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION;

    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM dbo.Persona WHERE idPersona = @IdPersona)
        BEGIN
            RAISERROR('La persona no existe.', 16, 1);
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

        -- Propagar el error al aplicativo
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
        DECLARE @ErrorState INT = ERROR_STATE();
        RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
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
        P.numeroDocumento,
        TD.idTipoDocumento,
        TD.nombreTipo AS nombreTipoDocumento,
        P.telefono,
        P.correoPersonal,
        D.idDireccion,
        D.idDistrito,
        D.calle,
        D.numero,
        D.pisoDepartamento,
        D.referencia,
        P.estado
    FROM dbo.Persona P
    INNER JOIN dbo.TipoDocumento TD ON P.idTipoDocumento = TD.idTipoDocumento
    INNER JOIN dbo.Direccion D ON P.idDireccion = D.idDireccion
    -- WHERE P.estado = 'Activo';
END
GO

/* ==================================================================================
   5. OBTENER PERSONA POR ID
   ================================================================================== */
CREATE OR ALTER PROCEDURE sp_Persona_ObtenerPorId
    @IdPersona INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        P.idPersona,
        P.nombres,
        P.apellidoPaterno,
        P.apellidoMaterno,
        TD.nombreTipo AS tipoDocumento,
        P.numeroDocumento,
        P.telefono,
        P.correoPersonal,
        P.idDireccion,
        D.calle,
        D.numero,
        D.referencia,
        P.estado
    FROM dbo.Persona P
    INNER JOIN dbo.TipoDocumento TD ON P.idTipoDocumento = TD.idTipoDocumento
    INNER JOIN dbo.Direccion D ON P.idDireccion = D.idDireccion
    WHERE P.idPersona = @IdPersona;
END
GO

/* ==================================================================================
   6. OBTENER PERSONA POR DOCUMENTO
   ================================================================================== */
CREATE OR ALTER PROCEDURE sp_Persona_ObtenerPorDocumento
    @IdTipoDocumento INT,
    @NumeroDocumento VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        P.idPersona,
        P.nombres,
        P.apellidoPaterno,
        P.apellidoMaterno,
        TD.nombreTipo AS tipoDocumento,
        P.numeroDocumento,
        P.telefono,
        P.correoPersonal,
        P.idDireccion,
        D.calle,
        D.numero,
        D.referencia,
        P.estado
    FROM dbo.Persona P
    INNER JOIN dbo.TipoDocumento TD ON P.idTipoDocumento = TD.idTipoDocumento
    INNER JOIN dbo.Direccion D ON P.idDireccion = D.idDireccion
    WHERE P.idTipoDocumento = @IdTipoDocumento
      AND P.numeroDocumento = @NumeroDocumento;
END
GO