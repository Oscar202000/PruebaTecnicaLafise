Sistema esta  desarrollado con backend en .NET 8 Web API y frontend en ASP.NET Core 8 MVC. Se utiliza Entity Framework Core (EF Core) como ORM.
OBJETIVOS
Crear y Actualizar mediante Stored Procedures (SP) en SQL Server.
Listar, Obtener y Eliminar usando DbContext de EF Core.

NOTA IMPORTANTE
En este proyecto no se usan migraciones de EF Core. La estructura de tablas y SPs se mantiene manualmente en SQL Server.

TECNOLOGÍAS
Backend API: ASP.NET Core 8 Web API
ORM: Entity Framework Core y Stored Procedures (SQL Server)
Base de datos: SQL Server
Frontend: ASP.NET Core 8 MVC + jQuery
UI/UX: Bootstrap 4 + SweetAlert2
Control de versiones: Git / GitHub

ESTRUCTURA DEL REPOSITORIO
/Aplicativo.Lafise Proyecto MVC (frontend)
/Servicio.Lafise Proyecto Web API (backend)
/Scripts
    GestionLafiseBD.sql Script de creación de BD, tablas y SPs
README.md Documentación de uso

REQUISITOS PREVIOS
.NET 8 SDK instalado
SQL Server (local o remoto)
Git

CONFIGURAR LA BASE DE DATOS

Abrir Scripts/GestionLafiseBD.sql en SQL Server Management Studio.

Ejecutar todo el script para crear la base GestionLafise, tablas Clientes y Cuentas, datos de ejemplo y SPs para CRUD.

EJECUTAR EL BACKEND (API)
Abrir Servicio.Lafise.sln en Visual Studio o VS Code.
En appsettings.json, configurar la conexión:
"ConnectionStrings": { "DefaultConnection": "Server=.;Database=GestionLafise;Trusted_Connection=True;" }

En la carpeta Servicio.Lafise ejecutar:
dotnet restore
dotnet build
dotnet run
La API quedará disponible en https://localhost:7007/api/v1/

EJECUTAR EL FRONTEND (APLICATIVO)

Abrir Aplicativo.Lafise.sln.

En appsettings.json ajustar la URL de la API si es necesario:
"ApiSettings": { "ServicioApi": "https://localhost:7007/api/v1/" }

En la carpeta Aplicativo.Lafise ejecutar:
dotnet restore
dotnet build
dotnet run
El frontend quedará disponible en https://localhost:5001/Clientes

USO DE LA APLICACIÓN

Listar: muestra todas las cuentas con columnas Número de Cuenta, Nombre, Identificación, Fecha Apertura, Saldo Disponible y Estado.

Filtrar: búsqueda en vivo por Nombre o Identificación.

Crear: clic en “Agregar Cuenta”, completar datos y confirmar.

Editar: seleccionar fila, clic en “Modificar Cuenta”, ajustar datos y guardar.

Eliminar: seleccionar fila, clic en “Eliminar Cuenta” y confirmar.
