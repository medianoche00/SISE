PRINT '=== DEFAULT VALUES ==='
GO

-- 1. Auditoría: Fecha automática al momento de insertar
IF NOT EXISTS (SELECT * FROM sys.default_constraints WHERE name = 'DF_Auditoria_Fecha')
BEGIN
    ALTER TABLE [dbo].[Auditoria] ADD CONSTRAINT [DF_Auditoria_Fecha] DEFAULT (GETDATE()) FOR [fechaHora];
END

-- 2. Postulación: Fecha automática de postulación y Estado inicial
IF NOT EXISTS (SELECT * FROM sys.default_constraints WHERE name = 'DF_Postulacion_Fecha')
BEGIN
    ALTER TABLE [dbo].[Postulacion] ADD CONSTRAINT [DF_Postulacion_Fecha] DEFAULT (GETDATE()) FOR [fechaPostulacion];
END

IF NOT EXISTS (SELECT * FROM sys.default_constraints WHERE name = 'DF_Postulacion_Estado')
BEGIN
    -- Estado inicial por defecto siempre será 'Pendiente'
    ALTER TABLE [dbo].[Postulacion] ADD CONSTRAINT [DF_Postulacion_Estado] DEFAULT ('Pendiente') FOR [estado];
END

-- 3. Oferta Laboral: Fecha de publicación por defecto HOY si no se envía
IF NOT EXISTS (SELECT * FROM sys.default_constraints WHERE name = 'DF_Oferta_FechaPub')
BEGIN
    ALTER TABLE [dbo].[OfertaLaboral] ADD CONSTRAINT [DF_Oferta_FechaPub] DEFAULT (GETDATE()) FOR [fechaPublicacion];
END
GO

PRINT '=== CHECK CONSTRAINTS ==='
GO

-- 1. Oferta Laboral: Validaciones de fechas y sueldo
ALTER TABLE [dbo].[OfertaLaboral] WITH CHECK ADD CONSTRAINT [CK_Oferta_Fechas] 
CHECK ([fechaCierre] >= [fechaPublicacion]); -- La oferta no puede cerrar antes de publicarse

ALTER TABLE [dbo].[OfertaLaboral] WITH CHECK ADD CONSTRAINT [CK_Oferta_Sueldo] 
CHECK ([sueldo] >= 0); -- No existen sueldos negativos

-- 2. Persona: Validación básica de documentos
ALTER TABLE [dbo].[Persona] WITH CHECK ADD CONSTRAINT [CK_Persona_DNI] 
CHECK (LEN([documentoIdentidad]) >= 8); -- DNI/Carnet debe tener longitud mínima razonable

-- 3. Experiencia Laboral: Fechas coherentes
ALTER TABLE [dbo].[ExperienciaLaboral] WITH CHECK ADD CONSTRAINT [CK_Experiencia_Fechas] 
CHECK ([fechaFin] IS NULL OR [fechaFin] >= [fechaInicio]); -- Si hay fecha fin, debe ser mayor al inicio

-- 4. ESTADOS COMO CONSTRAINTS (Restricción de valores permitidos)
-- En tu tabla 'Postulacion', el estado es nvarchar. Aquí forzamos que solo acepte valores válidos.
ALTER TABLE [dbo].[Postulacion] WITH CHECK ADD CONSTRAINT [CK_Postulacion_EstadoValido] 
CHECK ([estado] IN ('Pendiente', 'En Revisión', 'Entrevista', 'Finalista', 'Seleccionado', 'Rechazado', 'Cancelado'));

-- 5. Egresado: Año de egreso lógico
ALTER TABLE [dbo].[Egresado] WITH CHECK ADD CONSTRAINT [CK_Egresado_Anio] 
CHECK ([añoEgreso] BETWEEN 1950 AND YEAR(GETDATE()) + 1); -- Año válido
GO

PRINT '=== REGLAS ON DELETE ==='
GO

/* ESTRATEGIA:
   1. Datos Maestros (Carrera, Facultad) -> NO ACTION (No borrar si hay alumnos usándolos)
   2. Detalle de Usuario (Experiencia, Formación) -> CASCADE (Si borro al egresado, se borra su experiencia)
   3. Transaccional (Postulaciones) -> SET NULL o NO ACTION (Para no perder historial histórico)
*/

