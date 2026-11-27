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
using System.Windows.Shapes;
using YSMemiCardsApp.Data;

namespace YSMemiCardsApp.Controls
{
    /// <summary>
    /// Interaction logic for StudyDialog.xaml
    /// </summary>
    public partial class StudyDialog : Window
    {
        private Cards? _cardsToStudy;
        private readonly Cards? _cards;

        public StudyDialog(Cards? cards)
        {
            InitializeComponent();

            _cards = cards;
            BuildListToStudy(cards);
        }
        private void BuildListToStudy(Cards? cards)
        {
            var configuration = Configuration.LoadFromFile();
            _cardsToStudy = new Cards();
            if (cards == null)
            {
                return;
            }

            foreach (var card in cards.CardsList)
            {
                // todo: add logic to determine which cards to study
                bool shouldStudyCard =
                    (card.LastTimeUsed == null)
                    || ((DateTime.Now - card.LastTimeUsed).Value.TotalDays > configuration!.FirstDays && card.Level < DifficultyLevel.Medium)
                    || ((DateTime.Now - card.LastTimeUsed).Value.TotalDays > configuration!.SecondDays && card.Level < DifficultyLevel.Easy)
                    || ((DateTime.Now - card.LastTimeUsed).Value.TotalDays > 10);
                if (shouldStudyCard)
                {
                    _cardsToStudy.CardsList.Add(card);
                }
            }

            this.TotalCardsTextBlock.Text = cards.CardsList.Count.ToString();
            this.CardsToStudyTextBlock.Text = _cardsToStudy.CardsList.Count.ToString();
            this.StartStudySessionButton.IsEnabled = _cardsToStudy.CardsList.Count > 0;
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
