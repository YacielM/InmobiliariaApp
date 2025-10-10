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
                                c.FechaTerminacionAnticipada, c.MultaTerminacion, c.MultaPagada,
                                c.CreadoPor, c.TerminadoPor,
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
                            FechaTerminacionAnticipada = reader.IsDBNull(6) ? (DateTime?)null : reader.GetDateTime(6),
                            MultaTerminacion = reader.IsDBNull(7) ? (decimal?)null : reader.GetDecimal(7),
                            MultaPagada = !reader.IsDBNull(8) && reader.GetBoolean(8),
                            CreadoPor = reader.IsDBNull(9) ? null : reader.GetString(9),
                            TerminadoPor = reader.IsDBNull(10) ? null : reader.GetString(10),
                            Inmueble = new Inmueble { Direccion = reader.GetString(11) },
                            Inquilino = new Inquilino { NombreCompleto = reader.GetString(12) }
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
                                c.FechaTerminacionAnticipada, c.MultaTerminacion, c.MultaPagada,
                                c.CreadoPor, c.TerminadoPor,
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
                            FechaTerminacionAnticipada = reader.IsDBNull(6) ? (DateTime?)null : reader.GetDateTime(6),
                            MultaTerminacion = reader.IsDBNull(7) ? (decimal?)null : reader.GetDecimal(7),
                            MultaPagada = !reader.IsDBNull(8) && reader.GetBoolean(8),
                            CreadoPor = reader.IsDBNull(9) ? null : reader.GetString(9),
                            TerminadoPor = reader.IsDBNull(10) ? null : reader.GetString(10),
                            Inmueble = new Inmueble { Direccion = reader.GetString(11) },
                            Inquilino = new Inquilino { NombreCompleto = reader.GetString(12) }
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
                var sql = @"INSERT INTO Contratos 
                            (IdInmueble, IdInquilino, FechaInicio, FechaFin, MontoMensual, CreadoPor)
                            VALUES (@IdInmueble, @IdInquilino, @FechaInicio, @FechaFin, @MontoMensual, @CreadoPor)";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@IdInmueble", contrato.IdInmueble);
                    cmd.Parameters.AddWithValue("@IdInquilino", contrato.IdInquilino);
                    cmd.Parameters.AddWithValue("@FechaInicio", contrato.FechaInicio);
                    cmd.Parameters.AddWithValue("@FechaFin", contrato.FechaFin);
                    cmd.Parameters.AddWithValue("@MontoMensual", contrato.MontoMensual);
                    cmd.Parameters.AddWithValue("@CreadoPor", contrato.CreadoPor);
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

        //método para controlar superposición de fechas
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
        // Contratos vigentes (hoy entre FechaInicio y FechaFin)
        public List<Contrato> ObtenerVigentes()
        {
            var lista = new List<Contrato>();
            using (var connection = GetConnection())
            {
                connection.Open();
                var sql = @"SELECT c.IdContrato, c.IdInmueble, c.IdInquilino, c.FechaInicio, c.FechaFin, c.MontoMensual,
                                c.FechaTerminacionAnticipada, c.MultaTerminacion, c.MultaPagada,
                                i.Direccion,
                                inq.NombreCompleto
                            FROM Contratos c
                            INNER JOIN Inmuebles i ON c.IdInmueble = i.IdInmueble
                            INNER JOIN Inquilinos inq ON c.IdInquilino = inq.IdInquilino
                            WHERE c.FechaInicio <= CURDATE() AND c.FechaFin >= CURDATE()";
                using (var command = new MySqlCommand(sql, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
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
                                FechaTerminacionAnticipada = reader.IsDBNull(6) ? (DateTime?)null : reader.GetDateTime(6),
                                MultaTerminacion = reader.IsDBNull(7) ? (decimal?)null : reader.GetDecimal(7),
                                MultaPagada = !reader.IsDBNull(8) && reader.GetBoolean(8),
                                Inmueble = new Inmueble { Direccion = reader.GetString(9) },
                                Inquilino = new Inquilino { NombreCompleto = reader.GetString(10) }
                            });
                        }
                    }
                }
            }
            return lista;
        }


        // Contratos de un inmueble en particular
        public List<Contrato> ObtenerPorInmueble(int idInmueble)
        {
            var lista = new List<Contrato>();
            using (var conn = GetConnection())
            {
                var sql = @"SELECT c.IdContrato, c.IdInmueble, c.IdInquilino, c.FechaInicio, c.FechaFin, c.MontoMensual,
                                i.Direccion,
                                inq.NombreCompleto
                            FROM Contratos c
                            INNER JOIN Inmuebles i ON c.IdInmueble = i.IdInmueble
                            INNER JOIN Inquilinos inq ON c.IdInquilino = inq.IdInquilino
                            WHERE c.IdInmueble = @idInmueble";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@idInmueble", idInmueble);
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
        public int RenovarDesde(int idContratoOriginal, DateTime nuevaFechaInicio, DateTime nuevaFechaFin, decimal nuevoMontoMensual)
        {
            // Traer original para copiar campos base
            var original = ObtenerPorId(idContratoOriginal);
            if (original == null) return -1;

            using (var conn = GetConnection())
            {
                var sql = @"INSERT INTO Contratos (IdInmueble, IdInquilino, FechaInicio, FechaFin, MontoMensual)
                            VALUES (@IdInmueble, @IdInquilino, @FechaInicio, @FechaFin, @MontoMensual)";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@IdInmueble", original.IdInmueble);
                    cmd.Parameters.AddWithValue("@IdInquilino", original.IdInquilino);
                    cmd.Parameters.AddWithValue("@FechaInicio", nuevaFechaInicio);
                    cmd.Parameters.AddWithValue("@FechaFin", nuevaFechaFin);
                    cmd.Parameters.AddWithValue("@MontoMensual", nuevoMontoMensual);
                    conn.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        public int TerminarAnticipado(int idContrato, DateTime fechaCorte, decimal? multa = null, string terminadoPor = null)
        {
            using (var conn = GetConnection())
            {
                var sql = @"UPDATE Contratos
                            SET FechaFin = @FechaFin,
                                FechaTerminacionAnticipada = @FechaTerminacionAnticipada,
                                MultaTerminacion = @MultaTerminacion,
                                TerminadoPor = @TerminadoPor
                            WHERE IdContrato = @IdContrato";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@IdContrato", idContrato);
                    cmd.Parameters.AddWithValue("@FechaFin", fechaCorte);
                    cmd.Parameters.AddWithValue("@FechaTerminacionAnticipada", fechaCorte);
                    cmd.Parameters.AddWithValue("@MultaTerminacion", multa.HasValue ? multa.Value : (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@TerminadoPor", terminadoPor ?? (object)DBNull.Value);
                    conn.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        public void MarcarMultaPagada(int idContrato)
        {
            using (var conn = GetConnection())
            {
                var sql = "UPDATE Contratos SET MultaPagada = 1 WHERE IdContrato = @id";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", idContrato);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        
    }
}
