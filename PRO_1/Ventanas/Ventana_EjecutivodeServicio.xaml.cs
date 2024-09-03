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
    /// <summary>
    /// Lógica de interacción para Ventana_EjecutivodeServicio.xaml
    /// </summary>
    public partial class Ventana_EjecutivodeServicio : Window
    {
        private ObservableCollection<ListServicios> listServicios = new ObservableCollection<ListServicios>();
        private ListaDeClientes acceso_Cliente;

        public Ventana_EjecutivodeServicio(ListaDeClientes objetocliente)
        {
            this.acceso_Cliente = objetocliente;
            DataContext = acceso_Cliente;
            InitializeComponent();
        }

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

        public int UpdatePrecioTotal(string matriculaUsuario)
        {
            int x = 0;
            foreach (var item in acceso_Cliente.ListaGlobalClientes)
            {
                try
                {
                    if (item.Matricula.Equals(matriculaUsuario))
                    {
                        foreach (var itemListaDeServicios in item.ListaDeServicios)
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

        public void AgregarServicioALista(string nombreServicio, int precioServicio)
        {
            if (Label_UsuarioSeleccionado.Content != null)
            {
                foreach(var item in acceso_Cliente.ListaGlobalClientes)
                {
                    if(item.Matricula == Label_MatriculaUsuarioSeleccionado.Content.ToString())
                    {
                        var itemModificacionDeLista = item.ListaDeServicios;
                        itemModificacionDeLista.Add((nombreServicio, precioServicio));

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

        public void EliminarSeleccionadoDeLista_Click(object sender, RoutedEventArgs e)
        {

            if (Label_MatriculaUsuarioSeleccionado.Content == null || Lista_ServiciosSolicitados.SelectedItem == null)
                return;

            for (int i = acceso_Cliente.ListaGlobalClientes.Count - 1; i >= 0; i--)
            {
                var item = acceso_Cliente.ListaGlobalClientes[i];
                if (item.Matricula == Label_MatriculaUsuarioSeleccionado.Content.ToString())
                { 

                    var itemModificacionDeLista = item.ListaDeServicios;
                    var serviciosSeleccionado = (ListServicios)Lista_ServiciosSolicitados.SelectedItem;
                    foreach (var ListasEnItem in itemModificacionDeLista)
                    {
                        if (ListasEnItem.NombreServicio == serviciosSeleccionado.nombreServicio)
                        {
                            itemModificacionDeLista.Remove(ListasEnItem);
                            listServicios.Remove(serviciosSeleccionado);

                            Lista_ServiciosSolicitados.ItemsSource = null;
                            Lista_ServiciosSolicitados.ItemsSource = listServicios;

                            PrecioTotal_Label.Content = UpdatePrecioTotal(Label_MatriculaUsuarioSeleccionado.Content.ToString());
                            return;
                        }


                    }
                }


            }




        }


        public void EliminarTodoDeLista_Click(object sender, RoutedEventArgs e)
        {
            if (Label_UsuarioSeleccionado.Content != null)
            {
                foreach (var item in acceso_Cliente.ListaGlobalClientes)
                {
                    if (item.Matricula == Label_MatriculaUsuarioSeleccionado.Content.ToString())
                    {
                        var itemModificacionDeLista = item.ListaDeServicios;
                        itemModificacionDeLista.Clear();
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
        }

        public void ListaReciboCliente_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var SelectedItem = (Clientes)Lista_ClienteRecibo.SelectedItem;

            if (SelectedItem == null) return;

            Label_UsuarioSeleccionado.Content = $"{SelectedItem.Nombre} {SelectedItem.Apellido} {SelectedItem.Telefono} ";
            Label_MatriculaUsuarioSeleccionado.Content = SelectedItem.Matricula;

            Lista_ServiciosSolicitados.ItemsSource = null;

            foreach (var item in acceso_Cliente.ListaGlobalClientes)
            {
                if (item.Matricula == Label_MatriculaUsuarioSeleccionado.Content.ToString())
                {
                    listServicios.Clear();
                    foreach (var itemModificacionDeLista in item.ListaDeServicios)
                    {
                        listServicios.Add(new ListServicios(itemModificacionDeLista.NombreServicio, itemModificacionDeLista.PrecioServicio));
                    }

                    Lista_ServiciosSolicitados.ItemsSource = listServicios;
                    PrecioTotal_Label.Content = UpdatePrecioTotal(Label_MatriculaUsuarioSeleccionado.Content.ToString());


                }
            }


        }

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
                    acceso_Cliente.ListaGlobalClientes.Remove(SelectedItem);

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
                    acceso_Cliente.ListaGlobalClientes.Remove(SelectedItem);



                    MessageBox.Show("Entregado. Vehiculo removido del sistema.");
                }
            }

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

        //Actualiza la lista de Clientes
        public void ActualizarListas_Click(object sender, RoutedEventArgs e)
        {

            DataBase.CargarClientesDeBD(acceso_Cliente);

            Lista_BajaClientes.ItemsSource = null;
            Lista_BajaClientes.ItemsSource = acceso_Cliente.ListaGlobalClientes;

            Lista_ClientesParaModificar.ItemsSource = null;
            Lista_ClientesParaModificar.ItemsSource = acceso_Cliente.ListaGlobalClientes;

            Lista_ClienteRecibo.ItemsSource = null;
            Lista_ClienteRecibo.ItemsSource = acceso_Cliente.ListaGlobalClientes;


        }

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

        }

        public void VentadeServicios(object sender, RoutedEventArgs e)
        {
            
            
            if (Label_MatriculaUsuarioSeleccionado.Content.ToString() == "0") { MessageBox.Show("ERROR: No se proporciono un cliente."); return; }
            switch(sender)
            {
                case Button button when button == CobrarAlineacion1Tren_Button:

                    AgregarServicioALista("Alineacion para 1 tren", Precios.alineacion1Tren);
                    PrecioTotal_Label.Content = UpdatePrecioTotal(Label_MatriculaUsuarioSeleccionado.Content.ToString());


                    break;
                case Button button when button == CobrarAlineacion2Tren_Button:

                    AgregarServicioALista("Alineacion para 2 trenes", Precios.alineacion2Tren);
                    PrecioTotal_Label.Content = UpdatePrecioTotal(Label_MatriculaUsuarioSeleccionado.Content.ToString());

                    break;
                case Button button when button == CobrarLavadoAuto_Button:

                    AgregarServicioALista("Lavado de Auto", Precios.lavadoauto);
                    PrecioTotal_Label.Content = UpdatePrecioTotal(Label_MatriculaUsuarioSeleccionado.Content.ToString());

                    break;
                case Button button when button == CobrarLavadoCamioneta_Button:

                    AgregarServicioALista("Lavado de Camioneta", Precios.lavadocamioneta);
                    PrecioTotal_Label.Content = UpdatePrecioTotal(Label_MatriculaUsuarioSeleccionado.Content.ToString());

                    break;
                case Button button when button == CobrarLavadoCamionUtil_Button:

                    AgregarServicioALista("Lavado de Camion Utilitario", Precios.lavadocamionutilitario);
                    PrecioTotal_Label.Content = UpdatePrecioTotal(Label_MatriculaUsuarioSeleccionado.Content.ToString());

                    break;
                case Button button when button == CobrarLavadoCamion_Button:

                    AgregarServicioALista("Lavado de Camion Chico", Precios.lavadocamionchico);
                    PrecioTotal_Label.Content = UpdatePrecioTotal(Label_MatriculaUsuarioSeleccionado.Content.ToString());

                    break;
                case Button button when button == CobrarLavadoMoto_Button:

                    AgregarServicioALista("Lavado de Moto", Precios.lavadomoto);
                    PrecioTotal_Label.Content = UpdatePrecioTotal(Label_MatriculaUsuarioSeleccionado.Content.ToString());

                    break;
                case Button button when button == CobrarBalanceoAutoConValvula:

                    AgregarServicioALista("Balanceo de Auto", Precios.balanceoauto);
                    PrecioTotal_Label.Content = UpdatePrecioTotal(Label_MatriculaUsuarioSeleccionado.Content.ToString());

                    break;
                case Button button when button == CobrarBalanceoCamionetaConValvula:

                    AgregarServicioALista("Balanceo de Camioneta", Precios.balanceoamioneta);
                    PrecioTotal_Label.Content = UpdatePrecioTotal(Label_MatriculaUsuarioSeleccionado.Content.ToString());

                    break;

            }


        }

        public void ABMClienteMenu_Click(object sender, RoutedEventArgs e)
        {
            grid_SeccionFacturacion.Visibility = Visibility.Collapsed;
            grid_SeccionABMCliente.Visibility = Visibility.Visible;
        }

        public void ReciboDeClienteMenu_Click(object sender, RoutedEventArgs e)
        {
            grid_SeccionFacturacion.Visibility = Visibility.Visible;
            grid_SeccionABMCliente.Visibility = Visibility.Collapsed;
        }

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
                if (!int.TryParse(TelefonoCliente_TextBox.Text, out int t) || TelefonoCliente_TextBox.Text.Length != 9) 
                { MessageBox.Show("ERROR: Ingresar un numero telefonico valido"); return; }

                string patron = @"^[A-Z]{3}\d{4}$"; Regex regex = new Regex(patron);
                if(!regex.IsMatch(MatriculaVehiculoCliente_TextBox.Text)) { MessageBox.Show("ERROR: Debe de ingresar una matricula valida. EJ: 'ABC1234'"); return; }
                
                MessageBox.Show($"Usuario {MatriculaVehiculoCliente_TextBox.Text}, creado.");

                DataBase.AgregarClienteABDYAPP(NombreCliente_TextBox.Text,ApellidoCliente_TextBox.Text,int.Parse(TelefonoCliente_TextBox.Text),MarcaVehiculoCliente_TextBox.Text,ModeloVehiculoCliente_TextBox.Text,MatriculaVehiculoCliente_TextBox.Text, acceso_Cliente);
            }
            else MessageBox.Show("ERROR: Esta matrícula ya está ingresada en el sistema. Modificar o Eliminar el respectivo.");
            

        }

        

        public void GuardarCliente_Click(object sender, RoutedEventArgs e)
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

                DataBase.ModificarClienteDeBDYAPP(NombreActualCliente_TextBox.Text,ApellidoActualCliente_TextBox.Text,Convert.ToInt32(TelefonoActualCliente_TextBox.Text),MarcaVehiculoActualCliente_TextBox.Text
                    ,ModeloVehiculoActualCliente_TextBox.Text,MatriculaVehiculoActualCliente_TextBox.Content.ToString(),item.ClienteID,acceso_Cliente);

                MessageBox.Show("Usuario modificado!");

                //Se encontro el usuario deseado, se modifico y no hay necesidad de seguir iterando la lista, entonces salimos del for con un break;
                break;
                    
            }
        }

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

        public void BajaACliente_Click(object sender, RoutedEventArgs e)
        {
            var SelectedItem = (Clientes)Lista_BajaClientes.SelectedItem;

            if(SelectedItem != null)
            {
                acceso_Cliente.ListaGlobalClientes.Remove(SelectedItem);
                
            }
               
        }


    }
}
