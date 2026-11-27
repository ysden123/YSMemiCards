using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using YSMemiCardsApp.Data;

namespace YSMemiCardsApp.Controls
{
    /// <summary>
    /// Interaction logic for CardListDialog.xaml
    /// </summary>
    public partial class CardListDialog : Window
    {
        private readonly ObservableCollection<Card> _cardsViewList;
        private readonly ICollectionView _cardListView;
        private bool _changed = false;

        public CardListDialog(Cards cards)
        {
            InitializeComponent();

            _cardsViewList = new ObservableCollection<Card>(cards.CardsList);
            cardListView.ItemsSource = _cardsViewList;
            _cardListView = CollectionViewSource.GetDefaultView(_cardsViewList);
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var card = new Card()
            {
                CreatedAt = DateTime.Now,
            };

            var dialog = new CardEditDialog(card)
            {
                Owner = Window.GetWindow(this)
            };

            var result = dialog.ShowDialog();
            if (result == true)
            {
                _cardsViewList.Add(card);
                _cardListView.Refresh();
                _changed = true;
                ChangeButtonAvalability();
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var card = cardListView.SelectedItem as Card;
            if (card != null)
            {
                var dialog = new CardEditDialog(card)
                {
                    Owner = Window.GetWindow(this)
                };

                var result = dialog.ShowDialog();
                if (result == true)
                {
                    _cardListView.Refresh();
                    _changed = true;
                    ChangeButtonAvalability();
                }
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var card = cardListView.SelectedItem as Card;
            if (card != null)
            {
                if (_cardsViewList.Remove(card))
                {
                    _cardListView.Refresh();
                    _changed = true;
                    ChangeButtonAvalability();
                }
            }
        }

        private void SaveButton_Click(object? sender, RoutedEventArgs e)
        {
            SaveChanges();
        }

        private void SaveChanges()
        {
            var cards = new Cards { CardsList = new List<Card>([.. _cardsViewList]) };
            cards.SaveToFile();
            _changed = false;
            _cardListView.Refresh();
            ChangeButtonAvalability();
        }

        private void CardListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ChangeButtonAvalability();
        }

        private void ChangeButtonAvalability()
        {
            ButtonSave.IsEnabled = _changed;
            if (cardListView.SelectedItem == null)
            {
                ButtonEdit.IsEnabled = false;
                ButtonDelete.IsEnabled = false;
            }
            else
            {
                ButtonEdit.IsEnabled = true;
                ButtonDelete.IsEnabled = true;
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (_changed)
            {
                var result = MessageBox.Show("You have unsaved changes. Do you want to save them before closing?", "Unsaved Changes", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    SaveChanges();
                    e.Cancel = false;
                }
                else if (result == MessageBoxResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
            else
                e.Cancel = false;
        }
    }
}
