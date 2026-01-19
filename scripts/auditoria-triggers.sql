USE [SiseDB]
GO
/* ==================================================================================
   se toma de la SESSION_cONTEXT:
   - AuditUserId: ID del usuario que realiza la operacion
   - AuditDocRef: Documento que respalda la operacion (Credibilidad)
   ================================================================================== */

/* ==================================================================================
   1. TRIGGERS: ASP.NET IDENTITY TABLES
   ================================================================================== */

CREATE OR ALTER TRIGGER [dbo].[TRG_Audit_AspNetUsers] ON [dbo].[AspNetUsers]
AFTER INSERT, UPDATE, DELETE AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @idUser INT = CAST(SESSION_CONTEXT(N'AuditUserId') AS INT);
    DECLARE @dbUser NVARCHAR(100) = SYSTEM_USER;
    DECLARE @docRef NVARCHAR(100) = CAST(SESSION_CONTEXT(N'AuditDocRef') AS NVARCHAR(100));
    DECLARE @action NVARCHAR(20);
    IF EXISTS (SELECT * FROM inserted) AND EXISTS (SELECT * FROM deleted) SET @action = 'UPDATE';
    ELSE IF EXISTS (SELECT * FROM inserted) SET @action = 'INSERT';
    ELSE IF EXISTS (SELECT * FROM deleted) SET @action = 'DELETE';
    ELSE RETURN;

    INSERT INTO [dbo].[Auditoria] (nombreTabla, idRegistro, tipoAccion, idUsuario, valAntiguos, valNuevos, docRespaldo, usuarioDB)
    SELECT 'AspNetUsers', CAST(COALESCE(i.Id, d.Id) AS NVARCHAR(100)), @action, @idUser,
    (SELECT * FROM deleted d2 WHERE d2.Id = d.Id FOR JSON AUTO, WITHOUT_ARRAY_WRAPPER),
    (SELECT * FROM inserted i2 WHERE i2.Id = i.Id FOR JSON AUTO, WITHOUT_ARRAY_WRAPPER),
    @docRef,
    @dbUser
    FROM inserted i FULL OUTER JOIN deleted d ON i.Id = d.Id;
END
GO

CREATE OR ALTER TRIGGER [dbo].[TRG_Audit_AspNetRoles] ON [dbo].[AspNetRoles]
AFTER INSERT, UPDATE, DELETE AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @idUser INT = CAST(SESSION_CONTEXT(N'AuditUserId') AS INT);
    DECLARE @dbUser NVARCHAR(100) = SYSTEM_USER;
    DECLARE @docRef NVARCHAR(100) = CAST(SESSION_CONTEXT(N'AuditDocRef') AS NVARCHAR(100));
    DECLARE @action NVARCHAR(20);
    IF EXISTS (SELECT * FROM inserted) AND EXISTS (SELECT * FROM deleted) SET @action = 'UPDATE';
    ELSE IF EXISTS (SELECT * FROM inserted) SET @action = 'INSERT';
    ELSE IF EXISTS (SELECT * FROM deleted) SET @action = 'DELETE';
    ELSE RETURN;

    INSERT INTO [dbo].[Auditoria] (nombreTabla, idRegistro, tipoAccion, idUsuario, valAntiguos, valNuevos, docRespaldo, usuarioDB)
    SELECT 'AspNetRoles', CAST(COALESCE(i.Id, d.Id) AS NVARCHAR(100)), @action, @idUser,
    (SELECT * FROM deleted d2 WHERE d2.Id = d.Id FOR JSON AUTO, WITHOUT_ARRAY_WRAPPER),
    (SELECT * FROM inserted i2 WHERE i2.Id = i.Id FOR JSON AUTO, WITHOUT_ARRAY_WRAPPER),
    @docRef,
    @dbUser
    FROM inserted i FULL OUTER JOIN deleted d ON i.Id = d.Id;
END
GO

CREATE OR ALTER TRIGGER [dbo].[TRG_Audit_AspNetUserClaims] ON [dbo].[AspNetUserClaims]
AFTER INSERT, UPDATE, DELETE AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @idUser INT = CAST(SESSION_CONTEXT(N'AuditUserId') AS INT);
    DECLARE @dbUser NVARCHAR(100) = SYSTEM_USER;
    DECLARE @docRef NVARCHAR(100) = CAST(SESSION_CONTEXT(N'AuditDocRef') AS NVARCHAR(100));
    DECLARE @action NVARCHAR(20);
    IF EXISTS (SELECT * FROM inserted) AND EXISTS (SELECT * FROM deleted) SET @action = 'UPDATE';
    ELSE IF EXISTS (SELECT * FROM inserted) SET @action = 'INSERT';
    ELSE IF EXISTS (SELECT * FROM deleted) SET @action = 'DELETE';
    ELSE RETURN;

    INSERT INTO [dbo].[Auditoria] (nombreTabla, idRegistro, tipoAccion, idUsuario, valAntiguos, valNuevos, docRespaldo, usuarioDB)
    SELECT 'AspNetUserClaims', CAST(COALESCE(i.Id, d.Id) AS NVARCHAR(100)), @action, @idUser,
    (SELECT * FROM deleted d2 WHERE d2.Id = d.Id FOR JSON AUTO, WITHOUT_ARRAY_WRAPPER),
    (SELECT * FROM inserted i2 WHERE i2.Id = i.Id FOR JSON AUTO, WITHOUT_ARRAY_WRAPPER),
    @docRef,
    @dbUser
    FROM inserted i FULL OUTER JOIN deleted d ON i.Id = d.Id;
END
GO

