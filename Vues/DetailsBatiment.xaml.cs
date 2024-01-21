using CiteU.Modele;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CiteUContext = CiteU.Modele.CiteU;

namespace CiteU.Vues
{
    /// <summary>
    /// Logique d'interaction pour DetailsBatiment.xaml
    /// </summary>
    public partial class DetailsBatiment : Window
    {
        private Batiments _batiment;
        public DetailsBatiment(Batiments batiments)
        {
            InitializeComponent();
            _batiment = batiments;

            // Définir le contexte de données sur le bâtiment fourni
            DataContext = _batiment;

            // Assurez-vous que la fenêtre est centrée sur l'écran principal
            WindowStartupLocation = WindowStartupLocation.CenterOwner;

        }

        private async void SupprimeChambre_Click(object sender, RoutedEventArgs e)
        {
            // Demander une confirmation à l'utilisateur
            MessageBoxResult result = MessageBox.Show("Voulez-vous vraiment supprimer ce bâtiment ?", "Confirmation de suppression", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                if (DataContext is Batiments batiment)
                {
                    using (var context = new CiteUContext())
                    {
                        var batimentToDelete = context.Batiments
                            .Include(b => b.Chambres.Select(c => c.Lits.Select(l => l.Reservations)))  // Charger les relations en cascade
                            .SingleOrDefault(b => b.ID_Batiment == batiment.ID_Batiment);

                        if (batimentToDelete != null)
                        {
                            // Supprimer les réservations
                            foreach (var chambre in batimentToDelete.Chambres)
                            {
                                foreach (var lit in chambre.Lits)
                                {
                                    context.Reservations.RemoveRange(lit.Reservations);
                                }
                            }

                            // Supprimer les lits et chambres
                            context.Lits.RemoveRange(batimentToDelete.Chambres.SelectMany(c => c.Lits));
                            context.Chambres.RemoveRange(batimentToDelete.Chambres);

                            // Supprimer le bâtiment
                            context.Batiments.Remove(batimentToDelete);

                            // Sauvegarder les modifications
                            await context.SaveChangesAsync();
                        }
                    }

                    // Fermer la fenêtre après la suppression
                    Close();
                }
            }
        }

        private void Enregistrer_Click(object sender, RoutedEventArgs e)
        {
            // Enregistrez les modifications du bâtiment dans la base de données
            using (var context = new CiteUContext())
            {
                // Mettez à jour le bâtiment dans la base de données
                var batimentToUpdate = context.Batiments.Find(_batiment.ID_Batiment);

                if (batimentToUpdate != null)
                {
                    // Mettez à jour les propriétés du bâtiment
                    batimentToUpdate.Adresse_Batiment = _batiment.Adresse_Batiment;
                    batimentToUpdate.Description_Batiment = _batiment.Description_Batiment;

                    // Enregistrez les modifications
                    context.SaveChanges();
                }
            }

            // Fermer la fenêtre après l'enregistrement
            Close();
        }
    }
}
