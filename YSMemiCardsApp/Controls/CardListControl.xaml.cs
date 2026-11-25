using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using YSMemiCardsApp.Data;

namespace YSMemiCardsApp.Controls
{
    /// <summary>
    /// Interaction logic for CardListControl.xaml
    /// </summary>
    public partial class CardListControl : UserControl
    {
        private readonly ObservableCollection<Card> _cardsViewList;
        private readonly ICollectionView _cardListView;
        private bool _changed = false;
        private Window? _parentWindow;

        public CardListControl(Cards cards)
        {
            InitializeComponent();
            _cardsViewList = new ObservableCollection<Card>(cards.CardsList);
            cardListView.ItemsSource = _cardsViewList;
            _cardListView = CollectionViewSource.GetDefaultView(_cardsViewList);

            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            _parentWindow = Window.GetWindow(this);
            if (_parentWindow is not null)
            {
                _parentWindow.Closing += ParentWindow_Closing;
            }
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            if (_parentWindow is not null)
            {
                _parentWindow.Closing -= ParentWindow_Closing;
                _parentWindow = null;
            }
        }

        private void ParentWindow_Closing(object? sender, CancelEventArgs e)
        {
            if (_changed)
            {
                var result = MessageBox.Show(
                    "Not saved changes exist. Save changes?",
                    "Attention",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    var cards = new Cards { CardsList = new List<Card>([.. _cardsViewList]) };
                    cards.SaveToFile();
                }
            }
            _changed = false;
            e.Cancel = false;
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

        private void SaveButton_Click(object sender, RoutedEventArgs e)
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
    }
}
