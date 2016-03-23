using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using VnetPhotoManager.Domain;

namespace VnetPhotoManager.Repository
{
    public class PhotoOrderRepository : Repository<PhotoOrder>
    {
        public List<PhotoOrder> GetAll(string clientCode)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();

                var command = new SqlCommand(@"select distinct percorso from ordini o inner join dettordine d on o.id_ordine = d.id_ordine where o.codicecliente = @clientCode", conn);
                command.Parameters.Add("@clientCode", SqlDbType.VarChar).Value = clientCode;

                using (SqlDataReader dr = command.ExecuteReader(CommandBehavior.SingleResult))
                {
                    var items = new List<PhotoOrder>();
                    while (dr.Read())
                    {
                        var item = BuildFromRecord(dr);
                        items.Add(item);
                    }
                    return items;
                }
            }
        }
        protected override PhotoOrder BuildFromRecord(IDataRecord record)
        {
            return new PhotoOrder
            {
                Name = Path.GetFileName(Convert.ToString(record["percorso"])),
                //OrderId = Convert.ToInt32(record["id_ordine"]),
                FtpPath = Convert.ToString(record["percorso"]),
            };
        }
    }
}
