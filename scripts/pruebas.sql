use SiseDB

select * from information_schema.columns where table_name = 'Egresado'

EXEC sp_rename 'Egresado.añoEgreso', 'anioEgreso', 'COLUMN';

USE master;
GO
ALTER DATABASE [SiseDB] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
GO
DROP DATABASE [SiseDB];
GO
