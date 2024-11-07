using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRO_1.Clases
{
    //Clase que posee todas las instancias de precios utilizados por el programa. En formato "Static" para tener una unica clase manejando los precios para el programa.
    public class Precios
    {
        private static int _alineacion1trendesdeR17 = 5;
        public static int Alineacion1TrenDesdeR17
        {
            get
            {
                return _alineacion1trendesdeR17;
            }
            set { _alineacion1trendesdeR17 = value; }

        }

        private static int _alineacion1tren = 5;
        public static int Alineacion1Tren
        {
            get
            {
                return _alineacion1tren;
            }
            set { _alineacion1tren = value; }

        }

        private static int _alineacion2tren = 10;
        public static int Alineacion2Tren
        {
            get
            {
                return _alineacion2tren;
            }
            set { _alineacion2tren= value; }

        }

        private static int _lavadomoto = 20;
        public static int LavadoMoto
        {
            get
            {
                return _lavadomoto;
            }
            set { _lavadomoto =  value; }
        }

        private static int _lavadoauto = 25;
        public static int LavadoAuto
        {
            get
            {
                return _lavadoauto;
            }
            set { _lavadoauto = value; }
        }

        private static int _lavadocamioneta = 30;
        public static int LavadoCamioneta
        {
            get
            {
                return _lavadocamioneta;
            }
            set { _lavadocamioneta = value; }
        }

        private static int _lavadocamionchico = 35;
        public static int LavadoCamionChico
        {
            get
            {
                return _lavadocamionchico;
            }
            set { _lavadocamionchico = value; }
        }

        private static int _lavadocamionutilitario = 40;
        public static int LavadoCamionUtilitario
        {
            get
            {
                return _lavadocamionutilitario;
            }
            set { _lavadocamionutilitario = value; }
        }

        private static int _balanceoauto = 40;
        public static int BalanceoAuto
        {
            get
            {
                return _balanceoauto;
            }
            set { _balanceoauto = value; }
        }

        private static int _balanceocamioneta = 40;
        public static int BalanceoCamioneta
        {
            get
            {
                return _balanceocamioneta;
            }
            set { _balanceocamioneta = value; }
        }

        private static int _montajeneumatico = 40;
        public static int MontajeNeumatico
        {
            get
            {
                return _montajeneumatico;
            }
            set { _montajeneumatico =  value; }
        }

        private static List<Neumatico> _neumaticosmichelin = new List<Neumatico>();
        public static List<Neumatico> NeumaticosMichelin
        {
            get { return _neumaticosmichelin; }
            set { _neumaticosmichelin = value; }
        }

        private static List<Neumatico> _neumaticosbridgestone = new List<Neumatico>();
        public static List<Neumatico> NeumaticosBridgestone
        {
            get { return _neumaticosbridgestone; }
            set { _neumaticosbridgestone = value; }
        }

        private static List<Neumatico> _neumaticosPirelli = new List<Neumatico>();
        public static List<Neumatico> NeumaticosPirelli
        {
            get { return _neumaticosPirelli; }
            set { _neumaticosPirelli = value; }
        }

        private static int _parkingcomunMoto = 50;
        public static int ParkingComunMoto
        {
            get
            {
                return _parkingcomunMoto;
            }
            set { _parkingcomunMoto = value; }
        }

        private static int _parkingcomunAuto = 100;
        public static int ParkingComunAuto
        {
            get
            {
                return _parkingcomunAuto;
            }
            set { _parkingcomunAuto = value; }
        }

        private static int _parkingcomunCamioneta = 120;
        public static int ParkingComunCamioneta
        {
            get
            {
                return _parkingcomunCamioneta;
            }
            set { _parkingcomunCamioneta = value; }
        }

        private static int _parkingcomunPequeñoCamion= 120;
        public static int ParkingComunPequeñoCamion
        {
            get
            {
                return _parkingcomunPequeñoCamion;
            }
            set { _parkingcomunPequeñoCamion = value; }
        }

        private static int _parkingcomunPequeñoUtilitario= 120;
        public static int ParkingComunPequeñoUtilitario
        {
            get
            {
                return _parkingcomunPequeñoUtilitario;
            }
            set { _parkingcomunPequeñoUtilitario = value; }
        }
    }
}



