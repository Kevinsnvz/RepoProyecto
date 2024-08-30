using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRO_1.Clases
{
    public class ListServicios
    {

        public string nombreServicio { get; set; }
        public int precioServicio { get; set; }
        public int Servicio_ID { get; set; }

        int counter;
        public ListServicios(string nombreServicio, int precioServicio)
        {
            this.nombreServicio = nombreServicio;
            this.precioServicio = precioServicio;
            this.Servicio_ID = counter + 1;
        }

    }
}
