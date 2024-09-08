using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRO_1.Clases
{
    ///Clase que declara la una lista compuesta por los atributos de la clase Clientes.
    public class ListaDeClientes
    {
        private List<Clientes> _listaglobalclientes= new List<Clientes>();
        public List<Clientes> ListaGlobalClientes
        {
            get { return _listaglobalclientes; }
            set { _listaglobalclientes = value; }
            
        }


    }
}
