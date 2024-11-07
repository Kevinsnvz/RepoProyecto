using PRO_1.Clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using Camera = PRO_1.Clases.Camera;

namespace PRO_1.Ventanas
{
    /// <summary>
    /// Lógica de interacción para Ventana_OperativoDeCamarasYRespaldo.xaml
    /// </summary>
    public partial class Ventana_OperativoDeCamarasYRespaldo : Window
    {
        List<Camera> cameras = new List<Camera>();

        public Ventana_OperativoDeCamarasYRespaldo()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Al apretar el item de menu "Cerrar Sesion" cierra la sesion actual.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void CerrarSesionMenu_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow?.Close();
            this.Close();
        }

        /// <summary>
        /// Al apretar el item de menu "Iniciar Sesion" Abrir nuevamente la ventana de login, cerrando la actual.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void IniciarSesionMenu_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow?.Close();
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void _CamarasMenu_Click(object sender, RoutedEventArgs e)
        {
            CameraPanel.Visibility = Visibility.Visible;
            BackupPanel.Visibility = Visibility.Collapsed;
        }

        private void _RespaldoMenu_Click(object sender, RoutedEventArgs e)
        {
            CameraPanel.Visibility = Visibility.Collapsed;
            BackupPanel.Visibility = Visibility.Visible;
        }

        private void CameraPanelButton_Click(object sender, RoutedEventArgs e)
        {
            CameraADD_Panel.Visibility = Visibility.Visible;
            ActiveCameras_Stack.Visibility = Visibility.Collapsed;
        }

        private void AddCameraButton_Click(object sender, RoutedEventArgs e)
        {
            if(CameraNameTextBox.Text == string.Empty && CameraIPTextBox.Text == string.Empty)
            {
                MessageBox.Show("Para crear una camara debes de llenar todos los campos.");
                return;
            }

            string CameraName = CameraNameTextBox.Text;
            int CameraIP = int.Parse(CameraIPTextBox.Text);

            cameras.Add(new Camera(CameraIP,CameraName));

        }

        private void ActiveCameras_Click(object sender, RoutedEventArgs e)
        {
            CameraADD_Panel.Visibility = Visibility.Collapsed;
            ActiveCameras_Stack.Visibility = Visibility.Visible;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogVentana dialog = new DialogVentana();
            if (dialog.ShowDialog() == true)
            {
                // Access the text from the dialog's textboxes here
                string text1 = dialog.textBox1.Text;
                string text2 = dialog.textBox2.Text;

                // Do something with the text
            }
        }
    }
}
