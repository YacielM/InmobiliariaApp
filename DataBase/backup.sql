CREATE DATABASE IF NOT EXISTS inmobiliaria_db;
USE inmobiliaria_db;

-- =========================
-- 1. PROPIETARIOS
-- =========================
CREATE TABLE IF NOT EXISTS Propietarios (
    IdPropietario INT AUTO_INCREMENT PRIMARY KEY,
    Dni VARCHAR(8) NOT NULL,
    Apellido VARCHAR(100) NOT NULL,
    Nombre VARCHAR(100) NOT NULL,
    Telefono VARCHAR(20),
    Email VARCHAR(150) NOT NULL
);

INSERT INTO Propietarios (Dni, Apellido, Nombre, Telefono, Email) VALUES
('20123456', 'Pérez', 'Juan', '2664000000', 'juan.perez@mail.com'),
('20987654', 'Gómez', 'María', '2664111111', 'maria.gomez@mail.com'),
('22111222', 'Rodríguez', 'Carlos', '2664222222', 'carlos.rodriguez@mail.com'),
('46408906', 'Muñoz', 'Yaciel', '2664256205', 'yacielzombers@gmail.com'),
('30111222', 'López', 'Martina', '2664332211', 'martina.lopez@mail.com'),
('30222333', 'Suárez', 'Diego', '2664556677', 'diego.suarez@mail.com');

-- =========================
-- 2. INQUILINOS
-- =========================
CREATE TABLE IF NOT EXISTS Inquilinos (
    IdInquilino INT AUTO_INCREMENT PRIMARY KEY,
    Dni VARCHAR(8) NOT NULL,
    NombreCompleto VARCHAR(200) NOT NULL,
    Telefono VARCHAR(20),
    Email VARCHAR(150) NOT NULL
);

INSERT INTO Inquilinos (Dni, NombreCompleto, Telefono, Email) VALUES
('30123456', 'Ana Torres', '2664333333', 'ana.torres@mail.com'),
('30987654', 'Luis Fernández', '2664444444', 'luis.fernandez@mail.com'),
('31111222', 'Sofía Martínez', '2664555555', 'sofia.martinez@mail.com'),
('46778996', 'Luca Vega', '2664236577', 'chanchoconpatas@gmail.com'),
('46408555', 'Pedro Pascal', '2664236555', 'pobretes@gmail.com'),
('31222333', 'Valentina Ríos', '2664667788', 'valentina.rios@mail.com'),
('32333444', 'Mateo Herrera', '2664778899', 'mateo.herrera@mail.com');

-- =========================
-- 3. INMUEBLES
-- =========================
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

INSERT INTO Inmuebles (Direccion, Uso, Tipo, Ambientes, Precio, IdPropietario) VALUES
('Av. San Martín 123', 'Residencial', 'Casa', 4, 85000.00, 1),
('Belgrano 456', 'Comercial', 'Local', 1, 120000.00, 2),
('Rivadavia 789', 'Residencial', 'Departamento', 3, 65000.00, 3),
('Barrio 157 Viviendas C10', 'Residencial', 'Casa', 4, 250000.02, 4),
('Rivadavia 789 pepe', 'Residencial', 'Casa', 2, 88000.00, 4),
('Barrio 130 Viviendas C5', 'Residencial', 'Casa', 5, 818000.00, 4),
('San Juan 123', 'Residencial', 'Departamento', 2, 70000.00, 5),
('Mitre 456', 'Comercial', 'Local', 1, 150000.00, 6);

-- =========================
-- 4. CONTRATOS
-- =========================
CREATE TABLE IF NOT EXISTS Contratos (
    IdContrato INT AUTO_INCREMENT PRIMARY KEY,
    IdInmueble INT NOT NULL,
    IdInquilino INT NOT NULL,
    FechaInicio DATE NOT NULL,
    FechaFin DATE NOT NULL,
    MontoMensual DECIMAL(10,2) NOT NULL,
    FechaTerminacionAnticipada DATE DEFAULT NULL,
    MultaTerminacion DECIMAL(10,2) DEFAULT NULL,
    MultaPagada TINYINT(1) NOT NULL DEFAULT 0,
    CreadoPor VARCHAR(100) NOT NULL DEFAULT 'Sistema',
    TerminadoPor VARCHAR(100) DEFAULT NULL,
    FOREIGN KEY (IdInmueble) REFERENCES Inmuebles(IdInmueble),
    FOREIGN KEY (IdInquilino) REFERENCES Inquilinos(IdInquilino)
);

INSERT INTO Contratos (IdInmueble, IdInquilino, FechaInicio, FechaFin, MontoMensual, CreadoPor) VALUES
(1, 1, '2025-01-01', '2025-12-31', 85000.00, 'admin@inmo.com'),
(2, 2, '2025-03-01', '2026-02-28', 120000.00, 'admin@inmo.com'),
(3, 3, '2025-05-15', '2026-05-14', 65000.00, 'empleado@inmo.com'),
(7, 6, '2025-09-01', '2026-09-01', 70000.00, 'admin@inmo.com'),
(8, 7, '2025-08-15', '2026-08-15', 150000.00, 'empleado2@inmo.com');

-- =========================
-- 5. PAGOS
-- =========================
CREATE TABLE IF NOT EXISTS Pagos (
    IdPago INT AUTO_INCREMENT PRIMARY KEY,
    IdContrato INT NOT NULL,
    Fecha DATE NOT NULL,
    Importe DECIMAL(12,2) NOT NULL,
    CreadoPor VARCHAR(100) NOT NULL DEFAULT 'Sistema',
    FOREIGN KEY (IdContrato) REFERENCES Contratos(IdContrato)
);

INSERT INTO Pagos (IdContrato, Fecha, Importe, CreadoPor) VALUES
(1, '2025-10-01', 85000.00, 'admin@inmo.com'),
(2, '2025-10-05', 120000.00, 'admin@inmo.com'),
(3, '2025-10-15', 65000.00, 'empleado@inmo.com'),
(4, '2025-10-01', 70000.00, 'admin@inmo.com'),
(5, '2025-10-05', 150000.00, 'empleado2@inmo.com');

-- =========================
-- 6. USUARIOS
-- =========================
CREATE TABLE IF NOT EXISTS Usuarios (
    IdUsuario INT AUTO_INCREMENT PRIMARY KEY,
    Nombre VARCHAR(50) NOT NULL,
    Apellido VARCHAR(50) NOT NULL,
    Email VARCHAR(100) NOT NULL UNIQUE,
    Clave VARCHAR(255) NOT NULL,
    Rol VARCHAR(50) NOT NULL,
    Avatar VARCHAR(255)
);

INSERT INTO Usuarios (Nombre, Apellido, Email, Clave, Rol) VALUES
('Admin', 'Principal', 'admin@inmo.com', 'admin123', 'Administrador'),
('Empleado', 'Perez', 'empleado@inmo.com', 'empleado123', 'Empleado'),
('Empleado', 'García', 'empleado2@inmo.com', 'empleado456', 'Empleado');

