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

            SelecciondeMarcaNeumatico_ComboBox.Items.Add(Marcas[0]);
            SelecciondeMarcaNeumatico_ComboBox.Items.Add(Marcas[1]);
            SelecciondeMarcaNeumatico_ComboBox.Items.Add(Marcas[2]);

            
        }
        
        public void ActualizarComboBox()
        {
            NeumaticoMichelin_ComboBox.Items.Clear();
            NeumaticoBridgestone_ComboBox.Items.Clear();
            NeumaticoPirelli_ComboBox.Items.Clear();

            foreach (var neumatico in Precios.NeumaticosMichelin)
            {
                
                NeumaticoMichelin_ComboBox.Items.Add($"{neumatico.Ancho}/{neumatico.Perfil}/R{neumatico.Rodado}/{neumatico.IDNeumatico}");
            }
            foreach (var neumatico in Precios.NeumaticosBridgestone)
            {
                
                NeumaticoBridgestone_ComboBox.Items.Add($"{neumatico.Ancho}/{neumatico.Perfil}/R{neumatico.Rodado}/{neumatico.IDNeumatico}");
            }
            foreach (var neumatico in Precios.NeumaticosPirelli)
            {
                
                NeumaticoPirelli_ComboBox.Items.Add($"{neumatico.Ancho}/{neumatico.Perfil}/R{neumatico.Rodado}/{neumatico.IDNeumatico}");
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
            ActualizarComboBox();

            List<Neumatico> ListaNeumaticos = new List<Neumatico>();
            foreach(var Neumatico in Precios.NeumaticosMichelin)
            {
                ListaNeumaticos.Add(Neumatico);
            }
            foreach (var Neumatico in Precios.NeumaticosBridgestone)
            {
                ListaNeumaticos.Add(Neumatico);
            }
            foreach (var Neumatico in Precios.NeumaticosPirelli)
            {
                ListaNeumaticos.Add(Neumatico);
            }

            ListaNeumaticosAB_ListView.ItemsSource = null;
            ListaNeumaticosAB_ListView.ItemsSource = ListaNeumaticos;

            ListaDeUsuarios listaMuestra = new ListaDeUsuarios();
            DataBase.CargarUsuariosDeBDaAPP(acceso_usuarios);

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
            grid_ABNeumaticos.Visibility = Visibility.Collapsed;

        }

        private void Precios_MenuClick(object sender, RoutedEventArgs e)
        {

            grid_ABMJefes.Visibility = Visibility.Collapsed;
            grid_ModificacionPrecios.Visibility = Visibility.Visible;
            grid_ABNeumaticos.Visibility = Visibility.Collapsed;

        }

        private void ABNeumaticos_MenuClick(object sender, RoutedEventArgs e)
        {
            grid_ABMJefes.Visibility = Visibility.Collapsed;
            grid_ModificacionPrecios.Visibility = Visibility.Collapsed;
            grid_ABNeumaticos.Visibility = Visibility.Visible;
        }

        private void GuardarPreciosLavado_Click(object sender, RoutedEventArgs e)
        {
            string cambios = "Cambios Lavado: \n";
            if (!string.IsNullOrEmpty(PrecioLavadoAuto_TextBox.Text)) 
            { Precios.LavadoAuto = Convert.ToInt32(PrecioLavadoAuto_TextBox.Text); cambios += $"Auto- ${Precios.LavadoAuto}\n"; }

            if (!string.IsNullOrEmpty(PrecioLavadoCamioneta_TextBox.Text)) 
            { Precios.LavadoCamioneta = Convert.ToInt32(PrecioLavadoCamioneta_TextBox.Text); cambios += $"Camioneta - ${Precios.LavadoCamioneta}\n"; }

            if (!string.IsNullOrEmpty(PrecioLavadoCamion_TextBox.Text)) 
            { Precios.LavadoCamionChico = Convert.ToInt32(PrecioLavadoCamion_TextBox.Text); cambios += $"Camion - ${Precios.LavadoCamionChico}\n"; }

            if (!string.IsNullOrEmpty(PrecioLavadoCamionUtil_TextBox.Text)) 
            { Precios.LavadoCamionUtilitario = Convert.ToInt32(PrecioLavadoCamionUtil_TextBox.Text); cambios += $"Camion Util.- ${Precios.LavadoCamionUtilitario}\n"; }

            if (!string.IsNullOrEmpty(PrecioLavadoMoto_TextBox.Text)) 
            { Precios.LavadoMoto = Convert.ToInt32(PrecioLavadoMoto_TextBox.Text); cambios += $"Moto - ${Precios.LavadoMoto}\n"; }

            MessageBox.Show(cambios);

        }

        private void GuarderPreciosBalanceo_Click(object sender, RoutedEventArgs e)
        {
            string cambios = "Cambios Balanceo: \n";
            if (!string.IsNullOrEmpty(PrecioBalanceoAuto_TextBox.Text)) 
            { Precios.BalanceoAuto = Convert.ToInt32(PrecioBalanceoAuto_TextBox.Text); cambios += $"Auto- ${Precios.BalanceoAuto}\n"; }

            if (!string.IsNullOrEmpty(PrecioBalanceoCamioneta_TextBox.Text)) 
            { Precios.BalanceoCamioneta = Convert.ToInt32(PrecioBalanceoCamioneta_TextBox.Text); cambios += $"Camioneta- ${Precios.BalanceoCamioneta}\n"; }

            MessageBox.Show(cambios);
        }

        private void GuardarPreciosAlineacion_Click(object sender, RoutedEventArgs e)
        {
            string cambios = "Cambios Alineacion: \n";
            if (!string.IsNullOrEmpty(PrecioAlineacion_TextBox.Text)) 
            { Precios.Alineacion1Tren = Convert.ToInt32(PrecioAlineacion_TextBox.Text); cambios += $"Alineacion un tren - ${Precios.Alineacion1Tren}\n"; }

            if (!string.IsNullOrEmpty(PrecioAlineacion2Trenes_TextBox.Text)) 
            { Precios.Alineacion2Tren = Convert.ToInt32(PrecioAlineacion2Trenes_TextBox.Text); cambios += $"Alineacion 2 trenes - ${Precios.Alineacion2Tren}\n"; }

            if (!string.IsNullOrEmpty(PrecioAlineacionR17_TextBox.Text)) 
            { Precios.Alineacion1TrenDesdeR17 = Convert.ToInt32(PrecioAlineacionR17_TextBox.Text); cambios += $"Alieacion a partir R17- ${Precios.Alineacion1TrenDesdeR17}\n"; }

            MessageBox.Show(cambios);
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
                SelecciondeMarcaNeumatico_ComboBox.SelectedItem == null ||
                string.IsNullOrEmpty(ModeloNeumatico_TextBox.Text) ||
                string.IsNullOrEmpty(PrecioNeumatico_TextBox.Text) ||
                string.IsNullOrEmpty(StockNeumatico_TextBox.Text))
            { MessageBox.Show("ERROR: Todos los campos deben de estar completos y seleccionados para crear un nuevo Neumatico. "); return; }

            string Marca = SelecciondeMarcaNeumatico_ComboBox.SelectedItem.ToString();
            string Modelo = ModeloNeumatico_TextBox.Text;
            int Ancho = Convert.ToInt32(AnchoNeumatico_TextBox.Text);
            int Perfil = Convert.ToInt32(PerfilNeumatico_TextBox.Text);
            int Rodado = Convert.ToInt32(RodadoNeumatico_TextBox.Text);
            int Precio = Convert.ToInt32(PrecioNeumatico_TextBox.Text);
            int Stock = Convert.ToInt32(StockNeumatico_TextBox.Text);

            DataBase.CrearNeumaticoenBDyAPP(Marca, Modelo, Ancho, Perfil, Rodado, Precio, Stock);

        }

        private void MichelinComboBox_Selected(object sender, RoutedEventArgs e)
        {
            string NeumaticoSeleccionado = NeumaticoMichelin_ComboBox.SelectedItem.ToString();

            string[] partes = NeumaticoSeleccionado.Split('/');

            int ID = Convert.ToInt32(partes[3]);
            MichelinIDNeumatico_Label.Content = ID;
            
                    
        }

        private void NeumaticoBridgestoneComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string NeumaticoSeleccionado = NeumaticoBridgestone_ComboBox.SelectedItem.ToString();

            string[] partes = NeumaticoSeleccionado.Split('/');

            int ID = Convert.ToInt32(partes[3]);
            BridgestoneIDNeumatico_Label.Content = ID;
        }

        private void NeumaticoPirelliComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string NeumaticoSeleccionado = NeumaticoPirelli_ComboBox.SelectedItem.ToString();

            string[] partes = NeumaticoSeleccionado.Split('/');

            int ID = Convert.ToInt32(partes[3]);
            PirelliIDNeumatico_Label.Content = ID;
        }

        private void BajaNeumatico_Click(object sender, RoutedEventArgs e)
        {
            int ID = Convert.ToInt32(IDSeleccionado_TextBox.Content);

            List<Neumatico> ListaNeumaticos = new List<Neumatico>();
            foreach (var Neumatico in Precios.NeumaticosMichelin)
            {
                ListaNeumaticos.Add(Neumatico);
            }
            foreach (var Neumatico in Precios.NeumaticosBridgestone)
            {
                ListaNeumaticos.Add(Neumatico);
            }
            foreach (var Neumatico in Precios.NeumaticosPirelli)
            {
                ListaNeumaticos.Add(Neumatico);
            }

            foreach(var Neumatico in ListaNeumaticos)
            {
                if (ID == Neumatico.IDNeumatico)
                {
                    
                }
            }
        }
    }
}