CREATE OR ALTER TRIGGER [dbo].[TRG_Audit_AspNetRoleClaims] ON [dbo].[AspNetRoleClaims]
AFTER INSERT, UPDATE, DELETE AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @idUser INT = CAST(SESSION_CONTEXT(N'AuditUserId') AS INT);
    DECLARE @dbUser NVARCHAR(100) = SYSTEM_USER;
    DECLARE @docRef NVARCHAR(100) = CAST(SESSION_CONTEXT(N'AuditDocRef') AS NVARCHAR(100));
    DECLARE @action NVARCHAR(20);
    IF EXISTS (SELECT * FROM inserted) AND EXISTS (SELECT * FROM deleted) SET @action = 'UPDATE';
    ELSE IF EXISTS (SELECT * FROM inserted) SET @action = 'INSERT';
    ELSE IF EXISTS (SELECT * FROM deleted) SET @action = 'DELETE';
    ELSE RETURN;

    INSERT INTO [dbo].[Auditoria] (nombreTabla, idRegistro, tipoAccion, idUsuario, valAntiguos, valNuevos, docRespaldo, usuarioDB)
    SELECT 'AspNetRoleClaims', CAST(COALESCE(i.Id, d.Id) AS NVARCHAR(100)), @action, @idUser,
    (SELECT * FROM deleted d2 WHERE d2.Id = d.Id FOR JSON AUTO, WITHOUT_ARRAY_WRAPPER),
    (SELECT * FROM inserted i2 WHERE i2.Id = i.Id FOR JSON AUTO, WITHOUT_ARRAY_WRAPPER),
    @docRef,
    @dbUser
    FROM inserted i FULL OUTER JOIN deleted d ON i.Id = d.Id;
END
GO

CREATE OR ALTER TRIGGER [dbo].[TRG_Audit_AspNetUserLogins] ON [dbo].[AspNetUserLogins]
AFTER INSERT, UPDATE, DELETE AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @idUser INT = CAST(SESSION_CONTEXT(N'AuditUserId') AS INT);
    DECLARE @dbUser NVARCHAR(100) = SYSTEM_USER;
    DECLARE @docRef NVARCHAR(100) = CAST(SESSION_CONTEXT(N'AuditDocRef') AS NVARCHAR(100));
    DECLARE @action NVARCHAR(20);
    IF EXISTS (SELECT * FROM inserted) AND EXISTS (SELECT * FROM deleted) SET @action = 'UPDATE';
    ELSE IF EXISTS (SELECT * FROM inserted) SET @action = 'INSERT';
    ELSE IF EXISTS (SELECT * FROM deleted) SET @action = 'DELETE';
    ELSE RETURN;

    INSERT INTO [dbo].[Auditoria] (nombreTabla, idRegistro, tipoAccion, idUsuario, valAntiguos, valNuevos, docRespaldo, usuarioDB)
    SELECT 'AspNetUserLogins', 
    COALESCE(i.LoginProvider, d.LoginProvider) + '-' + COALESCE(i.ProviderKey, d.ProviderKey), 
    @action, @idUser,
    (SELECT * FROM deleted d2 WHERE d2.LoginProvider = d.LoginProvider AND d2.ProviderKey = d.ProviderKey FOR JSON AUTO, WITHOUT_ARRAY_WRAPPER),
    (SELECT * FROM inserted i2 WHERE i2.LoginProvider = i.LoginProvider AND i2.ProviderKey = i.ProviderKey FOR JSON AUTO, WITHOUT_ARRAY_WRAPPER),
    @docRef,
    @dbUser
    FROM inserted i FULL OUTER JOIN deleted d ON i.LoginProvider = d.LoginProvider AND i.ProviderKey = d.ProviderKey;
END
GO

CREATE OR ALTER TRIGGER [dbo].[TRG_Audit_AspNetUserRoles] ON [dbo].[AspNetUserRoles]
AFTER INSERT, UPDATE, DELETE AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @idUser INT = CAST(SESSION_CONTEXT(N'AuditUserId') AS INT);
    DECLARE @dbUser NVARCHAR(100) = SYSTEM_USER;
    DECLARE @docRef NVARCHAR(100) = CAST(SESSION_CONTEXT(N'AuditDocRef') AS NVARCHAR(100));
    DECLARE @action NVARCHAR(20);
    IF EXISTS (SELECT * FROM inserted) AND EXISTS (SELECT * FROM deleted) SET @action = 'UPDATE';
    ELSE IF EXISTS (SELECT * FROM inserted) SET @action = 'INSERT';
    ELSE IF EXISTS (SELECT * FROM deleted) SET @action = 'DELETE';
    ELSE RETURN;

    INSERT INTO [dbo].[Auditoria] (nombreTabla, idRegistro, tipoAccion, idUsuario, valAntiguos, valNuevos, docRespaldo, usuarioDB)
    SELECT 'AspNetUserRoles', 
    CAST(COALESCE(i.UserId, d.UserId) AS NVARCHAR) + '-' + CAST(COALESCE(i.RoleId, d.RoleId) AS NVARCHAR), 
    @action, @idUser,
    (SELECT * FROM deleted d2 WHERE d2.UserId = d.UserId AND d2.RoleId = d.RoleId FOR JSON AUTO, WITHOUT_ARRAY_WRAPPER),
    (SELECT * FROM inserted i2 WHERE i2.UserId = i.UserId AND i2.RoleId = i.RoleId FOR JSON AUTO, WITHOUT_ARRAY_WRAPPER),
    @docRef,
    @dbUser
    FROM inserted i FULL OUTER JOIN deleted d ON i.UserId = d.UserId AND i.RoleId = d.RoleId;
END
GO

CREATE OR ALTER TRIGGER [dbo].[TRG_Audit_AspNetUserTokens] ON [dbo].[AspNetUserTokens]
AFTER INSERT, UPDATE, DELETE AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @idUser INT = CAST(SESSION_CONTEXT(N'AuditUserId') AS INT);
    DECLARE @dbUser NVARCHAR(100) = SYSTEM_USER;
    DECLARE @docRef NVARCHAR(100) = CAST(SESSION_CONTEXT(N'AuditDocRef') AS NVARCHAR(100));
    DECLARE @action NVARCHAR(20);
    IF EXISTS (SELECT * FROM inserted) AND EXISTS (SELECT * FROM deleted) SET @action = 'UPDATE';
    ELSE IF EXISTS (SELECT * FROM inserted) SET @action = 'INSERT';
    ELSE IF EXISTS (SELECT * FROM deleted) SET @action = 'DELETE';
    ELSE RETURN;

    INSERT INTO [dbo].[Auditoria] (nombreTabla, idRegistro, tipoAccion, idUsuario, valAntiguos, valNuevos, docRespaldo, usuarioDB)
    SELECT 'AspNetUserTokens', 
    CAST(COALESCE(i.UserId, d.UserId) AS NVARCHAR) + '-' + COALESCE(i.LoginProvider, d.LoginProvider) + '-' + COALESCE(i.Name, d.Name), 
    @action, @idUser,
    (SELECT * FROM deleted d2 WHERE d2.UserId = d.UserId AND d2.LoginProvider = d.LoginProvider AND d2.Name = d.Name FOR JSON AUTO, WITHOUT_ARRAY_WRAPPER),
    (SELECT * FROM inserted i2 WHERE i2.UserId = i.UserId AND i2.LoginProvider = i.LoginProvider AND i2.Name = i.Name FOR JSON AUTO, WITHOUT_ARRAY_WRAPPER),
    @docRef,
    @dbUser
    FROM inserted i FULL OUTER JOIN deleted d ON i.UserId = d.UserId AND i.LoginProvider = d.LoginProvider AND i.Name = d.Name;
