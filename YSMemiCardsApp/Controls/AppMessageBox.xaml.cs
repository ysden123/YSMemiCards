using System.Windows;

namespace YSMemiCardsApp.Controls
{
    /// <summary>
    /// Interaction logic for AppMessageBox.xaml
    /// </summary>
    public partial class AppMessageBox : Window
    {
        public AppMessageBox(string message)
        {
            InitializeComponent();
            MessageTextBlock.Text = message;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
