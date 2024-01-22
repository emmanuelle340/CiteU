using CiteU.Modele;
using System.Collections.Generic;
using System.Windows;

namespace CiteU.Vues
{
    public partial class ChoixChambreWindow : Window
    {
        public Chambres ChambreChoisie { get; private set; }

        public ChoixChambreWindow(List<Chambres> chambresDisponibles)
        {
            InitializeComponent();

            // Remplir la ComboBox avec les chambres disponibles
            ChambresComboBox.ItemsSource = chambresDisponibles;
        }

        private void Valider_Click(object sender, RoutedEventArgs e)
        {
            // Récupérer la chambre sélectionnée
            ChambreChoisie = ChambresComboBox.SelectedItem as Chambres;

            // Vérifier si une chambre a été sélectionnée
            if (ChambreChoisie == null)
            {
                MessageBox.Show("Veuillez choisir une chambre.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                // Fermer la fenêtre avec le résultat "true" pour indiquer que l'utilisateur a validé son choix
                DialogResult = true;
            }
        }
    }
}
