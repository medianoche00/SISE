AUDITORÍA (Triggers)
Cumple con el requisito de registrar el SYSTEM_USER y guardar el estado anterior y nuevo

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

TRIGGERS FALTANTES a Auditoria

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