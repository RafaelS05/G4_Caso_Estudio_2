CREATE DATABASE CasoEstudioJN;
GO

USE CasoEstudioJN;
GO

CREATE TABLE CasasSistema (
	IdCasa BIGINT IDENTITY (1, 1) PRIMARY KEY NOT NULL,
	DescripcionCasa VARCHAR(30) NOT NULL,
	PrecioCasa Decimal(10, 2) NOT NULL,
	UsuarioAlquiler VARCHAR(30),
	FechaAlquiler Datetime
);

SELECT * FROM CasasSistema;

INSERT INTO [dbo].[CasasSistema] ([DescripcionCasa],[PrecioCasa],[UsuarioAlquiler],[FechaAlquiler])
VALUES ('Casa en San José',190000,null,null)
INSERT INTO [dbo].[CasasSistema] ([DescripcionCasa],[PrecioCasa],[UsuarioAlquiler],[FechaAlquiler])
VALUES ('Casa en Alajuela',145000,null,null)
INSERT INTO [dbo].[CasasSistema] ([DescripcionCasa],[PrecioCasa],[UsuarioAlquiler],[FechaAlquiler])
VALUES ('Casa en Cartago',115000,null,null)
INSERT INTO [dbo].[CasasSistema] ([DescripcionCasa],[PrecioCasa],[UsuarioAlquiler],[FechaAlquiler])
VALUES ('Casa en Heredia',122000,null,null)
INSERT INTO [dbo].[CasasSistema] ([DescripcionCasa],[PrecioCasa],[UsuarioAlquiler],[FechaAlquiler])
VALUES ('Casa en Guanacaste',105000,null,null)

---- SP's

CREATE PROCEDURE SP_ConsultarCasas
AS
BEGIN
    SELECT 
        DescripcionCasa,
        PrecioCasa,
        UsuarioAlquiler,
        CASE 
            WHEN UsuarioAlquiler IS NULL THEN 'Disponible'
            ELSE 'Reservada'
        END AS Estado,
        FechaAlquiler   -- sin CONVERT, la vista se encarga del formato
    FROM  CasasSistema
    WHERE PrecioCasa BETWEEN 115000 AND 180000
    ORDER BY
        CASE WHEN UsuarioAlquiler IS NULL THEN 0 ELSE 1 END ASC;
END
GO

CREATE PROCEDURE SP_ObtenerCasasDisponibles
AS
BEGIN
    SELECT 
        IdCasa,
        DescripcionCasa,
        PrecioCasa
    FROM  CasasSistema
    WHERE UsuarioAlquiler IS NULL;
END
GO

CREATE PROCEDURE SP_ObtenerPrecioCasa
    @IdCasa BIGINT
AS
BEGIN
    SELECT PrecioCasa
    FROM   CasasSistema
    WHERE  IdCasa = @IdCasa;
END
GO

CREATE PROCEDURE SP_AlquilarCasa
    @IdCasa          BIGINT,
    @UsuarioAlquiler VARCHAR(30),
    @FechaAlquiler   DATETIME
AS
BEGIN
    UPDATE CasasSistema
    SET 
        UsuarioAlquiler = @UsuarioAlquiler,
        FechaAlquiler   = @FechaAlquiler
    WHERE IdCasa          = @IdCasa
      AND UsuarioAlquiler IS NULL; 
END
GO