-- ---------------------------------------------------------
-- A. EXPERIENCIA LABORAL (Si se elimina el Egresado, se elimina su experiencia)
-- ---------------------------------------------------------
IF OBJECT_ID('FK__Experienc__idEgr__07C12930', 'F') IS NOT NULL 
    ALTER TABLE [dbo].[ExperienciaLaboral] DROP CONSTRAINT [FK__Experienc__idEgr__07C12930];

ALTER TABLE [dbo].[ExperienciaLaboral] WITH CHECK ADD CONSTRAINT [FK_Experiencia_Egresado_Cascade] 
FOREIGN KEY([idEgresado]) REFERENCES [dbo].[Egresado] ([idEgresado])
ON DELETE CASCADE; -- <--- CAMBIO IMPORTANTE


-- ---------------------------------------------------------
-- B. FORMACIÓN COMPLEMENTARIA (Si se elimina el Egresado, se elimina su formación)
-- ---------------------------------------------------------
IF OBJECT_ID('FK__Formacion__idEgr__08B54D69', 'F') IS NOT NULL 
    ALTER TABLE [dbo].[FormacionComplementaria] DROP CONSTRAINT [FK__Formacion__idEgr__08B54D69];

ALTER TABLE [dbo].[FormacionComplementaria] WITH CHECK ADD CONSTRAINT [FK_Formacion_Egresado_Cascade] 
FOREIGN KEY([idEgresado]) REFERENCES [dbo].[Egresado] ([idEgresado])
ON DELETE CASCADE;


-- ---------------------------------------------------------
-- C. POSTULACIÓN (Protección de Integridad)
-- Si se borra una Oferta, las postulaciones podrían quedar huérfanas o borrarse.
-- Generalmente se prefiere NO borrar postulaciones para mantener historial, 
-- pero si es una limpieza dura, usamos CASCADE. Aquí usaré CASCADE para consistencia.
-- ---------------------------------------------------------
IF OBJECT_ID('FK_Postulacion_Oferta', 'F') IS NOT NULL 
    ALTER TABLE [dbo].[Postulacion] DROP CONSTRAINT [FK_Postulacion_Oferta];

ALTER TABLE [dbo].[Postulacion] WITH CHECK ADD CONSTRAINT [FK_Postulacion_Oferta_Cascade] 
FOREIGN KEY([idOferta]) REFERENCES [dbo].[OfertaLaboral] ([idOferta])
ON DELETE CASCADE;


-- ---------------------------------------------------------
-- D. ROLES DE USUARIO (Si borro la Persona o el User de Identity)
-- El script original de Identity ya tiene Cascades, pero reforzamos tus tablas personalizadas.
-- ---------------------------------------------------------

-- Egresado -> Usuario
IF OBJECT_ID('FK_Egresado_Usuario', 'F') IS NOT NULL 
    ALTER TABLE [dbo].[Egresado] DROP CONSTRAINT [FK_Egresado_Usuario];

ALTER TABLE [dbo].[Egresado] WITH CHECK ADD CONSTRAINT [FK_Egresado_Usuario_Cascade] 
FOREIGN KEY([idUsuario]) REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE;

-- Administrativo -> Usuario
IF OBJECT_ID('FK_Administrativo_Usuario', 'F') IS NOT NULL 
    ALTER TABLE [dbo].[Administrativo] DROP CONSTRAINT [FK_Administrativo_Usuario];

ALTER TABLE [dbo].[Administrativo] WITH CHECK ADD CONSTRAINT [FK_Administrativo_Usuario_Cascade] 
FOREIGN KEY([idUsuario]) REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE;

GO
PRINT '=== PROCESO COMPLETADO EXITOSAMENTE ==='

USE [SiseDB];
GO

/* ==================================================================================
   1. BLINDAJE DE DATOS (CONSTRAINTS / CHECKS)
   ================================================================================== */
PRINT 'Aplicando blindaje de datos (Checks)...';

-- PERSONA: DNI exacto (8 números) y Género restringido
ALTER TABLE [dbo].[Persona] DROP CONSTRAINT IF EXISTS [CK_Persona_DNI_Formato];
ALTER TABLE [dbo].[Persona] WITH CHECK ADD CONSTRAINT [CK_Persona_DNI_Formato] 
CHECK ([documentoIdentidad] LIKE '[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]');

