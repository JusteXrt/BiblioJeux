using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TESTME
{
    internal class Util
    {
        private MySqlConnection conn;
        string connectionString = "Server=localhost;Database=biblio_jeux;User ID=root;Password=;";
        

        // 🔐 Fonction pour hasher un mot de passe avec SHA256
        private string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2")); // Convertit chaque byte en hexadécimal
                }
                return builder.ToString();
            }
        }
        private void AddUser(string username, string password, string role)
        {
            string hashedPassword = ComputeSha256Hash(password);

            try
            {
                using (conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO users (username, password, role) VALUES (@username, @password, @role)";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@password", hashedPassword);
                        cmd.Parameters.AddWithValue("@role", role);
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Utilisateur ajouté avec succès !");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'ajout de l'utilisateur : {ex.Message}");
            }
        }

    }
}
