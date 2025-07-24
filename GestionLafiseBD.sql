/*Creacion base de datos*/
Create database GestionLafise;
GO
/*Uso base de datos*/
Use GestionLafise;
Go
/*Creacion de tablas base de datos*/
CREATE TABLE [dbo].[Clientes](
	[idClientes] [BIGINT]  PRIMARY KEY IDENTITY(1,1)  NOT NULL,
	[UsuarioCreacion] [Varchar](50) NULL,
	[fechaCreacion] [datetime] NULL,
	[UsuarioModificacion] [Varchar](50),
	[FechaModificacion] [datetime],
	[FechaApertura] [datetime] null,
	[Nombre] [varchar](250) NULL,
    [Identificacion] [VARCHAR](max)
);

CREATE NONCLUSTERED INDEX IX_Clientes_Idclientes
    ON dbo.Clientes (idClientes);

CREATE TABLE [dbo].[Cuentas](
	[idCuentas] [BIGINT] PRIMARY KEY IDENTITY(1,1) NOT NULL,
	[idClientes] [BIGINT] NULL, 
	[UsuarioCreacion] [Varchar](50) NULL,
	[fechaCreacion] [datetime] NULL,
	[UsuarioModificacion] [Varchar](50),
	[FechaModificacion] [datetime],
	[SaldoDisponible] [decimal](18,4) NULL,
	[Estado] [varchar](50)  NULL,
	[FechaApertura] [datetime] null,
);

CREATE NONCLUSTERED INDEX IX_Clientes_Idcuentas
    ON dbo.Cuentas (idCuentas);

ALTER TABLE [dbo].[Cuentas] WITH CHECK ADD FOREIGN KEY([idClientes])
REFERENCES [dbo].[Clientes] ([idClientes]);
GO
/*Insert de datos iniciales base de datos*/

Declare @FechaProceso datetime ;

set @FechaProceso = Getdate();

insert into  [dbo].[Clientes](
 [UsuarioCreacion]
,[fechaCreacion]
,[FechaApertura]
,[Nombre]
,[Identificacion] )
values 
('sistema',@FechaProceso,@FechaProceso,'Oscar Arauz', '041-05052000-0000'),
('sistema',@FechaProceso,@FechaProceso,'Adrian Gonzalez', '001-05052000-0000'),
('sistema',@FechaProceso,@FechaProceso,'Lucas Gonzalez', '045-05052000-0000');

insert into [dbo].[Cuentas] ( 
[idClientes]
,[UsuarioCreacion]
,[fechaCreacion]
,[SaldoDisponible]
,[Estado]
,[FechaApertura])
values(1,'sistema',@FechaProceso,20000.52, 'Activo',@FechaProceso ),
(2,'sistema',@FechaProceso,1000.54, 'Activo',@FechaProceso ),
(3,'sistema',@FechaProceso,300000.56, 'Activo',@FechaProceso );

/*Procedimientos almacenados base de datos*/
---------------/*Clientes*/------------------
/*Inserta Clientes*/
GO
CREATE PROCEDURE SP_InsertaClientesCuentas
(  
    @Nombre VARCHAR(100) = null ,
	@Identificacion Varchar(MAX) = null,
    @SaldoDisponible DECIMAL(18,2) = null

)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

		DECLARE @IdCliente bigint;
		DECLARE @Sistemas Varchar(50) = 'Sistema';
		DECLARE @FechaProceso datetime = Getdate();
		DECLARE @Estado Varchar(50);

		set @Estado = 'Activo'
		BEGIN
        INSERT INTO Clientes (UsuarioCreacion,fechaCreacion,FechaApertura,Nombre, Identificacion)
        VALUES (@Sistemas,@FechaProceso,@FechaProceso,@Nombre,@Identificacion);
		END;
		BEGIN
        SET @IdCliente = SCOPE_IDENTITY();
		END;
		BEGIN
        INSERT INTO Cuentas (UsuarioCreacion,fechaCreacion,SaldoDisponible, Estado, FechaApertura, idClientes)
        VALUES (@Sistemas,@FechaProceso,@SaldoDisponible, @Estado, @FechaProceso, @IdCliente);
		END;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO
/*Actualiza*/
CREATE PROCEDURE SP_ActualizaClientes
(  

    @IdClientes bigint = null,
    @Nombre VARCHAR(100) = null,
	@Identificacion Varchar(MAX) = null

)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
		
		DECLARE @Sistemas Varchar(50) = 'Sistema';
		DECLARE @FechaProceso datetime = Getdate();

           update A
		   Set A.Nombre = @Nombre,
		       A.Identificacion = @Identificacion,
			   A.FechaModificacion = @FechaProceso,
			   A.UsuarioModificacion = @Sistemas
           from dbo.Clientes A
		   where A.idClientes = @IdClientes;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO
---------------/*Cuentas*/------------------
/*Obtener Cuentas*/
CREATE PROCEDURE SP_ObtenerCuentas

AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
		
		BEGIN

		SELECT   a.[idCuentas]
				,b.[idClientes]				
				,a.[SaldoDisponible]
				,a.[FechaApertura]
				,b.[Nombre]
				,b.[Identificacion]
				,a.[Estado]			
		 FROM [dbo].[Cuentas] a
		 INNER join [dbo].[Clientes] b on a.[idClientes] = b.[idClientes];

         END;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO
/*Inserta Cuentas*/
create PROCEDURE SP_InsertaCuentas
(  
    @Idcliente bigint = null ,
    @SaldoDisponible DECIMAL(18,2) = null

)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

		DECLARE @Sistemas Varchar(50) = 'Sistema';
		DECLARE @FechaProceso datetime = Getdate();
		DECLARE @Estado Varchar(50);

		set @Estado = 'Activo'
     
        INSERT INTO Cuentas (UsuarioCreacion,fechaCreacion,SaldoDisponible, Estado, FechaApertura, idClientes)
        VALUES (@Sistemas,@FechaProceso,@SaldoDisponible, @Estado, @FechaProceso, @IdCliente);

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO
/*Actualiza Cuentas*/
CREATE PROCEDURE SP_ActualizaCuentas
(  

    @IdCuentas bigint = null,
    @Estado VARCHAR(100) = null,
	@SaldoDsisponible decimal(18,4) = null

)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
		
		DECLARE @Sistemas Varchar(50) = 'Sistema';
		DECLARE @FechaProceso datetime = Getdate();
		BEGIN
           update A
		   Set A.SaldoDisponible = @SaldoDsisponible,
		       A.Estado = @Estado,
			   A.FechaModificacion = @FechaProceso,
			   A.UsuarioModificacion = @Sistemas
           from dbo.Cuentas A
		   where A.idCuentas = @IdCuentas;
		 END;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO
/*Obtener por ID Cuentas*/

CREATE PROCEDURE SP_ObtenerCuentasPorId
(
@idCuenta bigint 
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
		
		BEGIN

		SELECT   a.[idCuentas]
				,b.[idClientes]				
				,a.[SaldoDisponible]
				,a.[FechaApertura]
				,b.[Nombre]
				,b.[Identificacion]
				,a.[Estado]			
		 FROM [dbo].[Cuentas] a
		 INNER join [dbo].[Clientes] b on a.[idClientes] = b.[idClientes]
		 where idCuentas = @idCuenta;

         END;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO













