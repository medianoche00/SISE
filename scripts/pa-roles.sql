USE SiseDB
GO

/* ==================================================================================
   Guardar datos especificos del rol
   ================================================================================== */

CREATE OR ALTER PROCEDURE sp_Egresado_Rol_Guardar
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

CREATE OR ALTER PROCEDURE sp_Administrativo_Rol_Guardar
    @idPersona INT,
    @idCargoAdministrativo INT,
    @idUsuario INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION;
    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM Persona WHERE idPersona = @idPersona)
            THROW 51000, 'La persona especificada no existe.', 1;

        IF NOT EXISTS (SELECT 1 FROM CargoAdministrativo WHERE idCargoAdministrativo = @idCargoAdministrativo)
            THROW 51001, 'El cargo administrativo especificado no existe.', 1;

        -- Validar si ya es administrativo
        IF EXISTS (SELECT 1 FROM Administrativo WHERE idPersona = @idPersona)
            THROW 51002, 'Esta persona ya tiene un perfil administrativo activo.', 1;

        INSERT INTO [dbo].[Administrativo] (
            [idPersona],
            [idUsuario],
            [idCargoAdministrativo],
            [estado]
        )
        VALUES (
            @idPersona,
            @idUsuario,
            @idCargoAdministrativo,
            'Activo'
        );

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMessage, 16, 1);
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE sp_Representante_Rol_Guardar
    @idPersona INT,
    @idEmpresa INT,
    @idUsuario INT,
    @cargo NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION;
    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM Persona WHERE idPersona = @idPersona)
            THROW 51000, 'La persona especificada no existe.', 1;

        IF NOT EXISTS (SELECT 1 FROM Empresa WHERE idEmpresa = @idEmpresa)
            THROW 51001, 'La empresa especificada no existe.', 1;

        INSERT INTO [dbo].[Representante] (
            [idPersona],
            [idUsuario],
            [idEmpresa],
            [cargo],
            [estado]
        )
        VALUES (
            @idPersona,
            @idUsuario,
            @idEmpresa,
            @cargo,
            'Activo'
        );

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMessage, 16, 1);
    END CATCH
END
GO

/* ==================================================================================
   Listar datos especificos del rol
   ================================================================================== */

CREATE OR ALTER PROCEDURE sp_Egresado_Rol_Listar
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

CREATE OR ALTER PROCEDURE sp_Administrativo_Rol_Listar
    @idAdministrativo INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        A.idAdministrativo,
        A.idCargoAdministrativo,
        CA.nombreCargo,
        A.estado
    FROM dbo.Administrativo A
    JOIN dbo.CargoAdministrativo CA ON A.idCargoAdministrativo = CA.idCargoAdministrativo
    WHERE A.idAdministrativo = @idAdministrativo;
END
GO

CREATE OR ALTER PROCEDURE sp_Representante_Rol_Listar
    @idRepresentante INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        R.idRepresentante,
        R.idEmpresa,
        E.razonSocial,
        R.cargo,
        R.estado
    FROM dbo.Representante R
    JOIN dbo.Empresa E ON R.idEmpresa = E.idEmpresa
    WHERE R.idRepresentante = @idRepresentante;
END
GO