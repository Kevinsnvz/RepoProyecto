using PRO_1.Clases;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Lógica de interacción para Ventana_Gerente.xaml
    /// </summary>
    public partial class Ventana_Gerente : Window,INotifyPropertyChanged
    {
        private ListaDeUsuarios acceso_usuarios;
        private ListaDeClientes acceso_clientes;

        public event PropertyChangedEventHandler PropertyChanged;

        public Ventana_Gerente(ListaDeUsuarios usuarios, ListaDeClientes clientes)
        {
            this.acceso_clientes = clientes;
            this.acceso_usuarios = usuarios;
            InitializeComponent();

            List<string> Marcas = new List<string>() {"Michelin","Bridgestone","Pirelli" };

            MarcaNeumatico_ComboBox.Items.Add(Marcas[0]);
            MarcaNeumatico_ComboBox.Items.Add(Marcas[1]);
            MarcaNeumatico_ComboBox.Items.Add(Marcas[2]);

            
        }
        
        public void ActualizarComboBox()
        {

            foreach(var neumatico in Precios.NeumaticosMichelin)
            {
                NeumaticoMichelin_ComboBox.Items.Add($"{}");
            }
        }
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
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

        private void ABMJefes_MenuClick(object sender, RoutedEventArgs e)
        {

            grid_ABMJefes.Visibility = Visibility.Visible;
            grid_ModificacionPrecios.Visibility = Visibility.Collapsed;


        }

        private void Precios_MenuClick(object sender, RoutedEventArgs e)
        {

            grid_ABMJefes.Visibility = Visibility.Collapsed;
            grid_ModificacionPrecios.Visibility = Visibility.Visible;

        }

        private void GuardarPreciosLavado_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(PrecioLavadoAuto_TextBox.Text)) Precios.LavadoAuto = Convert.ToInt32(PrecioLavadoAuto_TextBox.Text);
            if (!string.IsNullOrEmpty(PrecioLavadoCamioneta_TextBox.Text)) Precios.LavadoCamioneta = Convert.ToInt32(PrecioLavadoCamioneta_TextBox.Text);
            if (!string.IsNullOrEmpty(PrecioLavadoCamion_TextBox.Text)) Precios.LavadoCamionChico = Convert.ToInt32(PrecioLavadoCamion_TextBox.Text);
            if (!string.IsNullOrEmpty(PrecioLavadoCamionUtil_TextBox.Text)) Precios.LavadoCamionUtilitario = Convert.ToInt32(PrecioLavadoCamionUtil_TextBox.Text);
            if (!string.IsNullOrEmpty(PrecioLavadoMoto_TextBox.Text)) Precios.LavadoMoto = Convert.ToInt32(PrecioLavadoMoto_TextBox.Text);

        }

        private void GuarderPreciosBalanceo_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(PrecioBalanceoAuto_TextBox.Text)) Precios.BalanceoAuto = Convert.ToInt32(PrecioBalanceoAuto_TextBox.Text);
            if (!string.IsNullOrEmpty(PrecioBalanceoCamioneta_TextBox.Text)) Precios.BalanceoCamioneta = Convert.ToInt32(PrecioBalanceoCamioneta_TextBox.Text);
        }

        private void GuardarPreciosAlineacion_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(PrecioAlineacion_TextBox.Text)) Precios.Alineacion1Tren = Convert.ToInt32(PrecioAlineacion_TextBox.Text);
            if (!string.IsNullOrEmpty(PrecioAlineacion2Trenes_TextBox.Text)) Precios.Alineacion2Tren = Convert.ToInt32(PrecioAlineacion2Trenes_TextBox.Text);
            if (!string.IsNullOrEmpty(PrecioAlineacionR17_TextBox.Text)) Precios.Alineacion1TrenDesdeR17 = Convert.ToInt32(PrecioAlineacionR17_TextBox.Text);
        }

        private void GuardarPreciosNeumatico_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CrearNeumatico_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrEmpty(AnchoNeumatico_TextBox.Text) ||
                string.IsNullOrEmpty(RodadoNeumatico_TextBox.Text) ||
                string .IsNullOrEmpty(PerfilNeumatico_TextBox.Text) ||
                MarcaNeumatico_ComboBox.SelectedItem == null)
            { MessageBox.Show("ERROR: Todos los campos deben de estar completos y seleccionados para crear un nuevo Neumatico. "); return; }

            string Marca = MarcaNeumatico_ComboBox.SelectedItem.ToString();
            string Modelo = ModeloNeumatico_TextBox.Text;
            int Ancho = Convert.ToInt32(AnchoNeumatico_TextBox.Text);
            int Perfil = Convert.ToInt32(PerfilNeumatico_TextBox.Text);
            int Rodado = Convert.ToInt32(RodadoNeumatico_TextBox.Text);

            Neumatico nuevoNeumatico = new Neumatico(Marca, Ancho, Perfil, Rodado);

            switch (Marca)
            {
                case "Michelin":

                    Precios.NeumaticosMichelin.Add(nuevoNeumatico);
                    MessageBox.Show($"Neumatico {Marca} creado exitosamente");

                    

                    break;
                case "Bridgestone":

                    Precios.NeumaticosBridgestone.Add(nuevoNeumatico);
                    MessageBox.Show($"Neumatico {Marca} creado exitosamente");

                    break;
                case "Pirelli":

                    Precios.NeumaticosPirelli.Add(nuevoNeumatico);
                    MessageBox.Show($"Neumatico {Marca} creado exitosamente");

                    break;

            }
            OnPropertyChanged();

        }
    }
}
