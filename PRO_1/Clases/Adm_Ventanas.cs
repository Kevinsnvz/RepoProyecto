using PRO_1.Ventanas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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


        /// <summary>
        /// Abre la ventana solicitada utilizando como parametro el rol que le corresponde a esa ventana.
        /// </summary>
        /// <param name="rol"> 
        /// <para>
        /// Roles correspondientes:
        /// "cajero"-"ejecutivo_servicio"-"gerente"-"jefe_servicio"
        /// </para>
        /// </param>
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
                    Ventana_Gerente ventana_Gerente = new Ventana_Gerente(Usuarios,Clientes);
                    ventana_Gerente.Show();
                    Application.Current.MainWindow?.Close();
                    DataBase.CargarNeumaticosDeBDaAPP();
                    break;
                case "jefe_servicio":
                    Ventana_Jefe ventana_Jefe = new Ventana_Jefe(Usuarios,Clientes,false);
                    ventana_Jefe.Show();
                    Application.Current.MainWindow?.Close();
                    break;
                case "operativo":
                    Ventana_OperativoDeCamarasYRespaldo ventana_operativo = new Ventana_OperativoDeCamarasYRespaldo();
                    ventana_operativo.Show();
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
        /// </summary>
        /// <param name="numVentana">
        /// <para>
        /// Ventanas correspondientes:
        /// 0: Mainwindow - 1: Ventana_Cajero - 2: Ventana_EjecutivodeServicio - 3: Ventana_Jefe - 4: Ventana_Gerente
        /// </para>
        /// </param>
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
                    Ventana_Jefe ventana_Jefe = new Ventana_Jefe(Usuarios, Clientes, true);
                    ventana_Jefe.Show();
                    break;
                case 4:
                    Ventana_Gerente ventana_Gerente = new Ventana_Gerente(Usuarios, Clientes);
                    ventana_Gerente.Show();
                    break;
                default:
                    Console.WriteLine("ERROR: No se dio una opcion valida, proporcionar un num valido. Referirse a descripcion de parametro.");
                    break;
            }
        }
        /// <summary>
        /// Abrir una ventana dependiendo del num que se ingrese, num que es asignado a una ventana especifica dentro del metodo. Esta sobrecarga agrega la posibilidad de aclarar si fue abierto o no por otra ventana.
        /// Mas informacion en parametro.
        /// </summary>
        /// <param name="numVentana">
        /// <para>
        /// Ventanas correspondientes:
        /// 0: Mainwindow - 1: Ventana_Cajero - 2: Ventana_EjecutivodeServicio - 3: Ventana_Jefe - 4: Ventana_Gerente
        /// </para>
        /// </param>
        /// <param name="abiertoPorOtraVentana"> True o False; Aclara si esta abierto por otra ventana o no, esto tiene consecuencias en su GUI. 
        /// </param>
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
                    Ventana_Jefe ventana_Jefe = new Ventana_Jefe(Usuarios, Clientes,abiertoPorOtraVentana);
                    ventana_Jefe.Show();

                        break;
                default:
                    Console.WriteLine("ERROR: No se dio una opcion valida, proporcionar un num valido. Referirse a descripcion de parametro.");
                    break;
            }
        }
        public static void Mod_Cliente(Clientes newcliente, Clientes oldcliente)
        {
            Clientes.ListaGlobalClientes.Remove(oldcliente);
            Clientes.ListaGlobalClientes.Add(newcliente);
        }

    }
}
