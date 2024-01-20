using CiteU.Modele;
using System;
using System.Collections.Generic;
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
using System.Windows;
using CiteUContext = CiteU.Modele.CiteU;
using System.Data.Entity;

namespace CiteU.Vues
{
    public partial class EtudiansDetails : Window
    {
        public EtudiansDetails(Etudiants selectedEtudiant)
        {
            InitializeComponent();

            // Assurez-vous d'ajuster ces propriétés en fonction de votre modèle d'étudiant
            DataContext = selectedEtudiant;
            // Rendre la fenêtre non déplaçable par la souris
            
            
            // Enlever le focus de la fenêtre
            //Keyboard.ClearFocus();
            // Enlever le focus de la souris
            //Mouse.Capture(null);
            
        }

        private void Attribution_Aleatoire_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new CiteUContext())
            {
                Etudiants selectedEtudiant = DataContext as Etudiants;

                var litsNonReserves = context.Lits
                    .Where(lit => lit.Reservations_ID_Reservation == null)
                    .ToList();

                if (litsNonReserves == null || litsNonReserves.Count == 0)
                {
                    MessageBox.Show("Il n'y a plus de place dans la CiteU", "ERREUR", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                List<Chambres> ToutesChambre = litsNonReserves
                    .Select(l => context.Chambres.FirstOrDefault(c => c.ID_Chambre == l.ChambresID_Chambre))
                    .ToList();

                if (selectedEtudiant.Handicape == 1)
                {
                    List<Chambres> chambresAuRezDeChaussee = ToutesChambre
                        .Where(c => c.Etage == 0)
                        .ToList();

                    if (chambresAuRezDeChaussee.Count == 0)
                    {
                        // Aucune chambre au rez-de-chaussée n'est disponible

                        MessageBoxResult result = MessageBox.Show("Aucune chambre au rez-de-chaussée n'est disponible. Voulez-vous attribuer une chambre non au rez-de-chaussée à une personne handicapée ?", "Attribution de chambre", MessageBoxButton.YesNo, MessageBoxImage.Question);

                        if (result == MessageBoxResult.Yes)
                        {
                            // L'utilisateur a cliqué sur "Oui", attribuer n'importe quelle chambre
                            // à l'étage suivant (modifier la logique selon vos besoins)
                            List<Chambres> chambresAuNiveauSuivant = ToutesChambre
                                .Where(c => c.Etage == 1) // Modifier selon la logique nécessaire
                                .ToList();

                            if (chambresAuNiveauSuivant.Count > 0)
                            {
                                // Faire quelque chose avec la chambre attribuée (par exemple, affecter à selectedEtudiant)
                                Chambres chambreAttribuee = chambresAuNiveauSuivant.First();
                                // ... faire quelque chose avec la chambre attribuée ...
                            }
                            else
                            {
                                MessageBox.Show("Il n'y a plus de place handicape dans la CiteU", "ERREUR", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    }
                    else
                    {
                        // Au moins une chambre au rez-de-chaussée est disponible
                        // Faire quelque chose avec la chambre attribuée (par exemple, affecter à selectedEtudiant)
                        Chambres chambreAttribuee = chambresAuRezDeChaussee.First();
                        
                    }
                }
            }
        }

    }

}


