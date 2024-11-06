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
        private ObservableCollection<ListServicios> listServicios = new ObservableCollection<ListServicios>();
        private ListaDeClientes acceso_Cliente;
        


        public Ventana_Cajero(ListaDeClientes objetocliente)
        {
            
            InitializeComponent();

            DataContext = objetocliente;
            this.acceso_Cliente = objetocliente;
            


        }

        //Al apretar el item de menu "Cerrar Sesion" cerrar la sesion, je re evidente
        public void CerrarSesionMenu_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow?.Close();
            this.Close();
        }

        //Al apretar el item de menu "Iniciar Sesion" Abrir nuevamente la ventana de login
        public void IniciarSesionMenu_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow?.Close();
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        //Actualiza la lista.
        public void Actualizar_Click(object sender, RoutedEventArgs e)
        {
            DataBase.CargarClientesDeBDaAPP(acceso_Cliente);

            Lista_ClientesACobrar.ItemsSource = null;
            Lista_ClientesACobrar.ItemsSource = acceso_Cliente.ListaGlobalClientes;
        }

        //Cuando se selecciona un item de esta lista se ejecuta el codigo. Establece los datos del cliente en la seccion factura de la ventana.
        public void Lista_ClientesACobrar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var SelectedItem = (Clientes)Lista_ClientesACobrar.SelectedItem;

            if (SelectedItem == null) return;

            NombreCliente_Label.Content = SelectedItem.Nombre;
            ApellidoCliente_Label.Content = SelectedItem.Apellido;
            TelefonoCliente_Label.Content = SelectedItem.Telefono.ToString();
            ModeloCliente_Label.Content = SelectedItem.Modelo;
            MarcaCliente_Label.Content = SelectedItem.Marca;
            MatriculaCliente_label.Content = SelectedItem.Matricula;
            Autorizado_label.Content = SelectedItem.Autorizado.ToString();
            IDCliente_label.Content = SelectedItem.ClienteID;

            

            

            ServiciosACobrar_Label.Content = string.Empty;

            var tempList = DataBase.CargarServicios(SelectedItem.ClienteID);

            foreach (var Servicio in tempList)
            {
                ServiciosACobrar_Label.Content += $"  - {Servicio.NombreServicio}: {Servicio.PrecioServicio}\n";
                Console.WriteLine(Servicio.NombreServicio + " " + Servicio.PrecioServicio);
            }
            ServiciosACobrar_Label.Content += "\n";

        }


        //Toma los datos del Cliente al igual que los servicios que solicito y los imprime en un recibo de formato PDF
        public void ImprimirRecibo(object sender, RoutedEventArgs e)
        {

            if(NombreCliente_Label == null ) return;

            String dest = "C:/Users/kevin/Desktop/recibo/recibo.pdf";


            try
            {

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
                    document.Add(new Paragraph(IDCliente_label.Content.ToString()));

                    iText.Layout.Element.Table table = new iText.Layout.Element.Table(new float[] { 3, 7 });
                    table.AddCell("Servicio");
                    table.AddCell("Precio");
                    var tempList = DataBase.CargarServicios(int.Parse(IDCliente_label.Content.ToString()));

                    int x = 0;
                    foreach(var i in tempList)
                    {
                        table.AddCell(i.NombreServicio);
                        table.AddCell(i.PrecioServicio.ToString());
                        x += i.PrecioServicio;
                    }

                    int TOTAL = x;

                    //Si el checkbox mostrado como "Autorizar Entregea?" es activado, modificar cliente a tener la autorizacion como TRUE si no, utilizar la autorizacion que contenga el cliente para la impresion.
                    if (Autorizar_CheckBox.IsChecked == true)
                    {
                        DataBase.ModificarClienteDeBDYAPP
                            (
                            NombreCliente_Label.Content.ToString(),
                            ApellidoCliente_Label.Content.ToString(),
                            Convert.ToInt32(TelefonoCliente_Label.Content),
                            MarcaCliente_Label.Content.ToString(),
                            ModeloCliente_Label.Content.ToString(),
                            MatriculaCliente_label.Content.ToString(),
                            Convert.ToInt32(IDCliente_label.Content),
                            true,
                            acceso_Cliente
                            );
                    }
                    else
                    {
                        DataBase.ModificarClienteDeBDYAPP
                            (
                            NombreCliente_Label.Content.ToString(),
                            ApellidoCliente_Label.Content.ToString(),
                            Convert.ToInt32(TelefonoCliente_Label.Content),
                            MarcaCliente_Label.Content.ToString(),
                            ModeloCliente_Label.Content.ToString(),
                            MatriculaCliente_label.Content.ToString(),
                            Convert.ToInt32(IDCliente_label.Content),
                            Convert.ToBoolean(Autorizado_label.Content),
                            acceso_Cliente
                            );
                    }

                    table.AddCell("TOTAL:");
                    table.AddCell(TOTAL.ToString());
                    document.Add(table);


                    document.Add(SPACING);
                    document.Add(new Paragraph("Kibe APPS")).SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.LEFT);
                    document.Close();

                    MessageBox.Show("Impreso con exito.");

                }

            }
            catch(IOException ex)
            {

                MessageBox.Show(ex.Message + " No se pudo adquirir acceso al archivo destino. Cerrar procesos que lo esten utilizando.");

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            
            
        }
        
    }
}
