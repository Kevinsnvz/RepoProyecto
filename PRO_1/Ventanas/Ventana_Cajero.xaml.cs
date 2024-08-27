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
        List<ListServicios> listServicios = new List<ListServicios>();
        ListaDeClientes acceso_Cliente;
        

        public Ventana_Cajero(ListaDeClientes objetocliente)
        {
            
            InitializeComponent();

            DataContext = objetocliente;
            this.acceso_Cliente = objetocliente;


        }

        public void UpdatePrecioTotal()
        {
            int x = 0;
            if (listServicios.Count > 0)
            {
                foreach (var item in listServicios)
                {
                    x = x + item.precioServicio;
                }
            }
            else { x = 0; }
            PrecioTotal_Label.Content ="Total: "+ x;
        }

        public void AgregarServicioALista(string nombre, int precio)
        {
            if (Combobos_Clientes.SelectedItem != null)
            {
                listServicios.Add(new ListServicios() { nombreServicio = nombre, precioServicio = precio});

                ListView_Servicios.ItemsSource = null;
                ListView_Servicios.ItemsSource = listServicios;
            }
            else
            {
                MessageBox.Show("ERROR: Seleccionar un cliente!");
            }
        }

        //Al apretar el item de menu "Cerrar Sesion" cerrar la sesion, je re evidente
        private void CerrarSesion(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow?.Close();
            this.Close();
        }

        //Al apretar el item de menu "Iniciar Sesion" Abrir nuevamente la ventana de login
        private void IniciarSesion(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow?.Close();
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        //Actualiza la lista de Clientes
        private void Actualizar_Click(object sender, RoutedEventArgs e)
        {
            Lista_Cliente.ItemsSource = null;
            Lista_Cliente.ItemsSource = acceso_Cliente.ListaGlobalClientes;

        }

        private void Autorizar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
