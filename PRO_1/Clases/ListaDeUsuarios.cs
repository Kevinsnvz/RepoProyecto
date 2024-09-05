using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRO_1.Clases
{
    public class ListaDeUsuarios
    {
        private  List<Usuarios> _listadeusuarios;

        public  List<Usuarios> ListaGlobalUsuarios
            {
                get { return _listadeusuarios; }
                set { _listadeusuarios = value; }
            }

    }
}