ALTER TABLE [dbo].[Persona] DROP CONSTRAINT IF EXISTS [CK_Persona_Genero];
ALTER TABLE [dbo].[Persona] WITH CHECK ADD CONSTRAINT [CK_Persona_Genero] 
CHECK ([genero] IN ('Masculino', 'Femenino', 'Otro'));

-- EMPRESA: RUC exacto (11 números)
ALTER TABLE [dbo].[Empresa] DROP CONSTRAINT IF EXISTS [CK_Empresa_RUC_Formato];
ALTER TABLE [dbo].[Empresa] WITH CHECK ADD CONSTRAINT [CK_Empresa_RUC_Formato] 
CHECK ([ruc] LIKE '[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]');

-- OFERTA LABORAL: Sueldo positivo y coherencia de fechas
ALTER TABLE [dbo].[OfertaLaboral] DROP CONSTRAINT IF EXISTS [CK_Oferta_Sueldo_Positivo];
ALTER TABLE [dbo].[OfertaLaboral] WITH CHECK ADD CONSTRAINT [CK_Oferta_Sueldo_Positivo] CHECK ([sueldo] > 0);

ALTER TABLE [dbo].[OfertaLaboral] DROP CONSTRAINT IF EXISTS [CK_Oferta_Fechas_Logicas];
ALTER TABLE [dbo].[OfertaLaboral] WITH CHECK ADD CONSTRAINT [CK_Oferta_Fechas_Logicas] 
CHECK ([fechaCierre] >= [fechaPublicacion]);

-- EGRESADO: Año de egreso lógico
ALTER TABLE [dbo].[Egresado] DROP CONSTRAINT IF EXISTS [CK_Egresado_Anio_Real];
ALTER TABLE [dbo].[Egresado] WITH CHECK ADD CONSTRAINT [CK_Egresado_Anio_Real] 
CHECK ([anioEgreso] BETWEEN 1960 AND YEAR(GETDATE()));

-- UNICIDAD (Evitar duplicados)
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'UQ_Persona_DNI')
    ALTER TABLE [dbo].[Persona] ADD CONSTRAINT [UQ_Persona_DNI] UNIQUE ([documentoIdentidad]);

GO

/* ==================================================================================
   2. SISTEMA INTEGRAL DE AUDITORÍA (TRIGGERS EN TODAS LAS TABLAS)
   ================================================================================== */
PRINT 'Configurando Auditoría Integral (Usuario SQL + JSON)...';

-- Asegurar tabla de Auditoría unificada
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
GO

-- Procedimiento interno para reducir código en Triggers (Opcional, pero para orden)
-- Aquí definimos los Triggers uno por uno para cada tabla:

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

-- C. AUDITORÍA EGRESADO
CREATE OR ALTER TRIGGER [dbo].[trg_Aud_Egresado] ON [dbo].[Egresado] AFTER INSERT, UPDATE, DELETE AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @acc NVARCHAR(20) = CASE WHEN EXISTS(SELECT * FROM inserted) AND EXISTS(SELECT * FROM deleted) THEN 'UPDATE' WHEN EXISTS(SELECT * FROM inserted) THEN 'INSERT' ELSE 'DELETE' END;
    INSERT INTO Auditoria (tablaAfectada, accion, usuarioSQL, datosAntiguos, datosNuevos)
    SELECT 'Egresado', @acc, SYSTEM_USER, (SELECT * FROM deleted FOR JSON AUTO), (SELECT * FROM inserted FOR JSON AUTO) FROM (SELECT 1 as c) as t;
END; GO

-- D. AUDITORÍA OFERTA LABORAL
CREATE OR ALTER TRIGGER [dbo].[trg_Aud_Oferta] ON [dbo].[OfertaLaboral] AFTER INSERT, UPDATE, DELETE AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @acc NVARCHAR(20) = CASE WHEN EXISTS(SELECT * FROM inserted) AND EXISTS(SELECT * FROM deleted) THEN 'UPDATE' WHEN EXISTS(SELECT * FROM inserted) THEN 'INSERT' ELSE 'DELETE' END;
    INSERT INTO Auditoria (tablaAfectada, accion, usuarioSQL, datosAntiguos, datosNuevos)
    SELECT 'OfertaLaboral', @acc, SYSTEM_USER, (SELECT * FROM deleted FOR JSON AUTO), (SELECT * FROM inserted FOR JSON AUTO) FROM (SELECT 1 as c) as t;
