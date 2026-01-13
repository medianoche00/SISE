/*
use master
go
drop database SiseDB
go
*/
CREATE DATABASE SiseDB
GO

USE SiseDB
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/* ==================================================================================
   1. TABLAS DEL SISTEMA DE IDENTITY (SEGURIDAD)
   ================================================================================== */

CREATE TABLE [dbo].[AspNetUsers](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [UserName] [nvarchar](256) NULL,
    [NormalizedUserName] [nvarchar](256) NULL,
    [Email] [nvarchar](256) NULL,
    [NormalizedEmail] [nvarchar](256) NULL,
    [EmailConfirmed] [bit] NOT NULL,
    [PasswordHash] [nvarchar](max) NULL,
    [SecurityStamp] [nvarchar](max) NULL,
    [ConcurrencyStamp] [nvarchar](max) NULL,
    [PhoneNumber] [nvarchar](max) NULL,
    [PhoneNumberConfirmed] [bit] NOT NULL,
    [TwoFactorEnabled] [bit] NOT NULL,
    [LockoutEnd] [datetimeoffset](7) NULL,
    [LockoutEnabled] [bit] NOT NULL,
    [AccessFailedCount] [int] NOT NULL,
    CONSTRAINT [PK_AspNetUsers] PRIMARY KEY CLUSTERED ([Id] ASC)
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[AspNetRoles](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [Name] [nvarchar](256) NULL,
    [NormalizedName] [nvarchar](256) NULL,
    [ConcurrencyStamp] [nvarchar](max) NULL,
    CONSTRAINT [PK_AspNetRoles] PRIMARY KEY CLUSTERED ([Id] ASC)
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[AspNetUserClaims](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [UserId] [int] NOT NULL,
    [ClaimType] [nvarchar](max) NULL,
    [ClaimValue] [nvarchar](max) NULL,
    CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY([UserId]) 
        REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[AspNetRoleClaims](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [RoleId] [int] NOT NULL,
    [ClaimType] [nvarchar](max) NULL,
    [ClaimValue] [nvarchar](max) NULL,
    CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY([RoleId]) 
        REFERENCES [dbo].[AspNetRoles] ([Id]) ON DELETE CASCADE
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[AspNetUserLogins](
    [LoginProvider] [nvarchar](450) NOT NULL,
    [ProviderKey] [nvarchar](450) NOT NULL,
    [ProviderDisplayName] [nvarchar](max) NULL,
    [UserId] [int] NOT NULL,
    CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY CLUSTERED ([LoginProvider] ASC, [ProviderKey] ASC),
    CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY([UserId]) 
        REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[AspNetUserRoles](
    [UserId] [int] NOT NULL,
    [RoleId] [int] NOT NULL,
    CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY CLUSTERED ([UserId] ASC, [RoleId] ASC),
    CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY([UserId]) 
        REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY([RoleId]) 
        REFERENCES [dbo].[AspNetRoles] ([Id]) ON DELETE CASCADE
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[AspNetUserTokens](
    [UserId] [int] NOT NULL,
    [LoginProvider] [nvarchar](450) NOT NULL,
    [Name] [nvarchar](450) NOT NULL,
    [Value] [nvarchar](max) NULL,
    CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY CLUSTERED ([UserId] ASC, [LoginProvider] ASC, [Name] ASC),
    CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY([UserId]) 
        REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
) ON [PRIMARY]
GO

/* ==================================================================================
   2. TABLAS MAESTRAS / CATALOGOS
   ================================================================================== */

CREATE TABLE [dbo].[TipoDocumento] (
    [idTipoDocumento] INT IDENTITY(1,1) PRIMARY KEY,
    [nombreTipo] NVARCHAR(50) NOT NULL, 
    [estado] NVARCHAR(20) NOT NULL DEFAULT 'Activo',
    CONSTRAINT CK_TipoDocumento_Estado CHECK (estado IN ('Activo', 'Eliminado'))
) ON [PRIMARY]
GO

-- es necesario que se inserten en ese orden
SET IDENTITY_INSERT [dbo].[TipoDocumento] ON;
INSERT INTO [dbo].[TipoDocumento] (idTipoDocumento, nombreTipo) VALUES (1, 'DNI');
INSERT INTO [dbo].[TipoDocumento] (idTipoDocumento, nombreTipo) VALUES (2, 'Carnet Extranjería');
INSERT INTO [dbo].[TipoDocumento] (idTipoDocumento, nombreTipo) VALUES (3, 'Pasaporte');
SET IDENTITY_INSERT [dbo].[TipoDocumento] OFF;

CREATE TABLE [dbo].[Facultad](
    [idFacultad] [int] IDENTITY(1,1) NOT NULL,
    [nombreFacultad] [nvarchar](150) NOT NULL,
    [estado] [nvarchar](20) NOT NULL DEFAULT 'Activo',
    CONSTRAINT [PK_Facultad] PRIMARY KEY CLUSTERED ([idFacultad] ASC),
    CONSTRAINT [CK_Facultad_Estado] CHECK ([estado] IN ('Activo', 'Eliminado'))
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Escuela](
    [idEscuela] [int] IDENTITY(1,1) NOT NULL,
    [idFacultad] [int] NOT NULL,
    [nombreEscuela] [nvarchar](150) NOT NULL,
    [estado] [nvarchar](20) NOT NULL DEFAULT 'Activo',
    CONSTRAINT [PK_Escuela] PRIMARY KEY CLUSTERED ([idEscuela] ASC),
    CONSTRAINT [FK_Escuela_Facultad] FOREIGN KEY([idFacultad]) 
        REFERENCES [dbo].[Facultad] ([idFacultad]) ON DELETE NO ACTION,
    CONSTRAINT [CK_Escuela_Estado] CHECK ([estado] IN ('Activo', 'Eliminado'))
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Carrera](
    [idCarrera] [int] IDENTITY(1,1) NOT NULL,
    [idEscuela] [int] NOT NULL,
    [nombreCarrera] [nvarchar](150) NOT NULL,
    [estado] [nvarchar](20) NOT NULL DEFAULT 'Activo',
    CONSTRAINT [PK_Carrera] PRIMARY KEY CLUSTERED ([idCarrera] ASC),
    CONSTRAINT [FK_Carrera_Escuela] FOREIGN KEY([idEscuela]) 
        REFERENCES [dbo].[Escuela] ([idEscuela]) ON DELETE NO ACTION,
    CONSTRAINT [CK_Carrera_Estado] CHECK ([estado] IN ('Activo', 'Eliminado'))
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[TipoContrato](
    [idTipoContrato] [int] IDENTITY(1,1) NOT NULL,
    [nombreTipo] [nvarchar](100) NOT NULL,
    [estado] [nvarchar](20) NOT NULL DEFAULT 'Activo',
    CONSTRAINT [PK_TipoContrato] PRIMARY KEY CLUSTERED ([idTipoContrato] ASC),
    CONSTRAINT [CK_TipoContrato_Estado] CHECK ([estado] IN ('Activo', 'Eliminado'))
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[ModalidadTrabajo](
    [idModalidadTrabajo] [int] IDENTITY(1,1) NOT NULL,
    [nombreModalidad] [nvarchar](100) NOT NULL,
    [estado] [nvarchar](20) NOT NULL DEFAULT 'Activo',
    CONSTRAINT [PK_ModalidadTrabajo] PRIMARY KEY CLUSTERED ([idModalidadTrabajo] ASC),
    CONSTRAINT [CK_ModalidadTrabajo_Estado] CHECK ([estado] IN ('Activo', 'Eliminado'))
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[TipoFormacion](
    [idTipoFormacion] [int] IDENTITY(1,1) NOT NULL,
    [nombreTipoFormacion] [nvarchar](100) NOT NULL,
    [estado] [nvarchar](20) NOT NULL DEFAULT 'Activo',
    CONSTRAINT [PK_TipoFormacion] PRIMARY KEY CLUSTERED ([idTipoFormacion] ASC),
    CONSTRAINT [CK_TipoFormacion_Estado] CHECK ([estado] IN ('Activo', 'Eliminado'))
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[CargoAdministrativo](
    [idCargoAdministrativo] [int] IDENTITY(1,1) NOT NULL,
    [nombreCargo] [nvarchar](100) NOT NULL,
    [estado] [nvarchar](20) NOT NULL DEFAULT 'Activo',
    CONSTRAINT [PK_CargoAdministrativo] PRIMARY KEY CLUSTERED ([idCargoAdministrativo] ASC),
    CONSTRAINT [CK_CargoAdministrativo_Estado] CHECK ([estado] IN ('Activo', 'Eliminado'))
) ON [PRIMARY]
GO

/* ==================================================================================
   3. TABLAS PRINCIPALES DEL NEGOCIO
   ================================================================================== */

CREATE TABLE [dbo].[Persona](
    [idPersona] [int] IDENTITY(1,1) NOT NULL,
    [nombres] [nvarchar](100) NOT NULL,
    [apellidoPaterno] [nvarchar](100) NOT NULL,
    [apellidoMaterno] [nvarchar](100) NOT NULL,
    [numeroDocumento] [varchar](20) NOT NULL,
    [idTipoDocumento] [int] NOT NULL,
    [telefono] [nvarchar](15) NULL,
    [correoPersonal] [nvarchar](150) NULL,
    [estado] [nvarchar](20) NOT NULL DEFAULT 'Activo',
    CONSTRAINT [PK_Persona] PRIMARY KEY CLUSTERED ([idPersona] ASC),
    CONSTRAINT [FK_Persona_TipoDocumento] FOREIGN KEY([idTipoDocumento]) 
        REFERENCES [dbo].[TipoDocumento] ([idTipoDocumento]) ON DELETE NO ACTION,
    CONSTRAINT [UQ_Persona_Documento] UNIQUE ([idTipoDocumento], [numeroDocumento]), -- evita que dos personas tengan el mismo numero solo si son del mismo tipo de documento
    CONSTRAINT [CK_Persona_Estado] CHECK ([estado] IN ('Activo', 'Eliminado')),
    CONSTRAINT [CK_Persona_ValidarDocumento] CHECK (
        (   -- caso DNI
            [idTipoDocumento] = 1 AND LEN([numeroDocumento]) = 8 AND [numeroDocumento] NOT LIKE '%[^0-9]%' )
        OR 
        (   -- caso no es dni (carnet de extrangeria, pasaporte)
            [idTipoDocumento] <> 1 AND LEN([numeroDocumento]) >= 3 )
    )
) ON [PRIMARY];
GO

CREATE TABLE [dbo].[Empresa](
    [idEmpresa] [int] IDENTITY(1,1) NOT NULL,
    [ruc] [char](11) NOT NULL,
    [razonSocial] [nvarchar](150) NOT NULL,
    [direccion] [nvarchar](255) NULL,
    [telefono] [nvarchar](15) NULL,
    [correo] [nvarchar](150) NULL,
    [descripcion] [nvarchar](255) NULL,
    [estado] [nvarchar](20) NOT NULL DEFAULT 'Registrada',
    CONSTRAINT [PK_Empresa] PRIMARY KEY CLUSTERED ([idEmpresa] ASC),
    CONSTRAINT [UQ_Empresa_RUC] UNIQUE ([ruc]),
    CONSTRAINT [CK_Empresa_Estado] CHECK ([estado] IN ('Registrada', 'Activo', 'Rechazada', 'Vetada', 'Eliminado'))
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Egresado](
    [idEgresado] [int] IDENTITY(1,1) NOT NULL,
    [idPersona] [int] NOT NULL,
    [idUsuario] [int] NOT NULL,
    [idCarrera] [int] NOT NULL,
    [codigoUniversitario] [nvarchar](20) NOT NULL,
    [anioEgreso] [int] NOT NULL,
    [estado] [nvarchar](20) NOT NULL DEFAULT 'Buscando Trabajo',
    CONSTRAINT [PK_Egresado] PRIMARY KEY CLUSTERED ([idEgresado] ASC),
    CONSTRAINT [FK_Egresado_Persona] FOREIGN KEY([idPersona]) 
        REFERENCES [dbo].[Persona] ([idPersona]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Egresado_Usuario] FOREIGN KEY([idUsuario]) 
        REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE, 
    CONSTRAINT [FK_Egresado_Carrera] FOREIGN KEY([idCarrera]) 
        REFERENCES [dbo].[Carrera] ([idCarrera]) ON DELETE NO ACTION,
    CONSTRAINT [UQ_Egresado_Codigo] UNIQUE ([codigoUniversitario]),
    CONSTRAINT [CK_Egresado_Estado] CHECK ([estado] IN ('Buscando Trabajo', 'Trabajando', 'Estudiando', 'Eliminado'))
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Administrativo](
    [idAdministrativo] [int] IDENTITY(1,1) NOT NULL,
    [idCargoAdministrativo] [int] NOT NULL,
    [idPersona] [int] NOT NULL,
    [idUsuario] [int] NOT NULL,
    [estado] [nvarchar](20) NOT NULL DEFAULT 'Activo',
    CONSTRAINT [PK_Administrativo] PRIMARY KEY CLUSTERED ([idAdministrativo] ASC),
    CONSTRAINT [FK_Administrativo_Cargo] FOREIGN KEY([idCargoAdministrativo]) 
        REFERENCES [dbo].[CargoAdministrativo] ([idCargoAdministrativo]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Administrativo_Persona] FOREIGN KEY([idPersona]) 
        REFERENCES [dbo].[Persona] ([idPersona]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Administrativo_Usuario] FOREIGN KEY([idUsuario]) 
        REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [CK_Administrativo_Estado] CHECK ([estado] IN ('Activo', 'Eliminado'))
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Representante](
    [idRepresentante] [int] IDENTITY(1,1) NOT NULL,
    [idEmpresa] [int] NOT NULL,
    [idPersona] [int] NOT NULL,
    [idUsuario] [int] NOT NULL,
    [cargo] [nvarchar](100) NULL,
    [estado] [nvarchar](20) NOT NULL DEFAULT 'Activo',
    CONSTRAINT [PK_Representante] PRIMARY KEY CLUSTERED ([idRepresentante] ASC),
    CONSTRAINT [FK_Representante_Empresa] FOREIGN KEY([idEmpresa]) 
        REFERENCES [dbo].[Empresa] ([idEmpresa]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Representante_Persona] FOREIGN KEY([idPersona]) 
        REFERENCES [dbo].[Persona] ([idPersona]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Representante_Usuario] FOREIGN KEY([idUsuario]) 
        REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [CK_Representante_Estado] CHECK ([estado] IN ('Activo', 'Eliminado'))
) ON [PRIMARY]
GO

/* ==================================================================================
   4. TABLAS TRANSACCIONALES
   ================================================================================== */

CREATE TABLE [dbo].[OfertaLaboral](
    [idOferta] [int] IDENTITY(1,1) NOT NULL,
    [idEmpresa] [int] NOT NULL,
    [titulo] [nvarchar](150) NOT NULL,
    [descripcion] [nvarchar](max) NULL,
    [requisitos] [nvarchar](max) NULL,
    [ubicacion] [nvarchar](150) NULL,
    [idTipoContrato] [int] NOT NULL,
    [sueldo] [decimal](10, 2) NULL,
    [idModalidadTrabajo] [int] NOT NULL,
    [fechaPublicacion] [date] NOT NULL DEFAULT GETDATE(),
    [fechaCierre] [date] NOT NULL,
    [idEgresadoGanador] [int] NULL,
    [estado] [nvarchar](20) NOT NULL DEFAULT 'Activo',
    CONSTRAINT [PK_OfertaLaboral] PRIMARY KEY CLUSTERED ([idOferta] ASC),
    CONSTRAINT [FK_OfertaLaboral_Empresa] FOREIGN KEY([idEmpresa]) 
        REFERENCES [dbo].[Empresa] ([idEmpresa]) ON DELETE NO ACTION,
    CONSTRAINT [FK_OfertaLaboral_TipoContrato] FOREIGN KEY([idTipoContrato]) 
        REFERENCES [dbo].[TipoContrato] ([idTipoContrato]) ON DELETE NO ACTION,
    CONSTRAINT [FK_OfertaLaboral_Modalidad] FOREIGN KEY([idModalidadTrabajo]) 
        REFERENCES [dbo].[ModalidadTrabajo] ([idModalidadTrabajo]) ON DELETE NO ACTION,
    CONSTRAINT [FK_OfertaLaboral_Ganador] FOREIGN KEY([idEgresadoGanador]) 
        REFERENCES [dbo].[Egresado] ([idEgresado]) ON DELETE NO ACTION,
    CONSTRAINT [CK_OfertaLaboral_Estado] CHECK ([estado] IN ('Activo', 'Cerrada', 'Expirada', 'Cancelada', 'Eliminado'))
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[Postulacion](
    [idPostulacion] [int] IDENTITY(1,1) NOT NULL,
    [idEgresado] [int] NOT NULL,
    [idOferta] [int] NOT NULL,
    [idRepresentanteEvaluador] [int] NULL,
    [fechaPostulacion] [datetime] NOT NULL DEFAULT GETDATE(),
    [fechaEvaluacion] [datetime] NULL,
    [comentarios] [nvarchar](500) NULL,
    [cartaPresentacion] [nvarchar](500) NULL,
    [estado] [nvarchar](50) NOT NULL DEFAULT 'Pendiente',
    CONSTRAINT [PK_Postulacion] PRIMARY KEY CLUSTERED ([idPostulacion] ASC),
    CONSTRAINT [FK_Postulacion_Egresado] FOREIGN KEY([idEgresado]) 
        REFERENCES [dbo].[Egresado] ([idEgresado]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Postulacion_Oferta] FOREIGN KEY([idOferta]) 
        REFERENCES [dbo].[OfertaLaboral] ([idOferta]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Postulacion_Representante] FOREIGN KEY([idRepresentanteEvaluador]) 
        REFERENCES [dbo].[Representante] ([idRepresentante]) ON DELETE NO ACTION,
    CONSTRAINT [CK_Postulacion_Estado] CHECK ([estado] IN ('Pendiente', 'En Revision', 'Seleccionado', 'Rechazada', 'Cancelada', 'Eliminado'))
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[ExperienciaLaboral](
    [idExperiencia] [int] IDENTITY(1,1) NOT NULL,
    [idEgresado] [int] NOT NULL,
    [empresa] [nvarchar](150) NOT NULL,
    [idEmpresaRegistrada] [int] NULL,
    [cargo] [nvarchar](150) NOT NULL,
    [fechaInicio] [date] NOT NULL,
    [fechaFin] [date] NULL,
    [descripcion] [nvarchar](max) NULL,
    [estado] [nvarchar](20) NOT NULL DEFAULT 'Validado',
    CONSTRAINT [PK_ExperienciaLaboral] PRIMARY KEY CLUSTERED ([idExperiencia] ASC),
    CONSTRAINT [FK_ExperienciaLaboral_Egresado] FOREIGN KEY([idEgresado]) 
        REFERENCES [dbo].[Egresado] ([idEgresado]) ON DELETE CASCADE,
    CONSTRAINT [FK_ExperienciaLaboral_Empresa] FOREIGN KEY([idEmpresaRegistrada]) 
        REFERENCES [dbo].[Empresa] ([idEmpresa]) ON DELETE NO ACTION,
    CONSTRAINT [CK_Experiencia_Estado] CHECK ([estado] IN ('Validado', 'Pendiente', 'Rechazado', 'Eliminado'))
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[FormacionComplementaria](
    [idFormacion] [int] IDENTITY(1,1) NOT NULL,
    [idEgresado] [int] NOT NULL,
    [idTipoFormacion] [int] NOT NULL,
    [nombreDelCurso] [nvarchar](150) NOT NULL,
    [institucion] [nvarchar](150) NULL,
    [fechaInicio] [date] NULL,
    [fechaFin] [date] NULL,
    [estado] [nvarchar](20) NOT NULL DEFAULT 'Validado',
    CONSTRAINT [PK_FormacionComplementaria] PRIMARY KEY CLUSTERED ([idFormacion] ASC),
    CONSTRAINT [FK_Formacion_Egresado] FOREIGN KEY([idEgresado]) 
        REFERENCES [dbo].[Egresado] ([idEgresado]) ON DELETE CASCADE,
    CONSTRAINT [FK_Formacion_Tipo] FOREIGN KEY([idTipoFormacion]) 
        REFERENCES [dbo].[TipoFormacion] ([idTipoFormacion]) ON DELETE NO ACTION,
    CONSTRAINT [CK_Formacion_Estado] CHECK ([estado] IN ('Validado', 'Pendiente', 'Rechazado', 'Eliminado'))
) ON [PRIMARY]
GO

/* ==================================================================================
   5. TABLA DE AUDITORIA
   ================================================================================== */

CREATE TABLE [dbo].[Auditoria](
    [idAuditoria] [int] IDENTITY(1,1) NOT NULL,
    [idUsuario] [int] NULL,
    [tablaAfectada] [nvarchar](100) NOT NULL,
    [columnaAfectada] [nvarchar](100) NULL,
    [accion] [nvarchar](20) NOT NULL,
    [valorAnterior] [nvarchar](max) NULL,
    [valorNuevo] [nvarchar](max) NULL,
    [fechaHora] [datetime2](7) NOT NULL DEFAULT GETDATE(),
    CONSTRAINT [PK_Auditoria] PRIMARY KEY CLUSTERED ([idAuditoria] ASC),
    CONSTRAINT [FK_Auditoria_Usuario] FOREIGN KEY([idUsuario]) 
        REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE SET NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO