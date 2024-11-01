﻿using PRO_1.Clases;
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
        
        /// <summary>
        /// Actualiza los combobox de la ventana actual con los neumaticos que contienen las listas.
        /// </summary>
        public void ActualizarColecciones()
        {
            NeumaticoMichelin_ComboBox.Items.Clear();
            NeumaticoBridgestone_ComboBox.Items.Clear();
            NeumaticoPirelli_ComboBox.Items.Clear();

            foreach (var neumatico in Precios.NeumaticosMichelin)
            {
                
                NeumaticoMichelin_ComboBox.Items.Add($"{neumatico.Ancho}/{neumatico.Perfil}/R{neumatico.Rodado}/{neumatico.IDNeumatico}/{neumatico.Modelo}");
            }
            foreach (var neumatico in Precios.NeumaticosBridgestone)
            {
                
                NeumaticoBridgestone_ComboBox.Items.Add($"{neumatico.Ancho}/{neumatico.Perfil}/R{neumatico.Rodado}/{neumatico.IDNeumatico}/{neumatico.Modelo}");
            }
            foreach (var neumatico in Precios.NeumaticosPirelli)
            {
                
                NeumaticoPirelli_ComboBox.Items.Add($"{neumatico.Ancho}/{neumatico.Perfil}/R{neumatico.Rodado}/{neumatico.IDNeumatico}/{neumatico.Modelo}");
            }

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
        /// Abre la ventana de Jefe de Servicios
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FuncionesDeJefe_Click(object sender, RoutedEventArgs e)
        {
            
            Adm_Ventanas.AbrirVentana(3);

        }
        /// <summary>
        /// Abre la ventana de Ejecutivo de Servicios
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FuncionesDeEJ_Click(object sender, RoutedEventArgs e)
        {
            Adm_Ventanas.AbrirVentana(2);
        }
        /// <summary>
        /// Modifica al Jefe Seleccionado con los campos dados.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ModificarJefe_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(UsernameJefeModificacion_TextBox.Text) ||
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
            catch (FormatException)
            {
                MessageBox.Show("ERROR: Algun campo fue llenado con un dato invalido. Reiterar y arreglar.");
            }
            ActualizarColecciones();
        }
        /// <summary>
        /// Actualiza las listas de la ventana actual, al igual que los combobox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ActualizarListas_Click(object sender, RoutedEventArgs e)
        {

            ActualizarColecciones();

        }
        /// <summary>
        /// Crea un jefe a la base de datos y a la lista local de Usuarios, mediante los campos dados.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CrearJefe_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(UsernameJefe_TextBox.Text) ||
                   string.IsNullOrEmpty(PasswordJefe_TextBox.Text))
                { return; }

                string username = UsernameJefe_TextBox.Text;
                string password = PasswordJefe_TextBox.Text;
            
                DataBase.AgregarUsuarioABDYAPP(username, password, 2, acceso_usuarios);
            }
            catch (FormatException)
            {
                MessageBox.Show("ERROR: Algun campo fue llenado con un dato invalido. Reiterar y arreglar.");
            }

            ActualizarColecciones();


        }
        /// <summary>
        /// Elimina al jefe seleccionado de la base de datos y la lista local de usuarios.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BajaAJefe_Click(object sender, RoutedEventArgs e)
        {
            var usuarioSeleccionado = (Usuarios)Lista_BajaJefes.SelectedItem;

            if(usuarioSeleccionado == null)
            {  return; }

            DataBase.BorrarUsuarioDeBDYAPP(usuarioSeleccionado.UsuarioID, acceso_usuarios);
            ActualizarColecciones();
        }
        /// <summary>
        /// Al seleccionar un Jefe de la lista "Lista_JefesParaModificar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// Colapsa todos los grids excepto el de ABM Jefes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ABMJefes_MenuClick(object sender, RoutedEventArgs e)
        {

            grid_ABMJefes.Visibility = Visibility.Visible;
            grid_ModificacionPrecios.Visibility = Visibility.Collapsed;
            grid_ABNeumaticos.Visibility = Visibility.Collapsed;
            ActualizarColecciones();

        }
        /// <summary>
        /// Colapsa todos los grids excepto el de Modificacion de Precios
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Precios_MenuClick(object sender, RoutedEventArgs e)
        {

            grid_ABMJefes.Visibility = Visibility.Collapsed;
            grid_ModificacionPrecios.Visibility = Visibility.Visible;
            grid_ABNeumaticos.Visibility = Visibility.Collapsed;
            ActualizarColecciones();

        }
        /// <summary>
        /// Colapsa todos los grids excepto el de AB Neumaticos.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ABNeumaticos_MenuClick(object sender, RoutedEventArgs e)
        {
            grid_ABMJefes.Visibility = Visibility.Collapsed;
            grid_ModificacionPrecios.Visibility = Visibility.Collapsed;
            grid_ABNeumaticos.Visibility = Visibility.Visible;
            ActualizarColecciones();
        }
        /// <summary>
        /// Guarda los nuevos valores de precios de la seccion de lavado en todos los campos que esten llenos, el resto sera ignorado.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GuardarPreciosLavado_Click(object sender, RoutedEventArgs e)
        {
            try
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
            catch(FormatException)
            {
                MessageBox.Show("ERROR: Algun campo fue llenado con un dato invalido. Reiterar y arreglar.");
            }
            ActualizarColecciones();

        }
        /// <summary>
        /// Guarda los nuevos valores de precios de la seccion de Balanceo en todos los campos que esten llenos, el resto sera ignorado.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GuarderPreciosBalanceo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string cambios = "Cambios Balanceo: \n";
                if (!string.IsNullOrEmpty(PrecioBalanceoAuto_TextBox.Text)) 
                { Precios.BalanceoAuto = Convert.ToInt32(PrecioBalanceoAuto_TextBox.Text); cambios += $"Auto- ${Precios.BalanceoAuto}\n"; }

                if (!string.IsNullOrEmpty(PrecioBalanceoCamioneta_TextBox.Text)) 
                { Precios.BalanceoCamioneta = Convert.ToInt32(PrecioBalanceoCamioneta_TextBox.Text); cambios += $"Camioneta- ${Precios.BalanceoCamioneta}\n"; }

                MessageBox.Show(cambios);
            }
            catch (FormatException)
            {
                MessageBox.Show("ERROR: Algun campo fue llenado con un dato invalido. Reiterar y arreglar.");
            }
            ActualizarColecciones();
        }
        /// <summary>
        /// Guarda los nuevos valores de precios de la seccion de Alineacion en todos los campos que esten llenos, el resto sera ignorado.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GuardarPreciosAlineacion_Click(object sender, RoutedEventArgs e)
        {
            try
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
            catch (FormatException)
            {
                MessageBox.Show("ERROR: Algun campo fue llenado con un dato invalido. Reiterar y arreglar.");
            }
            ActualizarColecciones();
        }
        /// <summary>
        /// Guarda los nuevos valores de precios de la seccion de Neumaticos en todos los campos que esten llenos, el resto sera ignorado.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GuardarPreciosNeumatico_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string cambios = "Cambios Neumaticos: \n";

                if(!string.IsNullOrEmpty(PrecioMontajeNeumatico_TextBox.Text))
                { Precios.MontajeNeumatico = Convert.ToInt32(PrecioMontajeNeumatico_TextBox.Text); cambios += $"Montaje Neumatico - ${Convert.ToInt32(PrecioMontajeNeumatico_TextBox.Text)}\n"; }
                if (!string.IsNullOrEmpty(PrecioNeumaticoMichelin_TextBox.Text) && !string.IsNullOrEmpty(MichelinIDNeumatico_Label.Content.ToString()))
                {
                    DataBase.ModificarPrecioNeumaticoBD(Convert.ToInt32(MichelinIDNeumatico_Label.Content), Convert.ToInt32(PrecioNeumaticoMichelin_TextBox.Text));
                    cambios += $"{NeumaticoMichelin_ComboBox.SelectedItem} - ${PrecioNeumaticoMichelin_TextBox.Text}\n";
                
                }
                if (!string.IsNullOrEmpty(PrecioNeumaticoBridgestone_TextBox.Text) && !string.IsNullOrEmpty(BridgestoneIDNeumatico_Label.Content.ToString()))
                {
                    DataBase.ModificarPrecioNeumaticoBD(Convert.ToInt32(BridgestoneIDNeumatico_Label.Content), Convert.ToInt32(PrecioNeumaticoBridgestone_TextBox.Text));
                    cambios += $"{NeumaticoBridgestone_ComboBox.SelectedItem} - ${PrecioNeumaticoBridgestone_TextBox.Text}\n";
                
                }
                if (!string.IsNullOrEmpty(PrecioNeumaticoPirelli_TextBox.Text) && !string.IsNullOrEmpty(PirelliIDNeumatico_Label.Content.ToString()))
                {
                    DataBase.ModificarPrecioNeumaticoBD(Convert.ToInt32(PirelliIDNeumatico_Label.Content), Convert.ToInt32(PrecioNeumaticoPirelli_TextBox.Text));
                    cambios += $"{NeumaticoPirelli_ComboBox.SelectedItem} - ${PrecioNeumaticoPirelli_TextBox.Text}\n";
                    
                }

                MessageBox.Show(cambios);
            }
            catch (FormatException)
            {
                MessageBox.Show("ERROR: Algun campo fue llenado con un dato invalido. Reiterar y arreglar.");
            }

            ActualizarColecciones();

        }
        /// <summary>
        /// Crea un neumatico el que sera añadido a la lista correspondiente dependiendo de su marca.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CrearNeumatico_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(AnchoNeumatico_TextBox.Text) ||
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

                List<Neumatico> ListaNeumaticos = new List<Neumatico>();
                foreach (var NeumaticoAdd in Precios.NeumaticosMichelin)
                {
                    ListaNeumaticos.Add(NeumaticoAdd);
                }
                foreach (var NeumaticoAdd in Precios.NeumaticosBridgestone)
                {
                    ListaNeumaticos.Add(NeumaticoAdd);
                }
                foreach (var NeumaticoAdd in Precios.NeumaticosPirelli)
                {
                    ListaNeumaticos.Add(NeumaticoAdd);
                }

                for(int i = 0;  i < ListaNeumaticos.Count ; i++)
                {
                    var NeumaticoCheck = ListaNeumaticos[i];

                    if (Marca == NeumaticoCheck.Marca && Modelo == NeumaticoCheck.Modelo && Ancho == NeumaticoCheck.Ancho && Perfil == NeumaticoCheck.Perfil && Rodado == NeumaticoCheck.Rodado)
                    { MessageBox.Show("ERROR: Neumatico ya existe en sistema."); break; }

                    if (i == ListaNeumaticos.Count - 1)
                        DataBase.CrearNeumaticoenBDyAPP(Marca, Modelo, Ancho, Perfil, Rodado, Precio, Stock);

                }
            }
            catch (FormatException)
            {
                MessageBox.Show("ERROR: Algun campo fue llenado con un dato invalido. Reiterar y arreglar.");
            }

            ActualizarColecciones();

        }
        /// <summary>
        /// Cuando se seleccione un item del combobox de Neumaticos Michelin, se ejecuta el codigo que pasa el ID correspondiente a un label.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MichelinComboBox_Selected(object sender, RoutedEventArgs e)
        {
            if (NeumaticoMichelin_ComboBox.Items.Count <= 0) return;

            string NeumaticoSeleccionado = NeumaticoMichelin_ComboBox.SelectedItem.ToString();

            string[] partes = NeumaticoSeleccionado.Split('/');

            int ID = Convert.ToInt32(partes[3]);
            MichelinIDNeumatico_Label.Content = ID;

            ActualizarColecciones();
        }
        /// <summary>
        /// Cuando se seleccione un item del combobox de Neumaticos Bridgestone, se ejecuta el codigo que pasa el ID correspondiente a un label.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NeumaticoBridgestoneComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (NeumaticoBridgestone_ComboBox.Items.Count <= 0) return;

            string NeumaticoSeleccionado = NeumaticoBridgestone_ComboBox.SelectedItem.ToString();

            string[] partes = NeumaticoSeleccionado.Split('/');

            int ID = Convert.ToInt32(partes[3]);
            BridgestoneIDNeumatico_Label.Content = ID;
        }
        /// <summary>
        /// Cuando se seleccione un item del combobox de Neumaticos Pirelli, se ejecuta el codigo que pasa el ID correspondiente a un label.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NeumaticoPirelliComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (NeumaticoPirelli_ComboBox.Items.Count <= 0) return;

            string NeumaticoSeleccionado = NeumaticoPirelli_ComboBox.SelectedItem.ToString();

            string[] partes = NeumaticoSeleccionado.Split('/');

            int ID = Convert.ToInt32(partes[3]);
            PirelliIDNeumatico_Label.Content = ID;
        }
        /// <summary>
        /// Se elimina al neumatico seleccionado de la lista y la base de datos.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BajaNeumatico_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(IDSeleccionado_TextBox.Content.ToString()))
                    { MessageBox.Show("ERROR: Debe seleccionar un cliente"); return; }
                int ID = Convert.ToInt32(IDSeleccionado_TextBox.Content);

                foreach (var Neumatico in Precios.NeumaticosMichelin)
                {
                    if (Neumatico.IDNeumatico == ID)
                    { DataBase.BorrarNeumaticoDeBDYAPP(ID, Precios.NeumaticosMichelin); break; }
                    MarcaSeleccionada_TextBox.Content = string.Empty;
                    ModeloSeleccionado_TextBox.Content = string.Empty;
                    IDSeleccionado_TextBox.Content = string.Empty;

                }
                foreach (var Neumatico in Precios.NeumaticosBridgestone)
                {
                    if (Neumatico.IDNeumatico == ID)
                    { DataBase.BorrarNeumaticoDeBDYAPP(ID, Precios.NeumaticosBridgestone); break; }
                    MarcaSeleccionada_TextBox.Content = string.Empty;
                    ModeloSeleccionado_TextBox.Content = string.Empty;
                    IDSeleccionado_TextBox.Content = string.Empty;
                }
                foreach (var Neumatico in Precios.NeumaticosPirelli)
                {
                    if (Neumatico.IDNeumatico == ID)
                    { DataBase.BorrarNeumaticoDeBDYAPP(ID, Precios.NeumaticosPirelli); break; }
                    MarcaSeleccionada_TextBox.Content = string.Empty;
                    ModeloSeleccionado_TextBox.Content = string.Empty;
                    IDSeleccionado_TextBox.Content = string.Empty;
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("ERROR: Algun campo fue llenado con un dato invalido. Reiterar y arreglar.");
            }

            ActualizarColecciones();

        }
        /// <summary>
        /// Cuando se selecciona un neumatico de la lista "ListaNeumaticosAB_ListView", se ejecuta el codigo que despliega los datos Marca, Modelo y ID a los labels correspondientes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListaNeumaticosABListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var NeumaticoSeleccionado = (Neumatico)ListaNeumaticosAB_ListView.SelectedItem;

            if (NeumaticoSeleccionado == null)
            { return; }

            MarcaSeleccionada_TextBox.Content = NeumaticoSeleccionado.Marca;
            ModeloSeleccionado_TextBox.Content = NeumaticoSeleccionado.Modelo;
            IDSeleccionado_TextBox.Content = NeumaticoSeleccionado.IDNeumatico;
        }
    }
}
