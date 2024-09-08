using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRO_1.Clases
{
    //Clase que declara los atributos de lo que compone un servicio, con su correspondiente constructor.
    public class ListServicios
    {

        private string _nombreservicio;
        public string nombreServicio
        {
            get { return _nombreservicio; }
            set { _nombreservicio = value; }
        }
        private int _precioservicio;
        public int precioServicio
        {
            get { return _precioservicio; }
            set { _precioservicio = value; }
        }
        private int _servicioid;
        public int Servicio_ID
        {
            get { return _servicioid; }
            set { _servicioid = value; }
        }

        //Constructor de la clase
        public ListServicios(string nombreServicio, int precioServicio)
        {
            this.nombreServicio = nombreServicio;
            this.precioServicio = precioServicio;
        }

    }
}
