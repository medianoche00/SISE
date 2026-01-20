USE SiseDB
GO

/* =========================================================
   PA: CREAR USUARIO BASE (IDENTITY)
   ========================================================= */
CREATE OR ALTER PROCEDURE pa_crear_usuario_base
    @UserName NVARCHAR(256),
    @Email NVARCHAR(256),
    @PasswordHash NVARCHAR(MAX),
    @RoleName NVARCHAR(100),
    @IdUsuario INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO AspNetUsers
    (
        UserName, NormalizedUserName,
        Email, NormalizedEmail,
        EmailConfirmed, PasswordHash,
        SecurityStamp, ConcurrencyStamp,
        PhoneNumberConfirmed, TwoFactorEnabled,
        LockoutEnabled, AccessFailedCount
    )
    VALUES
    (
        @UserName, UPPER(@UserName),
        @Email, UPPER(@Email),
        1, @PasswordHash,
        NEWID(), NEWID(),
        0, 0, 1, 0
    );

    SET @IdUsuario = SCOPE_IDENTITY();

    INSERT INTO AspNetUserRoles (UserId, RoleId)
    SELECT @IdUsuario, Id
    FROM AspNetRoles
    WHERE Name = @RoleName;
END
GO

/* =========================================================
   PA: CREAR EGRESADO
   ========================================================= */
CREATE OR ALTER PROCEDURE pa_crear_egresado
    -- Persona
    @Nombres NVARCHAR(100),
    @ApellidoPaterno NVARCHAR(100),
    @ApellidoMaterno NVARCHAR(100),
    @NumeroDocumento VARCHAR(20),
    @IdTipoDocumento INT,
    @Telefono NVARCHAR(15),
    @CorreoPersonal NVARCHAR(150),

    -- Dirección
    @IdDistrito INT,
    @Calle NVARCHAR(150),
    @Numero VARCHAR(20),

    -- Usuario
    @UserName NVARCHAR(256),
    @Email NVARCHAR(256),
    @PasswordHash NVARCHAR(MAX),

    -- Egresado
    @IdCarrera INT,
    @CodigoUniversitario NVARCHAR(20),
    @AnioEgreso INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRAN;

    BEGIN TRY
        DECLARE @IdDireccion INT, @IdPersona INT, @IdUsuario INT;

        INSERT INTO Direccion (idDistrito, calle, numero)
        VALUES (@IdDistrito, @Calle, @Numero);
        SET @IdDireccion = SCOPE_IDENTITY();

        INSERT INTO Persona
        (nombres, apellidoPaterno, apellidoMaterno, numeroDocumento,
         idTipoDocumento, idDireccion, telefono, correoPersonal)
        VALUES
        (@Nombres, @ApellidoPaterno, @ApellidoMaterno, @NumeroDocumento,
         @IdTipoDocumento, @IdDireccion, @Telefono, @CorreoPersonal);
        SET @IdPersona = SCOPE_IDENTITY();

        EXEC pa_crear_usuario_base
            @UserName, @Email, @PasswordHash, 'Egresado', @IdUsuario OUTPUT;

        INSERT INTO Egresado
        (idPersona, idUsuario, idCarrera, codigoUniversitario, anioEgreso)
        VALUES
        (@IdPersona, @IdUsuario, @IdCarrera, @CodigoUniversitario, @AnioEgreso);

        COMMIT;
    END TRY
    BEGIN CATCH
        ROLLBACK;
        THROW;
    END CATCH
END
GO

/* =========================================================
   PA: CREAR ADMINISTRATIVO / ADMINISTRADOR
   ========================================================= */
