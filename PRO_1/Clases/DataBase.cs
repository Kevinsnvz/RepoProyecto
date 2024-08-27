using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MySql.Data.MySqlClient;
using PRO_1.Ventanas;

namespace PRO_1.Clases
{
    public class DataBase
    {
        private const string connectionString = "SERVER=127.0.0.1;DATABASE=sys;UID=root;PASSWORD=rootpassword;";

        //Funcion para conseguir el rol de un usuario
        public static string GetJobRole(string username)
        {
            string jobRole = string.Empty;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string sql = "SELECT Job_Role FROM users_ WHERE username = @username;";
                    MySqlCommand cmd = new MySqlCommand(sql, connection);
                    cmd.Parameters.AddWithValue("@username", username);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            jobRole = reader.GetString("job_role");
                        }
                        else
                        {
                            Console.WriteLine("User not found.");
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine("MySqL ERROR:" + ex.Message);
                }

            }

            return jobRole;
        }

        //Funcion para ver si el ingreso de un usuario es correcto o no, si es correcto se ingresa a la ventana acorde al rol.
        public static bool isLogin(string username, string password)
        {

            
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string sql = $"SELECT * FROM users_ WHERE username = '{username}' AND password = '{password}';";
                    MySqlCommand cmd = new MySqlCommand(sql, connection);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        
                        
                        sql = $"SELECT Job_Role FROM users_ WHERE username = {username};";
                        cmd = null;
                        cmd = new MySqlCommand(sql, connection);
                        if (reader.HasRows)
                        {
                            
                            MessageBox.Show(reader.GetString("Job_Role"));
                            Adm_Ventanas.AbrirVentanaPorRol(reader.GetString("Job_Role"));
                            return true;
                            
                        }
                        else
                        {
                            MessageBox.Show("No valid job role found");
                            return false;
                        }
                    }
                    else
                    {
                        reader.Close();
                        MessageBox.Show("Nombre o Contraseña incorrecta.");
                        return false;
                    }
                }
                catch (MySqlException ex)
                {

                    Console.WriteLine($"MySQL Error: {ex.Message}");
                    return false;
                }
                catch (Exception ex)
                {

                    Console.WriteLine($"Error: {ex.Message}");
                    return false;
                }
            }
        }

        //Abre la conexion entre la base y el programa.
        public static bool openConnection()
        {

            string server = "127.0.0.1";
            string database = "sys";
            string uid = "root";
            string password = "rootpassword";

            string connString;
            connString = $"SERVER={server};DATABASE={database};UID={uid};PASSWORD={password};";
            MySqlConnection mySqlConnection = new MySqlConnection(connString);

            try
            {
                mySqlConnection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 0:
                        MessageBox.Show("Connection to server failed");
                        break;
                    case 1045:
                        MessageBox.Show("Server credentials wrong");
                        break;

                }
                return false;


            }
        }
    }
}
