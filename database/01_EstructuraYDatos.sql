-- 1. Tabla para el Worker (Pagos procesados)
CREATE TABLE PagosBoletaDigital (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    NumeroBoleta VARCHAR(50) NOT NULL,
    MontoCobrado DECIMAL(18,2) NOT NULL,
    FechaCobro DATETIME NOT NULL
);

-- 2. Tabla para el Dominio (Padrón de contribuyentes)
CREATE TABLE Contribuyentes (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    NumeroBoleta VARCHAR(50) NOT NULL UNIQUE,
    NombreCompleto VARCHAR(150) NOT NULL
);

-- 3. Tabla para la Seguridad (Usuarios de la API)
CREATE TABLE Usuarios (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Username VARCHAR(50) NOT NULL UNIQUE,
    PasswordHash VARCHAR(255) NOT NULL
);

-- 4. Datos de prueba iniciales para que el JOIN funcione
-- (Asegurate de que el NumeroBoleta coincida con alguno que procese tu Worker)
INSERT INTO Contribuyentes (NumeroBoleta, NombreCompleto) VALUES ('12345', 'Juan');