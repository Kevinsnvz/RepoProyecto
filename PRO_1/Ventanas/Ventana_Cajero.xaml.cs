using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using MySqlX.XDevAPI;
using Org.BouncyCastle.Ocsp;
using PRO_1.Clases;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Paragraph = iText.Layout.Element.Paragraph;

namespace PRO_1.Ventanas
{


    public partial class Ventana_Cajero : Window
    {
        private List<ListServicios> listServicios = new List<ListServicios>();
        private ListaDeClientes acceso_Cliente;
        


        public Ventana_Cajero(ListaDeClientes objetocliente)
        {
            
            InitializeComponent();

            DataContext = objetocliente;
            this.acceso_Cliente = objetocliente;
            


        }

        //Al apretar el item de menu "Cerrar Sesion" cerrar la sesion, je re evidente
        private void CerrarSesionMenu_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow?.Close();
            this.Close();
        }

        //Al apretar el item de menu "Iniciar Sesion" Abrir nuevamente la ventana de login
        private void IniciarSesionMenu_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow?.Close();
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void Actualizar_Click(object sender, RoutedEventArgs e)
        {
            Lista_ClientesACobrar.ItemsSource = null;
            Lista_ClientesACobrar.ItemsSource = acceso_Cliente.ListaGlobalClientes;
        }

        private void Lista_ClientesACobrar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var SelectedItem = (Clientes)Lista_ClientesACobrar.SelectedItem;

            if (SelectedItem == null) return;

            NombreCliente_Label.Content = SelectedItem.Nombre;
            ApellidoCliente_Label.Content = SelectedItem.Apellido;
            TelefonoCliente_Label.Content = SelectedItem.Telefono.ToString();
            ModeloCliente_Label.Content = SelectedItem.Modelo;
            MarcaCliente_Label.Content = SelectedItem.Marca;
            MatriculaCliente_label.Content = SelectedItem.Matricula;

            

            var SelectedItemServicios = SelectedItem.ListaDeServicios;

            ServiciosACobrar_Label.Content = string.Empty;

            foreach (var Servicio in SelectedItemServicios)
            {
                ServiciosACobrar_Label.Content += $"  - {Servicio.NombreServicio}: {Servicio.PrecioServicio}\n";
            }
            ServiciosACobrar_Label.Content += "\n";

        }

        private void ImprimirRecibo(object sender, RoutedEventArgs e)
        {

            if(NombreCliente_Label == null ) return;

            String dest = "C:/Users/ksnvz/Desktop/recibo.pdf";
            

            using (PdfWriter writer = new PdfWriter(dest))
            using (PdfDocument pdfDocument = new PdfDocument(writer))
            using (Document document = new Document(pdfDocument))
            {
                Paragraph paragraph = new Paragraph("FACTURA").SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER);
                Paragraph SPACING = new Paragraph();

                document.Add(paragraph);
                document.Add(SPACING);
                document.Add(SPACING);

                document.Add(new Paragraph("CLIENTE:"));
                document.Add(new Paragraph(NombreCliente_Label.Content.ToString()));
                document.Add(new Paragraph(ApellidoCliente_Label.Content.ToString()));
                document.Add(new Paragraph(TelefonoCliente_Label.Content.ToString()));
                document.Add(new Paragraph(MatriculaCliente_label.Content.ToString()));

                iText.Layout.Element.Table table = new iText.Layout.Element.Table(new float[] { 3, 7 });
                table.AddCell("Servicio");
                table.AddCell("Precio");

                int TOTAL = 0;
                foreach(var CLIENTES in acceso_Cliente.ListaGlobalClientes)
                {
                    if (CLIENTES.Matricula != MatriculaCliente_label.Content.ToString())
                        break;
                    foreach(var SERVICIOSENCLIENTES in CLIENTES.ListaDeServicios)
                    {
                        table.AddCell(SERVICIOSENCLIENTES.NombreServicio);
                        table.AddCell(SERVICIOSENCLIENTES.PrecioServicio.ToString());
                        TOTAL += SERVICIOSENCLIENTES.PrecioServicio;
                    }
                }
                table.AddCell("TOTAL:");
                table.AddCell(TOTAL.ToString());
                document.Add(table);
                

                document.Add(SPACING);
                document.Add(new Paragraph("Kibe APPS")).SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.LEFT);
                document.Close();
                        
            }
            
    }
        
    }
}
