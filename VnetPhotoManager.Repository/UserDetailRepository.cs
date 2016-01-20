using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using VnetPhotoManager.Domain;

namespace VnetPhotoManager.Repository
{
    public class UserDetailRepository : Repository<UserDetail>
    {
        public UserDetailRepository()
        {

        }

        public UserDetail GetUserDetail(string username)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();

                SqlCommand command = new SqlCommand(@"SELECT * FROM USERCLIENTI where EMAIL=@username", conn);
                command.Parameters.Add("@username", SqlDbType.VarChar).Value = username;

                using (SqlDataReader dr = command.ExecuteReader(CommandBehavior.SingleResult))
                {
                    var items = new List<UserDetail>();
                    while (dr.Read())
                    {
                        var item = BuildFromRecord(dr);
                        items.Add(item);
                    }
                    return items.FirstOrDefault();
                }
            }
        }

        protected override UserDetail BuildFromRecord(IDataRecord record)
        {
            return new UserDetail
            {
                StructureCode = Convert.ToString(record["CodiceStruttura"]),
                UserName = Convert.ToString(record["Email"]),
                Name = Convert.ToString(record["Nome"]),
                Surname = Convert.ToString(record["Cognome"])
            };
        }
    }


}
