# 🛠 Prueba Técnica Lafise: CRUD de Productos y Gestión de Productos

Sistema , desarrollado con **.NET 8 Web API** (backend) y **ASP.NET Core 8 MVC** (frontend). Utiliza **Entity Framework Core** como ORM y **Stored Procedures** en SQL Server.

---

## 🎯 OBJETIVOS

1. **Crear** y **Actualizar** mediante Stored Procedures (SP) en SQL Server.  
2. **Listar**, **Obtener** y **Eliminar** usando **DbContext** de EF Core.

> **Nota importante:**  
> No se usan migraciones de EF Core; las tablas y SPs se mantienen manualmente en SQL Server.

---

## 🛠 TECNOLOGÍAS

- **Backend API:** ASP.NET Core 8 Web API  
- **ORM:** Entity Framework Core + Stored Procedures (SQL Server)  
- **Base de datos:** SQL Server  
- **Frontend:** ASP.NET Core 8 MVC + jQuery  
- **UI/UX:** Bootstrap 4 + SweetAlert2  
- **Control de versiones:** Git / GitHub


---

## 📂 ESTRUCTURA DEL REPOSITORIO
/Aplicativo.Lafise → Proyecto MVC (frontend)
/Servicio.Lafise → Proyecto Web API (backend)
/Scripts
└─ GestionLafiseBD.sql → Script de creación de BD, tablas y SPs
README.md → Documentación de uso
---

---

## ⚙️ REQUISITOS PREVIOS

1. [.NET 8 SDK](https://dotnet.microsoft.com/download)  
2. SQL Server (local o remoto)  
3. Git  

---

## 🗄️ CONFIGURAR LA BASE DE DATOS

1. Abre `Scripts/GestionLafiseBD.sql` en SQL Server Management Studio  
2. Ejecuta todo el script para:
   - Crear la base **GestionLafise**  
   - Crear tablas **Clientes** y **Cuentas**  
   - Insertar datos de ejemplo  
   - Crear Stored Procedures para CRUD  

---

## 🚀 EJECUTAR EL BACKEND (API)

1. Abre la solución `Servicio.Lafise.sln` en Visual Studio o VS Code  
2. En `appsettings.json`, configura la conexión:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=.;Database=GestionLafise;Trusted_Connection=True;"
   }


-------
## 🎮 USO DE LA APLICACIÓN

### 1. Listar
Muestra todas las cuentas con las siguientes columnas:  
- **Número de Cuenta**  
- **Nombre**  
- **Identificación**  
- **Fecha Apertura**  
- **Saldo Disponible**  
- **Estado**  

---

### 2. Filtrar
Búsqueda en vivo por **Nombre** o **Identificación**.

---

### 3. Crear
1. Haz clic en **“Agregar Cuenta”**.  
2. Completa los campos del formulario.  
3. Pulsa **“Guardar”** y confirma la creación.

---

### 4. Editar
1. Selecciona la fila de la cuenta que quieras modificar.  
2. Haz clic en **“Modificar Cuenta”**.  
3. Ajusta los datos en el modal y pulsa **“Guardar”**.

---

### 5. Eliminar
1. Selecciona la fila de la cuenta que quieras borrar.  
2. Haz clic en **“Eliminar Cuenta”**.  
3. Confirma en el diálogo de SweetAlert2.  


   
