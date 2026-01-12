--AUDITORÍA (Triggers)
--Cumple con el requisito de registrar el SYSTEM_USER y guardar el estado anterior y nuevo

-- Tabla de Auditoría
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Auditoria]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Auditoria](
        idAuditoria INT IDENTITY(1,1) PRIMARY KEY,
        tablaAfectada NVARCHAR(50) NOT NULL,
        accion NVARCHAR(20) NOT NULL, -- INSERT, UPDATE, DELETE
        usuarioSQL NVARCHAR(100) DEFAULT SYSTEM_USER, -- <--- REQUISITO DEL COMPAÑERO
        fechaHora DATETIME DEFAULT GETDATE(),
        datosAntiguos NVARCHAR(MAX) NULL,
        datosNuevos NVARCHAR(MAX) NULL
    );
END
GO

-- Trigger Dinámico (Ejemplo para Empresa, replica para otras tablas cambiando el nombre)
CREATE OR ALTER TRIGGER [dbo].[trg_Auditoria_Empresa] ON [dbo].[Empresa] AFTER INSERT, UPDATE, DELETE AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Accion NVARCHAR(20);
    IF EXISTS (SELECT * FROM inserted) AND EXISTS (SELECT * FROM deleted) SET @Accion = 'UPDATE';
    ELSE IF EXISTS (SELECT * FROM inserted) SET @Accion = 'INSERT';
    ELSE IF EXISTS (SELECT * FROM deleted) SET @Accion = 'DELETE';
    ELSE RETURN;

    INSERT INTO [dbo].[Auditoria] (tablaAfectada, accion, usuarioSQL, datosAntiguos, datosNuevos)
    SELECT 'Empresa', @Accion, SYSTEM_USER, 
           (SELECT * FROM deleted FOR JSON AUTO), 
           (SELECT * FROM inserted FOR JSON AUTO);
END
GO
-- NOTA: Debes crear triggers similares para OfertaLaboral y Postulacion copiando este bloque y cambiando el nombre de la tabla.

--TRIGGERS FALTANTES a Auditoria

-- ==================================================================================
-- 1. TRIGGER PARA AUDITORÍA DE OFERTAS LABORALES
-- ==================================================================================
-- Registra cambios en sueldos, fechas, descripciones o estados (ej. si se cancela una oferta).
CREATE OR ALTER TRIGGER [dbo].[trg_Auditoria_OfertaLaboral] 
ON [dbo].[OfertaLaboral] 
AFTER INSERT, UPDATE, DELETE 
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @Accion NVARCHAR(20);
    DECLARE @DatosAntiguos NVARCHAR(MAX) = NULL;
    DECLARE @DatosNuevos NVARCHAR(MAX) = NULL;

    -- Determinar tipo de acción
    IF EXISTS (SELECT * FROM inserted) AND EXISTS (SELECT * FROM deleted)
        SET @Accion = 'UPDATE';
    ELSE IF EXISTS (SELECT * FROM inserted)
        SET @Accion = 'INSERT';
    ELSE IF EXISTS (SELECT * FROM deleted)
        SET @Accion = 'DELETE';
    ELSE
        RETURN;

    -- Capturar datos (JSON)
    IF @Accion IN ('UPDATE', 'DELETE')
        SET @DatosAntiguos = (SELECT * FROM deleted FOR JSON AUTO);
        
    IF @Accion IN ('INSERT', 'UPDATE')
        SET @DatosNuevos = (SELECT * FROM inserted FOR JSON AUTO);

    -- Insertar en Auditoría
    INSERT INTO [dbo].[Auditoria] (tablaAfectada, accion, usuarioSQL, datosAntiguos, datosNuevos)
    VALUES ('OfertaLaboral', @Accion, SYSTEM_USER, @DatosAntiguos, @DatosNuevos);
END
GO

-- ==================================================================================
-- 2. TRIGGER PARA AUDITORÍA DE POSTULACIONES
-- ==================================================================================
-- Fundamental para rastrear cuando un alumno es Aceptado/Rechazado y el feedback recibido.
CREATE OR ALTER TRIGGER [dbo].[trg_Auditoria_Postulacion] 
ON [dbo].[Postulacion] 
AFTER INSERT, UPDATE, DELETE 
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @Accion NVARCHAR(20);
    DECLARE @DatosAntiguos NVARCHAR(MAX) = NULL;
    DECLARE @DatosNuevos NVARCHAR(MAX) = NULL;

    -- Determinar tipo de acción
    IF EXISTS (SELECT * FROM inserted) AND EXISTS (SELECT * FROM deleted)
        SET @Accion = 'UPDATE';
    ELSE IF EXISTS (SELECT * FROM inserted)
        SET @Accion = 'INSERT';
    ELSE IF EXISTS (SELECT * FROM deleted)
        SET @Accion = 'DELETE';
    ELSE
        RETURN;

    -- Capturar datos (JSON)
    IF @Accion IN ('UPDATE', 'DELETE')
        SET @DatosAntiguos = (SELECT * FROM deleted FOR JSON AUTO);
        
    IF @Accion IN ('INSERT', 'UPDATE')
        SET @DatosNuevos = (SELECT * FROM inserted FOR JSON AUTO);

    -- Insertar en Auditoría
    INSERT INTO [dbo].[Auditoria] (tablaAfectada, accion, usuarioSQL, datosAntiguos, datosNuevos)
    VALUES ('Postulacion', @Accion, SYSTEM_USER, @DatosAntiguos, @DatosNuevos);
END
GO

PRINT '=== SISTEMA DE AUDITORÍA Y TRIGGERS ==='
GO