END
GO

/* ==================================================================================
   2. TRIGGERS: TABLAS MAESTRAS / CATALOGOS
   ================================================================================== */

CREATE OR ALTER TRIGGER [dbo].[TRG_Audit_Departamento] ON [dbo].[Departamento]
AFTER INSERT, UPDATE, DELETE AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @idUser INT = CAST(SESSION_CONTEXT(N'AuditUserId') AS INT);
    DECLARE @dbUser NVARCHAR(100) = SYSTEM_USER;
    DECLARE @docRef NVARCHAR(100) = CAST(SESSION_CONTEXT(N'AuditDocRef') AS NVARCHAR(100));
    DECLARE @action NVARCHAR(20);
    IF EXISTS (SELECT * FROM inserted) AND EXISTS (SELECT * FROM deleted) SET @action = 'UPDATE';
    ELSE IF EXISTS (SELECT * FROM inserted) SET @action = 'INSERT';
    ELSE IF EXISTS (SELECT * FROM deleted) SET @action = 'DELETE';
    ELSE RETURN;

    INSERT INTO [dbo].[Auditoria] (nombreTabla, idRegistro, tipoAccion, idUsuario, valAntiguos, valNuevos, docRespaldo, usuarioDB)
    SELECT 'Departamento', CAST(COALESCE(i.idDepartamento, d.idDepartamento) AS NVARCHAR(100)), @action, @idUser,
    (SELECT * FROM deleted d2 WHERE d2.idDepartamento = d.idDepartamento FOR JSON AUTO, WITHOUT_ARRAY_WRAPPER),
    (SELECT * FROM inserted i2 WHERE i2.idDepartamento = i.idDepartamento FOR JSON AUTO, WITHOUT_ARRAY_WRAPPER),
    @docRef,
    @dbUser
    FROM inserted i FULL OUTER JOIN deleted d ON i.idDepartamento = d.idDepartamento;
END
GO

CREATE OR ALTER TRIGGER [dbo].[TRG_Audit_Provincia] ON [dbo].[Provincia]
AFTER INSERT, UPDATE, DELETE AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @idUser INT = CAST(SESSION_CONTEXT(N'AuditUserId') AS INT);
    DECLARE @dbUser NVARCHAR(100) = SYSTEM_USER;
    DECLARE @docRef NVARCHAR(100) = CAST(SESSION_CONTEXT(N'AuditDocRef') AS NVARCHAR(100));
    DECLARE @action NVARCHAR(20);
    IF EXISTS (SELECT * FROM inserted) AND EXISTS (SELECT * FROM deleted) SET @action = 'UPDATE';
    ELSE IF EXISTS (SELECT * FROM inserted) SET @action = 'INSERT';
    ELSE IF EXISTS (SELECT * FROM deleted) SET @action = 'DELETE';
    ELSE RETURN;

    INSERT INTO [dbo].[Auditoria] (nombreTabla, idRegistro, tipoAccion, idUsuario, valAntiguos, valNuevos, docRespaldo, usuarioDB)
    SELECT 'Provincia', CAST(COALESCE(i.idProvincia, d.idProvincia) AS NVARCHAR(100)), @action, @idUser,
    (SELECT * FROM deleted d2 WHERE d2.idProvincia = d.idProvincia FOR JSON AUTO, WITHOUT_ARRAY_WRAPPER),
    (SELECT * FROM inserted i2 WHERE i2.idProvincia = i.idProvincia FOR JSON AUTO, WITHOUT_ARRAY_WRAPPER),
    @docRef,
    @dbUser
    FROM inserted i FULL OUTER JOIN deleted d ON i.idProvincia = d.idProvincia;
END
GO

CREATE OR ALTER TRIGGER [dbo].[TRG_Audit_Distrito] ON [dbo].[Distrito]
AFTER INSERT, UPDATE, DELETE AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @idUser INT = CAST(SESSION_CONTEXT(N'AuditUserId') AS INT);
    DECLARE @dbUser NVARCHAR(100) = SYSTEM_USER;
    DECLARE @docRef NVARCHAR(100) = CAST(SESSION_CONTEXT(N'AuditDocRef') AS NVARCHAR(100));
    DECLARE @action NVARCHAR(20);
    IF EXISTS (SELECT * FROM inserted) AND EXISTS (SELECT * FROM deleted) SET @action = 'UPDATE';
    ELSE IF EXISTS (SELECT * FROM inserted) SET @action = 'INSERT';
    ELSE IF EXISTS (SELECT * FROM deleted) SET @action = 'DELETE';
    ELSE RETURN;

    INSERT INTO [dbo].[Auditoria] (nombreTabla, idRegistro, tipoAccion, idUsuario, valAntiguos, valNuevos, docRespaldo, usuarioDB)
    SELECT 'Distrito', CAST(COALESCE(i.idDistrito, d.idDistrito) AS NVARCHAR(100)), @action, @idUser,
    (SELECT * FROM deleted d2 WHERE d2.idDistrito = d.idDistrito FOR JSON AUTO, WITHOUT_ARRAY_WRAPPER),
    (SELECT * FROM inserted i2 WHERE i2.idDistrito = i.idDistrito FOR JSON AUTO, WITHOUT_ARRAY_WRAPPER),
    @docRef,
    @dbUser
    FROM inserted i FULL OUTER JOIN deleted d ON i.idDistrito = d.idDistrito;
END
GO

