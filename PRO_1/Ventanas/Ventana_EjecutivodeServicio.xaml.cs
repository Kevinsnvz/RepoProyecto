using PRO_1.Clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using System.Collections.ObjectModel;


namespace PRO_1.Ventanas
{

    public partial class Ventana_EjecutivodeServicio : Window
    {
        private ObservableCollection<ListServicios> listServicios = new ObservableCollection<ListServicios>();
        private ListaDeClientes acceso_Cliente;
        private bool AbiertoPorOtraVentana;

        public Ventana_EjecutivodeServicio(ListaDeClientes objetocliente,bool AbiertoPorOtraVentana)
        {
            this.AbiertoPorOtraVentana = AbiertoPorOtraVentana;
            this.acceso_Cliente = objetocliente;
            DataContext = acceso_Cliente;

 
            InitializeComponent();

            if (AbiertoPorOtraVentana == true) { NuevaSesion_MenuItem.IsEnabled = false; }
        }


        /// <summary>
        /// Colapsa todos los stackpanes "Child" de el grid proporcionado, excepto el dado.
        /// </summary>
        /// <param name="visibleStackPanel"></param>
        /// <param name="gridContainer"></param>
        public void CollapseAllStackPanelsExcept(StackPanel visibleStackPanel, Grid gridContainer)
        {
            

            foreach (var child in gridContainer.Children)
            {
                if (child is StackPanel otrosStackPanel && otrosStackPanel != visibleStackPanel)
                {
                    otrosStackPanel.Visibility = Visibility.Collapsed;
                }
                else if (child is StackPanel elStackkPanel && elStackkPanel == visibleStackPanel)
                {
                    elStackkPanel.Visibility = Visibility.Visible;
                }
            }
        }
        /// <summary>
        /// Devuelve la suma de el valor de todos los servicios que contiene el cliente con la matricula dada. 
        /// </summary>
        /// <param name="matriculaUsuario"></param>
        /// <returns></returns>
        public int UpdatePrecioTotal(string matriculaUsuario)
        {
            int x = 0;
            foreach (var item in acceso_Cliente.ListaGlobalClientes)
            {
                try
                {
                    if (item.Matricula.Equals(matriculaUsuario))
                    {
                        var tempList = DataBase.CargarServicios(item.ClienteID);

                        foreach (var itemListaDeServicios in tempList)
                        {
                            x = x + itemListaDeServicios.PrecioServicio;
                        }

                    }
                }
                catch (ArgumentNullException)
                {

                    MessageBox.Show("ERROR: Se debe seleccionar un usuario de matricula valida.");
                }
            }
            return x;
        }
        /// <summary>
        /// Agrega un servicio a la lista servicios que contiene el cliente actualmente seleccionado en la seccion facturacion.
        /// </summary>
        /// <param name="nombreServicio"></param>
        /// <param name="precioServicio"></param>
        public void AgregarServicioALista(string nombreServicio, int precioServicio)
        {
            if (Label_UsuarioSeleccionado.Content != null)
            {
                foreach (var item in acceso_Cliente.ListaGlobalClientes)
                {
                    if (item.Matricula == Label_MatriculaUsuarioSeleccionado.Content.ToString())
                    {
                        var itemModificacionDeLista = item.ListaDeServicios;
                        itemModificacionDeLista.Add((nombreServicio, precioServicio));

                        item.ListaDeServicios = itemModificacionDeLista;
                        DataBase.AgregarServicio(item.ClienteID, nombreServicio, precioServicio);

                        var tempList = DataBase.CargarServicios(item.ClienteID);

                        listServicios.Add(new ListServicios(nombreServicio,precioServicio));
                        
                        

                        Lista_ServiciosSolicitados.ItemsSource = null;
                        Lista_ServiciosSolicitados.ItemsSource = listServicios;

                    }
                }
            }
            else
            {
                MessageBox.Show("ERROR: Seleccionar un cliente!");
            }

            Lista_ServiciosSolicitados.DataContext = acceso_Cliente.ListaGlobalClientes;
        }
        /// <summary>
        /// Elimina el servicio de la lista del cliente que esta actualmente seleccionado.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void EliminarSeleccionadoDeLista_Click(object sender, RoutedEventArgs e)
        {

            if (Label_MatriculaUsuarioSeleccionado.Content == null || Lista_ServiciosSolicitados.SelectedItem == null)
                return;

            for (int i = acceso_Cliente.ListaGlobalClientes.Count - 1; i >= 0; i--)
            {
                var item = acceso_Cliente.ListaGlobalClientes[i];
                if (item.Matricula == Label_MatriculaUsuarioSeleccionado.Content.ToString())
                {

                    var tempList = DataBase.CargarServicios(item.ClienteID);

                    var serviciosSeleccionado = (ListServicios)Lista_ServiciosSolicitados.SelectedItem;
                    foreach (var ListasEnItem in tempList)
                    {
                        if (ListasEnItem.NombreServicio == serviciosSeleccionado.nombreServicio)
                        {
                            DataBase.BorrarServicio(item.ClienteID, serviciosSeleccionado.nombreServicio);
                        }
                    }

                    Lista_ServiciosSolicitados.ItemsSource = null;
                    Lista_ServiciosSolicitados.Items.Clear();
                    Lista_ServiciosSolicitados.ItemsSource = null;

                    listServicios.Clear();
                    listServicios.Clear();

                    foreach (var itemModificacionDeLista in tempList)
                    {
                        Console.WriteLine(itemModificacionDeLista.NombreServicio + " " + itemModificacionDeLista.PrecioServicio);
                        listServicios.Add(new ListServicios(itemModificacionDeLista.NombreServicio, itemModificacionDeLista.PrecioServicio));
                    }

                    Lista_ServiciosSolicitados.ItemsSource = listServicios;
                    PrecioTotal_Label.Content = UpdatePrecioTotal(Label_MatriculaUsuarioSeleccionado.Content.ToString());
                }
            }
        }