CREATE OR ALTER PROCEDURE pa_crear_administrativo
    @IdCargoAdministrativo INT,

    -- Persona
    @Nombres NVARCHAR(100),
    @ApellidoPaterno NVARCHAR(100),
    @ApellidoMaterno NVARCHAR(100),
    @NumeroDocumento VARCHAR(20),
    @IdTipoDocumento INT,
    @CorreoPersonal NVARCHAR(150),

    -- Dirección
    @IdDistrito INT,
    @Calle NVARCHAR(150),
    @Numero VARCHAR(20),

    -- Usuario
    @UserName NVARCHAR(256),
    @Email NVARCHAR(256),
    @PasswordHash NVARCHAR(MAX),

    -- Rol
    @Rol NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRAN;

    BEGIN TRY
        DECLARE @IdDireccion INT, @IdPersona INT, @IdUsuario INT;

        INSERT INTO Direccion (idDistrito, calle, numero)
        VALUES (@IdDistrito, @Calle, @Numero);
        SET @IdDireccion = SCOPE_IDENTITY();

        INSERT INTO Persona
        (nombres, apellidoPaterno, apellidoMaterno, numeroDocumento,
         idTipoDocumento, idDireccion, correoPersonal)
        VALUES
        (@Nombres, @ApellidoPaterno, @ApellidoMaterno, @NumeroDocumento,
         @IdTipoDocumento, @IdDireccion, @CorreoPersonal);
        SET @IdPersona = SCOPE_IDENTITY();

        EXEC pa_crear_usuario_base
            @UserName, @Email, @PasswordHash, @Rol, @IdUsuario OUTPUT;

        INSERT INTO Administrativo
        (idCargoAdministrativo, idPersona, idUsuario)
        VALUES
        (@IdCargoAdministrativo, @IdPersona, @IdUsuario);

        COMMIT;
    END TRY
    BEGIN CATCH
        ROLLBACK;
        THROW;
    END CATCH
END
GO

/* =========================================================
   PA: CREAR REPRESENTANTE
   ========================================================= */
CREATE OR ALTER PROCEDURE pa_crear_representante
    @IdEmpresa INT,

    -- Persona
    @Nombres NVARCHAR(100),
    @ApellidoPaterno NVARCHAR(100),
    @ApellidoMaterno NVARCHAR(100),
    @NumeroDocumento VARCHAR(20),
    @IdTipoDocumento INT,
    @CorreoPersonal NVARCHAR(150),

    -- Dirección
    @IdDistrito INT,
    @Calle NVARCHAR(150),
    @Numero VARCHAR(20),

    -- Usuario
    @UserName NVARCHAR(256),
    @Email NVARCHAR(256),
    @PasswordHash NVARCHAR(MAX),

    @Cargo NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRAN;

    BEGIN TRY
        DECLARE @IdDireccion INT, @IdPersona INT, @IdUsuario INT;

        INSERT INTO Direccion (idDistrito, calle, numero)
        VALUES (@IdDistrito, @Calle, @Numero);
        SET @IdDireccion = SCOPE_IDENTITY();

        INSERT INTO Persona
        (nombres, apellidoPaterno, apellidoMaterno, numeroDocumento,
         idTipoDocumento, idDireccion, correoPersonal)
        VALUES
        (@Nombres, @ApellidoPaterno, @ApellidoMaterno, @NumeroDocumento,
         @IdTipoDocumento, @IdDireccion, @CorreoPersonal);
        SET @IdPersona = SCOPE_IDENTITY();

        EXEC pa_crear_usuario_base
            @UserName, @Email, @PasswordHash, 'Representante', @IdUsuario OUTPUT;

        INSERT INTO Representante
        (idEmpresa, idPersona, idUsuario, cargo)
        VALUES
        (@IdEmpresa, @IdPersona, @IdUsuario, @Cargo);

        COMMIT;
    END TRY
    BEGIN CATCH
        ROLLBACK;
        THROW;
    END CATCH
END
GO

/* =========================================================
   PA: LISTAR USUARIOS
   ========================================================= */
CREATE OR ALTER PROCEDURE pa_listar_usuarios
AS
BEGIN
    SELECT
        U.UserName,
        U.Email,
        R.Name AS Rol,
        COALESCE(E.estado, A.estado, R2.estado) AS Estado
    FROM AspNetUsers U
    JOIN AspNetUserRoles UR ON U.Id = UR.UserId
    JOIN AspNetRoles R ON UR.RoleId = R.Id
    LEFT JOIN Egresado E ON E.idUsuario = U.Id
    LEFT JOIN Administrativo A ON A.idUsuario = U.Id
    LEFT JOIN Representante R2 ON R2.idUsuario = U.Id;
END
GO

/* =========================================================
    PA: PROCEDIMIENTOS DE ROLES
   ========================================================= */

CREATE OR ALTER PROCEDURE sp_Username_Esta_Disponible
    @UserName NVARCHAR(256),
    @Disponible BIT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM AspNetUsers WHERE UserName = @UserName)
        SET @Disponible = 0;
    ELSE
        SET @Disponible = 1;
END