CREATE OR ALTER TRIGGER [dbo].[TRG_Audit_Direccion] ON [dbo].[Direccion]
AFTER INSERT, UPDATE, DELETE AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @idUser INT = CAST(SESSION_CONTEXT(N'AuditUserId') AS INT);
    DECLARE @dbUser NVARCHAR(100) = SYSTEM_USER;
    DECLARE @docRef NVARCHAR(100) = CAST(SESSION_CONTEXT(N'AuditDocRef') AS NVARCHAR(100));
    DECLARE @action NVARCHAR(20);
    IF EXISTS (SELECT * FROM inserted) AND EXISTS (SELECT * FROM deleted) SET @action = 'UPDATE';
    ELSE IF EXISTS (SELECT * FROM inserted) SET @action = 'INSERT';
    ELSE IF EXISTS (SELECT * FROM deleted) SET @action = 'DELETE';
    ELSE RETURN;

    INSERT INTO [dbo].[Auditoria] (nombreTabla, idRegistro, tipoAccion, idUsuario, valAntiguos, valNuevos, docRespaldo, usuarioDB)
    SELECT 'Direccion', CAST(COALESCE(i.idDistrito, d.idDistrito) AS NVARCHAR(100)), @action, @idUser,
    (SELECT * FROM deleted d2 WHERE d2.idDistrito = d.idDistrito FOR JSON AUTO, WITHOUT_ARRAY_WRAPPER),
    (SELECT * FROM inserted i2 WHERE i2.idDistrito = i.idDistrito FOR JSON AUTO, WITHOUT_ARRAY_WRAPPER),
    @docRef,
    @dbUser
    FROM inserted i FULL OUTER JOIN deleted d ON i.idDistrito = d.idDistrito;
END
GO

CREATE OR ALTER TRIGGER [dbo].[TRG_Audit_TipoDocumento] ON [dbo].[TipoDocumento]
AFTER INSERT, UPDATE, DELETE AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @idUser INT = CAST(SESSION_CONTEXT(N'AuditUserId') AS INT);
    DECLARE @dbUser NVARCHAR(100) = SYSTEM_USER;
    DECLARE @docRef NVARCHAR(100) = CAST(SESSION_CONTEXT(N'AuditDocRef') AS NVARCHAR(100));
    DECLARE @action NVARCHAR(20);
    IF EXISTS (SELECT * FROM inserted) AND EXISTS (SELECT * FROM deleted) SET @action = 'UPDATE';
    ELSE IF EXISTS (SELECT * FROM inserted) SET @action = 'INSERT';
    ELSE IF EXISTS (SELECT * FROM deleted) SET @action = 'DELETE';
    ELSE RETURN;

    INSERT INTO [dbo].[Auditoria] (nombreTabla, idRegistro, tipoAccion, idUsuario, valAntiguos, valNuevos, docRespaldo, usuarioDB)
    SELECT 'TipoDocumento', CAST(COALESCE(i.idTipoDocumento, d.idTipoDocumento) AS NVARCHAR(100)), @action, @idUser,
    (SELECT * FROM deleted d2 WHERE d2.idTipoDocumento = d.idTipoDocumento FOR JSON AUTO, WITHOUT_ARRAY_WRAPPER),
    (SELECT * FROM inserted i2 WHERE i2.idTipoDocumento = i.idTipoDocumento FOR JSON AUTO, WITHOUT_ARRAY_WRAPPER),
    @docRef,
    @dbUser
    FROM inserted i FULL OUTER JOIN deleted d ON i.idTipoDocumento = d.idTipoDocumento;
END
GO

CREATE OR ALTER TRIGGER [dbo].[TRG_Audit_Facultad] ON [dbo].[Facultad]
AFTER INSERT, UPDATE, DELETE AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @idUser INT = CAST(SESSION_CONTEXT(N'AuditUserId') AS INT);
    DECLARE @dbUser NVARCHAR(100) = SYSTEM_USER;
    DECLARE @docRef NVARCHAR(100) = CAST(SESSION_CONTEXT(N'AuditDocRef') AS NVARCHAR(100));
    DECLARE @action NVARCHAR(20);
    IF EXISTS (SELECT * FROM inserted) AND EXISTS (SELECT * FROM deleted) SET @action = 'UPDATE';
    ELSE IF EXISTS (SELECT * FROM inserted) SET @action = 'INSERT';
    ELSE IF EXISTS (SELECT * FROM deleted) SET @action = 'DELETE';
    ELSE RETURN;

    INSERT INTO [dbo].[Auditoria] (nombreTabla, idRegistro, tipoAccion, idUsuario, valAntiguos, valNuevos, docRespaldo, usuarioDB)
    SELECT 'Facultad', CAST(COALESCE(i.idFacultad, d.idFacultad) AS NVARCHAR(100)), @action, @idUser,
    (SELECT * FROM deleted d2 WHERE d2.idFacultad = d.idFacultad FOR JSON AUTO, WITHOUT_ARRAY_WRAPPER),
    (SELECT * FROM inserted i2 WHERE i2.idFacultad = i.idFacultad FOR JSON AUTO, WITHOUT_ARRAY_WRAPPER),
    @docRef,
    @dbUser
    FROM inserted i FULL OUTER JOIN deleted d ON i.idFacultad = d.idFacultad;
END
GO

CREATE OR ALTER TRIGGER [dbo].[TRG_Audit_Escuela] ON [dbo].[Escuela]
AFTER INSERT, UPDATE, DELETE AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @idUser INT = CAST(SESSION_CONTEXT(N'AuditUserId') AS INT);
    DECLARE @dbUser NVARCHAR(100) = SYSTEM_USER;
    DECLARE @docRef NVARCHAR(100) = CAST(SESSION_CONTEXT(N'AuditDocRef') AS NVARCHAR(100));
    DECLARE @action NVARCHAR(20);
    IF EXISTS (SELECT * FROM inserted) AND EXISTS (SELECT * FROM deleted) SET @action = 'UPDATE';
    ELSE IF EXISTS (SELECT * FROM inserted) SET @action = 'INSERT';
    ELSE IF EXISTS (SELECT * FROM deleted) SET @action = 'DELETE';
    ELSE RETURN;

    INSERT INTO [dbo].[Auditoria] (nombreTabla, idRegistro, tipoAccion, idUsuario, valAntiguos, valNuevos, docRespaldo, usuarioDB)
    SELECT 'Escuela', CAST(COALESCE(i.idEscuela, d.idEscuela) AS NVARCHAR(100)), @action, @idUser,
    (SELECT * FROM deleted d2 WHERE d2.idEscuela = d.idEscuela FOR JSON AUTO, WITHOUT_ARRAY_WRAPPER),
    (SELECT * FROM inserted i2 WHERE i2.idEscuela = i.idEscuela FOR JSON AUTO, WITHOUT_ARRAY_WRAPPER),
    @docRef,
    @dbUser
    FROM inserted i FULL OUTER JOIN deleted d ON i.idEscuela = d.idEscuela;