        /// <summary>
        /// Elimina todo de la lista del cliente actualmente seleccionado.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void EliminarTodoDeLista_Click(object sender, RoutedEventArgs e)
        {
            if (Label_UsuarioSeleccionado.Content != null)
            {
                foreach (var item in acceso_Cliente.ListaGlobalClientes)
                {


                    if (item.Matricula == Label_MatriculaUsuarioSeleccionado.Content.ToString())
                    {
                        var tempList = DataBase.CargarServicios(item.ClienteID);

                        foreach(var iteminlist in tempList)
                        {
                            DataBase.BorrarServicio(item.ClienteID,iteminlist.NombreServicio);
                        }
                        listServicios.Clear();

                    }
                }
            }
            else
            {
                MessageBox.Show("ERROR: Seleccionar un cliente!");
            }

            Lista_ServiciosSolicitados.ItemsSource = null;
            Lista_ServiciosSolicitados.ItemsSource = acceso_Cliente.ListaGlobalClientes;

            PrecioTotal_Label.Content = UpdatePrecioTotal(Label_MatriculaUsuarioSeleccionado.Content.ToString());

            ActualizarListas();
        }
        /// <summary>
        /// Cada que se eliga un Cliente de la lista, se ejecuta este codigo.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ListaReciboCliente_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var SelectedItem = (Clientes)Lista_ClienteRecibo.SelectedItem;

            if (SelectedItem == null) return;

            Label_UsuarioSeleccionado.Content = $"{SelectedItem.Nombre} {SelectedItem.Apellido} {SelectedItem.Telefono} ";
            Label_MatriculaUsuarioSeleccionado.Content = SelectedItem.Matricula;

            Lista_ServiciosSolicitados.ItemsSource = null;

