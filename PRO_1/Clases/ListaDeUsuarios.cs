using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRO_1.Clases
{
    //Clase que posee una lista compuesta por los atributos de la clase Usuarios.
    public class ListaDeUsuarios
    {
        private List<Usuarios> _listadeusuarios = new List<Usuarios>();

        public  List<Usuarios> ListaGlobalUsuarios
            {
                get { return _listadeusuarios; }
                set { _listadeusuarios = value; }
            }

    }


}
