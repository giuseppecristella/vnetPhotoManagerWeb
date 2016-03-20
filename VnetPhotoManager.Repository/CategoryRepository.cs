using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using VnetPhotoManager.Domain;

namespace VnetPhotoManager.Repository
{
    public class CategoryRepository : Repository<Category>
    {
        public CategoryRepository()
        {
                
        }

        public List<Category> GetCategories(string userEmail)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();

                SqlCommand command = new SqlCommand(@"SELECT * FROM USERCLIENTI U INNER JOIN Categorie C ON U.CODICESTRUTTURA = C.CODICESTRUTTURA AND U.EMAIL=@email", conn);
                command.Parameters.Add("@email", SqlDbType.VarChar).Value = userEmail;

                using (SqlDataReader dr = command.ExecuteReader(CommandBehavior.SingleResult))
                {
                    var items = new List<Category>();
                    while (dr.Read())
                    {
                        var item = BuildFromRecord(dr);
                        items.Add(item);
                    }
                    return items;
                }
            }
        }

        public byte[] GetCategoryImage(int productId)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();

                SqlCommand command = new SqlCommand(@"SELECT * FROM CATEGORIE WHERE ID_Categoria=@categoryId", conn);
                command.Parameters.Add("@categoryId", SqlDbType.Int).Value = productId;

                using (SqlDataReader dr = command.ExecuteReader(CommandBehavior.SingleResult))
                {
                    dr.Read();
                    return (byte[])dr["Immagine"];
                }
            }
        }

        protected override Category BuildFromRecord(IDataRecord record)
        {
            return new Category
            {
                Description = Convert.ToString(record["Descrizione"]),
                //ImgThumb = (Byte[])record["Immagine"],
                CategoryId = Convert.ToInt32(record["ID_Categoria"]), 
            };
        }
    }
}
