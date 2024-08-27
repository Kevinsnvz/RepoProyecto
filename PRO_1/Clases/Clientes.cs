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
        public string Nombre {  get; set; }
        public string Apellido {  get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string Matricula { get; set; }
        public int Telefono {  get; set; }
        
        public string NombreServicio {  get; set; }
        public int PrecioServicio { get; set; }

        List<(string NombreServicio, int PrecioServicio)> ListaDeServicios { get; set; }

        //Se declara la lista que contiene todos los atributos de cliente
        public List<(string Nombre, string Apellido, int Telefono, string Marca, string Modelo, string Matricula,List<(string NombreServicio,int PrecioServicio)> ListaDeServicios)> origenList_Cliente {  get; set; }

        
        //Constructor de la clase Clientes
        public Clientes()
        {
            origenList_Cliente = new List<(string Nombre, string Apellido, int Telefono, string Marca, string Modelo, string Matricula, List<(string NombreServicio, int PrecioServicio)> ListaDeServicios)>();     
        }
        

        //Metodo para agregar clientes a la lista
        public void AgregarCliente(string Nombre, string Apellido, int Telefono, string Marca, string Modelo, string Matricula)
        {
            bool existencia = false;
            foreach(var item in origenList_Cliente)
            {
                if(item.Matricula == Matricula)
                {
                    existencia = true;
                    MessageBox.Show("ERROR: Esta matricula ya esta ingresada al sistema. Eliminar o Modificar el cliente existente.");
                }
            }
            if (existencia == false)
            {
                List<(string NombreServicio, int PrecioServicio)> ListaDeServicios = new List<(string NombreServicio, int PrecioServicio)>();
                origenList_Cliente.Add((Nombre, Apellido, Telefono, Marca, Modelo, Matricula, ListaDeServicios));
                MessageBox.Show($"Cliente {Nombre} ingresado al sistema");
            }
            

            
        }

        public void RemoverCliente( string Matricula)
        {
            for (int i = origenList_Cliente.Count - 1; i >= 0; i--)
            {
                if (origenList_Cliente[i].Matricula == Matricula)
                {
                    origenList_Cliente.RemoveAt(i);
                }
            }
        }

        //Metodo para conseguir la lista de clientes. A su vez, imprime todos los Clientes por razones de debug.
        public List<(string Nombre, string Apellido, int Telefono, string Marca, string Modelo, string Matricula, List<(string NombreServicio, int PrecioServicio)> ListaDeServicios)> GetCliente()
        {
            foreach (var c in origenList_Cliente)
            {
                System.Diagnostics.Debug.WriteLine(c.ToString());
            }
            return origenList_Cliente.ToList();
        }

        public void AgregarAListaDeServicios(string NombreCliente, string ApellidoCliente, string MatriculaCliente, string NombreDeServicio,int PrecioDeServicio)
        {

            foreach (var item in origenList_Cliente)
            {
                if(item.Matricula == Matricula)
                {
                    var item_ = item.ListaDeServicios;

                    item_.Add((NombreDeServicio, PrecioDeServicio));

                }
            }

        }
        public void RemoverDeListaDeServicios(string NombreCliente, string ApellidoCliente, string MatriculaCliente, string NombreDeServicio, int PrecioDeServicio)
        {
            List <(string Nombre, string Apellido, int Telefono, string Marca, string Modelo, string Matricula, List<(string NombreServicio, int PrecioServicio)> ListaDeServicios)> copyof_List = new List<(string Nombre, string Apellido, int Telefono, string Marca, string Modelo, string Matricula, List<(string NombreServicio, int PrecioServicio)> ListaDeServicios)>();
            foreach (var item in origenList_Cliente)
            {
                if (item.Matricula == Matricula)
                {

                    var item_ = item.ListaDeServicios;

                    item_.Remove((NombreDeServicio, PrecioDeServicio));

                    copyof_List.Add((item.Nombre, item.Apellido, item.Telefono, item.Marca, item.Modelo, item.Matricula,item_));

                }
                else
                {
                    copyof_List.Add(item);
                }
            }
            origenList_Cliente = copyof_List;
        }

        public void ModificarAtributoDeLista(string parNombre, string parApellido, int parTelefono, string parMarca, string parModelo, string parMatricula, List<(string NombreServicio, int PrecioServicio)> parListaDeServicios)
        {

            List<(string Nombre, string Apellido, int Telefono, string Marca, string Modelo, string Matricula, List<(string NombreServicio, int PrecioServicio)> ListaDeServicios)> copyof_List = new List<(string Nombre, string Apellido, int Telefono, string Marca, string Modelo, string Matricula, List<(string NombreServicio, int PrecioServicio)> ListaDeServicios)>();

            foreach(var item in origenList_Cliente )
            {
                if(item.Matricula == parMatricula)

                {
                    copyof_List.Add((parNombre, parApellido, parTelefono, parMarca, parModelo, parMatricula, parListaDeServicios));
                    MessageBox.Show("Modificando cliente.");
                }
                else copyof_List.Add(item);

            }
            origenList_Cliente = copyof_List;
        }
    }
}
