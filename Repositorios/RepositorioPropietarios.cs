 using InmobiliariaApp.Models;
 using MySql.Data.MySqlClient;
 using Microsoft.Extensions.Configuration;
 using System.Collections.Generic;
 namespace InmobiliariaApp.Data.Repositorios
 {
    public class RepositorioPropietarios : ConexionBase
    {
        public RepositorioPropietarios(IConfiguration configuration) : base(configuration) { }
        // Método para obtener todos los propietarios
        public List<Propietario> GetAll()
        {
            var propietarios = new List<Propietario>();
            using (var connection = GetConnection())
            {
                connection.Open();
                var command = new MySqlCommand("SELECT IdPropietario, Dni, Apellido, Nombre, Telefono, Email FROM Propietarios", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        propietarios.Add(new Propietario
                        {
                            IdPropietario = reader.GetInt32("IdPropietario"),
                            Dni = reader.GetString("Dni"),
                            Apellido = reader.GetString("Apellido"),
                            Nombre = reader.GetString("Nombre"),
                            Telefono = reader.IsDBNull(reader.GetOrdinal("Telefono")) ? null : reader.GetString("Telefono"),
                            Email = reader.GetString("Email")
                        });
                    }
                }
            }
            return propietarios;
        }
        // Método para obtener un propietario por su ID
        public Propietario GetById(int id)
        {
            Propietario propietario = null;
            using (var connection = GetConnection())
            {
                connection.Open();
                var command = new MySqlCommand("SELECT IdPropietario, Dni, Apellido, Nombre, Telefono, Email FROM Propietarios WHERE IdPropietario = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        propietario = new Propietario
                        {
                            IdPropietario = reader.GetInt32("IdPropietario"),
                            Dni = reader.GetString("Dni"),
                            Apellido = reader.GetString("Apellido"),
                            Nombre = reader.GetString("Nombre"),
                            Telefono = reader.IsDBNull(reader.GetOrdinal("Telefono")) ? null : reader.GetString("Telefono"),
                            Email = reader.GetString("Email")
                        };
                    }
                }
            }
            return propietario;
        }
        // Método para insertar un nuevo propietario
        public int Insert(Propietario propietario)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                var command = new MySqlCommand(
                    "INSERT INTO Propietarios (Dni, Apellido, Nombre, Telefono, Email) VALUES (@Dni, @Apellido, @Nombre, @Telefono, @Email); SELECT LAST_INSERT_ID();", connection);
                command.Parameters.AddWithValue("@Dni", propietario.Dni);
                command.Parameters.AddWithValue("@Apellido", propietario.Apellido);
                command.Parameters.AddWithValue("@Nombre", propietario.Nombre);
                command.Parameters.AddWithValue("@Telefono", (object)propietario.Telefono ?? DBNull.Value); // Manejo de valores nulos
                command.Parameters.AddWithValue("@Email", propietario.Email);
                
                return Convert.ToInt32(command.ExecuteScalar()); // Devuelve el ID del nuevo registro
            }
        }
        // Método para actualizar un propietario existente
        public bool Update(Propietario propietario)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                var command = new MySqlCommand(
                    "UPDATE Propietarios SET Dni = @Dni, Apellido = @Apellido, Nombre = @Nombre, Telefono = @Telefono, Email = @Email WHERE IdPropietario = @Id", connection);
                command.Parameters.AddWithValue("@Dni", propietario.Dni);
                command.Parameters.AddWithValue("@Apellido", propietario.Apellido);
                command.Parameters.AddWithValue("@Nombre", propietario.Nombre);
                command.Parameters.AddWithValue("@Telefono", (object)propietario.Telefono ?? DBNull.Value);
                command.Parameters.AddWithValue("@Email", propietario.Email);
                command.Parameters.AddWithValue("@Id", propietario.IdPropietario);
                return command.ExecuteNonQuery() > 0;
            }
        }
        // Método para eliminar un propietario
        public bool Delete(int id)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                var command = new MySqlCommand("DELETE FROM Propietarios WHERE IdPropietario = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                return command.ExecuteNonQuery() > 0;
            }
        }
    }
 }