-- 1. CREACIÓN DE TABLA AUDITORÍA
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Auditoria]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Auditoria](
        [idAuditoria] INT IDENTITY(1,1) PRIMARY KEY,
        [tablaAfectada] NVARCHAR(100) NOT NULL,
        [accion] NVARCHAR(20) NOT NULL,
        [usuarioSQL] NVARCHAR(100) DEFAULT (SYSTEM_USER),
        [fechaHora] DATETIME2(7) DEFAULT (GETDATE()),
        [datosAntiguos] NVARCHAR(MAX) NULL,
        [datosNuevos] NVARCHAR(MAX) NULL
    );
END
ELSE
BEGIN
    -- Asegurar que existan las columnas si la tabla ya existía
    IF COL_LENGTH('dbo.Auditoria', 'usuarioSQL') IS NULL
        ALTER TABLE [dbo].[Auditoria] ADD [usuarioSQL] NVARCHAR(100) DEFAULT (SYSTEM_USER);
    IF COL_LENGTH('dbo.Auditoria', 'datosAntiguos') IS NULL
        ALTER TABLE [dbo].[Auditoria] ADD [datosAntiguos] NVARCHAR(MAX) NULL;
    IF COL_LENGTH('dbo.Auditoria', 'datosNuevos') IS NULL
        ALTER TABLE [dbo].[Auditoria] ADD [datosNuevos] NVARCHAR(MAX) NULL;
END
GO

-- 2. TRIGGERS DE AUDITORÍA (Uno por tabla)

-- A. AUDITORÍA PERSONA
CREATE OR ALTER TRIGGER [dbo].[trg_Aud_Persona] ON [dbo].[Persona] AFTER INSERT, UPDATE, DELETE AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @acc NVARCHAR(20) = CASE WHEN EXISTS(SELECT * FROM inserted) AND EXISTS(SELECT * FROM deleted) THEN 'UPDATE' WHEN EXISTS(SELECT * FROM inserted) THEN 'INSERT' ELSE 'DELETE' END;
    INSERT INTO Auditoria (tablaAfectada, accion, usuarioSQL, datosAntiguos, datosNuevos)
    SELECT 'Persona', @acc, SYSTEM_USER, (SELECT * FROM deleted FOR JSON AUTO), (SELECT * FROM inserted FOR JSON AUTO) FROM (SELECT 1 as c) as t;
END; GO

-- B. AUDITORÍA EMPRESA
CREATE OR ALTER TRIGGER [dbo].[trg_Aud_Empresa] ON [dbo].[Empresa] AFTER INSERT, UPDATE, DELETE AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @acc NVARCHAR(20) = CASE WHEN EXISTS(SELECT * FROM inserted) AND EXISTS(SELECT * FROM deleted) THEN 'UPDATE' WHEN EXISTS(SELECT * FROM inserted) THEN 'INSERT' ELSE 'DELETE' END;
    INSERT INTO Auditoria (tablaAfectada, accion, usuarioSQL, datosAntiguos, datosNuevos)
    SELECT 'Empresa', @acc, SYSTEM_USER, (SELECT * FROM deleted FOR JSON AUTO), (SELECT * FROM inserted FOR JSON AUTO) FROM (SELECT 1 as c) as t;
END; GO

-- C. AUDITORÍA OFERTA LABORAL
CREATE OR ALTER TRIGGER [dbo].[trg_Aud_Oferta] ON [dbo].[OfertaLaboral] AFTER INSERT, UPDATE, DELETE AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @acc NVARCHAR(20) = CASE WHEN EXISTS(SELECT * FROM inserted) AND EXISTS(SELECT * FROM deleted) THEN 'UPDATE' WHEN EXISTS(SELECT * FROM inserted) THEN 'INSERT' ELSE 'DELETE' END;
    INSERT INTO Auditoria (tablaAfectada, accion, usuarioSQL, datosAntiguos, datosNuevos)
    SELECT 'OfertaLaboral', @acc, SYSTEM_USER, (SELECT * FROM deleted FOR JSON AUTO), (SELECT * FROM inserted FOR JSON AUTO) FROM (SELECT 1 as c) as t;
END; GO

-- D. AUDITORÍA POSTULACIÓN
CREATE OR ALTER TRIGGER [dbo].[trg_Aud_Postulacion] ON [dbo].[Postulacion] AFTER INSERT, UPDATE, DELETE AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @acc NVARCHAR(20) = CASE WHEN EXISTS(SELECT * FROM inserted) AND EXISTS(SELECT * FROM deleted) THEN 'UPDATE' WHEN EXISTS(SELECT * FROM inserted) THEN 'INSERT' ELSE 'DELETE' END;
    INSERT INTO Auditoria (tablaAfectada, accion, usuarioSQL, datosAntiguos, datosNuevos)
    SELECT 'Postulacion', @acc, SYSTEM_USER, (SELECT * FROM deleted FOR JSON AUTO), (SELECT * FROM inserted FOR JSON AUTO) FROM (SELECT 1 as c) as t;
END; GO

-- E. AUDITORÍA EXPERIENCIA LABORAL
CREATE OR ALTER TRIGGER [dbo].[trg_Aud_Experiencia] ON [dbo].[ExperienciaLaboral] AFTER INSERT, UPDATE, DELETE AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @acc NVARCHAR(20) = CASE WHEN EXISTS(SELECT * FROM inserted) AND EXISTS(SELECT * FROM deleted) THEN 'UPDATE' WHEN EXISTS(SELECT * FROM inserted) THEN 'INSERT' ELSE 'DELETE' END;
    INSERT INTO Auditoria (tablaAfectada, accion, usuarioSQL, datosAntiguos, datosNuevos)
    SELECT 'ExperienciaLaboral', @acc, SYSTEM_USER, (SELECT * FROM deleted FOR JSON AUTO), (SELECT * FROM inserted FOR JSON AUTO) FROM (SELECT 1 as c) as t;
END; GO