END; GO

-- E. AUDITORÍA POSTULACIÓN (Crítico para cambios de estado de selección)
CREATE OR ALTER TRIGGER [dbo].[trg_Aud_Postulacion] ON [dbo].[Postulacion] AFTER INSERT, UPDATE, DELETE AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @acc NVARCHAR(20) = CASE WHEN EXISTS(SELECT * FROM inserted) AND EXISTS(SELECT * FROM deleted) THEN 'UPDATE' WHEN EXISTS(SELECT * FROM inserted) THEN 'INSERT' ELSE 'DELETE' END;
    INSERT INTO Auditoria (tablaAfectada, accion, usuarioSQL, datosAntiguos, datosNuevos)
    SELECT 'Postulacion', @acc, SYSTEM_USER, (SELECT * FROM deleted FOR JSON AUTO), (SELECT * FROM inserted FOR JSON AUTO) FROM (SELECT 1 as c) as t;
END; GO

-- F. AUDITORÍA EXPERIENCIA LABORAL
CREATE OR ALTER TRIGGER [dbo].[trg_Aud_Experiencia] ON [dbo].[ExperienciaLaboral] AFTER INSERT, UPDATE, DELETE AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @acc NVARCHAR(20) = CASE WHEN EXISTS(SELECT * FROM inserted) AND EXISTS(SELECT * FROM deleted) THEN 'UPDATE' WHEN EXISTS(SELECT * FROM inserted) THEN 'INSERT' ELSE 'DELETE' END;
    INSERT INTO Auditoria (tablaAfectada, accion, usuarioSQL, datosAntiguos, datosNuevos)
    SELECT 'ExperienciaLaboral', @acc, SYSTEM_USER, (SELECT * FROM deleted FOR JSON AUTO), (SELECT * FROM inserted FOR JSON AUTO) FROM (SELECT 1 as c) as t;
END; GO

/* ==================================================================================
   3. AJUSTE DE REGLAS ON DELETE (PROTECCIÓN REFERENCIAL)
   ================================================================================== */
PRINT 'Ajustando reglas de borrado (On Delete)...';

-- Protección de Carrera y Facultad (No Action)
ALTER TABLE [dbo].[Escuela] DROP CONSTRAINT IF EXISTS [FK_Escuela_Facultad];
ALTER TABLE [dbo].[Escuela] ADD CONSTRAINT [FK_Escuela_Facultad] FOREIGN KEY (idFacultad) REFERENCES Facultad(idFacultad) ON DELETE NO ACTION;

-- Borrado en Cascada para datos del perfil del Egresado
ALTER TABLE [dbo].[ExperienciaLaboral] DROP CONSTRAINT IF EXISTS [FK_Experiencia_Egresado];
ALTER TABLE [dbo].[ExperienciaLaboral] ADD CONSTRAINT [FK_Experiencia_Egresado] FOREIGN KEY (idEgresado) REFERENCES Egresado(idEgresado) ON DELETE CASCADE;

GO
PRINT '=== SCRIPT MAESTRO DE INTEGRIDAD Y AUDITORÍA FINALIZADO ===';

CONSTRAINT CK_Empresa_Estado CHECK (estado IN ('Registrada', 'Activa', 'Rechazada', 'Vetada', 'Inactiva'))
Para Tabla EMPRESA: (El tuyo le faltaba "Vetada" y "Rechazada")

SQL