END
GO

CREATE OR ALTER TRIGGER [dbo].[TRG_Audit_Carrera] ON [dbo].[Carrera]
AFTER INSERT, UPDATE, DELETE AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @idUser INT = CAST(SESSION_CONTEXT(N'AuditUserId') AS INT);
    DECLARE @dbUser NVARCHAR(100) = SYSTEM_USER;
    DECLARE @docRef NVARCHAR(100) = CAST(SESSION_CONTEXT(N'AuditDocRef') AS NVARCHAR(100));
    DECLARE @action NVARCHAR(20);
    IF EXISTS (SELECT * FROM inserted) AND EXISTS (SELECT * FROM deleted) SET @action = 'UPDATE';
    ELSE IF EXISTS (SELECT * FROM inserted) SET @action = 'INSERT';
    ELSE IF EXISTS (SELECT * FROM deleted) SET @action = 'DELETE';
    ELSE RETURN;

    INSERT INTO [dbo].[Auditoria] (nombreTabla, idRegistro, tipoAccion, idUsuario, valAntiguos, valNuevos, docRespaldo, usuarioDB)
    SELECT 'Carrera', CAST(COALESCE(i.idCarrera, d.idCarrera) AS NVARCHAR(100)), @action, @idUser,
    (SELECT * FROM deleted d2 WHERE d2.idCarrera = d.idCarrera FOR JSON AUTO, WITHOUT_ARRAY_WRAPPER),
    (SELECT * FROM inserted i2 WHERE i2.idCarrera = i.idCarrera FOR JSON AUTO, WITHOUT_ARRAY_WRAPPER),
    @docRef,
    @dbUser
    FROM inserted i FULL OUTER JOIN deleted d ON i.idCarrera = d.idCarrera;
END
GO

CREATE OR ALTER TRIGGER [dbo].[TRG_Audit_TipoContrato] ON [dbo].[TipoContrato]
AFTER INSERT, UPDATE, DELETE AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @idUser INT = CAST(SESSION_CONTEXT(N'AuditUserId') AS INT);
    DECLARE @dbUser NVARCHAR(100) = SYSTEM_USER;
    DECLARE @docRef NVARCHAR(100) = CAST(SESSION_CONTEXT(N'AuditDocRef') AS NVARCHAR(100));
    DECLARE @action NVARCHAR(20);
    IF EXISTS (SELECT * FROM inserted) AND EXISTS (SELECT * FROM deleted) SET @action = 'UPDATE';
    ELSE IF EXISTS (SELECT * FROM inserted) SET @action = 'INSERT';
    ELSE IF EXISTS (SELECT * FROM deleted) SET @action = 'DELETE';
    ELSE RETURN;

    INSERT INTO [dbo].[Auditoria] (nombreTabla, idRegistro, tipoAccion, idUsuario, valAntiguos, valNuevos, docRespaldo, usuarioDB)
    SELECT 'TipoContrato', CAST(COALESCE(i.idTipoContrato, d.idTipoContrato) AS NVARCHAR(100)), @action, @idUser,
    (SELECT * FROM deleted d2 WHERE d2.idTipoContrato = d.idTipoContrato FOR JSON AUTO, WITHOUT_ARRAY_WRAPPER),
    (SELECT * FROM inserted i2 WHERE i2.idTipoContrato = i.idTipoContrato FOR JSON AUTO, WITHOUT_ARRAY_WRAPPER),
    @docRef,
    @dbUser
    FROM inserted i FULL OUTER JOIN deleted d ON i.idTipoContrato = d.idTipoContrato;
END
GO

CREATE OR ALTER TRIGGER [dbo].[TRG_Audit_ModalidadTrabajo] ON [dbo].[ModalidadTrabajo]
AFTER INSERT, UPDATE, DELETE AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @idUser INT = CAST(SESSION_CONTEXT(N'AuditUserId') AS INT);
    DECLARE @dbUser NVARCHAR(100) = SYSTEM_USER;
    DECLARE @docRef NVARCHAR(100) = CAST(SESSION_CONTEXT(N'AuditDocRef') AS NVARCHAR(100));
    DECLARE @action NVARCHAR(20);
    IF EXISTS (SELECT * FROM inserted) AND EXISTS (SELECT * FROM deleted) SET @action = 'UPDATE';
    ELSE IF EXISTS (SELECT * FROM inserted) SET @action = 'INSERT';
    ELSE IF EXISTS (SELECT * FROM deleted) SET @action = 'DELETE';
    ELSE RETURN;

    INSERT INTO [dbo].[Auditoria] (nombreTabla, idRegistro, tipoAccion, idUsuario, valAntiguos, valNuevos, docRespaldo, usuarioDB)
    SELECT 'ModalidadTrabajo', CAST(COALESCE(i.idModalidadTrabajo, d.idModalidadTrabajo) AS NVARCHAR(100)), @action, @idUser,
    (SELECT * FROM deleted d2 WHERE d2.idModalidadTrabajo = d.idModalidadTrabajo FOR JSON AUTO, WITHOUT_ARRAY_WRAPPER),
    (SELECT * FROM inserted i2 WHERE i2.idModalidadTrabajo = i.idModalidadTrabajo FOR JSON AUTO, WITHOUT_ARRAY_WRAPPER),
    @docRef,
    @dbUser
    FROM inserted i FULL OUTER JOIN deleted d ON i.idModalidadTrabajo = d.idModalidadTrabajo;
END
GO

