using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MySql.Data.MySqlClient;
using Mysqlx;
using MySqlX.XDevAPI;
using Org.BouncyCastle.Asn1.Mozilla;
using PRO_1.Ventanas;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace PRO_1.Clases
{
    public class DataBase
    {
        private const string connectionString = "SERVER=127.0.0.1;DATABASE=sys;UID=root;PASSWORD=rootpassword;";


        /// <summary>
        /// Obtiene el usuario a modificar mediante su id, lo borra, y crea el nuevo con los parametros dados.
        /// </summary>
        /// <param name="idUsuario"> ID del Usuario</param>
        /// <param name="listaUsuarios"> Lista de usuarios al cual aplicar los cambios.</param>
        /// <returns></returns>
        public static bool ModificarUsuarioDeBDYAPP(string username, string password,string rol, int idUsuario, ListaDeUsuarios listaUsuarios)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    //Como el id es unico, tomamos los campos nuevos borrando en base a la id vieja y tomando los nuevos valores de atributo para el nuevo cliente.
                    string sql = $"DELETE FROM users_ WHERE id = {idUsuario};";
                    MySqlCommand cmd = new MySqlCommand(sql, connection);

                    int FilasBorradas = cmd.ExecuteNonQuery();

                    if (FilasBorradas > 0)
                    {
                        
                        Console.WriteLine("Cliente eliminado exitosamente de BD.");

                        for(int i = 0; i <= listaUsuarios.ListaGlobalUsuarios.Count; i++)
                        {
                            var usuario = listaUsuarios.ListaGlobalUsuarios[i];
                            if(usuario.UsuarioID == idUsuario)
                            { continue;  }

                            listaUsuarios.ListaGlobalUsuarios.Remove(usuario);

                            break;
                        }
                        

                        Console.WriteLine("Cliente eliminado exitosamente de LISTA.");


                        string sql1 = $"INSERT INTO users_ (username,password,job_role) VALUES ('{username}','{password}','{rol}'); SELECT LAST_INSERT_ID();";
                        MySqlCommand cmd1 = new MySqlCommand(sql1, connection);

                        int FilasAgregadas = cmd1.ExecuteNonQuery();

                        if (FilasAgregadas > 0)
                        {
                            Console.WriteLine("Usuario creado exitosamente a BD.");

                            Usuarios usuarioAagregar = new Usuarios(username,password,rol,Convert.ToInt32(cmd1.LastInsertedId));
                            listaUsuarios.ListaGlobalUsuarios.Add(usuarioAagregar);

                            Console.WriteLine("Usuario creado exitosamente en LISTA");

                            MessageBox.Show("Usuario modificado exitosamente");
                            return true;
                        }
                        else { MessageBox.Show("ERROR: Usuario no creado. Error desconocido."); return false; }



                    }
                    else { MessageBox.Show("ERROR: Usuario no creado. Error desconocido."); return false; }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("MySqL ERROR:" + ex.Message);
                    return false;
                }

            }
        }

        /// <summary>
        /// Borra el usuario de la Base de datos asi como de la lista dada.
        /// </summary>
        /// <param name="id_usuario"> ID del Usuario</param>
        /// <param name="listaDeUsuarios">Lista al cual aplicar el cambio.</param>
        /// <returns></returns>
        public static bool BorrarUsuarioDeBDYAPP(int id_usuario,ListaDeUsuarios listaDeUsuarios)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {

                try
                {
                    connection.Open();

                    string sqlquery = $"DELETE FROM users_ WHERE id = {id_usuario}";
                    MySqlCommand comm = new MySqlCommand(sqlquery, connection);

                    int FilasAfectadas = comm.ExecuteNonQuery();

                    if (FilasAfectadas > 0)
                    {
                        Console.WriteLine($"Usuario ID:{id_usuario} eliminado exitosamente de BD");

                        foreach (var usuario in listaDeUsuarios.ListaGlobalUsuarios)
                        {
                            if (usuario.UsuarioID == id_usuario) { continue; }

                            Usuarios usuarioABorrar = new Usuarios(usuario.Username, usuario.Password,usuario.Rol, usuario.UsuarioID);
                            listaDeUsuarios.ListaGlobalUsuarios.Remove(usuarioABorrar);
                            MessageBox.Show($"Cliente {id_usuario} eliminado exitosamente de LISTA");
                            break;
                        }

                        return true;
                    }
                    else
                    {
                        MessageBox.Show($"ERROR: Cliente {id_usuario} no fue eliminado. Error desconocido.");
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("MySQL ERROR: " + ex.ToString());
                }

            }
            return false;
        }

        /// <summary>
        /// Metodo <c>AgregarUsuarioDeBDYAPP</c> Agrega usuarios a la base de datos y a su vez, la lista de la app.
        /// </summary>
        /// <param name="username">Nombre del usuario.</param>
        /// <param name="password">Contraseña del usuario.</param>
        /// <param name="job_role">Rol del trabajador.
        /// <para>
        /// Debe elegir uno de los roles predeterminados:
        /// 0 - cajero / 1 - ejecutivo_servicio / 2 - jefe_servicio
        /// </para>
        /// </param>
        /// <param name="ListaDeUsuarios">Lista al cual aplicar el cambio.</param>
        /// <returns></returns>
        public static bool AgregarUsuarioABDYAPP(string username,string password,int job_role,ListaDeUsuarios ListaDeUsuarios)
        {
            string rolSeleccionado = string.Empty;

            switch(job_role)
            {
                case 0:
                    rolSeleccionado = "cajero";
                    break;
                case 1:
                    rolSeleccionado = "ejecutivo_servicio";
                    break;
                case 2:
                    rolSeleccionado = "jefe_servicio";
                    break;
            }

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = $"INSERT INTO users_ (username,password,job_role) VALUES ('{username}','{password}','{rolSeleccionado}'); SELECT LAST_INSERT_ID();";

                    MySqlCommand cmd = new MySqlCommand(query, connection);

                    int FilasAfectadas = cmd.ExecuteNonQuery();

                    if ( FilasAfectadas > 0 )
                    {
                        Console.WriteLine("Usuario creado exitosamente en BD");

                        Usuarios usuario = new Usuarios(username, password, rolSeleccionado, Convert.ToInt32(cmd.LastInsertedId));
                        ListaDeUsuarios.ListaGlobalUsuarios.Add(usuario);
                        Console.WriteLine("Usuario creado exitosamente en LISTA");

                        MessageBox.Show("Usuario creado exitosamente.");
                        return true;

                    }
                    else
                    {
                        MessageBox.Show("ERROR: No se creo el usuario. Error desconocido.");
                        return false;
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message);
                    return false;
                }
            }


        }

        /// <summary>
        /// Solicita los usuarios de la base de datos y los inserta a la lista dada, borrando los usuarios actuales de ella en el proceso.
        /// </summary>
        /// <param name="ListaUsuarios">Lista al cual aplicar el cambio.</param>
        /// <returns></returns>
        public static bool CargarUsuariosDeBD(ListaDeUsuarios ListaUsuarios)
        {

            using(MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = $"SELECT username,password,job_role,id FROM users_ ";

                    using(MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {

                            ListaUsuarios.ListaGlobalUsuarios.Clear();

                            while (reader.Read())
                            {
                                Usuarios Usuario = new Usuarios(reader.GetString(0), reader.GetString(1), reader.GetString(2),reader.GetInt32(3));
                                ListaUsuarios.ListaGlobalUsuarios.Add(Usuario);
                                Console.WriteLine("Usuario creado exitosamente.");

                            }
                            return true;
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message);
                    return false;
                }
            }

        }

        /// <summary>
        /// Borra el cliente de la Base de Datos asi como de la lista dada.
        /// </summary>
        /// <param name="id"> ID del Cliente</param>
        /// <param name="ListaClientes"> Lista al cual aplicar el cambio.</param>
        /// <returns></returns>
        public static bool BorrarClienteDeBDYAPP(int id,ListaDeClientes ListaClientes)
        {
            using(MySqlConnection connection = new MySqlConnection(connectionString))
            {

                try
                {
                    connection.Open();

                    string sqlquery = $"DELETE FROM Clientes WHERE id = {id}";
                    MySqlCommand comm = new MySqlCommand(sqlquery,connection);

                    int FilasAfectadas = comm.ExecuteNonQuery();

                    if(FilasAfectadas > 0)
                    {
                        Console.WriteLine($"Cliente ID:{id} eliminado exitosamente de BD");

                        foreach (var cliente in ListaClientes.ListaGlobalClientes)
                        {
                            if (cliente.ClienteID == id) { continue; }

                            Clientes ClienteABorrar = new Clientes(cliente.Nombre, cliente.Apellido, cliente.Marca, cliente.Modelo, cliente.Matricula, cliente.Telefono, cliente.ClienteID,cliente.ListaDeServicios,cliente.Autorizado);
                            ListaClientes.ListaGlobalClientes.Remove(ClienteABorrar);
                            Console.WriteLine($"Cliente {id} eliminado exitosamente de LISTA");
                            break;
                        }
                        
                        return true;
                    }
                    else
                    {
                        Console.WriteLine($"ERROR: Cliente {id} no fue eliminado. Error desconocido.");
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("MySQL ERROR: " + ex.ToString());
                }

            }
            return false;
        }

        /// <summary>
        /// Crea un cliente para la Lista dada y a la Base de Datos.
        /// </summary>
        /// <param name="ListaClientes"> Lista al cual aplicar el cambio.</param>
        /// <returns></returns>
        public static bool AgregarClienteABDYAPP(string nombre, string apellido, int telefono, string marca, string modelo, string matricula, ListaDeClientes ListaClientes)
        {
            
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string sql = $"INSERT INTO Clientes (nombre,apellido,telefono,marca,modelo,matricula,autorizacion) VALUES ('{nombre}','{apellido}','{telefono}','{marca}','{modelo}','{matricula}',false); SELECT LAST_INSERT_ID();";
                    MySqlCommand cmd = new MySqlCommand(sql, connection);

                    int FilasAfectadas = cmd.ExecuteNonQuery();

                    if (FilasAfectadas > 0)
                    {
                        cmd = new MySqlCommand(sql, connection);
                        Console.WriteLine("Usuario creado exitosamente.");
                        Clientes Cliente = new Clientes(nombre, apellido, marca, modelo, matricula,telefono,Convert.ToInt32(cmd.LastInsertedId));
                        ListaClientes.ListaGlobalClientes.Add(Cliente);
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

        /// <summary>
        /// Toma todos los datos para la modificacion del cliente nuevo, borrando el viejo en base a la id, QUE ES UNICA, y crea el cliente nuevo en la lista de la app asi como en la base de datos.
        /// </summary>
        /// <param name="id"> ID del Cliente</param>
        /// <param name="ListaClientes"> Lista del cual obtener y modificar al cliente</param>
        /// <returns></returns>
        public static bool ModificarClienteDeBDYAPP(string nombre, string apellido, int telefono, string marca, string modelo, string matricula, int idCliente, bool autorizacion, ListaDeClientes ListaClientes)
        {

            List<(string nombreServicio, int precioServicio)> copiadelista = null;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    //Como el id es unico, tomamos los campos nuevos borrando en base a la id vieja y tomando los nuevos valores de atributo para el nuevo cliente.
                    string sql = $"DELETE FROM Clientes WHERE id = {idCliente};";
                    MySqlCommand cmd = new MySqlCommand(sql, connection);

                    int FilasBorradas = cmd.ExecuteNonQuery();

                    if (FilasBorradas > 0)
                    {
                        ;
                        Console.WriteLine("Cliente eliminado exitosamente de BD.");

                        foreach (var Cliente in ListaClientes.ListaGlobalClientes)
                        {
                            if (Cliente.ClienteID == idCliente)
                            { continue; }

                            ListaClientes.ListaGlobalClientes.Remove(Cliente);
                            copiadelista = Cliente.ListaDeServicios;

                        }

                        Console.WriteLine("Cliente eliminado exitosamente de LISTA.");


                        string sql1 = $"INSERT INTO Clientes (nombre,apellido,telefono,marca,modelo,matricula,autorizacion) VALUES ('{nombre}','{apellido}','{telefono}','{marca}','{modelo}','{matricula}',{autorizacion}); SELECT LAST_INSERT_ID();";
                        MySqlCommand cmd1 = new MySqlCommand(sql1, connection);

                        int FilasAgregadas = cmd1.ExecuteNonQuery();

                        if (FilasAgregadas > 0)
                        {
                            Console.WriteLine("Cliente creado exitosamente a BD.");

                            Clientes clienteagregar = new Clientes(nombre, apellido, marca, modelo, matricula, telefono, Convert.ToInt32(cmd.LastInsertedId),copiadelista,autorizacion);
                            ListaClientes.ListaGlobalClientes.Add(clienteagregar);

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
        /// <summary>
        /// Solicita los Clientes existentes en la Base de Datos y los carga a la lista dada.
        /// </summary>
        /// <param name="ListaClientes">Lista al cual aplicar el cambio.</param>
        public static void CargarClientesDeBD(ListaDeClientes ListaClientes)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string sql = "SELECT id,nombre,apellido,telefono,marca,modelo,matricula,autorizacion FROM clientes";
                    using (MySqlCommand cmd = new MySqlCommand(sql, connection))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            ListaClientes.ListaGlobalClientes.Clear();

                            while (reader.Read())
                            {

                                Clientes cliente = new Clientes(reader.GetString(1), reader.GetString(2), reader.GetString(4), reader.GetString(5), reader.GetString(6), reader.GetInt32(3), reader.GetInt32(0), reader.GetBoolean(7));
                                ListaClientes.ListaGlobalClientes.Add(cliente);
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

        /// <summary>
        /// Metodo para obtener el rol de un usuario
        /// </summary>
        /// <param name="username">Nombre de Usuario del cual obtener el dato.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Metodo para ver si el ingreso de un usuario es correcto o no, verificado contra la Base de Datos, si es correcto se ingresa a la ventana acorde al rol.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static bool VerificacionLogin(string username, string password)
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

        /// <summary>
        /// Abre la conexion a la base.
        /// </summary>
        /// <returns></returns>
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
