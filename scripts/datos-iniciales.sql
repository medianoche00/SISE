USE SiseDB
GO

/* ==================================================================================
   1. POBLADO DE CATALOGOS (GEOGRAFIA Y ACADEMICO)
   ================================================================================== */
PRINT 'Insertando Datos Geográficos (Lima)...'

-- Departamentos
INSERT INTO [dbo].[Departamento] (nombreDepartamento) VALUES ('Lima'), ('Arequipa'), ('Cusco');

-- Provincias (Asumiendo ID 1 es Lima, 2 Arequipa)
DECLARE @IdDeptLima INT = (SELECT idDepartamento FROM Departamento WHERE nombreDepartamento = 'Lima');
DECLARE @IdDeptArequipa INT = (SELECT idDepartamento FROM Departamento WHERE nombreDepartamento = 'Arequipa');

INSERT INTO [dbo].[Provincia] (idDepartamento, nombreProvincia) VALUES 
(@IdDeptLima, 'Lima'), 
(@IdDeptLima, 'Cañete'),
(@IdDeptArequipa, 'Arequipa');

-- Distritos
DECLARE @IdProvLima INT = (SELECT idProvincia FROM Provincia WHERE nombreProvincia = 'Lima' AND idDepartamento = @IdDeptLima);

INSERT INTO [dbo].[Distrito] (idProvincia, nombreDistrito) VALUES 
(@IdProvLima, 'Cercado de Lima'),
(@IdProvLima, 'Miraflores'),
(@IdProvLima, 'San Isidro'),
(@IdProvLima, 'Los Olivos'),
(@IdProvLima, 'San Juan de Lurigancho');
GO

PRINT 'Insertando Datos Académicos...'
-- Facultades
INSERT INTO [dbo].[Facultad] (nombreFacultad) VALUES 
('Ingeniería'), 
('Negocios'), 
('Humanidades');

-- Escuelas y Carreras (Ejemplo para Ingeniería)
DECLARE @IdFacIng INT = (SELECT idFacultad FROM Facultad WHERE nombreFacultad = 'Ingeniería');
INSERT INTO [dbo].[Escuela] (idFacultad, nombreEscuela) VALUES (@IdFacIng, 'Escuela de Tecnologías de Información');

DECLARE @IdEscuelaTI INT = (SELECT idEscuela FROM Escuela WHERE nombreEscuela = 'Escuela de Tecnologías de Información');
INSERT INTO [dbo].[Carrera] (idEscuela, nombreCarrera) VALUES 
(@IdEscuelaTI, 'Ingeniería de Sistemas'), 
(@IdEscuelaTI, 'Ingeniería de Software'),
(@IdEscuelaTI, 'Redes y Comunicaciones');

-- Catalogos de Trabajo
INSERT INTO [dbo].[TipoContrato] (nombreTipo) VALUES ('Tiempo Completo'), ('Medio Tiempo'), ('Prácticas Pre-Profesionales'), ('Prácticas Profesionales');
INSERT INTO [dbo].[ModalidadTrabajo] (nombreModalidad) VALUES ('Presencial'), ('Remoto'), ('Híbrido');
INSERT INTO [dbo].[CargoAdministrativo] (nombreCargo) VALUES ('Jefe de Bolsa de Trabajo'), ('Analista de Selección'), ('Asistente Administrativo'), ('Administrador del Sistema');
INSERT INTO [dbo].[TipoFormacion] (nombreTipoFormacion) VALUES ('Curso'), ('Diplomado'), ('Certificación'), ('Taller');
GO

/* ==================================================================================
   2. ROLES DE IDENTITY
   ================================================================================== */
PRINT 'Insertando Roles...'
INSERT INTO [dbo].[AspNetRoles] ([Name], [NormalizedName], [ConcurrencyStamp]) VALUES 
('Administrador', 'ADMINISTRADOR', NEWID()),
('Administrativo', 'ADMINISTRATIVO', NEWID()),
('Egresado', 'EGRESADO', NEWID()),
('Representante', 'REPRESENTANTE', NEWID());
GO

/* ==================================================================================
   3. USUARIOS Y PERFILES (PERSONAS)
   ================================================================================== */

-- ----------------------------------------------------------------------------------
-- VARIABLES DE AYUDA
-- ----------------------------------------------------------------------------------
DECLARE @IdDistrito INT = (SELECT TOP 1 idDistrito FROM Distrito WHERE nombreDistrito = 'San Isidro');
DECLARE @IdTipoDocDNI INT = 1; -- DNI
DECLARE @FechaActual DATETIME = GETDATE();
DECLARE @SecurityStamp UNIQUEIDENTIFIER;

-- ----------------------------------------------------------------------------------
-- USUARIO 1: ADMINISTRADOR (Rol: Administrador, Tabla: Administrativo)
-- ----------------------------------------------------------------------------------
PRINT 'Creando Administrador...'
SET @SecurityStamp = NEWID();

