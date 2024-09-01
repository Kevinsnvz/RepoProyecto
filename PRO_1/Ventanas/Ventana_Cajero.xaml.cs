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


    public partial class Ventana_Cajero : Window
    {
        private List<ListServicios> listServicios = new List<ListServicios>();
        private ListaDeClientes acceso_Cliente;
        

        public Ventana_Cajero(ListaDeClientes objetocliente)
        {
            
            InitializeComponent();

            DataContext = objetocliente;
            this.acceso_Cliente = objetocliente;


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

        private void Actualizar_Click(object sender, RoutedEventArgs e)
        {
            Lista_ClientesACobrar.ItemsSource = null;
            Lista_ClientesACobrar.ItemsSource = acceso_Cliente.ListaGlobalClientes;
        }

        private void Lista_ClientesACobrar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var SelectedItem = (Clientes)Lista_ClientesACobrar.SelectedItem;

            if (SelectedItem == null) return;

            NombreCliente_Label.Content = SelectedItem.Nombre;
            ApellidoCliente_Label.Content = SelectedItem.Apellido;
            TelefonoCliente_Label.Content = SelectedItem.Telefono.ToString();
            ModeloCliente_Label.Content = SelectedItem.Modelo;
            MarcaCliente_Label.Content = SelectedItem.Marca;
            MatriculaCliente_label.Content = SelectedItem.Matricula;

        }
    }
}