CREATE OR ALTER TRIGGER [dbo].[TRG_Audit_TipoFormacion] ON [dbo].[TipoFormacion]
AFTER INSERT, UPDATE, DELETE AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @idUser INT = CAST(SESSION_CONTEXT(N'AuditUserId') AS INT);
    DECLARE @dbUser NVARCHAR(100) = SYSTEM_USER;
    DECLARE @docRef NVARCHAR(100) = CAST(SESSION_CONTEXT(N'AuditDocRef') AS NVARCHAR(100));
    DECLARE @action NVARCHAR(20);
    IF EXISTS (SELECT * FROM inserted) AND EXISTS (SELECT * FROM deleted) SET @action = 'UPDATE';
    ELSE IF EXISTS (SELECT * FROM inserted) SET @action = 'INSERT';
    ELSE IF EXISTS (SELECT * FROM deleted) SET @action = 'DELETE';
    ELSE RETURN;

    INSERT INTO [dbo].[Auditoria] (nombreTabla, idRegistro, tipoAccion, idUsuario, valAntiguos, valNuevos, docRespaldo, usuarioDB)
    SELECT 'TipoFormacion', CAST(COALESCE(i.idTipoFormacion, d.idTipoFormacion) AS NVARCHAR(100)), @action, @idUser,
    (SELECT * FROM deleted d2 WHERE d2.idTipoFormacion = d.idTipoFormacion FOR JSON AUTO, WITHOUT_ARRAY_WRAPPER),
    (SELECT * FROM inserted i2 WHERE i2.idTipoFormacion = i.idTipoFormacion FOR JSON AUTO, WITHOUT_ARRAY_WRAPPER),
    @docRef,
    @dbUser
    FROM inserted i FULL OUTER JOIN deleted d ON i.idTipoFormacion = d.idTipoFormacion;
END
GO

CREATE OR ALTER TRIGGER [dbo].[TRG_Audit_CargoAdministrativo] ON [dbo].[CargoAdministrativo]
AFTER INSERT, UPDATE, DELETE AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @idUser INT = CAST(SESSION_CONTEXT(N'AuditUserId') AS INT);
    DECLARE @dbUser NVARCHAR(100) = SYSTEM_USER;
    DECLARE @docRef NVARCHAR(100) = CAST(SESSION_CONTEXT(N'AuditDocRef') AS NVARCHAR(100));
    DECLARE @action NVARCHAR(20);
    IF EXISTS (SELECT * FROM inserted) AND EXISTS (SELECT * FROM deleted) SET @action = 'UPDATE';
    ELSE IF EXISTS (SELECT * FROM inserted) SET @action = 'INSERT';
    ELSE IF EXISTS (SELECT * FROM deleted) SET @action = 'DELETE';
    ELSE RETURN;

    INSERT INTO [dbo].[Auditoria] (nombreTabla, idRegistro, tipoAccion, idUsuario, valAntiguos, valNuevos, docRespaldo, usuarioDB)
    SELECT 'CargoAdministrativo', CAST(COALESCE(i.idCargoAdministrativo, d.idCargoAdministrativo) AS NVARCHAR(100)), @action, @idUser,
    (SELECT * FROM deleted d2 WHERE d2.idCargoAdministrativo = d.idCargoAdministrativo FOR JSON AUTO, WITHOUT_ARRAY_WRAPPER),
    (SELECT * FROM inserted i2 WHERE i2.idCargoAdministrativo = i.idCargoAdministrativo FOR JSON AUTO, WITHOUT_ARRAY_WRAPPER),
    @docRef,
    @dbUser
    FROM inserted i FULL OUTER JOIN deleted d ON i.idCargoAdministrativo = d.idCargoAdministrativo;
END
GO

/* ==================================================================================
   3. TRIGGERS: TABLAS PRINCIPALES DEL NEGOCIO
   ================================================================================== */

CREATE OR ALTER TRIGGER [dbo].[TRG_Audit_Persona] ON [dbo].[Persona]
AFTER INSERT, UPDATE, DELETE AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @idUser INT = CAST(SESSION_CONTEXT(N'AuditUserId') AS INT);
    DECLARE @dbUser NVARCHAR(100) = SYSTEM_USER;
    DECLARE @docRef NVARCHAR(100) = CAST(SESSION_CONTEXT(N'AuditDocRef') AS NVARCHAR(100));
    DECLARE @action NVARCHAR(20);
    IF EXISTS (SELECT * FROM inserted) AND EXISTS (SELECT * FROM deleted) SET @action = 'UPDATE';
    ELSE IF EXISTS (SELECT * FROM inserted) SET @action = 'INSERT';
    ELSE IF EXISTS (SELECT * FROM deleted) SET @action = 'DELETE';
    ELSE RETURN;

    INSERT INTO [dbo].[Auditoria] (nombreTabla, idRegistro, tipoAccion, idUsuario, valAntiguos, valNuevos, docRespaldo, usuarioDB)
    SELECT 'Persona', CAST(COALESCE(i.idPersona, d.idPersona) AS NVARCHAR(100)), @action, @idUser,
    (SELECT * FROM deleted d2 WHERE d2.idPersona = d.idPersona FOR JSON AUTO, WITHOUT_ARRAY_WRAPPER),
    (SELECT * FROM inserted i2 WHERE i2.idPersona = i.idPersona FOR JSON AUTO, WITHOUT_ARRAY_WRAPPER),
    @docRef,
    @dbUser
    FROM inserted i FULL OUTER JOIN deleted d ON i.idPersona = d.idPersona;
END
GO

CREATE OR ALTER TRIGGER [dbo].[TRG_Audit_Empresa] ON [dbo].[Empresa]
AFTER INSERT, UPDATE, DELETE AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @idUser INT = CAST(SESSION_CONTEXT(N'AuditUserId') AS INT);
    DECLARE @dbUser NVARCHAR(100) = SYSTEM_USER;
    DECLARE @docRef NVARCHAR(100) = CAST(SESSION_CONTEXT(N'AuditDocRef') AS NVARCHAR(100));
    DECLARE @action NVARCHAR(20);
    IF EXISTS (SELECT * FROM inserted) AND EXISTS (SELECT * FROM deleted) SET @action = 'UPDATE';
    ELSE IF EXISTS (SELECT * FROM inserted) SET @action = 'INSERT';
    ELSE IF EXISTS (SELECT * FROM deleted) SET @action = 'DELETE';
    ELSE RETURN;

    INSERT INTO [dbo].[Auditoria] (nombreTabla, idRegistro, tipoAccion, idUsuario, valAntiguos, valNuevos, docRespaldo, usuarioDB)
    SELECT 'Empresa', CAST(COALESCE(i.idEmpresa, d.idEmpresa) AS NVARCHAR(100)), @action, @idUser,
    (SELECT * FROM deleted d2 WHERE d2.idEmpresa = d.idEmpresa FOR JSON AUTO, WITHOUT_ARRAY_WRAPPER),
    (SELECT * FROM inserted i2 WHERE i2.idEmpresa = i.idEmpresa FOR JSON AUTO, WITHOUT_ARRAY_WRAPPER),
    @docRef,
    @dbUser
    FROM inserted i FULL OUTER JOIN deleted d ON i.idEmpresa = d.idEmpresa;
