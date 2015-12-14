using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace VnetPhotoManager.Web.Account
{
    public partial class Register : Page
    {
        protected void CreateUser_Click(object sender, EventArgs e)
        {

            var clientCode = txtClientCode.Text;
            var clientId = CreateUserWithClientCode(clientCode);
            if (clientId.Equals(0))
            {
                ltCodeError.Text = "Il Codice Cliente inserito non esiste.";
                return;
            }
            var authservice = new vnetauthenticationservice.AuthenticationService();
            authservice.CreateUser(Email.Text, Password.Text, Email.Text, "a", "b", true, true);
            // Save UserClienti 
            
            var retCreateUser = CreateUserClient(clientId, txtClientCode.Text, txtName.Text, txtSurname.Text, "", "", "", "", "", "", txtPhone.Text, Email.Text,
                Password.Text, DateTime.Now, true);
            if (retCreateUser) ltrSuccess.Text = "Utente creato con successo.";
        }

        public bool CreateUserClient(int clientId, string codiceStruttura, string nome, string cognome, string indirizzo, string citta, string provincia, string cap,
            string nazione, string telefono, string cellulare, string email, string password, DateTime registratoIl, bool attivo)
        {
            var connetionString = "Data Source=gpaafjxn5d.database.windows.net;Initial Catalog=GestPhoto_Admin;User ID=marco.bianchin;Password=BigDragon99";
            using (var connection = new SqlConnection(connetionString))
            {
                using (var command = new SqlCommand("InsertCliente", connection))
                {
                    command.Parameters.Clear();
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@IDCliente", clientId);
                    command.Parameters.AddWithValue("@CodiceStruttura", txtClientCode.Text);
                    command.Parameters.AddWithValue("@Nome", nome);
                    command.Parameters.AddWithValue("@Cognome", cognome);
                    command.Parameters.AddWithValue("@Indirizzo", indirizzo);
                    command.Parameters.AddWithValue("@Citta", citta);
                    command.Parameters.AddWithValue("@Provincia", provincia);
                    command.Parameters.AddWithValue("@CAP", cap);
                    command.Parameters.AddWithValue("@Nazione", nazione);
                    command.Parameters.AddWithValue("@Telefono", telefono);
                    command.Parameters.AddWithValue("@Cellulare", cellulare);
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Password", password);
                    command.Parameters.AddWithValue("@RegistratoIl", registratoIl);
                    command.Parameters.AddWithValue("@Attivo", attivo);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
            return true;
        }

        public int CreateUserWithClientCode(string clientCodeFromUI)
        {
            int clientId = 0;
            var connetionString = "Data Source=gpaafjxn5d.database.windows.net;Initial Catalog=GestPhoto_Admin;User ID=marco.bianchin;Password=BigDragon99";

            using (var connection = new SqlConnection(connetionString))
            {
                using (var command = new SqlCommand("GetCliente", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Codice", clientCodeFromUI);

                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.HasRows)
                        while (reader.Read())
                        {
                            clientId = Convert.ToInt32(reader["Id_Cliente"]);
                        }
                    reader.Close();
                }
                connection.Close();
            }
            return clientId;
        }
    }
}