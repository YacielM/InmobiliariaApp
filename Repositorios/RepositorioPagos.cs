using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;
using InmobiliariaApp.Models;

namespace InmobiliariaApp.Data.Repositorios
{
    public class RepositorioPagos : ConexionBase
    {
        public RepositorioPagos(IConfiguration configuration) : base(configuration) { }

        public List<Pago> ObtenerTodos()
        {
            var lista = new List<Pago>();
            using (var conn = GetConnection())
            {
                var sql = @"SELECT p.IdPago, p.IdContrato, p.Fecha, p.Importe,
                   i.Direccion, inq.NombreCompleto
            FROM Pagos p
            INNER JOIN Contratos c ON p.IdContrato = c.IdContrato
            INNER JOIN Inmuebles i ON c.IdInmueble = i.IdInmueble
            INNER JOIN Inquilinos inq ON c.IdInquilino = inq.IdInquilino";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    conn.Open();
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        lista.Add(new Pago
                        {
                            IdPago = reader.GetInt32(0),
                            IdContrato = reader.GetInt32(1),
                            Fecha = reader.GetDateTime(2),
                            Importe = reader.GetDecimal(3),
                            Contrato = new Contrato
                            {
                                IdContrato = reader.GetInt32(1),
                                Inmueble = new Inmueble { Direccion = reader.GetString(4) },
                                Inquilino = new Inquilino { NombreCompleto = reader.GetString(5) }
                            }
                        });
                    }
                }
            }
            return lista;
        }

        public Pago? ObtenerPorId(int id)
        {
            Pago? pago = null;
            using (var conn = GetConnection())
            {
                var sql = @"SELECT IdPago, IdContrato, Fecha, Importe
                            FROM Pagos WHERE IdPago=@id";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    conn.Open();
                    var reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        pago = new Pago
                        {
                            IdPago = reader.GetInt32(0),
                            IdContrato = reader.GetInt32(1),
                            Fecha = reader.GetDateTime(2),
                            Importe = reader.GetDecimal(3)
                        };
                    }
                }
            }
            return pago;
        }

        public int Alta(Pago pago)
        {
            using (var conn = GetConnection())
            {
                var sql = @"INSERT INTO Pagos (IdContrato, Fecha, Importe)
                            VALUES (@IdContrato, @Fecha, @Importe)";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@IdContrato", pago.IdContrato);
                    cmd.Parameters.AddWithValue("@Fecha", pago.Fecha);
                    cmd.Parameters.AddWithValue("@Importe", pago.Importe);
                    conn.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        public int Modificacion(Pago pago)
        {
            using (var conn = GetConnection())
            {
                var sql = @"UPDATE Pagos SET IdContrato=@IdContrato, Fecha=@Fecha, Importe=@Importe
                            WHERE IdPago=@IdPago";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@IdContrato", pago.IdContrato);
                    cmd.Parameters.AddWithValue("@Fecha", pago.Fecha);
                    cmd.Parameters.AddWithValue("@Importe", pago.Importe);
                    cmd.Parameters.AddWithValue("@IdPago", pago.IdPago);
                    conn.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        public int Baja(int id)
        {
            using (var conn = GetConnection())
            {
                var sql = "DELETE FROM Pagos WHERE IdPago=@id";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    conn.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
