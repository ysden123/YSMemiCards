using System.Windows;
using YSMemiCardsApp.Controls;
using YSMemiCardsApp.Data;

namespace YSMemiCardsApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Cards _cards;
        public MainWindow()
        {
            InitializeComponent();
            _cards = Cards.LoadFromFile() ?? new Cards();
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
            if (fvi != null && fvi.FileVersion != null)
            {
                string version = fvi.FileVersion;
                Title = $"{Title} {version}";
            }
        }

        private void CardstMenuItemClick(object sender, RoutedEventArgs e)
        {
            CC.Content = new CardListControl(_cards);
        }

        private void StudyMenuItemClick(object sender, RoutedEventArgs e)
        {
            CC.Content = new StudyControl(_cards);
        }
    }
}