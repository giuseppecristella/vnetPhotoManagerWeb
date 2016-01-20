using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using VnetPhotoManager.Domain;

namespace VnetPhotoManager.Repository
{
    public class OrderRepository : Repository<Order>
    {
        public OrderRepository()
        {

        }

        public int SaveOrder(Order order)
        {
            object ret;
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();

                SqlCommand command = new SqlCommand(@"insert into ordini (ID_Pagamento, CodiceCliente, Numero, Codice, SubCodice, Descrizione, Creato, Consegna, Note, Consegnato)
                    values (@idPagamento, @CodiceCliente, @Numero, @Codice, @SubCodice, @Descrizione, @Creato, @Consegna, @Note, @Consegnato);select @@IDENTITY;", conn);

                command.Parameters.Add("@idPagamento", SqlDbType.VarChar).Value = order.PaymentId;
                command.Parameters.Add("@CodiceCliente", SqlDbType.VarChar).Value = order.ClientCode;
                command.Parameters.Add("@Numero", SqlDbType.Int).Value = order.OrderNumber;
                command.Parameters.Add("@Codice", SqlDbType.VarChar).Value = order.Code;
                command.Parameters.Add("@SubCodice", SqlDbType.VarChar).Value = order.SubCode;
                command.Parameters.Add("@Descrizione", SqlDbType.VarChar).Value = order.Description;
                command.Parameters.Add("@Creato", SqlDbType.SmallDateTime).Value = order.Created;
                command.Parameters.Add("@Consegna", SqlDbType.SmallDateTime).Value = order.Delivered;
                command.Parameters.Add("@Note", SqlDbType.VarChar).Value = order.Note;
                command.Parameters.Add("@Consegnato", SqlDbType.Bit).Value = order.IdDelivered;

                //command.Parameters.Add("@id", SqlDbType.Int).Direction = ParameterDirection.Output;

                try
                {
                    ret = command.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    conn.Close();
                    command.Dispose();
                }

                return Convert.ToInt32(ret);
            }

        }

        public int SaveOrderDetail(OrderDetail orderDetail)
        {
            object ret;
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();

                SqlCommand command =
                    new SqlCommand(
                        @"insert into DettOrdine (ID_Ordine, ID_Catalogo, Percorso, Copie) 
                        values (@ID_Ordine, @ID_Catalogo, @Percorso, @Copie);select @@IDENTITY;",
                        conn);

                command.Parameters.Add("@ID_Ordine", SqlDbType.Int).Value = orderDetail.OrderId;
                command.Parameters.Add("@ID_Catalogo", SqlDbType.Int).Value = orderDetail.ProductId;
                command.Parameters.Add("@Percorso", SqlDbType.VarChar).Value = orderDetail.FtpPhotoPath;
                command.Parameters.Add("@Copie", SqlDbType.Int).Value = orderDetail.CopyNumber;

                try
                {
                    ret = command.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    conn.Close();
                    command.Dispose();
                }
                return Convert.ToInt32(ret);
            }
        }

        public Order GetLastOrder()
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();

                SqlCommand command = new SqlCommand(@"SELECT top 1 * FROM Ordini order by Numero desc", conn);

                using (SqlDataReader dr = command.ExecuteReader(CommandBehavior.SingleResult))
                {
                    var items = new List<Order>();
                    while (dr.Read())
                    {
                        var item = BuildFromRecord(dr);
                        items.Add(item);
                    }
                    return items.FirstOrDefault();
                }
            }
        }

        protected override Order BuildFromRecord(IDataRecord record)
        {
            // TODO: completare mapping
            return new Order
            {
                OrderNumber = Convert.ToInt32(record["Numero"])
            };
        }
    }


}
