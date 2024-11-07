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
using System.Windows.Shapes;

namespace PRO_1.Ventanas
{
    /// <summary>
    /// Lógica de interacción para DialogVentana.xaml
    /// </summary>
    public partial class DialogVentana : Window
    {
        public DialogVentana()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Access the text from the textboxes
            string text1 = textBox1.Text;
            string text2 = textBox2.Text;

            // Do something with the text, e.g., close the dialog:
            DialogResult = true;
            Close();
        }
    }
}
