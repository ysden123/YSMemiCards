using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using YSMemiCardsApp.Data;

namespace YSMemiCardsApp.Controls
{
    /// <summary>
    /// Interaction logic for StudyControl.xaml
    /// </summary>
    public partial class StudyControl : UserControl
    {
        private Cards? _cardsToStudy;
        private readonly Cards? _cards;

        public StudyControl(Cards? cards)
        {
            InitializeComponent();
            _cards = cards;
            BuildListToStudy(cards);
        }

        private void BuildListToStudy(Cards? cards)
        {
            _cardsToStudy = new Cards();
            if (cards == null)
            {
                return;
            }

            foreach (var card in cards.CardsList)
            {
                // todo: add logic to determine which cards to study
                _cardsToStudy.CardsList.Add(card);
            }
        }

        private void Process()
        {
            Window? owner = Window.GetWindow(this);
            foreach (var card in _cardsToStudy!.CardsList)
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
                var result = responseDialog.ShowDialog();
                if (result == true)
                {
                    card.Level = responseDialog.DifficultyLevel;
                    card.LastTimeUsed = DateTime.Now;
                }
                else
                {
                    // User closed the dialog without selecting a difficulty level
                    // Handle this case as needed (e.g., skip the card, set a default difficulty level, etc.)
                }
            }
        }

        private void StartStudySession_Click(object sender, RoutedEventArgs e)
        {
            StartStudySessionButton.IsEnabled = false;
            Process();
            _cards!.SaveToFile();
        }
    }
}
