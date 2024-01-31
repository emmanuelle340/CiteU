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
            using (var context = new CiteUContext())

                LoadData();
            // Utilisez le gestionnaire d'événements CollectionChanged pour détecter les modifications dans la liste de chambres
            ListDeChambre.CollectionChanged += (s, e) => OnListDeChambreChanged();
            // Mettre à jour la vue après le chargement des données
            ChambresView = CollectionViewSource.GetDefaultView(ListDeChambre);
            OnPropertyChanged(nameof(ListDeChambre));
            OnPropertyChanged(nameof(ChambresView));

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
            // Mettre à jour la vue après le chargement des données
            ChambresView = CollectionViewSource.GetDefaultView(ListDeChambre);
            OnPropertyChanged(nameof(ListDeChambre));
            OnPropertyChanged(nameof(ChambresView));
        }

        private void CreateFilterButtons()
        {
            // Mettre à jour la vue après le chargement des données
            ChambresView = CollectionViewSource.GetDefaultView(ListDeChambre);
            OnPropertyChanged(nameof(ListDeChambre));
            OnPropertyChanged(nameof(ChambresView));
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

        

        private void Lettre_Click(object sender, RoutedEventArgs e, string lettreFiltre)
        {
            LoadData();
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
            LoadData();

        }

        private void OnListDeChambreChanged()
        {
            // Mettez à jour la vue des chambres
            ChambresView?.Refresh();
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public event PropertyChangedEventHandler PropertyChanged;

        private void Rafraichir_Click_1(object sender, RoutedEventArgs e)
        {
            // Charger les données dans la liste de chambres existante
            LoadData();

            // Mettre à jour la vue des chambres
            ChambresView.Refresh();

            premieresLettres = new List<string>();

            // Supprimer les anciens boutons de filtre
            MonStackPanel.Children.Clear();
            // Recréer les boutons de filtre
            premieresLettres = ListDeChambre
                .Select(chambre => chambre.Nom_Chambre.FirstOrDefault().ToString())
                .Where(premiereLettre => !string.IsNullOrEmpty(premiereLettre))
                .Distinct()
                .OrderBy(letter => letter)
                .ToList();

            CreateFilterButtons();

            // Notifier le changement dans la propriété premieresLettres
            OnPropertyChanged(nameof(premieresLettres));
        }


    }
}