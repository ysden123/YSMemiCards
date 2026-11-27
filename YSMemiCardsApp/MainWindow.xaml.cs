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
        private Cards _cards;
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
            new CardListDialog(_cards)
            {
                Owner = Window.GetWindow(this)
            }.ShowDialog();

            _cards = Cards.LoadFromFile() ?? new Cards();
        }

        private void StudyMenuItemClick(object sender, RoutedEventArgs e)
        {
            var cardsToStudy = BuildListToStudy();

            if (cardsToStudy.CardsList.Count == 0)
            {
                MessageBox.Show("No cards to study.", "Study Options", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            MessageBoxResult result = MessageBox.Show($"Totoal card number {_cards.CardsList.Count}\nCards to study: {cardsToStudy.CardsList.Count}\nDo you want to study?", "Study Options",
                MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes)
            {
                return;
            }

            Window? owner = Window.GetWindow(this);
            foreach (var card in cardsToStudy!.CardsList)
            {
                var wordShownDialog = new AppMessageBox($"Word: {card.Word}\n\nPress OK to see the translation.")
                {
                    Owner = owner
                };
                wordShownDialog.ShowDialog();

                var responseDialog = new ResponseEvaluationDialog(card)
                {
                    Owner = owner
                };
                var studtRresult = responseDialog.ShowDialog();
                if (studtRresult == true)
                {
                    card.Level = responseDialog.DifficultyLevel;
                    card.LastTimeUsed = DateTime.Now;
                }
            }

            _cards.SaveToFile();
            _cards = Cards.LoadFromFile() ?? new Cards();
        }

        private Cards BuildListToStudy()
        {
            var configuration = Configuration.LoadFromFile();
            var cardsToStudy = new Cards();

            if (_cards == null)
            {
                return cardsToStudy;
            }

            foreach (var card in _cards.CardsList)
            {
                // todo: add logic to determine which cards to study
                bool shouldStudyCard =
                    (card.LastTimeUsed == null)
                    || ((DateTime.Now - card.LastTimeUsed).Value.TotalDays > configuration!.FirstDays && card.Level < DifficultyLevel.Medium)
                    || ((DateTime.Now - card.LastTimeUsed).Value.TotalDays > configuration!.SecondDays && card.Level < DifficultyLevel.Easy)
                    || ((DateTime.Now - card.LastTimeUsed).Value.TotalDays > 10);
                if (shouldStudyCard)
                {
                    cardsToStudy.CardsList.Add(card);
                }
            }
            return cardsToStudy;
        }

        private void ConfigurationMenuItemClick(object sender, RoutedEventArgs e)
        {
            var configuration = Configuration.LoadFromFile() ?? new Configuration();

            var dialog = new ConfigurationDialog()
            {
                Owner = Window.GetWindow(this),
                DataContext = configuration
            };

            var result = dialog.ShowDialog();
            if (result == true)
            {
                Configuration.SaveToFile(configuration);
            }
        }
    }
}