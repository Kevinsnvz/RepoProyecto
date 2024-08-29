using PRO_1.Clases;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
    /// Lógica de interacción para Ventana_EjecutivodeServicio.xaml
    /// </summary>
    public partial class Ventana_EjecutivodeServicio : Window
    {
        List<ListServicios> listServicios = new List<ListServicios>();
        ListaDeClientes acceso_Cliente;

        public Ventana_EjecutivodeServicio(ListaDeClientes objetocliente)
        {
            this.acceso_Cliente = objetocliente;
            DataContext = acceso_Cliente;
            InitializeComponent();
        }

        public void UpdatePrecioTotal(string matriculaUsuario)
        {
            int x = 0;
            foreach(var item in acceso_Cliente.ListaGlobalClientes)
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
                catch(ArgumentNullException)
                {

                    MessageBox.Show("ERROR: Se debe seleccionar un usuario de matricula valida.");
                }
            }
            PrecioTotal_Label.Content = "Total: " + x;
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

                        listServicios.Add(new ListServicios() { nombreServicio = nombreServicio, precioServicio = precioServicio });

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
            Lista_BajaClientes.ItemsSource = null;
            Lista_BajaClientes.ItemsSource = acceso_Cliente.ListaGlobalClientes;

            Lista_ClientesParaModificar.ItemsSource = null;
            Lista_ClientesParaModificar.ItemsSource = acceso_Cliente.ListaGlobalClientes;

            Lista_ClienteRecibo.ItemsSource = null;
            Lista_ClienteRecibo.ItemsSource = acceso_Cliente.ListaGlobalClientes;


        }

        private void Autorizar_Click(object sender, RoutedEventArgs e)
        {

        }
        private void BalanceoSeccion_Click(object sender, RoutedEventArgs e)
        {
            Alineacion_Stack.Visibility = Visibility.Hidden;
            Lavado_stack.Visibility = Visibility.Hidden;
            Balanceo_Stack.Visibility = Visibility.Visible;
            Neumatico_stack.Visibility = Visibility.Hidden;
        }
        private void LavaderoSeccion_Click(object sender, RoutedEventArgs e)
        {
            Alineacion_Stack.Visibility = Visibility.Hidden;
            Lavado_stack.Visibility = Visibility.Visible;
            Balanceo_Stack.Visibility = Visibility.Hidden;
            Neumatico_stack.Visibility = Visibility.Hidden;
        }
        private void AlineacionSeccion_Click(object sender, RoutedEventArgs e)
        {
            Alineacion_Stack.Visibility = Visibility.Visible;
            Lavado_stack.Visibility = Visibility.Hidden;
            Balanceo_Stack.Visibility = Visibility.Hidden;
            Neumatico_stack.Visibility = Visibility.Hidden;
        }
        private void NeumaticoSeccion_Click(object sender, RoutedEventArgs e)
        {
            Alineacion_Stack.Visibility = Visibility.Hidden;
            Lavado_stack.Visibility = Visibility.Hidden;
            Balanceo_Stack.Visibility = Visibility.Hidden;
            Neumatico_stack.Visibility = Visibility.Visible;
        }
        private void ParkingSeccion_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Alineacion1Tren_Click(object sender, RoutedEventArgs e)
        {

            AgregarServicioALista("Alineacion para 1 tren", Precios.alineacion1Tren);
            UpdatePrecioTotal(Label_MatriculaUsuarioSeleccionado.Content.ToString());



        }

        private void Alineacion2Tren_Click(object sender, RoutedEventArgs e)
        {
            AgregarServicioALista("Alineacion para 2 trenes", Precios.alineacion2Tren);
            UpdatePrecioTotal(Label_MatriculaUsuarioSeleccionado.Content.ToString());


        }

        private void MotocicletaLavado_Click(object sender, RoutedEventArgs e)
        {
            AgregarServicioALista("Lavado de Moto", Precios.lavadomoto);
            UpdatePrecioTotal(Label_MatriculaUsuarioSeleccionado.Content.ToString());
        }

        private void AutoLavado_Click(object sender, RoutedEventArgs e)
        {
            AgregarServicioALista("Lavado de Auto", Precios.lavadoauto);
            UpdatePrecioTotal(Label_MatriculaUsuarioSeleccionado.Content.ToString());

        }

        private void CamionetaLavado_Click(object sender, RoutedEventArgs e)
        {
            AgregarServicioALista("Lavado de Camioneta", Precios.lavadocamioneta);
            UpdatePrecioTotal(Label_MatriculaUsuarioSeleccionado.Content.ToString());
        }

        private void CamionChicoLavado_Click(object sender, RoutedEventArgs e)
        {

            AgregarServicioALista("Lavado de Camion Chico", Precios.lavadocamionchico);
            UpdatePrecioTotal(Label_MatriculaUsuarioSeleccionado.Content.ToString());
        }

        private void CamionUtilitarioLavado_Click(object sender, RoutedEventArgs e)
        {
            AgregarServicioALista("Lavado de Camion Utilitario", Precios.lavadocamionutilitario);
            UpdatePrecioTotal(Label_MatriculaUsuarioSeleccionado.Content.ToString());
        }

        private void EliminarTodoDeLista_Click(object sender, RoutedEventArgs e)
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
            UpdatePrecioTotal(Label_MatriculaUsuarioSeleccionado.Content.ToString());
        }

        private void BalanceoAuto_Click(object sender, RoutedEventArgs e)
        {
            AgregarServicioALista("Balanceo de Auto", Precios.balanceoauto);
            UpdatePrecioTotal(Label_MatriculaUsuarioSeleccionado.Content.ToString());
        }

        private void BalanceoCamioneta_Click(object sender, RoutedEventArgs e)
        {
            AgregarServicioALista("Balanceo de Camioneta", Precios.balanceoamioneta);
            UpdatePrecioTotal(Label_MatriculaUsuarioSeleccionado.Content.ToString());
        }

        private void EliminarSeleccionadoDeLista_Click(object sender, RoutedEventArgs e)
        {


        }

        private void ABMClienteMenu_Click(object sender, RoutedEventArgs e)
        {
            grid_SeccionFacturacion.Visibility = Visibility.Collapsed;
            grid_SeccionABMCliente.Visibility = Visibility.Visible;
        }

        private void ReciboDeClienteMenu_Click(object sender, RoutedEventArgs e)
        {
            grid_SeccionFacturacion.Visibility = Visibility.Visible;
            grid_SeccionABMCliente.Visibility = Visibility.Collapsed;
        }

        private void CrearCliente_Click(object sender, RoutedEventArgs e)
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
                if (!int.TryParse(TelefonoCliente_TextBox.Text, out int t) || TelefonoCliente_TextBox.Text.Length != 9) { MessageBox.Show("ERROR: Ingresar un numero telefonico valido"); return; }
                string patron = @"^[A-Z]{3}\d{4}$"; Regex regex = new Regex(patron);
                if(!regex.IsMatch(MatriculaVehiculoCliente_TextBox.Text)) { MessageBox.Show("ERROR: Debe de ingresar una matricula valida. EJ: 'ABC1234'"); return; }
                Clientes Cliente = new Clientes(NombreCliente_TextBox.Text, ApellidoCliente_TextBox.Text, MarcaVehiculoCliente_TextBox.Text, ModeloVehiculoCliente_TextBox.Text, MatriculaVehiculoCliente_TextBox.Text, Convert.ToInt32(TelefonoCliente_TextBox.Text));
                acceso_Cliente.ListaGlobalClientes.Add(Cliente);
                MessageBox.Show($"Usuario {MatriculaVehiculoCliente_TextBox.Text}, creado.");
            }
            else MessageBox.Show("ERROR: Esta matrícula ya está ingresada en el sistema. Modificar o Eliminar el respectivo.");
            

        }

        

        private void GuardarCliente_Click(object sender, RoutedEventArgs e)
        {
                for (int i = acceso_Cliente.ListaGlobalClientes.Count - 1; i >= 0; i--)
                {
                    var item = acceso_Cliente.ListaGlobalClientes[i];
                    if (item.Matricula == MatriculaVehiculoActualCliente_TextBox.Content.ToString())
                    {
                        MessageBox.Show(MatriculaVehiculoActualCliente_TextBox.Content.ToString());

                        acceso_Cliente.ListaGlobalClientes.Remove(item);

                        Clientes clientes = new Clientes(NombreActualCliente_TextBox.Text, ApellidoActualCliente_TextBox.Text, MarcaVehiculoActualCliente_TextBox.Text, ModeloVehiculoActualCliente_TextBox.Text, MatriculaVehiculoActualCliente_TextBox.Content.ToString(), int.Parse(TelefonoActualCliente_TextBox.Text));

                        acceso_Cliente.ListaGlobalClientes.Add(clientes);

                        MessageBox.Show("Usuario modificado!");
                    }
                }
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
                MatriculaVehiculoActualCliente_TextBox.Content = SelectedItem.Matricula;
            }
            

        }

        private void BajaACliente_Click(object sender, RoutedEventArgs e)
        {
            var SelectedItem = (Clientes)Lista_BajaClientes.SelectedItem;

            if(SelectedItem != null)
            {
                acceso_Cliente.ListaGlobalClientes.Remove(SelectedItem);
            }
               
        }

        private void Lista_Cliente_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var SelectedItem = (Clientes)Lista_ClienteRecibo.SelectedItem;

            if (SelectedItem != null)
            {
                Label_UsuarioSeleccionado.Content = $"{SelectedItem.Nombre} {SelectedItem.Apellido} {SelectedItem.Telefono} ";
                Label_MatriculaUsuarioSeleccionado.Content = SelectedItem.Matricula;
                Lista_ServiciosSolicitados.ItemsSource = null;
            }
        }
    }
}
