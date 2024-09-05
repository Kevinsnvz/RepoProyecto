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
    /// Lógica de interacción para Ventana_Jefe.xaml
    /// </summary>
    public partial class Ventana_Jefe : Window
    {
        private ObservableCollection<ListServicios> listServicios = new ObservableCollection<ListServicios> ();
        private ListaDeClientes acceso_cliente;
        private ListaDeUsuarios acceso_usuarios;

        public Ventana_Jefe(ListaDeUsuarios usuarios, ListaDeClientes clientes)
        {
            this.acceso_cliente = clientes;
            this.acceso_usuarios = usuarios;

            InitializeComponent();
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

        private void FuncionesDeEJ_Click(object sender, RoutedEventArgs e)
        {
            Adm_Ventanas.AbrirVentana(2);
        }

        private void ActualizarListas_Click(object sender, RoutedEventArgs e)
        {
            DataBase.CargarUsuariosDeBD(acceso_usuarios);

            Lista_BajaEjecutivos.ItemsSource = null;
            Lista_BajaEjecutivos.ItemsSource = acceso_usuarios.ListaGlobalUsuarios;

            Lista_EjecutivosParaModificar.ItemsSource = null;
            Lista_EjecutivosParaModificar.ItemsSource = acceso_usuarios.ListaGlobalUsuarios;

        }

        private void BajaAEjecutivo_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void CrearEjecutivo_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ListaEjecutivosParaModificar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void GuardarEjecutivo_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
