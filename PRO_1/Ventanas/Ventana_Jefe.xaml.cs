using PRO_1.Clases;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PRO_1.Ventanas
{
    /// <summary>
    /// Lógica de interacción para Ventana_Jefe.xaml
    /// </summary>
    public partial class Ventana_Jefe : Window
    {
        private ListaDeClientes acceso_cliente;
        private ListaDeUsuarios acceso_usuarios;

        public Ventana_Jefe(ListaDeUsuarios usuarios, ListaDeClientes clientes,bool AbiertoPorOtraVentana)
        {
            this.acceso_cliente = clientes;
            this.acceso_usuarios = usuarios;

            InitializeComponent();

            if(AbiertoPorOtraVentana == true) { NuevaSesion_MenuItem.IsEnabled = false; FuncionesEJ_MenuItem.Visibility = Visibility.Hidden; }
        }

        /// <summary>
        /// Al apretar el item de menu "Cerrar Sesion" cierra la sesion.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void CerrarSesionMenu_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow?.Close();

            this.Close();
        }

        /// <summary>
        /// Al apretar el item de menu "Iniciar Sesion" Abrir nuevamente la ventana de login
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void IniciarSesionMenu_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow?.Close();
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
        /// <summary>
        /// Abre la ventana de Ejecutivo de servicios.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FuncionesDeEJ_Click(object sender, RoutedEventArgs e)
        {
            Adm_Ventanas.AbrirVentana(2);
        }
        /// <summary>
        /// Actualiza las listas de la ventana actual.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ActualizarListas_Click(object sender, RoutedEventArgs e)
        {
            ListaDeUsuarios listaMuestra = new ListaDeUsuarios();
            DataBase.CargarUsuariosDeBDaAPP(acceso_usuarios);

            foreach (var usuario in acceso_usuarios.ListaGlobalUsuarios)
            {
               if(usuario.Rol == "ejecutivo_servicio")
                    listaMuestra.ListaGlobalUsuarios.Add(usuario);
            }

            

            Lista_BajaEjecutivos.ItemsSource = null;
            Lista_BajaEjecutivos.ItemsSource = listaMuestra.ListaGlobalUsuarios;

            Lista_EjecutivosParaModificar.ItemsSource = null;
            Lista_EjecutivosParaModificar.ItemsSource = listaMuestra.ListaGlobalUsuarios;

        }
        /// <summary>
        /// Elimina al Ejecutivo seleccionado de la lista local de usuarios y la base de datos.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BajaAEjecutivo_Click(object sender, RoutedEventArgs e)
        {
            var UsuarioSeleccionado = (Usuarios)Lista_BajaEjecutivos.SelectedItem;

            if(UsuarioSeleccionado == null) 
            { return; }

            DataBase.BorrarUsuarioDeBDYAPP(UsuarioSeleccionado.UsuarioID, acceso_usuarios);
            
        }
        /// <summary>
        /// Crea un ejecutivo, añadiendolo a la lista local de usuarios y la base de datos.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CrearEjecutivo_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrEmpty(UsernameEjecutivo_TextBox.Text) ||
                string.IsNullOrEmpty(PasswordEjecutivo_TextBox.Text)  )
                { return; }
            string username = UsernameEjecutivo_TextBox.Text;
            string password = PasswordEjecutivo_TextBox.Text.Trim();
            DataBase.AgregarUsuarioABDYAPP(username, password,1,acceso_usuarios);

        }
        /// <summary>
        /// Cuando se selecciona un ejecutivo de la lista "Lista_EjecutivosParaModificar", se ejecuta el codigo despliegando los datos a los labels correspondientes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListaEjecutivosParaModificar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var UsuarioSeleccionado = (Usuarios)Lista_EjecutivosParaModificar.SelectedItem;

            if(UsuarioSeleccionado == null)
            { MessageBox.Show("Debe seleccionar un usuario de la lista."); return; }

            UsernameEjecutivoModificacion_TextBox.Text = UsuarioSeleccionado.Username;
            PasswordEjecutivoModificacion_TextBox.Text = UsuarioSeleccionado.Password;
            IDEjecutivoModificacion_TextBox.Text = UsuarioSeleccionado.UsuarioID.ToString();
            RolEjecutivoModificacion_TextBox.Content = UsuarioSeleccionado.Rol;
        }
        /// <summary>
        /// Modifica al ejecutivo seleccionado mediante su ID con los campos dados.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ModificarEjecutivo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(UsernameEjecutivoModificacion_TextBox.Text) ||
                   string.IsNullOrEmpty(PasswordEjecutivoModificacion_TextBox.Text) ||
                   string.IsNullOrEmpty(IDEjecutivoModificacion_TextBox.Text) ||
                   string.IsNullOrEmpty(RolEjecutivoModificacion_TextBox.Content.ToString()))
                { MessageBox.Show("ERROR: Ningun campo debe estar vacio."); return; }

                DataBase.ModificarUsuarioDeBDYAPP(UsernameEjecutivoModificacion_TextBox.Text, PasswordEjecutivoModificacion_TextBox.Text
                                                  , RolEjecutivoModificacion_TextBox.Content.ToString()
                                                  , Convert.ToInt32(IDEjecutivoModificacion_TextBox.Text)
                                                  , acceso_usuarios);
            }
            catch(FormatException)
            {
                MessageBox.Show("ERROR: Algun campo fue llenado con un dato invalido. Reiterar y arreglar.");
            }
            
        }
    }
}
