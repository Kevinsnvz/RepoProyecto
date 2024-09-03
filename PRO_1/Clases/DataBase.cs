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

        //Crea un cliente para la Lista de clientes en la app como en la BD.
        public static bool AgregarClienteABDYAPP(string nombre, string apellido, int telefono, string marca, string modelo, string matricula, ListaDeClientes lista)
        {
            
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string sql = $"INSERT INTO Clientes (nombre,apellido,telefono,marca,modelo,matricula) VALUES ('{nombre}','{apellido}','{telefono}','{marca}','{modelo}','{matricula}'); SELECT LAST_INSERT_ID();";
                    MySqlCommand cmd = new MySqlCommand(sql, connection);

                    int FilasAfectadas = cmd.ExecuteNonQuery();

                    if (FilasAfectadas > 0)
                    {
                        cmd = new MySqlCommand(sql, connection);
                        Console.WriteLine("Usuario creado exitosamente.");
                        Clientes Cliente = new Clientes(nombre, apellido, marca, modelo, matricula,telefono,Convert.ToInt32(cmd.LastInsertedId));
                        lista.ListaGlobalClientes.Add(Cliente);
                        return true;
                    }
                    else { Console.WriteLine("ERROR: Usuario no creado. Error desconocido."); return false; }
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine("MySqL ERROR:" + ex.Message);
                    return false;
                }

            }
        }

        //Toma todos los datos para la modificacion del cliente nuevo, borrando el viejo en base a la id, QUE ES UNICA, y crea el cliente nuevo en la lista de la app asi como en la base de datos.
        public static bool ModificarClienteDeBDYAPP(string nombre, string apellido, int telefono, string marca, string modelo, string matricula,int id, ListaDeClientes lista)
        {

            List<(string nombreServicio, int precioServicio)> copiadelista = null;
            bool copiadeAutorizacion = false;
            foreach (var Cliente in lista.ListaGlobalClientes)
            {
                if (Cliente.ClienteID == id) { continue; }

                copiadelista = Cliente.ListaDeServicios;
                copiadeAutorizacion = Cliente.Autorizado;
            }

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    //Como el id es unico, tomamos los campos nuevos borrando en base a la id vieja y tomando los nuevos valores de atributo para el nuevo cliente.
                    string sql = $"DELETE FROM Clientes WHERE id = {id};";
                    MySqlCommand cmd = new MySqlCommand(sql, connection);

                    int FilasBorradas = cmd.ExecuteNonQuery();

                    if (FilasBorradas > 0)
                    {
                        ;
                        Console.WriteLine("Cliente eliminado exitosamente de BD.");

                        Clientes clienteaborrar = new Clientes(nombre, apellido, marca, modelo, matricula, telefono, id);
                        lista.ListaGlobalClientes.Remove(clienteaborrar);

                        Console.WriteLine("Cliente eliminado exitosamente de LISTA.");


                        string sql1 = $"INSERT INTO Clientes (nombre,apellido,telefono,marca,modelo,matricula) VALUES ('{nombre}','{apellido}','{telefono}','{marca}','{modelo}','{matricula}'); SELECT LAST_INSERT_ID();";
                        MySqlCommand cmd1 = new MySqlCommand(sql1, connection);

                        int FilasAgregadas = cmd1.ExecuteNonQuery();

                        if (FilasAgregadas > 0)
                        {
                            Console.WriteLine("Cliente creado exitosamente a BD.");

                            Clientes clienteagregar = new Clientes(nombre, apellido, marca, modelo, matricula, telefono, Convert.ToInt32(cmd.LastInsertedId),copiadelista,copiadeAutorizacion);
                            lista.ListaGlobalClientes.Add(clienteagregar);

                            Console.WriteLine("Cliente creado exitosamente en LISTA");
                            return true;
                        }
                        else { Console.WriteLine("ERROR: Usuario no creado. Error desconocido."); return false; }

                        

                    }
                    else { Console.WriteLine("ERROR: Usuario no creado. Error desconocido."); return false; }
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine("MySqL ERROR:" + ex.Message);
                    return false;
                }

            }
        }
        //HAY QUE AGREGAR OTRO ATRIBUTO A LA FILA DE CLIENTES (AUTORIZADO) ASI EN CARGA SE CONSERVA ESTE DATO. ES IMPORTANTE CONSERVAR. ESTE METODO ESTA HECHO PARA SER CARGADO UNA VEZ EN ARRANQUE! Se esta usando de forma incorrecta!
        public static void CargarClientesDeBD(ListaDeClientes lista)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string sql = "SELECT id,nombre,apellido,telefono,marca,modelo,matricula FROM clientes";
                    using (MySqlCommand cmd = new MySqlCommand(sql, connection))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            lista.ListaGlobalClientes.Clear();

                            while (reader.Read())
                            {

                                Clientes cliente = new Clientes(reader.GetString(1), reader.GetString(2), reader.GetString(4), reader.GetString(5), reader.GetString(6), reader.GetInt32(3), reader.GetInt32(0));
                                lista.ListaGlobalClientes.Add(cliente);
                            }
                        }
                    }

                    
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine("MySqL ERROR:" + ex.Message);
                }

            }
        }

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
