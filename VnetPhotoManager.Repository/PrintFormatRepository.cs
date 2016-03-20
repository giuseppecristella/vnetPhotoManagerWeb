using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using VnetPhotoManager.Domain;

namespace VnetPhotoManager.Repository
{
    public class PrintFormatRepository : Repository<PrintFormat>
    {
        public PrintFormatRepository()
        {

        }
        /// <summary>
        /// Restituisce la lista dei formati disponibili (Tab. Catalogo)
        /// </summary>
        /// <param name="username">Email del cliente</param>
        public List<PrintFormat> GetPhotoPrintFormats(string userEmail)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();

                SqlCommand command = new SqlCommand(@"SELECT * FROM USERCLIENTI U INNER JOIN CATALOGO C ON U.CODICESTRUTTURA = C.CODICESTRUTTURA AND U.EMAIL=@email", conn);
                command.Parameters.Add("@email", SqlDbType.VarChar).Value = userEmail;

                using (SqlDataReader dr = command.ExecuteReader(CommandBehavior.SingleResult))
                {
                    var items = new List<PrintFormat>();
                    while (dr.Read())
                    {
                        var item = BuildFromRecord(dr);
                        items.Add(item);
                    }
                    return items;
                }
            }
        }

        /// <summary>
        /// Restituisce l'immagine del prodotto
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public byte[] GetProductImage(int productId)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();

                SqlCommand command = new SqlCommand(@"SELECT * FROM CATALOGO WHERE ID_Catalogo=@productId", conn);
                command.Parameters.Add("@productId", SqlDbType.Int).Value = productId;

                using (SqlDataReader dr = command.ExecuteReader(CommandBehavior.SingleResult))
                {
                    dr.Read();
                    return (byte[])dr["Immagine"];
                }
            }
        }

        protected override PrintFormat BuildFromRecord(IDataRecord record)
        {
            return new PrintFormat
            {
                ProductId = Convert.ToInt32(record["ID_Catalogo"]),
                Code = Convert.ToString(record["CodiceStruttura"]),
                Description = Convert.ToString(record["Descrizione"]),
                Price = Convert.ToDouble(record["Prezzo"]),
                CategoryId = Convert.ToInt32(record["ID_Categoria"]),
                ImgThumb = (Byte[])record["Immagine"]
            };
        }
    }
}
