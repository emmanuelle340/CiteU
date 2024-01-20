using CiteU.Modele;
using CiteU;
using CiteUContext = CiteU.Modele.CiteU;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Effects;

namespace CiteU.Vues
{
    public partial class Mesbatiments : UserControl
    {
        public ObservableCollection<Batiments> ListOfBatiments { get; set; }

        public Mesbatiments()
        {
            InitializeComponent();

            // Initialiser la liste des bâtiments lors de la création de la classe
            ListOfBatiments = new ObservableCollection<Batiments>();
            ListOfBatiments.CollectionChanged += (s, e) => { /* Mettre à jour l'interface utilisateur ici */ };
            
            UpdateListOfBatiments();

            DataContext = this;
        }

        // Mettre à jour la liste des bâtiments à partir de la base de données
        private void UpdateListOfBatiments()
        {
            using (var context = new CiteUContext())
            {
                ListOfBatiments.Clear();
                foreach (var batiment in context.Batiments.ToList())
                {
                    ListOfBatiments.Add(batiment);
                }
            }
        }

        private void AjouterBatiment_Click(object sender, RoutedEventArgs e)
        {
            // Accéder à la fenêtre principale
            Window mainWindow = Window.GetWindow(this);

            if (mainWindow != null)
            {
                // Appliquer un effet de flou à la fenêtre principale
                BlurEffect blurEffect = new BlurEffect { Radius = 5 };
                mainWindow.Effect = blurEffect;

                var dialog = new AjouterBatimentDialog();

                // Définir la fenêtre principale comme propriétaire de la boîte de dialogue
                dialog.Owner = mainWindow;

                // Centrer la boîte de dialogue par rapport à la fenêtre principale
                dialog.Left = mainWindow.Left + (mainWindow.Width - dialog.Width) / 2;
                dialog.Top = mainWindow.Top + (mainWindow.Height - dialog.Height) / 2;

                // Gérer la fermeture de la boîte de dialogue
                dialog.Closed += (s, args) =>
                {
                    // Restaurer la fenêtre principale sans flou
                    mainWindow.Effect = null;

                    // Mettre à jour la liste des bâtiments après la fermeture de la boîte de dialogue
                    UpdateListOfBatiments();
                    MesChambres mesChambres = new MesChambres();
                };

                // Afficher la boîte de dialogue
                dialog.ShowDialog();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Obtenez le bâtiment sélectionné depuis la source de données
            Batiments monbatiment = ((Button)sender).DataContext as Batiments;

            // Créer et afficher la fenêtre de détails du bâtiment
            DetailsBatiment detailsWindow = new DetailsBatiment(monbatiment);

            // Obtenez la fenêtre principale
            Window mainWindow = Window.GetWindow(this);

            // Centrer la fenêtre de détails par rapport à la fenêtre principale
            detailsWindow.Left = mainWindow.Left + (mainWindow.Width - detailsWindow.Width) / 2;
            detailsWindow.Top = mainWindow.Top + (mainWindow.Height - detailsWindow.Height) / 2;

            // Appliquer un effet de flou à la fenêtre principale
            var blurEffect = new BlurEffect { Radius = 8 };
            mainWindow.Effect = blurEffect;

            // Gérer la fermeture de la fenêtre de détails
            detailsWindow.Closed += (s, args) =>
            {
                // Supprimer l'effet de flou après la fermeture de la fenêtre de détails
                mainWindow.Effect = null;

                // Mettre à jour la liste des bâtiments après la fermeture de la fenêtre de détails
                UpdateListOfBatiments();
                MesChambres mesChambres = new MesChambres();
            };

            // Afficher la fenêtre de détails du bâtiment
            detailsWindow.ShowDialog();
        }

        private void SupprimerBatiment_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Batiments batimentToDelete)
            {
                MessageBoxResult result = MessageBox.Show("Voulez-vous vraiment supprimer ce bâtiment ?", "Confirmation de suppression", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    using (var context = new CiteUContext())
                    {
                        // Supprimer le bâtiment de la base de données
                        context.Batiments.Remove(batimentToDelete);
                        context.SaveChanges();
                    }

                    // Mettre à jour la liste des bâtiments après la suppression
                    UpdateListOfBatiments();
                    MesChambres mesChambres = new MesChambres();
                }
            }
        }
    }
}