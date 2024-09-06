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
        private static ListaDeUsuarios Usuarios = new ListaDeUsuarios();

      

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
                    Ventana_EjecutivodeServicio ventana_ejecutivo = new Ventana_EjecutivodeServicio(Clientes,false);
                    ventana_ejecutivo.Show();
                    Application.Current.MainWindow?.Close();
                    break;
                case "gerente":
                    Ventana_Gerente ventana_Gerente = new Ventana_Gerente();
                    ventana_Gerente.Show();
                    Application.Current.MainWindow?.Close();
                    break;
                case "jefe_servicio":
                    Ventana_Jefe ventana_Jefe = new Ventana_Jefe(Usuarios,Clientes);
                    ventana_Jefe.Show();
                    Application.Current.MainWindow?.Close();
                    break;
                default:
                    MessageBox.Show("No se dio un rol valido");
                    break;
            }
        }


        /// <summary>
        /// Abrir una ventana dependiendo del num que se ingrese, num que es asignado a una ventana especifica dentro del metodo.
        /// Mas informacion en parametro.
        /// 0 - MainWindow / 1 - Ventana Cajero / 2 - Ventana Ejecutivo / 3 - Ventana Jefe
        /// </summary>
        /// <param name="numVentana"></param>
        public static void AbrirVentana(int numVentana)
        {
            switch (numVentana)
            {
                case 0:
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    break;
                case 1:
                    Ventana_Cajero ventana_Cajero = new Ventana_Cajero(Clientes);
                    ventana_Cajero.Show();
                    break;
                case 2:
                    Ventana_EjecutivodeServicio ventana_ejecutivo = new Ventana_EjecutivodeServicio(Clientes,true);
                    ventana_ejecutivo.Show();
                    break;
                case 3:
                    Ventana_Jefe ventana_Jefe = new Ventana_Jefe(Usuarios,Clientes);
                    break;
                default:
                    Console.WriteLine("ERROR: No se dio una opcion valida, proporcionar un num valido. Referirse a descripcion de parametro.");
                    break;
            }
        }
        public static void AbrirVentana(int numVentana,bool abiertoPorOtraVentana)
        {
            switch (numVentana)
            {
                case 0:
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    break;
                case 1:
                    Ventana_Cajero ventana_Cajero = new Ventana_Cajero(Clientes);
                    ventana_Cajero.Show();
                    break;
                case 2:
                    Ventana_EjecutivodeServicio ventana_ejecutivo = new Ventana_EjecutivodeServicio(Clientes, abiertoPorOtraVentana);
                    ventana_ejecutivo.Show();
                    break;
                case 3:
                    Ventana_Jefe ventana_Jefe = new Ventana_Jefe(Usuarios, Clientes);
                    break;
                default:
                    Console.WriteLine("ERROR: No se dio una opcion valida, proporcionar un num valido. Referirse a descripcion de parametro.");
                    break;
            }
        }

    }
}
