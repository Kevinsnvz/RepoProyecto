using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRO_1.Clases
{
    public class ListaDeServiciosCliente
    {
        private List<(string NombreServicio, int PrecioServicio)> _listadeservicios;

        public List<(string NombreServicio, int PrecioServicio)> ListaDeServicios
        {
            get { return _listadeservicios; }
            set { _listadeservicios = value; }
        }

    }
}
