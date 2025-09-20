CREATE DATABASE IF NOT EXISTS inmobiliaria_db;
USE inmobiliaria_db;


CREATE TABLE IF NOT EXISTS Propietarios (
    IdPropietario INT AUTO_INCREMENT PRIMARY KEY,
    Dni VARCHAR(8) NOT NULL,
    Apellido VARCHAR(100) NOT NULL,
    Nombre VARCHAR(100) NOT NULL,
    Telefono VARCHAR(20),
    Email VARCHAR(150) NOT NULL
);


CREATE TABLE IF NOT EXISTS Inquilinos (
    IdInquilino INT AUTO_INCREMENT PRIMARY KEY,
    Dni VARCHAR(8) NOT NULL,
    NombreCompleto VARCHAR(200) NOT NULL,
    Telefono VARCHAR(20),
    Email VARCHAR(150) NOT NULL
);

CREATE TABLE IF NOT EXISTS Inmuebles (
    IdInmueble INT AUTO_INCREMENT PRIMARY KEY,
    Direccion VARCHAR(200) NOT NULL,
    Uso VARCHAR(50) NOT NULL,
    Tipo VARCHAR(50) NOT NULL,
    Ambientes INT NOT NULL,
    Precio DECIMAL(10,2) NOT NULL,
    IdPropietario INT NOT NULL,
    FOREIGN KEY (IdPropietario) REFERENCES Propietarios(IdPropietario)
);

CREATE TABLE IF NOT EXISTS Contratos (
    IdContrato INT AUTO_INCREMENT PRIMARY KEY,
    IdInmueble INT NOT NULL,
    IdInquilino INT NOT NULL,
    FechaInicio DATE NOT NULL,
    FechaFin DATE NOT NULL,
    MontoMensual DECIMAL(10,2) NOT NULL,
    FOREIGN KEY (IdInmueble) REFERENCES Inmuebles(IdInmueble),
    FOREIGN KEY (IdInquilino) REFERENCES Inquilinos(IdInquilino)
);

-- 1. Propietarios
INSERT INTO Propietarios (Dni, Apellido, Nombre, Telefono, Email) VALUES
('20123456', 'Pérez', 'Juan', '2664000000', 'juan.perez@mail.com'),
('20987654', 'Gómez', 'María', '2664111111', 'maria.gomez@mail.com'),
('22111222', 'Rodríguez', 'Carlos', '2664222222', 'carlos.rodriguez@mail.com');

-- 2. Inquilinos
INSERT INTO Inquilinos (Dni, NombreCompleto, Telefono, Email) VALUES
('30123456', 'Ana Torres', '2664333333', 'ana.torres@mail.com'),
('30987654', 'Luis Fernández', '2664444444', 'luis.fernandez@mail.com'),
('31111222', 'Sofía Martínez', '2664555555', 'sofia.martinez@mail.com');

-- 3. Inmuebles
INSERT INTO Inmuebles (Direccion, Uso, Tipo, Ambientes, Precio, IdPropietario) VALUES
('Av. San Martín 123', 'Residencial', 'Casa', 4, 85000.00, 1),
('Belgrano 456', 'Comercial', 'Local', 1, 120000.00, 2),
('Rivadavia 789', 'Residencial', 'Departamento', 3, 65000.00, 3);

-- 4. Contratos
INSERT INTO Contratos (IdInmueble, IdInquilino, FechaInicio, FechaFin, MontoMensual) VALUES
(1, 1, '2025-01-01', '2025-12-31', 85000.00),
(2, 2, '2025-03-01', '2026-02-28', 120000.00),
(3, 3, '2025-05-15', '2026-05-14', 65000.00);