            foreach (var item in acceso_Cliente.ListaGlobalClientes)
            {
                if (item.ClienteID == SelectedItem.ClienteID)
                {
                    listServicios.Clear();
                    var tempList = DataBase.CargarServicios(item.ClienteID);

                    foreach (var itemModificacionDeLista in tempList)
                    {
                        Console.WriteLine(itemModificacionDeLista.NombreServicio + " " + itemModificacionDeLista.PrecioServicio);
                        listServicios.Add(new ListServicios(itemModificacionDeLista.NombreServicio, itemModificacionDeLista.PrecioServicio));
                    }

                    Lista_ServiciosSolicitados.ItemsSource = listServicios;
                    PrecioTotal_Label.Content = UpdatePrecioTotal(Label_MatriculaUsuarioSeleccionado.Content.ToString());


                }
            }


        }
        /// <summary>
        /// Se despliega un MessageBox distinto dependiendo de la autorizacion del cliente seleccionado, pidiendo una segunda confirmacion de entrega. Entregando el vehiculo y eliminando al cliente del sistema.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void EntregarVehiculo_Click(object sender, RoutedEventArgs e)
        {
            var SelectedItem = (Clientes)Lista_ClienteRecibo.SelectedItem;

            if (SelectedItem == null)
            { MessageBox.Show("ERROR: Se debe seleccionar un usuario"); return; }

            if (SelectedItem.Autorizado == false)
            {
                MessageBoxButton buttons = MessageBoxButton.YesNo;
                MessageBoxImage icon = MessageBoxImage.Question;
                string caption = "Confirmar.";
                string message = "La entrega de este vehiculo no fue autorizada. ¿Estas seguro de esto?";
                MessageBoxResult result = MessageBox.Show(message, caption, buttons, icon);

                if (result == MessageBoxResult.No) return;

                if (result == MessageBoxResult.Yes)
                {
                    DataBase.BorrarClienteDeBDYAPP(SelectedItem.ClienteID, acceso_Cliente);

                    MessageBox.Show("Entregado. Vehiculo removido del sistema.");
                }

            }
            else
            {
                MessageBoxButton buttons = MessageBoxButton.YesNo;
                MessageBoxImage icon = MessageBoxImage.Question;
                string caption = "Confirmar.";
                string message = "¿Entregar vehiculo? Sera eliminado del sistema.";
                MessageBoxResult result = MessageBox.Show(message, caption, buttons, icon);

                if (result == MessageBoxResult.No) return;

                if (result == MessageBoxResult.Yes)
                {
                    DataBase.BorrarClienteDeBDYAPP(SelectedItem.ClienteID, acceso_Cliente);



                    MessageBox.Show("Entregado. Vehiculo removido del sistema.");
                }
            }

            ActualizarListas();

        }

