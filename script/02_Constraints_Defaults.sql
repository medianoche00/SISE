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