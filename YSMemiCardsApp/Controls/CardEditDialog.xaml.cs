using System.Windows;
using YSMemiCardsApp.Data;

namespace YSMemiCardsApp.Controls
{
    /// <summary>
    /// Interaction logic for CardEditDialog.xaml
    /// </summary>
    public partial class CardEditDialog : Window
    {
        public CardEditDialog(Card card)
        {
            InitializeComponent();
            DataContext = card;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
