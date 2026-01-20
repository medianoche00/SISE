USE [SiseDB]
GO

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

CREATE OR ALTER TRIGGER [dbo].[trg_ValidaUsuarioUnico_Representante] 
ON [dbo].[Representante] 
AFTER INSERT, UPDATE 
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (
        SELECT 1 FROM inserted i 
        WHERE i.idUsuario IS NOT NULL 
        AND (
            EXISTS (SELECT 1 FROM [dbo].[Administrativo] a WHERE a.idUsuario = i.idUsuario) OR
            EXISTS (SELECT 1 FROM [dbo].[Egresado] e WHERE e.idUsuario = i.idUsuario)
        )
    )
    BEGIN
        RAISERROR ('ERROR DE INTEGRIDAD: El usuario ya tiene otro rol asignado (Admin o Egresado).', 16, 1);
        ROLLBACK TRANSACTION;
    END
END
GO