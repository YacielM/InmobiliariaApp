using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;
using InmobiliariaApp.Models;

namespace InmobiliariaApp.Data.Repositorios
{
    public class RepositorioUsuarios : ConexionBase
    {
        public RepositorioUsuarios(IConfiguration configuration) : base(configuration) { }

        public List<Usuario> ObtenerTodos()
        {
            var lista = new List<Usuario>();
            using (var conn = GetConnection())
            {
                var sql = "SELECT IdUsuario, Nombre, Apellido, Email, Clave, Rol, Avatar FROM Usuarios";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    conn.Open();
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        lista.Add(new Usuario
                        {
                            IdUsuario = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Apellido = reader.GetString(2),
                            Email = reader.GetString(3),
                            Clave = reader.GetString(4),
                            Rol = reader.GetString(5),
                            Avatar = reader.IsDBNull(6) ? null : reader.GetString(6)
                        });
                    }
                }
            }
            return lista;
        }

        public Usuario? ObtenerPorId(int id)
        {
            Usuario? usuario = null;
            using (var conn = GetConnection())
            {
                var sql = "SELECT IdUsuario, Nombre, Apellido, Email, Clave, Rol, Avatar FROM Usuarios WHERE IdUsuario=@id";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    conn.Open();
                    var reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        usuario = new Usuario
                        {
                            IdUsuario = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Apellido = reader.GetString(2),
                            Email = reader.GetString(3),
                            Clave = reader.GetString(4),
                            Rol = reader.GetString(5),
                            Avatar = reader.IsDBNull(6) ? null : reader.GetString(6)
                        };
                    }
                }
            }
            return usuario;
        }

        public int Alta(Usuario usuario)
        {
            using (var conn = GetConnection())
            {
                var sql = @"INSERT INTO Usuarios (Nombre, Apellido, Email, Clave, Rol, Avatar)
                            VALUES (@Nombre, @Apellido, @Email, @Clave, @Rol, @Avatar)";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                    cmd.Parameters.AddWithValue("@Apellido", usuario.Apellido);
                    cmd.Parameters.AddWithValue("@Email", usuario.Email);
                    cmd.Parameters.AddWithValue("@Clave", usuario.Clave);
                    cmd.Parameters.AddWithValue("@Rol", usuario.Rol);
                    cmd.Parameters.AddWithValue("@Avatar", (object?)usuario.Avatar ?? DBNull.Value);
                    conn.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        public int Modificacion(Usuario usuario)
        {
            using (var conn = GetConnection())
            {
                var sql = @"UPDATE Usuarios 
                            SET Nombre=@Nombre, Apellido=@Apellido, Email=@Email, Clave=@Clave, Rol=@Rol, Avatar=@Avatar
                            WHERE IdUsuario=@IdUsuario";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                    cmd.Parameters.AddWithValue("@Apellido", usuario.Apellido);
                    cmd.Parameters.AddWithValue("@Email", usuario.Email);
                    cmd.Parameters.AddWithValue("@Clave", usuario.Clave);
                    cmd.Parameters.AddWithValue("@Rol", usuario.Rol);
                    cmd.Parameters.AddWithValue("@Avatar", (object?)usuario.Avatar ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@IdUsuario", usuario.IdUsuario);
                    conn.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        public int Baja(int id)
        {
            using (var conn = GetConnection())
            {
                var sql = "DELETE FROM Usuarios WHERE IdUsuario=@id";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    conn.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        public Usuario? ObtenerPorEmail(string email)
        {
            Usuario? usuario = null;
            using (var conn = GetConnection())
            {
                var sql = "SELECT IdUsuario, Nombre, Apellido, Email, Clave, Rol, Avatar FROM Usuarios WHERE Email=@Email";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    conn.Open();
                    var reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        usuario = new Usuario
                        {
                            IdUsuario = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Apellido = reader.GetString(2),
                            Email = reader.GetString(3),
                            Clave = reader.GetString(4),
                            Rol = reader.GetString(5),
                            Avatar = reader.IsDBNull(6) ? null : reader.GetString(6)
                        };
                    }
                }
            }
            return usuario;
        }
    }
}
