using PRO_1.Clases;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Lógica de interacción para Ventana_EjecutivodeServicio.xaml
    /// </summary>
    public partial class Ventana_EjecutivodeServicio : Window
    {
        List<ListServicios> listServicios = new List<ListServicios>();
        Clientes acceso_Cliente;

        public Ventana_EjecutivodeServicio(Clientes objetocliente)
        {
            this.acceso_Cliente = objetocliente;
            InitializeComponent();
        }

        public void UpdatePrecioTotal()
        {
            int x = 0;
            if (listServicios.Count > 0)
            {
                foreach (var item in listServicios)
                {
                    x = x + item.precio;
                }
            }
            else { x = 0; }
            PrecioTotal_Label.Content = "Total: " + x;
        }

        public void AgregarServicioALista(string nombre, int precio)
        {
            if (Combobos_ClientesFacturacion.SelectedItem != null)
            {
                listServicios.Add(new ListServicios() { nombre = nombre, precio = precio });

                ListView_Servicios.ItemsSource = null;
                ListView_Servicios.ItemsSource = listServicios;
            }
            else
            {
                MessageBox.Show("ERROR: Seleccionar un cliente!");
            }
        }

        //Al apretar el item de menu "Cerrar Sesion" cerrar la sesion, je re evidente
        private void CerrarSesionMenu_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow?.Close();
            this.Close();
        }

        //Al apretar el item de menu "Iniciar Sesion" Abrir nuevamente la ventana de login
        private void IniciarSesionMenu_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow?.Close();
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        //Actualiza la lista de Clientes
        private void Actualizar_Click(object sender, RoutedEventArgs e)
        {
            ObservableCollection<Clientes> clientes = new ObservableCollection<Clientes>();

            foreach (var Cliente_Objeto in acceso_Cliente.GetCliente())
            {
                clientes.Add(new Clientes() { Nombre = Cliente_Objeto.Nombre, Apellido = Cliente_Objeto.Apellido, Telefono = Cliente_Objeto.Telefono, Marca = Cliente_Objeto.Marca, Modelo = Cliente_Objeto.Modelo, Matricula = Cliente_Objeto.Matricula,  });
                string toComboParcial = $"{Cliente_Objeto.Nombre} {Cliente_Objeto.Apellido} {Cliente_Objeto.Telefono} {Cliente_Objeto.Matricula}";
                string toComboCompleto = $"{Cliente_Objeto.Nombre} {Cliente_Objeto.Apellido} {Cliente_Objeto.Telefono} {Cliente_Objeto.Marca} {Cliente_Objeto.Modelo} {Cliente_Objeto.Matricula}";
                
                Combobos_ClientesFacturacion.Items.Clear();
                Combobos_ClientesFacturacion.Items.Add(toComboParcial);

            }

            Lista_ClientesParaModificar.ItemsSource = null;
            Lista_ClientesParaModificar.ItemsSource = clientes;

            Lista_BajaClientes.ItemsSource = null;
            Lista_BajaClientes.ItemsSource = clientes;

            Lista_Cliente.ItemsSource = null;
            Lista_Cliente.ItemsSource = clientes;



        }

        private void Autorizar_Click(object sender, RoutedEventArgs e)
        {

        }
        private void Balanceo_Click(object sender, RoutedEventArgs e)
        {
            Alineacion_Stack.Visibility = Visibility.Hidden;
            Lavado_stack.Visibility = Visibility.Hidden;
            Balanceo_Stack.Visibility = Visibility.Visible;
            Neumatico_stack.Visibility = Visibility.Hidden;
        }
        private void Lavadero_Click(object sender, RoutedEventArgs e)
        {
            Alineacion_Stack.Visibility = Visibility.Hidden;
            Lavado_stack.Visibility = Visibility.Visible;
            Balanceo_Stack.Visibility = Visibility.Hidden;
            Neumatico_stack.Visibility = Visibility.Hidden;
        }
        private void Alineacion_Click(object sender, RoutedEventArgs e)
        {
            Alineacion_Stack.Visibility = Visibility.Visible;
            Lavado_stack.Visibility = Visibility.Hidden;
            Balanceo_Stack.Visibility = Visibility.Hidden;
            Neumatico_stack.Visibility = Visibility.Hidden;
        }
        private void Neumatico_Click(object sender, RoutedEventArgs e)
        {
            Alineacion_Stack.Visibility = Visibility.Hidden;
            Lavado_stack.Visibility = Visibility.Hidden;
            Balanceo_Stack.Visibility = Visibility.Hidden;
            Neumatico_stack.Visibility = Visibility.Visible;
        }
        private void Parking_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Servicios_Selected_Click(object sender, RoutedEventArgs e)
        {

        }



        private void Alineacion1Tren_Click(object sender, RoutedEventArgs e)
        {

            AgregarServicioALista("Alineacion para 1 tren", Precios.alineacion1Tren);
            UpdatePrecioTotal();



        }

        private void Alineacion2Tren_Click(object sender, RoutedEventArgs e)
        {
            AgregarServicioALista("Alineacion para 2 trenes", Precios.alineacion2Tren);
            UpdatePrecioTotal();


        }

        private void Motocicleta_Click(object sender, RoutedEventArgs e)
        {
            AgregarServicioALista("Lavado de Moto", Precios.lavadomoto);
            UpdatePrecioTotal();
        }

        private void Auto_Click(object sender, RoutedEventArgs e)
        {
            AgregarServicioALista("Lavado de Auto", Precios.lavadoauto);
            UpdatePrecioTotal();

        }

        private void Camioneta_Click(object sender, RoutedEventArgs e)
        {
            AgregarServicioALista("Lavado de Camioneta", Precios.lavadocamioneta);
            UpdatePrecioTotal();
        }

        private void CamionChico_Click(object sender, RoutedEventArgs e)
        {

            AgregarServicioALista("Lavado de Camion Chico", Precios.lavadocamionchico);
            UpdatePrecioTotal();
        }

        private void CamionUtilitario_Click(object sender, RoutedEventArgs e)
        {
            AgregarServicioALista("Lavado de Camion Utilitario", Precios.lavadocamionutilitario);
            UpdatePrecioTotal();
        }

        private void EliminarTodoDeLista_Click(object sender, RoutedEventArgs e)
        {
            listServicios.Clear();
            ListView_Servicios.ItemsSource = null;
            ListView_Servicios.ItemsSource = listServicios;
            UpdatePrecioTotal();
        }

        private void EliminarSeleccionadoDeLista_Click(object sender, RoutedEventArgs e)
        {


        }



        private void BalanceoAuto_Click(object sender, RoutedEventArgs e)
        {
            AgregarServicioALista("Balanceo de Auto", Precios.balanceoauto);
            UpdatePrecioTotal();
        }
        private void BalanceoCamioneta_Click(object sender, RoutedEventArgs e)
        {
            AgregarServicioALista("Balanceo de Camioneta", Precios.balanceoamioneta);
            UpdatePrecioTotal();
        }

        private void ABMClienteMenu_Click(object sender, RoutedEventArgs e)
        {
            grid_SeccionFacturacion.Visibility = Visibility.Collapsed;
            grid_SeccionBajaCliente.Visibility = Visibility.Visible;
        }

        private void ReciboDeClienteMenu_Click(object sender, RoutedEventArgs e)
        {
            grid_SeccionFacturacion.Visibility = Visibility.Visible;
            grid_SeccionBajaCliente.Visibility = Visibility.Collapsed;
        }

        private void IngresarCliente_Click(object sender, RoutedEventArgs e)
        {
            if(int.TryParse(TelefonoCliente_TextBox.Text, out int val))
            {
                acceso_Cliente.AgregarCliente(NombreCliente_TextBox.Text,ApellidoCliente_TextBox.Text,int.Parse(TelefonoCliente_TextBox.Text),MarcaVehiculoCliente_TextBox.Text,ModeloVehiculoCliente_TextBox.Text,MatriculaVehiculoCliente_TextBox.Text);
            }
            else
            {
                MessageBox.Show("ERROR: El cliente ingresado no es valido");
            }
            
        }

        

        private void GuardarCliente_Click(object sender, RoutedEventArgs e)
        {
            var SelectedItem = (Clientes)Lista_ClientesParaModificar.SelectedItem;
            List<(string NombreServicio, int PrecioServicio)> SelectedItemList;
            if (SelectedItem != null)
            {
                foreach (var item in acceso_Cliente.GetCliente())
                {

                    if (SelectedItem.Matricula == item.Matricula)
                    {
                        SelectedItemList = item.ListaDeServicios;

                        bool existencia;
                        foreach(var item_ in acceso_Cliente.GetCliente())
                        {
                            
                        }
                        acceso_Cliente.RemoverCliente(item.Matricula);
                        acceso_Cliente.AgregarCliente(NombreActualCliente_TextBox.Text ,ApellidoActualCliente_TextBox.Text, int.Parse(TelefonoActualCliente_TextBox.Text), MarcaVehiculoActualCliente_TextBox.Text, ModeloVehiculoActualCliente_TextBox.Text, MatriculaVehiculoActualCliente_TextBox.Text);

                        MessageBox.Show("Cliente modificado");
                    }
                }
            }
            else MessageBox.Show("Debe Seleccionar un cliente");
            
            

        }

        private void Lista_ClientesParaModificar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            var SelectedItem = (Clientes)Lista_ClientesParaModificar.SelectedItem;
          

            
            if(SelectedItem != null)
            {
                NombreActualCliente_TextBox.Text = SelectedItem.Nombre;
                ApellidoActualCliente_TextBox.Text = SelectedItem.Apellido;
                TelefonoActualCliente_TextBox.Text = SelectedItem.Telefono.ToString();
                MarcaVehiculoActualCliente_TextBox.Text = SelectedItem.Marca;
                ModeloVehiculoActualCliente_TextBox.Text = SelectedItem.Modelo;
                MatriculaVehiculoActualCliente_TextBox.Text = SelectedItem.Matricula;
            }
            else MessageBox.Show("Debe Seleccionar un cliente");

        }

        private void BajaACliente_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
