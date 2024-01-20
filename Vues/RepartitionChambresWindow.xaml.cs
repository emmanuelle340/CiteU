// RepartitionChambresWindow.xaml.cs
using System;
using System.Windows;

namespace CiteU.Vues
{
    public partial class RepartitionChambresWindow : Window
    {
        public int CapaciteChambre1 { get; set; }
        public int CapaciteChambre2 { get; set; }

        public RepartitionChambresWindow(int capaciteChambre1, int capaciteChambre2)
        {
            InitializeComponent();

            CapaciteChambre1 = capaciteChambre1;
            CapaciteChambre2 = capaciteChambre2;

            DataContext = this;
        }

        private void Enregistrer_Click(object sender, RoutedEventArgs e)
        {
            // Récupérer les valeurs des TextBox
            if (!int.TryParse(txtCapaciteChambre1.Text, out int nouvelleCapacite1) || !int.TryParse(txtCapaciteChambre2.Text, out int nouvelleCapacite2))
            {
                MessageBox.Show("Veuillez entrer des valeurs numériques valides.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Vérifier que la somme des capacités reste égale à la capacité d'origine
            if (nouvelleCapacite1 + nouvelleCapacite2 != CapaciteChambre1 + CapaciteChambre2)
            {
                MessageBox.Show("La somme des capacités des chambres doit être égale à la capacité d'origine.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Mettre à jour les propriétés
            CapaciteChambre1 = nouvelleCapacite1;
            CapaciteChambre2 = nouvelleCapacite2;

            DialogResult = true;
        }
    }
}