-- 1. Dirección
INSERT INTO [dbo].[Direccion] (idDistrito, calle, numero, estado) VALUES (@IdDistrito, 'Av. Arequipa', '1010', 'Activo');
DECLARE @IdDirAdmin INT = SCOPE_IDENTITY();

-- 2. Persona
INSERT INTO [dbo].[Persona] (nombres, apellidoPaterno, apellidoMaterno, numeroDocumento, idTipoDocumento, idDireccion, correoPersonal, estado)
VALUES ('Juan', 'Perez', 'Admin', '10000001', @IdTipoDocDNI, @IdDirAdmin, 'admin@sise.edu.pe', 'Activo');
DECLARE @IdPersAdmin INT = SCOPE_IDENTITY();

-- 3. AspNetUsers
INSERT INTO [dbo].[AspNetUsers] 
(UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount)
VALUES 
('admin', 'ADMIN', 'admin@sise.edu.pe', 'ADMIN@SISE.EDU.PE', 1, 
'AQAAAAIAAYagAAAAEMPrGu1OqHz9tDYivMRwd9epbz6spWDrsoGP1+UtPo+DECdfk21gQ6I24nzHT5xg7A==', -- Hash Dummy, actualizar via app si falla
@SecurityStamp, NEWID(), 1, 0, 1, 0);
DECLARE @IdUserAdmin INT = SCOPE_IDENTITY();

-- 4. Rol
INSERT INTO [dbo].[AspNetUserRoles] (UserId, RoleId) 
SELECT @IdUserAdmin, Id FROM AspNetRoles WHERE Name = 'Administrador';

-- 5. Tabla Administrativo (Cargo: Admin del sistema)
DECLARE @IdCargoAdmin INT = (SELECT TOP 1 idCargoAdministrativo FROM CargoAdministrativo WHERE nombreCargo LIKE '%Administrador%');
INSERT INTO [dbo].[Administrativo] (idCargoAdministrativo, idPersona, idUsuario, estado)
VALUES (@IdCargoAdmin, @IdPersAdmin, @IdUserAdmin, 'Activo');


-- ----------------------------------------------------------------------------------
-- USUARIO 2: STAFF (Rol: Administrativo, Tabla: Administrativo)
-- ----------------------------------------------------------------------------------
PRINT 'Creando Staff...'
SET @SecurityStamp = NEWID();

-- 1. Dirección
INSERT INTO [dbo].[Direccion] (idDistrito, calle, numero, estado) VALUES (@IdDistrito, 'Calle Los Pinos', '200', 'Activo');
DECLARE @IdDirStaff INT = SCOPE_IDENTITY();

-- 2. Persona
INSERT INTO [dbo].[Persona] (nombres, apellidoPaterno, apellidoMaterno, numeroDocumento, idTipoDocumento, idDireccion, correoPersonal, estado)
VALUES ('Maria', 'Gomez', 'Staff', '20000002', @IdTipoDocDNI, @IdDirStaff, 'staff@sise.edu.pe', 'Activo');
DECLARE @IdPersStaff INT = SCOPE_IDENTITY();

-- 3. AspNetUsers
INSERT INTO [dbo].[AspNetUsers] (UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount)
VALUES ('staff', 'STAFF', 'staff@sise.edu.pe', 'STAFF@SISE.EDU.PE', 1, 'AQAAAAIAAYagAAAAEMPrGu1OqHz9tDYivMRwd9epbz6spWDrsoGP1+UtPo+DECdfk21gQ6I24nzHT5xg7A==', @SecurityStamp, NEWID(), 1, 0, 1, 0);
DECLARE @IdUserStaff INT = SCOPE_IDENTITY();

-- 4. Rol
INSERT INTO [dbo].[AspNetUserRoles] (UserId, RoleId) SELECT @IdUserStaff, Id FROM AspNetRoles WHERE Name = 'Administrativo';

-- 5. Tabla Administrativo (Cargo: Analista)
DECLARE @IdCargoStaff INT = (SELECT TOP 1 idCargoAdministrativo FROM CargoAdministrativo WHERE nombreCargo LIKE '%Analista%');
INSERT INTO [dbo].[Administrativo] (idCargoAdministrativo, idPersona, idUsuario, estado)
VALUES (@IdCargoStaff, @IdPersStaff, @IdUserStaff, 'Activo');


-- ----------------------------------------------------------------------------------
-- USUARIO 3: EGRESADO (Rol: Egresado, Tabla: Egresado)
-- ----------------------------------------------------------------------------------
PRINT 'Creando Egresado...'
SET @SecurityStamp = NEWID();

