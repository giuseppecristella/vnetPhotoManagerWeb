using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using VnetPhotoManager.Domain;

namespace VnetPhotoManager.Repository
{
    public class PaymentMethodRepository : Repository<PaymentType>
    {
        public PaymentMethodRepository()
        {

        }

        public List<PaymentType> GetPaymentTypes(string structureCode)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();

                SqlCommand command = new SqlCommand(@"SELECT * FROM PagamentiWEB WHERE CodiceStruttura=@structureCode", conn);
                command.Parameters.Add("@structureCode", SqlDbType.VarChar).Value = structureCode;

                using (SqlDataReader dr = command.ExecuteReader(CommandBehavior.SingleResult))
                {
                    var items = new List<PaymentType>();
                    while (dr.Read())
                    {
                        var item = BuildFromRecord(dr);
                        items.Add(item);
                    }
                    return items;
                }
            }
        }

        protected override PaymentType BuildFromRecord(IDataRecord record)
        {
            return new PaymentType
            {
                PaymentId = Convert.ToInt32(record["ID_Pagamento"]),
                Description = Convert.ToString(record["Descrizione"]),
                PaypalCode = Convert.ToString(record["CodicePaypal"])
            };
        }
    }
}
