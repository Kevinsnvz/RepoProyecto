using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRO_1.Clases
{
    public class ListaDeFechasIngresos
    {

        private List<Clientes> _listadefechasdeingreso = new List<Clientes>();

        public List<Clientes> ListaGlobalFechasDeIngreso
        {
            get { return _listadefechasdeingreso; }
            set { _listadefechasdeingreso = value; }
        }

    }
}
