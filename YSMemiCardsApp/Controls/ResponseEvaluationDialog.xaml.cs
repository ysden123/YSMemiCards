using System.Windows;
using YSMemiCardsApp.Data;

namespace YSMemiCardsApp.Controls
{
    /// <summary>
    /// Interaction logic for ResponseEvaluationDialog.xaml
    /// </summary>
    public partial class ResponseEvaluationDialog : Window
    {
        public DifficultyLevel DifficultyLevel { get; private set; }
        public ResponseEvaluationDialog(Card card)
        {
            InitializeComponent();
            DataContext = card;
        }

        private void HardButton_Click(object sender, RoutedEventArgs e)
        {
            DifficultyLevel = DifficultyLevel.Hard;
            DialogResult = true;
        }

        private void MediumButton_Click(object sender, RoutedEventArgs e)
        {
            DifficultyLevel = DifficultyLevel.Medium;
            DialogResult = true;
        }

        private void EasyButton_Click(object sender, RoutedEventArgs e)
        {
            DifficultyLevel = DifficultyLevel.Easy;
            DialogResult = true;
        }
    }
}
