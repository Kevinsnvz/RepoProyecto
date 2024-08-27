using Org.BouncyCastle.Bcpg;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace PRO_1.Clases
{
    public class Clientes
    {
        //Inicializacion y establecimiento de gets/sets para atributos de Clientes
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string Matricula { get; set; }
        public int Telefono { get; set; }

        public List<(string NombreServicio, int PrecioServicio)> ListaDeServicios { get; set; }

        //Constructor de la clase Clientes
        public Clientes(string nombre,string apellido,string marca, string modelo, string matricula, int telefono)
        {
            this.Nombre = nombre;
            this.Apellido = apellido;
            this.Marca = marca;
            this.Modelo = modelo;
            this.Matricula = matricula;
            this.Telefono = telefono;
            this.ListaDeServicios = new List<(string NombreServicio, int PrecioServicio)>();
        }
    }   
}
