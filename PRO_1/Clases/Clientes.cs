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
        private string _nombre;
        public string Nombre 
        {
            get { return _nombre; }
            set {  _nombre = value; } 
        }

        private string _apellido;
        public string Apellido
        {
            get { return _apellido; }
            set { _apellido = value; }
        }

        private string _marca;
        public string Marca
        {
            get { return _marca; }
            set { _marca = value; }
        }

        private string _modelo;
        public string Modelo
        {
            get { return _modelo; }
            set { _modelo = value; }
        }

        private string _matricula;
        public string Matricula
        {
            get { return _matricula; }
            set { _matricula = value; }
        }

        private int _telefono;
        public int Telefono
        {
            get { return _telefono; }
            set { _telefono = value; }
        }

        private bool _autorizado;
        public bool Autorizado
        {
            get { return _autorizado; }
            set { _autorizado = value; }
        }

        private int _clienteID;

        public int ClienteID
        {
            get { return _clienteID; }
            set { _clienteID = value; }
        }


        private List<(string NombreServicio, int PrecioServicio)> _listadeservicios;

        public List<(string NombreServicio, int PrecioServicio)> ListaDeServicios
        {
            get { return _listadeservicios; }
            set { _listadeservicios = value; }
        }

        //Constructor de la clase Clientes
        public  Clientes(string nombre, string apellido, string marca, string modelo, string matricula, int telefono, int id)
        {
            this.Nombre = nombre;
            this.Apellido = apellido;
            this.Marca = marca;
            this.Modelo = modelo;
            this.Matricula = matricula;
            this.Telefono = telefono;
            this.ListaDeServicios = new List<(string NombreServicio, int PrecioServicio)>();
            this.Autorizado = false;
            this.ClienteID = id;
        }
        //Constructor sobrecarga
        public Clientes(string nombre, string apellido, string marca, string modelo, string matricula, int telefono, int id,List<(string NombreServicio, int PrecioServicio)> lista, bool autorizado)
        {
            this.Nombre = nombre;
            this.Apellido = apellido;
            this.Marca = marca;
            this.Modelo = modelo;
            this.Matricula = matricula;
            this.Telefono = telefono;
            this.ListaDeServicios = lista;
            this.Autorizado = autorizado;
            this.ClienteID = id;
        }
    }   
}