END
GO

CREATE OR ALTER TRIGGER [dbo].[TRG_Audit_Egresado] ON [dbo].[Egresado]
AFTER INSERT, UPDATE, DELETE AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @idUser INT = CAST(SESSION_CONTEXT(N'AuditUserId') AS INT);
    DECLARE @dbUser NVARCHAR(100) = SYSTEM_USER;
    DECLARE @docRef NVARCHAR(100) = CAST(SESSION_CONTEXT(N'AuditDocRef') AS NVARCHAR(100));
    DECLARE @action NVARCHAR(20);
    IF EXISTS (SELECT * FROM inserted) AND EXISTS (SELECT * FROM deleted) SET @action = 'UPDATE';
    ELSE IF EXISTS (SELECT * FROM inserted) SET @action = 'INSERT';
    ELSE IF EXISTS (SELECT * FROM deleted) SET @action = 'DELETE';
    ELSE RETURN;

    INSERT INTO [dbo].[Auditoria] (nombreTabla, idRegistro, tipoAccion, idUsuario, valAntiguos, valNuevos, docRespaldo, usuarioDB)
    SELECT 'Egresado', CAST(COALESCE(i.idEgresado, d.idEgresado) AS NVARCHAR(100)), @action, @idUser,
    (SELECT * FROM deleted d2 WHERE d2.idEgresado = d.idEgresado FOR JSON AUTO, WITHOUT_ARRAY_WRAPPER),
    (SELECT * FROM inserted i2 WHERE i2.idEgresado = i.idEgresado FOR JSON AUTO, WITHOUT_ARRAY_WRAPPER),
    @docRef,
    @dbUser
    FROM inserted i FULL OUTER JOIN deleted d ON i.idEgresado = d.idEgresado;
END
GO

CREATE OR ALTER TRIGGER [dbo].[TRG_Audit_Administrativo] ON [dbo].[Administrativo]
AFTER INSERT, UPDATE, DELETE AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @idUser INT = CAST(SESSION_CONTEXT(N'AuditUserId') AS INT);
    DECLARE @dbUser NVARCHAR(100) = SYSTEM_USER;
    DECLARE @docRef NVARCHAR(100) = CAST(SESSION_CONTEXT(N'AuditDocRef') AS NVARCHAR(100));
    DECLARE @action NVARCHAR(20);
    IF EXISTS (SELECT * FROM inserted) AND EXISTS (SELECT * FROM deleted) SET @action = 'UPDATE';
    ELSE IF EXISTS (SELECT * FROM inserted) SET @action = 'INSERT';
    ELSE IF EXISTS (SELECT * FROM deleted) SET @action = 'DELETE';
    ELSE RETURN;

    INSERT INTO [dbo].[Auditoria] (nombreTabla, idRegistro, tipoAccion, idUsuario, valAntiguos, valNuevos, docRespaldo, usuarioDB)
    SELECT 'Administrativo', CAST(COALESCE(i.idAdministrativo, d.idAdministrativo) AS NVARCHAR(100)), @action, @idUser,
    (SELECT * FROM deleted d2 WHERE d2.idAdministrativo = d.idAdministrativo FOR JSON AUTO, WITHOUT_ARRAY_WRAPPER),
    (SELECT * FROM inserted i2 WHERE i2.idAdministrativo = i.idAdministrativo FOR JSON AUTO, WITHOUT_ARRAY_WRAPPER),
    @docRef,
    @dbUser
    FROM inserted i FULL OUTER JOIN deleted d ON i.idAdministrativo = d.idAdministrativo;
END
GO

CREATE OR ALTER TRIGGER [dbo].[TRG_Audit_Representante] ON [dbo].[Representante]
AFTER INSERT, UPDATE, DELETE AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @idUser INT = CAST(SESSION_CONTEXT(N'AuditUserId') AS INT);
    DECLARE @dbUser NVARCHAR(100) = SYSTEM_USER;
    DECLARE @docRef NVARCHAR(100) = CAST(SESSION_CONTEXT(N'AuditDocRef') AS NVARCHAR(100));
    DECLARE @action NVARCHAR(20);
    IF EXISTS (SELECT * FROM inserted) AND EXISTS (SELECT * FROM deleted) SET @action = 'UPDATE';
    ELSE IF EXISTS (SELECT * FROM inserted) SET @action = 'INSERT';
    ELSE IF EXISTS (SELECT * FROM deleted) SET @action = 'DELETE';
    ELSE RETURN;

    INSERT INTO [dbo].[Auditoria] (nombreTabla, idRegistro, tipoAccion, idUsuario, valAntiguos, valNuevos, docRespaldo, usuarioDB)
    SELECT 'Representante', CAST(COALESCE(i.idRepresentante, d.idRepresentante) AS NVARCHAR(100)), @action, @idUser,
    (SELECT * FROM deleted d2 WHERE d2.idRepresentante = d.idRepresentante FOR JSON AUTO, WITHOUT_ARRAY_WRAPPER),
    (SELECT * FROM inserted i2 WHERE i2.idRepresentante = i.idRepresentante FOR JSON AUTO, WITHOUT_ARRAY_WRAPPER),
    @docRef,
    @dbUser
    FROM inserted i FULL OUTER JOIN deleted d ON i.idRepresentante = d.idRepresentante;
END
GO

/* ==================================================================================
   4. TRIGGERS: TABLAS TRANSACCIONALES
   ================================================================================== */

