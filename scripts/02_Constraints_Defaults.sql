PRINT '=== CONSTRAINTS Y DEFAULTS ==='
GO

/* -------------------------------------------------------------------------
   PASO PREVIO: CORRECCIÓN DE TIPOS DE DATOS
   Para que los constraints de estado ('Activo', 'Pendiente') funcionen,
   las columnas deben ser texto (NVARCHAR), no BIT (1/0).
   ------------------------------------------------------------------------- */
-- Deshabilitar constraints por defecto para poder modificar columnas
DECLARE @ConstraintName nvarchar(200)
SELECT @ConstraintName = Name FROM sys.default_constraints WHERE parent_object_id = OBJECT_ID('Empresa') AND parent_column_id = (SELECT column_id FROM sys.columns WHERE object_id = OBJECT_ID('Empresa') AND name = 'estado')
IF @ConstraintName IS NOT NULL EXEC('ALTER TABLE Empresa DROP CONSTRAINT ' + @ConstraintName)

SELECT @ConstraintName = Name FROM sys.default_constraints WHERE parent_object_id = OBJECT_ID('OfertaLaboral') AND parent_column_id = (SELECT column_id FROM sys.columns WHERE object_id = OBJECT_ID('OfertaLaboral') AND name = 'estado')
IF @ConstraintName IS NOT NULL EXEC('ALTER TABLE OfertaLaboral DROP CONSTRAINT ' + @ConstraintName)

SELECT @ConstraintName = Name FROM sys.default_constraints WHERE parent_object_id = OBJECT_ID('Egresado') AND parent_column_id = (SELECT column_id FROM sys.columns WHERE object_id = OBJECT_ID('Egresado') AND name = 'estado')
IF @ConstraintName IS NOT NULL EXEC('ALTER TABLE Egresado DROP CONSTRAINT ' + @ConstraintName)

SELECT @ConstraintName = Name FROM sys.default_constraints WHERE parent_object_id = OBJECT_ID('Postulacion') AND parent_column_id = (SELECT column_id FROM sys.columns WHERE object_id = OBJECT_ID('Postulacion') AND name = 'estado')
IF @ConstraintName IS NOT NULL EXEC('ALTER TABLE Postulacion DROP CONSTRAINT ' + @ConstraintName)

-- Modificar columnas a Texto
ALTER TABLE [dbo].[Empresa] ALTER COLUMN [estado] NVARCHAR(50) NOT NULL;
ALTER TABLE [dbo].[OfertaLaboral] ALTER COLUMN [estado] NVARCHAR(50) NOT NULL;
ALTER TABLE [dbo].[Egresado] ALTER COLUMN [estado] NVARCHAR(50) NOT NULL;
ALTER TABLE [dbo].[Postulacion] ALTER COLUMN [estado] NVARCHAR(50) NOT NULL;
GO

/* -------------------------------------------------------------------------
   SECCIÓN 1: DEFAULT VALUES (Valores Automáticos)
   ------------------------------------------------------------------------- */
-- Auditoría: Fecha automática
IF NOT EXISTS (SELECT * FROM sys.default_constraints WHERE name = 'DF_Auditoria_Fecha')
    ALTER TABLE [dbo].[Auditoria] ADD CONSTRAINT [DF_Auditoria_Fecha] DEFAULT (GETDATE()) FOR [fechaHora];

-- Postulación: Fecha automática y Estado inicial 'Pendiente'
IF NOT EXISTS (SELECT * FROM sys.default_constraints WHERE name = 'DF_Postulacion_Fecha')
    ALTER TABLE [dbo].[Postulacion] ADD CONSTRAINT [DF_Postulacion_Fecha] DEFAULT (GETDATE()) FOR [fechaPostulacion];

IF NOT EXISTS (SELECT * FROM sys.default_constraints WHERE name = 'DF_Postulacion_Estado')
    ALTER TABLE [dbo].[Postulacion] ADD CONSTRAINT [DF_Postulacion_Estado] DEFAULT ('Pendiente') FOR [estado];

-- Oferta Laboral: Fecha publicación hoy
IF NOT EXISTS (SELECT * FROM sys.default_constraints WHERE name = 'DF_Oferta_FechaPub')
    ALTER TABLE [dbo].[OfertaLaboral] ADD CONSTRAINT [DF_Oferta_FechaPub] DEFAULT (GETDATE()) FOR [fechaPublicacion];
GO

/* -------------------------------------------------------------------------
   SECCIÓN 2: CHECK CONSTRAINTS (Validaciones de Negocio)
   ------------------------------------------------------------------------- */

