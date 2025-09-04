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


INSERT INTO Propietarios (Dni, Apellido, Nombre, Telefono, Email) VALUES
('12345678', 'Pérez', 'Juan', '2664000000', 'juan.perez@mail.com'),
('87654321', 'Gómez', 'María', '2664111111', 'maria.gomez@mail.com');

INSERT INTO Inquilinos (Dni, NombreCompleto, Telefono, Email) VALUES
('11223344', 'Carlos López', '2664222222', 'carlos.lopez@mail.com'),
('44332211', 'Ana Torres', '2664333333', 'ana.torres@mail.com');
