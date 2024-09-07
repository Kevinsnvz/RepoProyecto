using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace PRO_1.Clases
{
    public class Neumatico
    {
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

        private int _ancho;
        public int Ancho
        {
            get { return _ancho; }
            set { _ancho = value; }
        }

        private int _perfil;
        public int Perfil
        {
            get { return _perfil; }
            set { _perfil = value; }
        }

        private int _rodado;
        public int Rodado
        {
            get { return _rodado; }
            set { _rodado = value; }
        }

        private int _precio;
        public int Precio
        {
            get { return _precio; }
            set { _precio = value; }
        }

        private int _idneumatico;
        public int IDNeumatico
        {
            get { return _idneumatico; }
            set { _idneumatico = value; }
        }

        private int _stock;
        public int Stock
        {
            get { return _stock; }
            set { _stock = value; }
        }

        public Neumatico(string marca, string modelo, int ancho, int perfil, int rodado,int stock, int IDNeumatico)
        {

            this.Marca = marca;
            this.Modelo = modelo;
            this.Ancho = ancho;
            this.Perfil = perfil;
            this.Rodado = rodado;
            this.Precio = 0;
            this.IDNeumatico = IDNeumatico;
            this.Stock = stock;
        }

        public Neumatico(string marca,string modelo,int ancho,int perfil,int rodado, int stock,int IDNeumatico,int precio)
        {

            this.Marca = marca;
            this.Modelo = modelo;
            this.Ancho = ancho;
            this.Perfil = perfil;
            this.Rodado = rodado;
            this.Precio = precio;
            this.Stock = stock;
            this.IDNeumatico = IDNeumatico;

        }
    }
}
