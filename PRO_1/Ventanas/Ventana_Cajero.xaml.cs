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
        Clientes acceso_Cliente;
        

        public Ventana_Cajero(Clientes objetocliente)
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
            PrecioTotal_Label.Content ="Total: "+ x;
        }

        public void AgregarServicioALista(string nombre, int precio)
        {
            if (Combobos_Clientes.SelectedItem != null)
            {
                listServicios.Add(new ListServicios() { nombre = nombre, precio = precio});

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
            ObservableCollection<Clientes> clientes = new ObservableCollection<Clientes>();

            foreach (var Cliente_Objeto in acceso_Cliente.GetCliente())
            {
                clientes.Add(new Clientes() {Nombre = Cliente_Objeto.Nombre, Apellido = Cliente_Objeto.Apellido, Telefono = Cliente_Objeto.Telefono, Marca = Cliente_Objeto.Marca, Modelo = Cliente_Objeto.Modelo, Matricula = Cliente_Objeto.Matricula });
                string toCombo = $"{Cliente_Objeto.Nombre} {Cliente_Objeto.Apellido} {Cliente_Objeto.Telefono} {Cliente_Objeto.Matricula}";
                Combobos_Clientes.Items.Add(toCombo);
            }

            Lista_Cliente.ItemsSource = clientes;

            
                
        }

        private void Autorizar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
