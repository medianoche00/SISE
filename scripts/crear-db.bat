@echo off
setlocal

REM Validar argumento
IF "%~1"=="" (
    echo ❌ Debes proporcionar la ruta de la carpeta que contiene los scripts SQL.
    echo Ejemplo: crear_bd.bat "C:\ruta\de\scripts"
    exit /b 1
)

set SCRIPT_DIR=%~1
set SQLCMD="sqlcmd"
set SERVER=(localdb)\MSSQLLocalDB

echo Ejecutando tablas-constraints-ondelete.sql...
%SQLCMD% -b -S %SERVER% -i "%SCRIPT_DIR%\tablas-constraints-ondelete.sql"
IF ERRORLEVEL 1 (
    echo ❌ Error al ejecutar tablas-constraints-ondelete.sql. Cancelando el resto.
    exit /b 1
)

echo Ejecutando auditoria-triggers.sql...
%SQLCMD% -b -S %SERVER% -i "%SCRIPT_DIR%\auditoria-triggers.sql"

echo Ejecutando otros-triggers.sql...
%SQLCMD% -b -S %SERVER% -i "%SCRIPT_DIR%\otros-triggers.sql"

echo Ejecutando pa-catalogos.sql...
%SQLCMD% -b -S %SERVER% -i "%SCRIPT_DIR%\pa-catalogos.sql"

echo Ejecutando pa-expediente.sql...
%SQLCMD% -b -S %SERVER% -i "%SCRIPT_DIR%\pa-expediente.sql"

echo Ejecutando pa-persona.sql...
%SQLCMD% -b -S %SERVER% -i "%SCRIPT_DIR%\pa-persona.sql"

echo Ejecutando pa-roles.sql...
%SQLCMD% -b -S %SERVER% -i "%SCRIPT_DIR%\pa-roles.sql"

echo Ejecutando pa-ubicacion.sql...
%SQLCMD% -b -S %SERVER% -i "%SCRIPT_DIR%\pa-ubicacion.sql"

echo Ejecutando pa-usuarios.sql...
%SQLCMD% -b -S %SERVER% -i "%SCRIPT_DIR%\pa-usuarios.sql"

echo Ejecutando datos-iniciales.sql...
%SQLCMD% -b -S %SERVER% -i "%SCRIPT_DIR%\datos-iniciales.sql"

echo ✅ Todos los scripts ejecutados correctamente.
endlocal
pause
