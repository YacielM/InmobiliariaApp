 using InmobiliariaApp.Models;
 using MySql.Data.MySqlClient;
 using Microsoft.Extensions.Configuration;
 using System.Collections.Generic;
 namespace InmobiliariaApp.Data.Repositorios
 {
    public class RepositorioInquilinos : ConexionBase
    {
        public RepositorioInquilinos(IConfiguration configuration) : base(configuration) { }
        // Método para obtener todos los inquilinos
        public List<Inquilino> GetAll()
        {
            var inquilinos = new List<Inquilino>();
            using (var connection = GetConnection())
            {
                connection.Open();
                var command = new MySqlCommand("SELECT IdInquilino, Dni, NombreCompleto, Telefono, Email FROM Inquilinos", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        inquilinos.Add(new Inquilino
                        {
                            IdInquilino = reader.GetInt32("IdInquilino"),
                            Dni = reader.GetString("Dni"),
                            NombreCompleto = reader.GetString("NombreCompleto"),
                            Telefono = reader.IsDBNull(reader.GetOrdinal("Telefono")) ? null : reader.GetString("Telefono"),
                            Email = reader.GetString("Email")
                        });
                    }
                }
            }
            return inquilinos;
        }
        // Método para obtener un inquilino por su ID
        public Inquilino GetById(int id)
        {
            Inquilino inquilino = null;
            using (var connection = GetConnection())
            {
                connection.Open();
                var command = new MySqlCommand("SELECT IdInquilino, Dni, NombreCompleto, Telefono, Email FROM Inquilinos WHERE IdInquilino = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        inquilino = new Inquilino
                        {
                            IdInquilino = reader.GetInt32("IdInquilino"),
                            Dni = reader.GetString("Dni"),
                            NombreCompleto = reader.GetString("NombreCompleto"),
                            Telefono = reader.IsDBNull(reader.GetOrdinal("Telefono")) ? null : reader.GetString("Telefono"),
                            Email = reader.GetString("Email")
                        };
                    }
                }
            }
            return inquilino;
        }
        // Método para insertar un nuevo inquilino
        public int Insert(Inquilino inquilino)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                var command = new MySqlCommand(
                    "INSERT INTO Inquilinos (Dni, NombreCompleto, Telefono, Email) VALUES (@Dni, @NombreCompleto, @Telefono, @Email); SELECT LAST_INSERT_ID();", connection);
                command.Parameters.AddWithValue("@Dni", inquilino.Dni);
                command.Parameters.AddWithValue("@NombreCompleto", inquilino.NombreCompleto);
                command.Parameters.AddWithValue("@Telefono", (object)inquilino.Telefono ?? DBNull.Value);
                command.Parameters.AddWithValue("@Email", inquilino.Email);
                
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }
        // Método para actualizar un inquilino existente
        public bool Update(Inquilino inquilino)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                var command = new MySqlCommand(
                    "UPDATE Inquilinos SET Dni = @Dni, NombreCompleto = @NombreCompleto, Telefono = @Telefono, Email = @Email WHERE IdInquilino = @Id", connection);
                command.Parameters.AddWithValue("@Dni", inquilino.Dni);
                command.Parameters.AddWithValue("@NombreCompleto", inquilino.NombreCompleto);
                command.Parameters.AddWithValue("@Telefono", (object)inquilino.Telefono ?? DBNull.Value);
                command.Parameters.AddWithValue("@Email", inquilino.Email);
                command.Parameters.AddWithValue("@Id", inquilino.IdInquilino);
                return command.ExecuteNonQuery() > 0;
            }
        }
        // Método para eliminar un inquilino
        public bool Delete(int id)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                var command = new MySqlCommand("DELETE FROM Inquilinos WHERE IdInquilino = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                return command.ExecuteNonQuery() > 0;
            }
        }
    }
 }