-- 1. Dirección
INSERT INTO [dbo].[Direccion] (idDistrito, calle, numero, estado) VALUES (@IdDistrito, 'Av. Universitaria', '555', 'Activo');
DECLARE @IdDirEgre INT = SCOPE_IDENTITY();

-- 2. Persona
INSERT INTO [dbo].[Persona] (nombres, apellidoPaterno, apellidoMaterno, numeroDocumento, idTipoDocumento, idDireccion, correoPersonal, estado)
VALUES ('Carlos', 'Lopez', 'Egresado', '30000003', @IdTipoDocDNI, @IdDirEgre, 'alumno@sise.edu.pe', 'Activo');
DECLARE @IdPersEgre INT = SCOPE_IDENTITY();

-- 3. AspNetUsers
INSERT INTO [dbo].[AspNetUsers] (UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount)
VALUES ('alumno', 'ALUMNO', 'alumno@sise.edu.pe', 'ALUMNO@SISE.EDU.PE', 1, 'AQAAAAIAAYagAAAAEMPrGu1OqHz9tDYivMRwd9epbz6spWDrsoGP1+UtPo+DECdfk21gQ6I24nzHT5xg7A==', @SecurityStamp, NEWID(), 1, 0, 1, 0);
DECLARE @IdUserEgre INT = SCOPE_IDENTITY();

-- 4. Rol
INSERT INTO [dbo].[AspNetUserRoles] (UserId, RoleId) SELECT @IdUserEgre, Id FROM AspNetRoles WHERE Name = 'Egresado';

-- 5. Tabla Egresado
DECLARE @IdCarrera INT = (SELECT TOP 1 idCarrera FROM Carrera WHERE nombreCarrera LIKE '%Sistemas%');
INSERT INTO [dbo].[Egresado] (idPersona, idUsuario, idCarrera, codigoUniversitario, anioEgreso, estado)
VALUES (@IdPersEgre, @IdUserEgre, @IdCarrera, 'U20200001', 2023, 'Buscando Trabajo');


-- ----------------------------------------------------------------------------------
-- USUARIO 4: REPRESENTANTE EMPRESA (Rol: Representante, Tabla: Representante + Empresa)
-- ----------------------------------------------------------------------------------
PRINT 'Creando Empresa y Representante...'
SET @SecurityStamp = NEWID();

-- A. Crear la Empresa primero
-- 1. Dirección Empresa
INSERT INTO [dbo].[Direccion] (idDistrito, calle, numero, estado) VALUES (@IdDistrito, 'Av. Javier Prado', '888', 'Activo');
DECLARE @IdDirEmpresa INT = SCOPE_IDENTITY();

-- 2. Empresa
INSERT INTO [dbo].[Empresa] (idDireccion, ruc, razonSocial, correo, estado)
VALUES (@IdDirEmpresa, '20123456789', 'Tech Solutions SAC', 'contacto@techsolutions.com', 'Activa');
DECLARE @IdEmpresa INT = SCOPE_IDENTITY();

-- B. Crear al Usuario Representante
-- 1. Dirección Persona
INSERT INTO [dbo].[Direccion] (idDistrito, calle, numero, estado) VALUES (@IdDistrito, 'Calle Los Jazmines', '123', 'Activo');
DECLARE @IdDirRep INT = SCOPE_IDENTITY();

-- 2. Persona
INSERT INTO [dbo].[Persona] (nombres, apellidoPaterno, apellidoMaterno, numeroDocumento, idTipoDocumento, idDireccion, correoPersonal, estado)
VALUES ('Roberto', 'Diaz', 'Gerente', '40000004', @IdTipoDocDNI, @IdDirRep, 'reclutador@techsolutions.com', 'Activo');
DECLARE @IdPersRep INT = SCOPE_IDENTITY();

-- 3. AspNetUsers
INSERT INTO [dbo].[AspNetUsers] (UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount)
VALUES ('reclutador', 'RECLUTADOR', 'reclutador@techsolutions.com', 'RECLUTADOR@TECHSOLUTIONS.COM', 1, 'AQAAAAIAAYagAAAAEMPrGu1OqHz9tDYivMRwd9epbz6spWDrsoGP1+UtPo+DECdfk21gQ6I24nzHT5xg7A==', @SecurityStamp, NEWID(), 1, 0, 1, 0);
DECLARE @IdUserRep INT = SCOPE_IDENTITY();

-- 4. Rol
INSERT INTO [dbo].[AspNetUserRoles] (UserId, RoleId) SELECT @IdUserRep, Id FROM AspNetRoles WHERE Name = 'Representante';

-- 5. Tabla Representante
INSERT INTO [dbo].[Representante] (idEmpresa, idPersona, idUsuario, cargo, estado)
VALUES (@IdEmpresa, @IdPersRep, @IdUserRep, 'Jefe de RRHH', 'Activo');

PRINT 'Base de datos poblada exitosamente.'
GO