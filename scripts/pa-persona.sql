USE SiseDB
GO

/* ==================================================================================
   1. INSERTAR PERSONA
   ================================================================================== */
CREATE OR ALTER PROCEDURE sp_Persona_Insertar
    @Nombres NVARCHAR(100),
    @ApellidoPaterno NVARCHAR(100),
    @ApellidoMaterno NVARCHAR(100),
    @IdTipoDocumento INT,
    @NumeroDocumento VARCHAR(20),
    @IdDireccion INT,
    @Telefono NVARCHAR(15) = NULL,
    @CorreoPersonal NVARCHAR(150) = NULL,
    @IdPersonaGenerado INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION;

    BEGIN TRY
        -- Validar dirección
        IF NOT EXISTS (
            SELECT 1 
            FROM dbo.Direccion
            WHERE idDireccion = @IdDireccion
              AND estado = 'Activo'
        )
        BEGIN
            RAISERROR('La dirección no existe o no está activa.', 16, 1);
        END

        -- Validar duplicidad de documento
        IF EXISTS (
            SELECT 1 
            FROM dbo.Persona
            WHERE idTipoDocumento = @IdTipoDocumento
              AND numeroDocumento = @NumeroDocumento
        )
        BEGIN
            RAISERROR('El número de documento ya está registrado.', 16, 1);
        END

        INSERT INTO dbo.Persona (
            nombres,
            apellidoPaterno,
            apellidoMaterno,
            numeroDocumento,
            idTipoDocumento,
            idDireccion,
            telefono,
            correoPersonal,
            estado
        )
        VALUES (
            TRIM(@Nombres),
            TRIM(@ApellidoPaterno),
            TRIM(@ApellidoMaterno),
            @NumeroDocumento,
            @IdTipoDocumento,
            @IdDireccion,
            @Telefono,
            LOWER(@CorreoPersonal),
            'Activo'
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
    @Nombres NVARCHAR(100),
    @ApellidoPaterno NVARCHAR(100),
    @ApellidoMaterno NVARCHAR(100),
    @IdTipoDocumento INT,
    @NumeroDocumento VARCHAR(20),
    @IdDireccion INT,
    @Telefono NVARCHAR(15) = NULL,
    @CorreoPersonal NVARCHAR(150) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION;

    BEGIN TRY
        -- Validar persona
        IF NOT EXISTS (SELECT 1 FROM dbo.Persona WHERE idPersona = @IdPersona)
        BEGIN
            RAISERROR('La persona no existe.', 16, 1);
        END

        -- Validar dirección
        IF NOT EXISTS (
            SELECT 1 
            FROM dbo.Direccion
            WHERE idDireccion = @IdDireccion
              AND estado = 'Activo'
        )
        BEGIN
            RAISERROR('La dirección no existe o no está activa.', 16, 1);
        END

        -- Validar duplicidad de documento
        IF EXISTS (
            SELECT 1 
            FROM dbo.Persona
            WHERE idTipoDocumento = @IdTipoDocumento
              AND numeroDocumento = @NumeroDocumento
              AND idPersona <> @IdPersona
        )
        BEGIN
            RAISERROR('El número de documento ya pertenece a otra persona.', 16, 1);
        END

        UPDATE dbo.Persona
        SET
            nombres = TRIM(@Nombres),
            apellidoPaterno = TRIM(@ApellidoPaterno),
            apellidoMaterno = TRIM(@ApellidoMaterno),
            numeroDocumento = @NumeroDocumento,
            idTipoDocumento = @IdTipoDocumento,
            idDireccion = @IdDireccion,
            telefono = @Telefono,
            correoPersonal = LOWER(@CorreoPersonal)
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
        TD.nombreTipo AS tipoDocumento,
        P.numeroDocumento,
        P.telefono,
        P.correoPersonal,
        D.calle,
        D.numero,
        D.referencia,
        P.estado
    FROM dbo.Persona P
    INNER JOIN dbo.TipoDocumento TD ON P.idTipoDocumento = TD.idTipoDocumento
    INNER JOIN dbo.Direccion D ON P.idDireccion = D.idDireccion
    WHERE P.estado = 'Activo';
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