using PRO_1.Ventanas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace PRO_1.Clases
{
    
    public class Adm_Ventanas
    {
        private static ListaDeClientes Clientes = new ListaDeClientes();

        //Abrir una ventana dependiendo del rol en string que se le ingrese
        public static void AbrirVentanaPorRol(string rol)
        {
            switch (rol)
            {
                case "cajero":
                    Ventana_Cajero ventana_Cajero = new Ventana_Cajero(Clientes);
                    ventana_Cajero.Show();
                    Application.Current.MainWindow?.Close();
                    break;
                case "ejecutivo_servicio":
                    Ventana_EjecutivodeServicio ventana_ejecutivo = new Ventana_EjecutivodeServicio(Clientes);
                    ventana_ejecutivo.Show();
                    Application.Current.MainWindow?.Close();
                    break;
                case "gerente":
                    Ventana_Gerente ventana_Gerente = new Ventana_Gerente();
                    ventana_Gerente.Show();
                    Application.Current.MainWindow?.Close();
                    break;
                default:
                    MessageBox.Show("No se dio un rol valido");
                    break;
            }
        }
        //Abrir una ventana dependiendo del nombre en string que se le ingrese. El nombre debe de ser el nombre de la clase ventana, siendo Case Sensitive
        public static void AbrirVentana(string nombreVentana)
        {
            switch (nombreVentana)
            {
                case "MainWindow":
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    break;
                case "Ventana_Cajero":
                    Ventana_Cajero ventana_Cajero = new Ventana_Cajero(Clientes);
                    ventana_Cajero.Show();
                    break;
                case "Ventana_EjecutivodeServicio":
                    Ventana_EjecutivodeServicio ventana_ejecutivo = new Ventana_EjecutivodeServicio(Clientes);
                    ventana_ejecutivo.Show();
                    break;
                default:
                    MessageBox.Show("No se dio una ventana valida");
                    break;
            }
        }
    }
}