        /// <summary>
        /// Al apretar el item de menu "Cerrar Sesion" cierra la sesion actual.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void CerrarSesionMenu_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow?.Close();
            this.Close();
        }

        /// <summary>
        /// Al apretar el item de menu "Iniciar Sesion" Abrir nuevamente la ventana de login, cerrando la actual.
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
        /// Actualiza las listas de Clientes en la ventana
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ActualizarListas_Click(object sender, RoutedEventArgs e)
        {
            ActualizarListas();


        }
        /// <summary>
        /// Despliega las distintas secciones para cobrar, utilizando el metodo "CollapseAllStackPanelsExcept" para colapsar el resto de secciones excepto la dada.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void SeccionDeCobranza(object sender, RoutedEventArgs e)
        {
            switch (sender)
            {
                case Button button when button == SeccionAlineacion_Button:

                    CollapseAllStackPanelsExcept(Alineacion_Stack, Grid_Ventas);
                    
                    break;
                case Button button when button == SeccionBalanceo_Button:

                    CollapseAllStackPanelsExcept(Balanceo_Stack, Grid_Ventas);

                    break;
                case Button button when button == SeccionLavado_Button:

                    CollapseAllStackPanelsExcept(Lavado_stack, Grid_Ventas);

                    break;
                case Button button when button == SeccionNeumatico_Button:

                    CollapseAllStackPanelsExcept(Neumatico_stack, Grid_Ventas);

                    break;
                case Button button when button == SeccionParking_Button:

                    CollapseAllStackPanelsExcept(Parking_Stack, Grid_Ventas);

                    break;


            }

            ActualizarListas();

        }
        /// <summary>
        /// Adjunta todos los botones que tengan el proposito de agregar un servicio a un usuario y agrega el correspondiente servicio al boton apretado.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void VentadeServicios(object sender, RoutedEventArgs e)
        {
            
            
            if (Label_MatriculaUsuarioSeleccionado.Content.ToString() == "0") { MessageBox.Show("ERROR: No se proporciono un cliente."); return; }
            switch(sender)
            {
                case Button button when button == CobrarAlineacion1Tren_Button:

                    AgregarServicioALista("Alineacion para 1 tren", Precios.Alineacion1Tren);
                    PrecioTotal_Label.Content = UpdatePrecioTotal(Label_MatriculaUsuarioSeleccionado.Content.ToString());


                    break;
                case Button button when button == CobrarAlineacion2Tren_Button:

                    AgregarServicioALista("Alineacion para 2 trenes", Precios.Alineacion2Tren);
                    PrecioTotal_Label.Content = UpdatePrecioTotal(Label_MatriculaUsuarioSeleccionado.Content.ToString());

                    break;
                case Button button when button == CobrarLavadoAuto_Button:

                    AgregarServicioALista("Lavado de Auto", Precios.LavadoAuto);
                    PrecioTotal_Label.Content = UpdatePrecioTotal(Label_MatriculaUsuarioSeleccionado.Content.ToString());

                    break;
                case Button button when button == CobrarLavadoCamioneta_Button:

                    AgregarServicioALista("Lavado de Camioneta", Precios.LavadoCamioneta);
                    PrecioTotal_Label.Content = UpdatePrecioTotal(Label_MatriculaUsuarioSeleccionado.Content.ToString());

                    break;
                case Button button when button == CobrarLavadoCamionUtil_Button:

                    AgregarServicioALista("Lavado de Camion Utilitario", Precios.LavadoCamionUtilitario);
                    PrecioTotal_Label.Content = UpdatePrecioTotal(Label_MatriculaUsuarioSeleccionado.Content.ToString());

                    break;
                case Button button when button == CobrarLavadoCamion_Button:

                    AgregarServicioALista("Lavado de Camion Chico", Precios.LavadoCamionChico);
                    PrecioTotal_Label.Content = UpdatePrecioTotal(Label_MatriculaUsuarioSeleccionado.Content.ToString());

                    break;
                case Button button when button == CobrarLavadoMoto_Button:

                    AgregarServicioALista("Lavado de Moto", Precios.LavadoMoto);
                    PrecioTotal_Label.Content = UpdatePrecioTotal(Label_MatriculaUsuarioSeleccionado.Content.ToString());

                    break;
                case Button button when button == CobrarBalanceoAutoConValvula:

                    AgregarServicioALista("Balanceo de Auto", Precios.BalanceoAuto);
                    PrecioTotal_Label.Content = UpdatePrecioTotal(Label_MatriculaUsuarioSeleccionado.Content.ToString());

                    break;
                case Button button when button == CobrarBalanceoCamionetaConValvula:

                    AgregarServicioALista("Balanceo de Camioneta", Precios.BalanceoCamioneta);
                    PrecioTotal_Label.Content = UpdatePrecioTotal(Label_MatriculaUsuarioSeleccionado.Content.ToString());
                    break;
                case Button button when button == MontajeNeumatico_Button:

                    AgregarServicioALista("Montaje de Neumatico", Precios.MontajeNeumatico);
                    PrecioTotal_Label.Content = UpdatePrecioTotal(Label_MatriculaUsuarioSeleccionado.Content.ToString());
                    break;
                case Button button when button == BridgestoneNeumatico_Button:

                    foreach(var neu in Precios.NeumaticosBridgestone)
                    {
                        if (neu.IDNeumatico == int.Parse(BridgestoneIDNeumatico_Label.Content.ToString()))
                        {
                            AgregarServicioALista($"Neumatico {neu.Modelo}", neu.Precio);
                        }
                    }

                    PrecioTotal_Label.Content = UpdatePrecioTotal(Label_MatriculaUsuarioSeleccionado.Content.ToString());
                    break;
                case Button button when button == MichelinNeumatico_Button:

                    foreach (var neu in Precios.NeumaticosMichelin)
                    {
                        if (neu.IDNeumatico == int.Parse(MichelinIDNeumatico_Label.Content.ToString()))
                        {
                            AgregarServicioALista($"Neumatico {neu.Modelo}", neu.Precio);
                        }
                    }
                    PrecioTotal_Label.Content = UpdatePrecioTotal(Label_MatriculaUsuarioSeleccionado.Content.ToString());
                    break;
                case Button button when button == PirelliNeumatico_Button:

                    foreach (var neu in Precios.NeumaticosPirelli)
                    {
                        if (neu.IDNeumatico == int.Parse(PirelliIDNeumatico_Label.Content.ToString()))
                        {
                            AgregarServicioALista($"Neumatico {neu.Modelo}", neu.Precio);
                        }
                    }

                    PrecioTotal_Label.Content = UpdatePrecioTotal(Label_MatriculaUsuarioSeleccionado.Content.ToString());
                    break;

            }

            ActualizarListas();

        }

        /// <summary>
        /// Colapsa las secciones que no sean ABM Clientes, dejando visible esta.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ABMClienteMenu_Click(object sender, RoutedEventArgs e)
        {
            grid_SeccionFacturacion.Visibility = Visibility.Collapsed;
            grid_SeccionABMCliente.Visibility = Visibility.Visible;
            ActualizarListas();
        }
        /// <summary>
        /// Colapsa las secciones que no sean Facturacion, dejando visible esta.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ReciboDeClienteMenu_Click(object sender, RoutedEventArgs e)
        {
            grid_SeccionFacturacion.Visibility = Visibility.Visible;
            grid_SeccionABMCliente.Visibility = Visibility.Collapsed;
            ActualizarListas();
        }
        /// <summary>
        /// Crea el cliente con todos los parametros dados, creandolos en la lista local y en la base de datos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void CrearCliente_Click(object sender, RoutedEventArgs e)
        {
            bool existeMatricula = acceso_Cliente.ListaGlobalClientes.Any(item => item.Matricula == MatriculaVehiculoCliente_TextBox.Text);

            if (!existeMatricula)
            {
                if (string.IsNullOrEmpty(NombreCliente_TextBox.Text) ||
                    string.IsNullOrEmpty(ApellidoCliente_TextBox.Text) ||
                    string.IsNullOrEmpty(MarcaVehiculoCliente_TextBox.Text) ||
                    string.IsNullOrEmpty(ModeloVehiculoCliente_TextBox.Text) ||
                    string.IsNullOrEmpty(MatriculaVehiculoCliente_TextBox.Text) ||
                    string.IsNullOrEmpty(TelefonoCliente_TextBox.Text)
                    )
                {
                    MessageBox.Show("ERROR: Todos los campos deben estar llenos");
                    return;
                }
                if (!int.TryParse(TelefonoCliente_TextBox.Text, out int _) || TelefonoCliente_TextBox.Text.Length != 9) 
                { MessageBox.Show("ERROR: Ingresar un numero telefonico valido"); return; }

                string patron = @"^[A-Z]{3}\d{4}$"; Regex regex = new Regex(patron);
                if(!regex.IsMatch(MatriculaVehiculoCliente_TextBox.Text)) { MessageBox.Show("ERROR: Debe de ingresar una matricula valida. EJ: 'ABC1234'"); return; }
                
                MessageBox.Show($"Usuario {MatriculaVehiculoCliente_TextBox.Text}, creado.");

                DataBase.AgregarClienteABDYAPP(NombreCliente_TextBox.Text,ApellidoCliente_TextBox.Text,int.Parse(TelefonoCliente_TextBox.Text),MarcaVehiculoCliente_TextBox.Text,ModeloVehiculoCliente_TextBox.Text,MatriculaVehiculoCliente_TextBox.Text, acceso_Cliente);
            }
            else MessageBox.Show("ERROR: Esta matrícula ya está ingresada en el sistema. Modificar o Eliminar el respectivo.");

            ActualizarListas();
            

        }

        
        /// <summary>
        /// Modifica el cliente seleccionado con los campos dados.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ModificarCliente_Click(object sender, RoutedEventArgs e)
        {
            //Si los campos (Donde se inserta el parametro) esta vacio, corta la ejecucion del resto de la funcion y muestra el mensaje error correspondiente.
            if (string.IsNullOrEmpty(NombreActualCliente_TextBox.Text) ||
                    string.IsNullOrEmpty(ApellidoActualCliente_TextBox.Text) ||
                    string.IsNullOrEmpty(MarcaVehiculoActualCliente_TextBox.Text) ||
                    string.IsNullOrEmpty(ModeloVehiculoActualCliente_TextBox.Text) ||
                    string.IsNullOrEmpty(MatriculaVehiculoActualCliente_TextBox.Content.ToString()) ||
                    string.IsNullOrEmpty(TelefonoActualCliente_TextBox.Text)
                    )
            {
                MessageBox.Show("ERROR: Todos los campos deben estar llenos");
                return;
            }

            ///Reitera todo el contenido hasta encontrar la el usuario que contiene la matricula del usuario que se quiere modificiar.
            for (int i = acceso_Cliente.ListaGlobalClientes.Count - 1; i >= 0; i--)
            {
                var item = acceso_Cliente.ListaGlobalClientes[i];

                ///Si no es la matricula que buscamos, cancela la iteracion actual del for loop y salta a la siguiente sin ejecutar el codigo que lo procede.
                if (item.Matricula != MatriculaVehiculoActualCliente_TextBox.Content.ToString()) continue;

                if (!int.TryParse(TelefonoActualCliente_TextBox.Text, out int _) || TelefonoCliente_TextBox.Text.Length != 9)
                { MessageBox.Show("ERROR: Ingresar un numero telefonico valido"); return; }
                string patron = @"^[A-Z]{3}\d{4}$"; Regex regex = new Regex(patron);
                if (!regex.IsMatch(MatriculaVehiculoActualCliente_TextBox.Content.ToString())) { MessageBox.Show("ERROR: Debe de ingresar una matricula valida. EJ: 'ABC1234'"); return; }

                DataBase.ModificarClienteDeBDYAPP(NombreActualCliente_TextBox.Text,ApellidoActualCliente_TextBox.Text,Convert.ToInt32(TelefonoActualCliente_TextBox.Text),MarcaVehiculoActualCliente_TextBox.Text
                    ,ModeloVehiculoActualCliente_TextBox.Text,MatriculaVehiculoActualCliente_TextBox.Content.ToString(),item.ClienteID,item.Autorizado,acceso_Cliente);

                MessageBox.Show("Usuario modificado!");

                //Se encontro el usuario deseado, se modifico y no hay necesidad de seguir iterando la lista, entonces salimos del for con un break;
                break;
                    
            }
            ActualizarListas();
        }

        /// <summary>
        /// Cuando se selecciona un Cliente en la lista "Lista_ClientesParaModificar" se ejecuta el codigo que aplica los datos de el cliente seleccionado a los textBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Lista_ClientesParaModificar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            var SelectedItem = (Clientes)Lista_ClientesParaModificar.SelectedItem;
          

            
            if(SelectedItem != null)
            {
                NombreActualCliente_TextBox.Text = SelectedItem.Nombre;
                ApellidoActualCliente_TextBox.Text = SelectedItem.Apellido;
                TelefonoActualCliente_TextBox.Text = SelectedItem.Telefono.ToString();
                MarcaVehiculoActualCliente_TextBox.Text = SelectedItem.Marca;
                ModeloVehiculoActualCliente_TextBox.Text = SelectedItem.Modelo;
                MatriculaVehiculoActualCliente_TextBox.Content = SelectedItem.Matricula;
            }
            

        }
        /// <summary>
        /// Elimina al cliente seleccionado de la lista local y de la base de datos.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void BajaACliente_Click(object sender, RoutedEventArgs e)
        {
            var SelectedItem = (Clientes)Lista_BajaClientes.SelectedItem;

            if(SelectedItem != null)
            {
                DataBase.BorrarClienteDeBDYAPP(SelectedItem.ClienteID, acceso_Cliente);
                
                
            }

            ActualizarListas();

        }

        public void ActualizarListas()
        {
            Lista_BajaClientes.ItemsSource = null;
            Lista_ClientesParaModificar.ItemsSource = null;
            Lista_ClienteRecibo.ItemsSource = null;

            DataBase.CargarClientesDeBDaAPP(acceso_Cliente);


            Lista_BajaClientes.ItemsSource = acceso_Cliente.ListaGlobalClientes;
            Lista_ClientesParaModificar.ItemsSource = acceso_Cliente.ListaGlobalClientes;
            Lista_ClienteRecibo.ItemsSource = acceso_Cliente.ListaGlobalClientes;

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


        }

        /// <summary>
        /// Cuando se seleccione un item del combobox de Neumaticos Michelin, se ejecuta el codigo que pasa el ID correspondiente a un label.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NeumaticoMichelinComboBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (NeumaticoMichelin_ComboBox.Items.Count <= 0) return;

            string NeumaticoSeleccionado = NeumaticoMichelin_ComboBox.SelectedItem.ToString();

            string[] partes = NeumaticoSeleccionado.Split('/');

            int ID = Convert.ToInt32(partes[3]);
            MichelinIDNeumatico_Label.Content = ID;

            ActualizarListas();
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

            ActualizarListas();
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

            ActualizarListas();
        }
    }
}