-- PERSONA: DNI (8 dígitos) y Género
ALTER TABLE [dbo].[Persona] DROP CONSTRAINT IF EXISTS [CK_Persona_DNI_Formato];
ALTER TABLE [dbo].[Persona] WITH CHECK ADD CONSTRAINT [CK_Persona_DNI_Formato] 
CHECK ([documentoIdentidad] LIKE '[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]');

ALTER TABLE [dbo].[Persona] DROP CONSTRAINT IF EXISTS [CK_Persona_Genero];
ALTER TABLE [dbo].[Persona] WITH CHECK ADD CONSTRAINT [CK_Persona_Genero] 
CHECK ([genero] IN ('Masculino', 'Femenino', 'Otro'));

-- Evitar DNI duplicado
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'UQ_Persona_DNI')
    ALTER TABLE [dbo].[Persona] ADD CONSTRAINT [UQ_Persona_DNI] UNIQUE ([documentoIdentidad]);

-- EMPRESA: RUC (11 dígitos) y Estados
ALTER TABLE [dbo].[Empresa] DROP CONSTRAINT IF EXISTS [CK_Empresa_RUC_Formato];
ALTER TABLE [dbo].[Empresa] WITH CHECK ADD CONSTRAINT [CK_Empresa_RUC_Formato] 
CHECK ([ruc] LIKE '[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]');

-- OFERTA LABORAL: Sueldo positivo y Fechas lógicas
ALTER TABLE [dbo].[OfertaLaboral] DROP CONSTRAINT IF EXISTS [CK_Oferta_Sueldo_Positivo];
ALTER TABLE [dbo].[OfertaLaboral] WITH CHECK ADD CONSTRAINT [CK_Oferta_Sueldo_Positivo] CHECK ([sueldo] >= 0);

ALTER TABLE [dbo].[OfertaLaboral] DROP CONSTRAINT IF EXISTS [CK_Oferta_Fechas_Logicas];
ALTER TABLE [dbo].[OfertaLaboral] WITH CHECK ADD CONSTRAINT [CK_Oferta_Fechas_Logicas] CHECK ([fechaCierre] >= [fechaPublicacion]);

-- EGRESADO: Año lógico
ALTER TABLE [dbo].[Egresado] DROP CONSTRAINT IF EXISTS [CK_Egresado_Anio_Real];
ALTER TABLE [dbo].[Egresado] WITH CHECK ADD CONSTRAINT [CK_Egresado_Anio_Real] 
CHECK ([añoEgreso] BETWEEN 1960 AND YEAR(GETDATE()) + 1);

-- EXPERIENCIA LABORAL: Fechas
ALTER TABLE [dbo].[ExperienciaLaboral] DROP CONSTRAINT IF EXISTS [CK_Experiencia_Fechas];
ALTER TABLE [dbo].[ExperienciaLaboral] WITH CHECK ADD CONSTRAINT [CK_Experiencia_Fechas] 
CHECK ([fechaFin] IS NULL OR [fechaFin] >= [fechaInicio]);

/* -------------------------------------------------------------------------
   SECCIÓN 3: VALIDACIÓN DE ESTADOS PERMITIDOS
   ------------------------------------------------------------------------- */
ALTER TABLE [dbo].[Postulacion] DROP CONSTRAINT IF EXISTS [CK_Postulacion_EstadoValido];
ALTER TABLE [dbo].[Postulacion] WITH CHECK ADD CONSTRAINT [CK_Postulacion_EstadoValido] 
CHECK ([estado] IN ('Pendiente', 'En Revisión', 'Entrevista', 'Finalista', 'Seleccionado', 'Rechazado', 'Cancelado'));

ALTER TABLE [dbo].[OfertaLaboral] DROP CONSTRAINT IF EXISTS [CK_Oferta_Estado];
ALTER TABLE [dbo].[OfertaLaboral] WITH CHECK ADD CONSTRAINT [CK_Oferta_Estado] 
CHECK ([estado] IN ('Activa', 'Cerrada', 'Expirada', 'Cancelada'));

ALTER TABLE [dbo].[Empresa] DROP CONSTRAINT IF EXISTS [CK_Empresa_Estado];
ALTER TABLE [dbo].[Empresa] WITH CHECK ADD CONSTRAINT [CK_Empresa_Estado] 
CHECK ([estado] IN ('Registrada', 'Activa', 'Rechazada', 'Vetada', 'Inactiva'));
GO