CREATE OR ALTER TRIGGER [dbo].[TRG_Audit_OfertaLaboral] ON [dbo].[OfertaLaboral]
AFTER INSERT, UPDATE, DELETE AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @idUser INT = CAST(SESSION_CONTEXT(N'AuditUserId') AS INT);
    DECLARE @dbUser NVARCHAR(100) = SYSTEM_USER;
    DECLARE @docRef NVARCHAR(100) = CAST(SESSION_CONTEXT(N'AuditDocRef') AS NVARCHAR(100));
    DECLARE @action NVARCHAR(20);
    IF EXISTS (SELECT * FROM inserted) AND EXISTS (SELECT * FROM deleted) SET @action = 'UPDATE';
    ELSE IF EXISTS (SELECT * FROM inserted) SET @action = 'INSERT';
    ELSE IF EXISTS (SELECT * FROM deleted) SET @action = 'DELETE';
    ELSE RETURN;

    INSERT INTO [dbo].[Auditoria] (nombreTabla, idRegistro, tipoAccion, idUsuario, valAntiguos, valNuevos, docRespaldo, usuarioDB)
    SELECT 'OfertaLaboral', CAST(COALESCE(i.idOferta, d.idOferta) AS NVARCHAR(100)), @action, @idUser,
    (SELECT * FROM deleted d2 WHERE d2.idOferta = d.idOferta FOR JSON AUTO, WITHOUT_ARRAY_WRAPPER),
    (SELECT * FROM inserted i2 WHERE i2.idOferta = i.idOferta FOR JSON AUTO, WITHOUT_ARRAY_WRAPPER),
    @docRef,
    @dbUser
    FROM inserted i FULL OUTER JOIN deleted d ON i.idOferta = d.idOferta;
END
GO

CREATE OR ALTER TRIGGER [dbo].[TRG_Audit_Postulacion] ON [dbo].[Postulacion]
AFTER INSERT, UPDATE, DELETE AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @idUser INT = CAST(SESSION_CONTEXT(N'AuditUserId') AS INT);
    DECLARE @dbUser NVARCHAR(100) = SYSTEM_USER;
    DECLARE @docRef NVARCHAR(100) = CAST(SESSION_CONTEXT(N'AuditDocRef') AS NVARCHAR(100));
    DECLARE @action NVARCHAR(20);
    IF EXISTS (SELECT * FROM inserted) AND EXISTS (SELECT * FROM deleted) SET @action = 'UPDATE';
    ELSE IF EXISTS (SELECT * FROM inserted) SET @action = 'INSERT';
    ELSE IF EXISTS (SELECT * FROM deleted) SET @action = 'DELETE';
    ELSE RETURN;

    INSERT INTO [dbo].[Auditoria] (nombreTabla, idRegistro, tipoAccion, idUsuario, valAntiguos, valNuevos, docRespaldo, usuarioDB)
    SELECT 'Postulacion', CAST(COALESCE(i.idPostulacion, d.idPostulacion) AS NVARCHAR(100)), @action, @idUser,
    (SELECT * FROM deleted d2 WHERE d2.idPostulacion = d.idPostulacion FOR JSON AUTO, WITHOUT_ARRAY_WRAPPER),
    (SELECT * FROM inserted i2 WHERE i2.idPostulacion = i.idPostulacion FOR JSON AUTO, WITHOUT_ARRAY_WRAPPER),
    @docRef,
    @dbUser
    FROM inserted i FULL OUTER JOIN deleted d ON i.idPostulacion = d.idPostulacion;
END
GO

CREATE OR ALTER TRIGGER [dbo].[TRG_Audit_ExperienciaLaboral] ON [dbo].[ExperienciaLaboral]
AFTER INSERT, UPDATE, DELETE AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @idUser INT = CAST(SESSION_CONTEXT(N'AuditUserId') AS INT);
    DECLARE @dbUser NVARCHAR(100) = SYSTEM_USER;
    DECLARE @docRef NVARCHAR(100) = CAST(SESSION_CONTEXT(N'AuditDocRef') AS NVARCHAR(100));
    DECLARE @action NVARCHAR(20);
    IF EXISTS (SELECT * FROM inserted) AND EXISTS (SELECT * FROM deleted) SET @action = 'UPDATE';
    ELSE IF EXISTS (SELECT * FROM inserted) SET @action = 'INSERT';
    ELSE IF EXISTS (SELECT * FROM deleted) SET @action = 'DELETE';
    ELSE RETURN;

    INSERT INTO [dbo].[Auditoria] (nombreTabla, idRegistro, tipoAccion, idUsuario, valAntiguos, valNuevos, docRespaldo, usuarioDB)
    SELECT 'ExperienciaLaboral', CAST(COALESCE(i.idExperiencia, d.idExperiencia) AS NVARCHAR(100)), @action, @idUser,
    (SELECT * FROM deleted d2 WHERE d2.idExperiencia = d.idExperiencia FOR JSON AUTO, WITHOUT_ARRAY_WRAPPER),
    (SELECT * FROM inserted i2 WHERE i2.idExperiencia = i.idExperiencia FOR JSON AUTO, WITHOUT_ARRAY_WRAPPER),
    @docRef,
    @dbUser
    FROM inserted i FULL OUTER JOIN deleted d ON i.idExperiencia = d.idExperiencia;
END
GO

CREATE OR ALTER TRIGGER [dbo].[TRG_Audit_FormacionComplementaria] ON [dbo].[FormacionComplementaria]
AFTER INSERT, UPDATE, DELETE AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @idUser INT = CAST(SESSION_CONTEXT(N'AuditUserId') AS INT);
    DECLARE @dbUser NVARCHAR(100) = SYSTEM_USER;
    DECLARE @docRef NVARCHAR(100) = CAST(SESSION_CONTEXT(N'AuditDocRef') AS NVARCHAR(100));
    DECLARE @action NVARCHAR(20);
    IF EXISTS (SELECT * FROM inserted) AND EXISTS (SELECT * FROM deleted) SET @action = 'UPDATE';
    ELSE IF EXISTS (SELECT * FROM inserted) SET @action = 'INSERT';
    ELSE IF EXISTS (SELECT * FROM deleted) SET @action = 'DELETE';
    ELSE RETURN;

    INSERT INTO [dbo].[Auditoria] (nombreTabla, idRegistro, tipoAccion, idUsuario, valAntiguos, valNuevos, docRespaldo, usuarioDB)
    SELECT 'FormacionComplementaria', CAST(COALESCE(i.idFormacion, d.idFormacion) AS NVARCHAR(100)), @action, @idUser,
    (SELECT * FROM deleted d2 WHERE d2.idFormacion = d.idFormacion FOR JSON AUTO, WITHOUT_ARRAY_WRAPPER),
    (SELECT * FROM inserted i2 WHERE i2.idFormacion = i.idFormacion FOR JSON AUTO, WITHOUT_ARRAY_WRAPPER),
    @docRef,
    @dbUser
    FROM inserted i FULL OUTER JOIN deleted d ON i.idFormacion = d.idFormacion;
END
GO