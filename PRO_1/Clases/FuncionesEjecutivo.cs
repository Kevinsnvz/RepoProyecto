using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PRO_1.Clases
{
    public class FuncionesEjecutivo
    {
        public static int UpdatePrecioTotal(string matriculaUsuario, ListaDeClientes lista_a_buscar)
        {
            if(matriculaUsuario == null) { MessageBox.Show("ERROR: No se proporciono una matricula valida."); return 0; }
            if(lista_a_buscar == null) { MessageBox.Show("ERROR: No se proporciono una lista valida."); return 0; }
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

    }
}
