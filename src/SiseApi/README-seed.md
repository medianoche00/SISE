# Base de Datos y Seeding

Este proyecto incluye un sistema automatizado para poblar la base de datos con información inicial para desarrollo.

## Cómo funciona
El proceso se ejecuta automáticamente al iniciar la aplicación (`Program.cs`) si se detecta que estamos en entorno `Development`.
1. Aplica migraciones pendientes (`Update-Database` automático).
2. Ejecuta `DbSeeder.cs`.
3. Lee archivos JSON desde la carpeta `/SeedData`.

## Datos de Prueba (Usuarios)

Por defecto, se crean los siguientes usuarios si no existen:

| Usuario | Contraseña (Dev) | Rol |
|---------|------------------|-----|
| admin | `Admin123!` | Administrador |
| demograd| `User123!` | Egresado |

**Nota de Seguridad:**
Las contraseñas en el archivo `seed-usuario.json` son texto plano solo para referencia. El código del seeder (`DbSeeder.cs`) las convierte a hash antes de guardar. 
> **IMPORTANTE:** El algoritmo actual en `DbSeeder.FakeHashPassword` es solo demostrativo. Debe reemplazarse por el servicio de hashing real que use la aplicación (ej. `BCrypt`, `PBKDF2`).

## Agregar nuevos datos
1. Modificar el archivo `.json` correspondiente en `SeedData`.
2. Reiniciar la API. El seeder es **idempotente**, verificará si el registro existe por su clave natural (ej. Nombre, DNI, Usuario) y solo insertará si falta.

## Resetear Base de Datos
Si necesitas limpiar todo y empezar de cero en local:
```sql
-- Ejecutar en SQL Server Management Studio
USE [SiseDb]; -- tu base de datos
-- Cuidado: Esto borra datos
DELETE FROM Usuario;
DELETE FROM Rol;
-- etc... o simplemente drop database y reinicia la API.