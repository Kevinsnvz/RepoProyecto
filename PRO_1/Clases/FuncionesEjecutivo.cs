using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace PRO_1.Clases
{
    public class FuncionesEjecutivo
    {
        public static int UpdatePrecioTotal(string matriculaUsuario, ListaDeClientes lista_a_buscar)
        {
            int x = 0;
            foreach (var item in lista_a_buscar.ListaGlobalClientes)
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

        public static void EntregarVehiculo(ListView listView, ListaDeClientes lista_a_buscar)
        {
            var SelectedItem = (Clientes)listView.SelectedItem;

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
                    lista_a_buscar.ListaGlobalClientes.Remove(SelectedItem);

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
                    lista_a_buscar.ListaGlobalClientes.Remove(SelectedItem);



                    MessageBox.Show("Entregado. Vehiculo removido del sistema.");
                }
            }

        }

    }

    
}
