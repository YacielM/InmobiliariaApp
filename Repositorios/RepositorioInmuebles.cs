using InmobiliariaApp.Models;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace InmobiliariaApp.Data.Repositorios
{
    public class RepositorioInmuebles : ConexionBase
    {
        public RepositorioInmuebles(IConfiguration configuration) : base(configuration) { }

        public List<Inmueble> ObtenerTodos()
        {
            var lista = new List<Inmueble>();
            using (var conn = GetConnection())
            {
                conn.Open();
                var sql = @"SELECT i.IdInmueble, i.Direccion, i.Uso, i.Tipo, i.Ambientes, i.Precio, i.IdPropietario,
                                   p.Nombre, p.Apellido
                            FROM Inmuebles i
                            INNER JOIN Propietarios p ON i.IdPropietario = p.IdPropietario
                            ORDER BY i.IdInmueble DESC";
                using (var cmd = new MySqlCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Inmueble
                        {
                            IdInmueble = reader.GetInt32(0),
                            Direccion = reader.GetString(1),
                            Uso = reader.GetString(2),
                            Tipo = reader.GetString(3),
                            Ambientes = reader.GetInt32(4),
                            Precio = reader.GetDecimal(5),
                            IdPropietario = reader.GetInt32(6),
                            Propietario = new Propietario
                            {
                                Nombre = reader.GetString(7),
                                Apellido = reader.GetString(8)
                            }
                        });
                    }
                }
            }
            return lista;
        }

        public Inmueble ObtenerPorId(int id)
        {
            Inmueble inmueble = null;
            using (var conn = GetConnection())
            {
                conn.Open();
                var sql = @"SELECT i.IdInmueble, i.Direccion, i.Uso, i.Tipo, i.Ambientes, i.Precio, i.IdPropietario,
                                   p.Nombre, p.Apellido
                            FROM Inmuebles i
                            INNER JOIN Propietarios p ON i.IdPropietario = p.IdPropietario
                            WHERE i.IdInmueble = @id";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            inmueble = new Inmueble
                            {
                                IdInmueble = reader.GetInt32(0),
                                Direccion = reader.GetString(1),
                                Uso = reader.GetString(2),
                                Tipo = reader.GetString(3),
                                Ambientes = reader.GetInt32(4),
                                Precio = reader.GetDecimal(5),
                                IdPropietario = reader.GetInt32(6),
                                Propietario = new Propietario
                                {
                                    Nombre = reader.GetString(7),
                                    Apellido = reader.GetString(8)
                                }
                            };
                        }
                    }
                }
            }
            return inmueble;
        }

        public int Alta(Inmueble inmueble)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                var sql = @"INSERT INTO Inmuebles (Direccion, Uso, Tipo, Ambientes, Precio, IdPropietario)
                            VALUES (@Direccion, @Uso, @Tipo, @Ambientes, @Precio, @IdPropietario);
                            SELECT LAST_INSERT_ID();";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Direccion", inmueble.Direccion);
                    cmd.Parameters.AddWithValue("@Uso", inmueble.Uso);
                    cmd.Parameters.AddWithValue("@Tipo", inmueble.Tipo);
                    cmd.Parameters.AddWithValue("@Ambientes", inmueble.Ambientes);
                    cmd.Parameters.AddWithValue("@Precio", inmueble.Precio);
                    cmd.Parameters.AddWithValue("@IdPropietario", inmueble.IdPropietario);

                    var id = cmd.ExecuteScalar();
                    return System.Convert.ToInt32(id);
                }
            }
        }

        public int Modificacion(Inmueble inmueble)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                var sql = @"UPDATE Inmuebles
                            SET Direccion=@Direccion, Uso=@Uso, Tipo=@Tipo, Ambientes=@Ambientes,
                                Precio=@Precio, IdPropietario=@IdPropietario
                            WHERE IdInmueble=@IdInmueble";
            using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Direccion", inmueble.Direccion);
                    cmd.Parameters.AddWithValue("@Uso", inmueble.Uso);
                    cmd.Parameters.AddWithValue("@Tipo", inmueble.Tipo);
                    cmd.Parameters.AddWithValue("@Ambientes", inmueble.Ambientes);
                    cmd.Parameters.AddWithValue("@Precio", inmueble.Precio);
                    cmd.Parameters.AddWithValue("@IdPropietario", inmueble.IdPropietario);
                    cmd.Parameters.AddWithValue("@IdInmueble", inmueble.IdInmueble);
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        public int Baja(int id)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                var sql = "DELETE FROM Inmuebles WHERE IdInmueble=@id";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    return cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
