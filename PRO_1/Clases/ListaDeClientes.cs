using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRO_1.Clases
{
    public class ListaDeClientes
    {
        private List<Clientes> _listadeclientes = new List<Clientes>();

        public List<Clientes> ListaGlobalClientes
        {
                get { return _listadeclientes; }
                set { _listadeclientes = value; }
        }

    }
}
