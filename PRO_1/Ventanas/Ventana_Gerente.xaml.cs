using PRO_1.Clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Lógica de interacción para Ventana_Gerente.xaml
    /// </summary>
    public partial class Ventana_Gerente : Window
    {
        private ListaDeUsuarios acceso_usuarios;
        private ListaDeClientes acceso_clientes;
        public Ventana_Gerente(ListaDeUsuarios usuarios, ListaDeClientes clientes)
        {
            this.acceso_clientes = clientes;
            this.acceso_usuarios = usuarios;
            InitializeComponent();
        }
        //Al apretar el item de menu "Cerrar Sesion" cerrar la sesion, je re evidente
        public void CerrarSesionMenu_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow?.Close();

            this.Close();
        }

        //Al apretar el item de menu "Iniciar Sesion" Abrir nuevamente la ventana de login
        public void IniciarSesionMenu_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow?.Close();
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void FuncionesDeJefe_Click(object sender, RoutedEventArgs e)
        {

            Adm_Ventanas.AbrirVentana(3);

        }

        private void FuncionesDeEJ_Click(object sender, RoutedEventArgs e)
        {
            Adm_Ventanas.AbrirVentana(2);
        }

        private void ModificarJefe_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrEmpty(UsernameJefeModificacion_TextBox.Text) ||
                string.IsNullOrEmpty(PasswordJefeModificacion_TextBox.Text) ||
                string.IsNullOrEmpty(IDJefeModificacion_TextBox.Text) ||
                string.IsNullOrEmpty(RolJefeModificacion_TextBox.Content.ToString()))
            { MessageBox.Show("ERROR: Ningun campo debe estar vacio"); return; }

            string username = UsernameJefeModificacion_TextBox.Text;
            string password = UsernameJefeModificacion_TextBox.Text;
            string rol = RolJefeModificacion_TextBox.Content.ToString();
            int id = Convert.ToInt32(IDJefeModificacion_TextBox.Text);

            DataBase.ModificarUsuarioDeBDYAPP(username, password, rol, id, acceso_usuarios);
        }

        private void ActualizarListas_Click(object sender, RoutedEventArgs e)
        {
            ListaDeUsuarios listaMuestra = new ListaDeUsuarios();
            DataBase.CargarUsuariosDeBD(acceso_usuarios);

            foreach (var usuario in acceso_usuarios.ListaGlobalUsuarios)
            {
                if (usuario.Rol == "jefe_servicio")
                    listaMuestra.ListaGlobalUsuarios.Add(usuario);
            }

            Lista_BajaJefes.ItemsSource = null;
            Lista_BajaJefes.ItemsSource = listaMuestra.ListaGlobalUsuarios;

            Lista_JefesParaModificar.ItemsSource = null;
            Lista_JefesParaModificar.ItemsSource = listaMuestra.ListaGlobalUsuarios;
        }

        private void CrearJefe_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrEmpty(UsernameJefe_TextBox.Text) ||
               string.IsNullOrEmpty(PasswordJefe_TextBox.Text))
            { return; }

            string username = UsernameJefe_TextBox.Text;
            string password = PasswordJefe_TextBox.Text;
            
            DataBase.AgregarUsuarioABDYAPP(username, password, 2, acceso_usuarios);

        }

        private void BajaAJefe_Click(object sender, RoutedEventArgs e)
        {
            var usuarioSeleccionado = (Usuarios)Lista_BajaJefes.SelectedItem;

            if(usuarioSeleccionado == null)
            {  return; }

            DataBase.BorrarUsuarioDeBDYAPP(usuarioSeleccionado.UsuarioID, acceso_usuarios);
        }

        private void ListaJefesParaModificar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var usuarioSeleccionado = (Usuarios)Lista_JefesParaModificar.SelectedItem;

            if(usuarioSeleccionado == null)
            { MessageBox.Show("Debe seleccionar un usuario de la lista."); return; }

            UsernameJefeModificacion_TextBox.Text = usuarioSeleccionado.Username;
            PasswordJefeModificacion_TextBox.Text = usuarioSeleccionado.Password;
            RolJefeModificacion_TextBox.Content = usuarioSeleccionado.Rol;
            IDJefeModificacion_TextBox.Text = usuarioSeleccionado.UsuarioID.ToString();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
