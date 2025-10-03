using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using InmobiliariaApp.Data;


namespace InmobiliariaApp.Models
{
    public class RepositorioContratos : ConexionBase
    {
        public RepositorioContratos(IConfiguration configuration) : base(configuration) { }

        public List<Contrato> ObtenerTodos()
        {
            var lista = new List<Contrato>();
            using (var conn = GetConnection())
            {
                var sql = @"SELECT c.IdContrato, c.IdInmueble, c.IdInquilino, c.FechaInicio, c.FechaFin, c.MontoMensual,
                                   i.Direccion,
                                   inq.NombreCompleto
                            FROM Contratos c
                            INNER JOIN Inmuebles i ON c.IdInmueble = i.IdInmueble
                            INNER JOIN Inquilinos inq ON c.IdInquilino = inq.IdInquilino";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    conn.Open();
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        lista.Add(new Contrato
                        {
                            IdContrato = reader.GetInt32(0),
                            IdInmueble = reader.GetInt32(1),
                            IdInquilino = reader.GetInt32(2),
                            FechaInicio = reader.GetDateTime(3),
                            FechaFin = reader.GetDateTime(4),
                            MontoMensual = reader.GetDecimal(5),
                            Inmueble = new Inmueble { Direccion = reader.GetString(6) },
                            Inquilino = new Inquilino { NombreCompleto = reader.GetString(7) }
                        });
                    }
                }
            }
            return lista;
        }

        public Contrato ObtenerPorId(int id)
        {
            Contrato contrato = null;
            using (var conn = GetConnection())
            {
                var sql = @"SELECT c.IdContrato, c.IdInmueble, c.IdInquilino, c.FechaInicio, c.FechaFin, c.MontoMensual,
                                   i.Direccion,
                                   inq.NombreCompleto
                            FROM Contratos c
                            INNER JOIN Inmuebles i ON c.IdInmueble = i.IdInmueble
                            INNER JOIN Inquilinos inq ON c.IdInquilino = inq.IdInquilino
                            WHERE c.IdContrato = @id";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    conn.Open();
                    var reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        contrato = new Contrato
                        {
                            IdContrato = reader.GetInt32(0),
                            IdInmueble = reader.GetInt32(1),
                            IdInquilino = reader.GetInt32(2),
                            FechaInicio = reader.GetDateTime(3),
                            FechaFin = reader.GetDateTime(4),
                            MontoMensual = reader.GetDecimal(5),
                            Inmueble = new Inmueble { Direccion = reader.GetString(6) },
                            Inquilino = new Inquilino { NombreCompleto = reader.GetString(7) }
                        };
                    }
                }
            }
            return contrato;
        }

        public int Alta(Contrato contrato)
        {
            int res = -1;
            using (var conn = GetConnection())
            {
                var sql = @"INSERT INTO Contratos (IdInmueble, IdInquilino, FechaInicio, FechaFin, MontoMensual)
                            VALUES (@IdInmueble, @IdInquilino, @FechaInicio, @FechaFin, @MontoMensual)";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@IdInmueble", contrato.IdInmueble);
                    cmd.Parameters.AddWithValue("@IdInquilino", contrato.IdInquilino);
                    cmd.Parameters.AddWithValue("@FechaInicio", contrato.FechaInicio);
                    cmd.Parameters.AddWithValue("@FechaFin", contrato.FechaFin);
                    cmd.Parameters.AddWithValue("@MontoMensual", contrato.MontoMensual);
                    conn.Open();
                    res = cmd.ExecuteNonQuery();
                }
            }
            return res;
        }

        public int Modificacion(Contrato contrato)
        {
            int res = -1;
            using (var conn = GetConnection())
            {
                var sql = @"UPDATE Contratos SET IdInmueble=@IdInmueble, IdInquilino=@IdInquilino, 
                            FechaInicio=@FechaInicio, FechaFin=@FechaFin, MontoMensual=@MontoMensual
                            WHERE IdContrato=@IdContrato";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@IdInmueble", contrato.IdInmueble);
                    cmd.Parameters.AddWithValue("@IdInquilino", contrato.IdInquilino);
                    cmd.Parameters.AddWithValue("@FechaInicio", contrato.FechaInicio);
                    cmd.Parameters.AddWithValue("@FechaFin", contrato.FechaFin);
                    cmd.Parameters.AddWithValue("@MontoMensual", contrato.MontoMensual);
                    cmd.Parameters.AddWithValue("@IdContrato", contrato.IdContrato);
                    conn.Open();
                    res = cmd.ExecuteNonQuery();
                }
            }
            return res;
        }

        public int Baja(int id)
        {
            int res = -1;
            using (var conn = GetConnection())
            {
                var sql = "DELETE FROM Contratos WHERE IdContrato=@id";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    conn.Open();
                    res = cmd.ExecuteNonQuery();
                }
            }
            return res;
        }

        // 🔎 Nuevo: método para controlar superposición de fechas
        public bool ExisteSuperposicion(int idInmueble, DateTime inicio, DateTime fin, int? idContrato = null)
        {
            using (var conn = GetConnection())
            {
                var sql = @"SELECT COUNT(*) FROM Contratos
                            WHERE IdInmueble = @IdInmueble
                              AND IdContrato <> IFNULL(@IdContrato, 0)
                              AND ( (FechaInicio <= @Fin AND FechaFin >= @Inicio) )";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@IdInmueble", idInmueble);
                    cmd.Parameters.AddWithValue("@Inicio", inicio);
                    cmd.Parameters.AddWithValue("@Fin", fin);
                    cmd.Parameters.AddWithValue("@IdContrato", idContrato.HasValue ? idContrato.Value : 0);
                    conn.Open();
                    var count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
        }
    }
}