CONSTRAINT CK_Empresa_Estado CHECK (estado IN ('Registrada', 'Activa', 'Rechazada', 'Vetada', 'Inactiva'))
Para Tabla OFERTA LABORAL: (El tuyo no tenía "Expirada" ni "Cancelada")
CONSTRAINT CK_Oferta_Estado CHECK (estado IN ('Activa', 'Cerrada', 'Expirada', 'Cancelada'))
Para Tabla EGRESADO
CONSTRAINT CK_Egresado_Estado CHECK (estado IN ('Buscando Trabajo', 'Trabajando', 'Estudiando', 'Inactivo'))
Para Tabla POSTULACIÓN
CONSTRAINT CK_Postulacion_Estado CHECK (estado IN ('Pendiente', 'En Revision', 'Seleccionado', 'Rechazada', 'Cancelada'))
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
CREATE OR ALTER TRIGGER [dbo].[trg_Auditoria_OfertaLaboral] ON [dbo].[OfertaLaboral] AFTER INSERT, UPDATE, DELETE AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Accion NVARCHAR(20);
    IF EXISTS (SELECT * FROM inserted) AND EXISTS (SELECT * FROM deleted) SET @Accion = 'UPDATE';
    ELSE IF EXISTS (SELECT * FROM inserted) SET @Accion = 'INSERT';
    ELSE IF EXISTS (SELECT * FROM deleted) SET @Accion = 'DELETE';
    ELSE RETURN;

    INSERT INTO [dbo].[Auditoria] (tablaAfectada, accion, usuarioSQL, datosAntiguos, datosNuevos)
    SELECT 'OfertaLaboral', @Accion, SYSTEM_USER, 
           (SELECT * FROM deleted FOR JSON AUTO), 
           (SELECT * FROM inserted FOR JSON AUTO);
END
GO
CREATE OR ALTER TRIGGER [dbo].[trg_Auditoria_Postulacion] ON [dbo].[Postulacion] AFTER INSERT, UPDATE, DELETE AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Accion NVARCHAR(20);
    IF EXISTS (SELECT * FROM inserted) AND EXISTS (SELECT * FROM deleted) SET @Accion = 'UPDATE';
    ELSE IF EXISTS (SELECT * FROM inserted) SET @Accion = 'INSERT';
    ELSE IF EXISTS (SELECT * FROM deleted) SET @Accion = 'DELETE';
    ELSE RETURN;

    INSERT INTO [dbo].[Auditoria] (tablaAfectada, accion, usuarioSQL, datosAntiguos, datosNuevos)
    SELECT 'Postulacion', @Accion, SYSTEM_USER, 
           (SELECT * FROM deleted FOR JSON AUTO), 
           (SELECT * FROM inserted FOR JSON AUTO);
END
GO
(Roles Exclusivos)Son los que evitan que un usuario sea "Egresado" y "Administrativo" al mismo tiempo.-- Valida que un Egresado no sea Admin ni Rep
CREATE OR ALTER TRIGGER [dbo].[trg_ValidaUsuarioUnico_Egresado] ON [dbo].[Egresado] AFTER INSERT, UPDATE AS
BEGIN
    IF EXISTS (
        SELECT 1 FROM inserted i WHERE i.idUsuario IS NOT NULL AND (
            EXISTS (SELECT 1 FROM [dbo].[Administrativo] a WHERE a.idUsuario = i.idUsuario) OR
            EXISTS (SELECT 1 FROM [dbo].[Representante] r WHERE r.idUsuario = i.idUsuario)
        )
    )
    BEGIN
        RAISERROR ('ERROR DE INTEGRIDAD: El usuario ya tiene otro rol asignado (Admin o Rep).', 16, 1);
        ROLLBACK TRANSACTION;
    END
END
GO

-- Valida que un Admin no sea Egresado ni Rep
CREATE OR ALTER TRIGGER [dbo].[trg_ValidaUsuarioUnico_Admin] ON [dbo].[Administrativo] AFTER INSERT, UPDATE AS
BEGIN
    IF EXISTS (
        SELECT 1 FROM inserted i WHERE i.idUsuario IS NOT NULL AND (
            EXISTS (SELECT 1 FROM [dbo].[Egresado] e WHERE e.idUsuario = i.idUsuario) OR
            EXISTS (SELECT 1 FROM [dbo].[Representante] r WHERE r.idUsuario = i.idUsuario)
        )
    )
    BEGIN
        RAISERROR ('ERROR DE INTEGRIDAD: El usuario ya tiene otro rol asignado (Egresado o Rep).', 16, 1);
        ROLLBACK TRANSACTION;
    END
END
GO