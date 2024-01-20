using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Linq;
using CiteU.Modele;
using CiteUContext = CiteU.Modele.CiteU;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Data;
using System;
using System.Runtime.Remoting.Contexts;
using System.Windows.Media.Effects;

namespace CiteU.Vues
{
    public partial class MesChambres : UserControl, INotifyPropertyChanged
    {
        public ObservableCollection<Chambres> ListDeChambre
        {
            get { return (ObservableCollection<Chambres>)GetValue(ListDeChambreProperty); }
            set { SetValue(ListDeChambreProperty, value); }
        }

        public static readonly DependencyProperty ListDeChambreProperty =
            DependencyProperty.Register("ListDeChambre", typeof(ObservableCollection<Chambres>), typeof(MesChambres));

        private List<string> premieresLettres;

        public ICollectionView ChambresView { get; set; }

        public MesChambres()
        {
            InitializeComponent();
            LoadData();
            DataContext = this;
            CreateFilterButtons();
        }

        private void LoadData()
        {
            using (var context = new CiteUContext())
            {
                ListDeChambre = new ObservableCollection<Chambres>(context.Chambres.ToList());

                premieresLettres = ListDeChambre
                    .Select(chambre => chambre.Nom_Chambre.FirstOrDefault().ToString())
                    .Where(premiereLettre => !string.IsNullOrEmpty(premiereLettre))
                    .Distinct()
                    .OrderBy(letter => letter)
                    .ToList();
            }

            ChambresView = CollectionViewSource.GetDefaultView(ListDeChambre);
        }

        private void CreateFilterButtons()
        {
            // Ajouter le bouton "Tout"
            var toutButton = new Button
            {
                Content = "Tout",
                Margin = new Thickness(5),
                Width = 70,
                Height = 30
            };

            toutButton.Click += Tout_Click;
            MonStackPanel.Children.Add(toutButton);

            foreach (var lettre in premieresLettres)
            {
                var button = new Button
                {
                    Content = "Bâtiment " + lettre,
                    Margin = new Thickness(5),
                    Width = 70,
                    Height = 30
                };

                button.Click += (sender, e) => Lettre_Click(sender, e, lettre);
                MonStackPanel.Children.Add(button);
            }

            
        }

        private void Tout_Click(object sender, RoutedEventArgs e)
        {
            ChambresView.Filter = null;
        }

        private void Lettre_Click(object sender, RoutedEventArgs e,string lettreFiltre)
        {
           

            if (!string.IsNullOrEmpty(lettreFiltre))
            {
                ChambresView.Filter = c =>
                    ((Chambres)c).Nom_Chambre.StartsWith(lettreFiltre, StringComparison.OrdinalIgnoreCase);
                ChambresView.Refresh();
            }
        }

        private void InfoChambres(object sender, RoutedEventArgs e)
        {
            var selectedRoom = (Chambres)((Button)sender).DataContext;


            // Create a new instance of the RoomDetailsWindow and pass the selected room
            var roomDetailsWindow = new RoomDetailsWindow(selectedRoom);

            // Display the window as a dialog
            roomDetailsWindow.ShowDialog();

            
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public event PropertyChangedEventHandler PropertyChanged;
    }
}