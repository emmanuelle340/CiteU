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

        private void SupprimeChambre_Click(object sender, RoutedEventArgs e)
        {
            // Demander une confirmation à l'utilisateur
            MessageBoxResult result = MessageBox.Show("Voulez-vous vraiment supprimer ce bâtiment ?", "Confirmation de suppression", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                // Assurez-vous que DataContext est un Batiments valide
                if (DataContext is Batiments batiment)
                {
                    // Utilisez un contexte EF pour supprimer le bâtiment de la base de données
                    using (var context = new CiteUContext())
                    {
                        // Chargez le bâtiment avec ses chambres associées depuis la base de données
                        var batimentToDelete = context.Batiments
                            .Include(b => b.Chambres)
                            .Single(b => b.ID_Batiment == batiment.ID_Batiment);

                        // Chargez les lits associés à toutes les chambres du bâtiment
                        context.Entry(batimentToDelete)
                            .Collection(b => b.Chambres)
                            .Query()
                            .Include(c => c.Lits)
                            .Load();

                        // Supprimez tous les lits associés aux chambres du bâtiment
                        foreach (var chambre in batimentToDelete.Chambres)
                        {
                            context.Lits.RemoveRange(chambre.Lits);
                        }

                        // Supprimez toutes les chambres associées au bâtiment
                        context.Chambres.RemoveRange(batimentToDelete.Chambres);

                        // Supprimez le bâtiment (ce qui supprimera également toutes les chambres)
                        context.Batiments.Remove(batimentToDelete);

                        // Enregistrez les modifications dans la base de données
                        context.SaveChanges();

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
