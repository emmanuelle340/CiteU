using CiteU.Modele;
using CiteU.Vues;
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
using System.Runtime.Remoting.Contexts;
using System.Data.Entity.Infrastructure;

namespace CiteU.Vues
{
    public partial class EtudiansDetails : Window
    {
        private Etudiants selectedEtudiant;

        public EtudiansDetails(Etudiants etudiant)

        {
            InitializeComponent();
            selectedEtudiant = etudiant;

            DataContext = selectedEtudiant;
            
            DisplayStudentDetails();
        }

        private void DisplayStudentDetails()
        {
            Etudiants selectedEtudiant = DataContext as Etudiants;

            // Afficher le sexe
            if (selectedEtudiant.Sexe == "F")
            {
                SexeTextBlock.Text = "Sexe: Féminin";
            }
            else
            {
                SexeTextBlock.Text = "Sexe: Masculin";
            }

            // Afficher le statut handicapé
            if (selectedEtudiant.Handicape == 1)
            {
                HandicapTextBlock.Text = "Handicap: Oui";
            }
            else
            {
                HandicapTextBlock.Text = "Handicap: Non";
            }

            // Afficher les chambres réservées
            try
            {
                if (selectedEtudiant.Reservations_ID_Reservation != null)
                {
                    ChambresTextBlock.Text = "Chambre réservée: ";
                    Reservations reservation;

                    using (var context = new CiteUContext())
                    {
                        reservation = context.Reservations.FirstOrDefault(a => a.ID_Reservation == selectedEtudiant.Reservations_ID_Reservation);

                        if (reservation != null && reservation.Lits != null && reservation.Lits.Chambres != null)
                        {
                            ChambresTextBlock.Text = "Chambre " + reservation.Lits.Chambres.Nom_Chambre;
                        }
                        else
                        {
                            ChambresTextBlock.Text = "Informations sur la chambre non disponibles";
                        }
                    }
                }
                else
                {
                    ChambresTextBlock.Text = "Aucune chambre réservée";
                }
            }
            catch (Exception ex)
            {
                // Gérer l'exception ici (affichage d'un message, journalisation, etc.)
                ChambresTextBlock.Text = "Erreur lors de l'affichage des informations sur la chambre.";
                MessageBox.Show($"Une erreur s'est produite : {ex.Message}", "ERREUR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Attribution_Aleatoire_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var newcontext = new CiteUContext())
                {
                    Etudiants selectedEtudiant = DataContext as Etudiants;

                    var litsNonReserves = newcontext.Lits
                        .Where(lit => lit.Reservations_ID_Reservation == null)
                        .ToList();

                    if (litsNonReserves == null || litsNonReserves.Count == 0)
                    {
                        MessageBox.Show("Il n'y a plus de place dans la CiteU", "ERREUR", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    if (selectedEtudiant.Reservations_ID_Reservation != null)
                    {
                        MessageBox.Show("Cet étudiant possède déjà une chambre", "ERREUR", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    List<Chambres> ToutesChambre = litsNonReserves
                        .Select(l => newcontext.Chambres.FirstOrDefault(c => c.ID_Chambre == l.ChambresID_Chambre))
                        .ToList();

                    List<Chambres> chambresAuRezDeChaussee = ToutesChambre
                        .Where(c => c.Etage == 0)
                        .ToList();

                    if (selectedEtudiant.Handicape == 1 && chambresAuRezDeChaussee.Count == 0)
                    {
                        MessageBoxResult result = MessageBox.Show("Aucune chambre au rez-de-chaussée n'est disponible. Voulez-vous attribuer une chambre non au rez-de-chaussée à une personne handicapée ?", "Attribution de chambre", MessageBoxButton.YesNo, MessageBoxImage.Question);

                        if (result == MessageBoxResult.No) return;

                        chambresAuRezDeChaussee = ToutesChambre
                            .Where(c => c.Etage == 1)
                            .ToList();

                        if (selectedEtudiant.Sexe == "F")
                        {
                            chambresAuRezDeChaussee = chambresAuRezDeChaussee
                                .Where(c => c.Etage % 2 != 0 && c.Etage != 0)
                                .ToList();
                        }
                        else if (selectedEtudiant.Sexe == "M")
                        {
                            chambresAuRezDeChaussee = chambresAuRezDeChaussee
                                .Where(c => c.Etage % 2 != 1)
                                .ToList();
                        }

                        if (chambresAuRezDeChaussee.Count == 0)
                        {
                            MessageBox.Show("Il n'y a plus de place au 1er étage. On ne peut pas mettre un handicapé à d'autres étages que le rez-de-chaussée ou le 1er étage", "ERREUR", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                    }

                    Chambres chambreAttribuee = chambresAuRezDeChaussee.FirstOrDefault();
                    Lits litAttribue = chambreAttribuee?.Lits.FirstOrDefault(d => d.Reservations_ID_Reservation == null);

                    if (litAttribue == null)
                    {
                        MessageBox.Show("Erreur lors de l'attribution de la chambre. Veuillez réessayer.", "ERREUR", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    Reservations reservations = new Reservations
                    {
                        ID_Etudiant = selectedEtudiant.ID_Etudiant,
                        ID_Chambre = chambreAttribuee.ID_Chambre,
                        Date_Debut = DateTime.Now,
                        Date_Fin = DateTime.Now.AddMonths(6),
                        Statut_Paiement = "Non payé"
                    };
                    reservations.Lits = litAttribue;

                    newcontext.Reservations.Add(reservations);
                    newcontext.SaveChanges();

                    selectedEtudiant.Reservations_ID_Reservation = reservations.ID_Reservation;
                    litAttribue.Reservations_ID_Reservation = reservations.ID_Reservation;

                    newcontext.SaveChanges();

                    MessageBox.Show($"L'étudiant {selectedEtudiant.Nom} a été attribué à la chambre {chambreAttribuee.Nom_Chambre} pour 6 mois. Il doit maintenant la payer.", "Attribution chambre", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (DbUpdateException dbUpdateEx)
            {
                MessageBox.Show("Erreur lors de la mise à jour de la base de données. Veuillez vérifier les contraintes de clé étrangère.", "ERREUR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur s'est produite : {ex.Message}", "ERREUR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Attribution_Click(object sender, RoutedEventArgs e)
        {
            try { 
            using (var newcontext = new CiteUContext())
            {
                Etudiants selectedEtudiant = DataContext as Etudiants;

                // Vérifier si l'étudiant a déjà une chambre
                if (selectedEtudiant.Reservations_ID_Reservation != null)
                {
                    MessageBox.Show("Cet étudiant possède déjà une chambre", "ERREUR", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Récupérer les lits non réservés
                var litsNonReserves = newcontext.Lits
                    .Where(lit => lit.Reservations_ID_Reservation == null)
                    .ToList();

                if (litsNonReserves == null || litsNonReserves.Count == 0)
                {
                    MessageBox.Show("Il n'y a plus de place dans la CiteU", "ERREUR", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Afficher une liste des chambres avec au moins un lit vide
                var chambresDisponibles = litsNonReserves
                    .Select(l => l.Chambres)
                    .Distinct()
                    .ToList();

                if (chambresDisponibles.Count == 0)
                {
                    MessageBox.Show("Aucune chambre avec un lit disponible n'est disponible.", "ERREUR", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Filtrer les chambres en fonction du type d'étudiant
                if (selectedEtudiant.Handicape == 1)
                {
                    // Afficher uniquement les chambres du rez-de-chaussée pour un étudiant handicapé
                    chambresDisponibles = chambresDisponibles
                        .Where(c => c.Etage == 0)
                        .ToList();
                }
                else if (selectedEtudiant.Sexe == "F")
                {
                    // Afficher uniquement les chambres des étages impairs pour une fille
                    chambresDisponibles = chambresDisponibles
                        .Where(c => c.Etage % 2 != 0 && c.Etage != 0)
                        .ToList();
                }
                else if (selectedEtudiant.Sexe == "M")
                {
                    // Afficher uniquement les chambres des étages pairs pour un garçon
                    chambresDisponibles = chambresDisponibles
                        .Where(c => c.Etage % 2 == 0 && c.Etage != 0)
                        .ToList();
                }

                if (chambresDisponibles.Count == 0)
                {
                    MessageBox.Show("Aucune chambre disponible pour ce type d'étudiant.", "ERREUR", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Afficher une fenêtre pour que l'utilisateur choisisse une chambre
                var choixChambreWindow = new ChoixChambreWindow(chambresDisponibles);
                if (choixChambreWindow.ShowDialog() == true)
                {
                    // L'utilisateur a fait un choix
                    Chambres chambreChoisie = choixChambreWindow.ChambreChoisie;

                    // Créer une nouvelle réservation et l'associer à l'étudiant
                    Reservations nouvelleReservation = new Reservations
                    {
                        ID_Etudiant = selectedEtudiant.ID_Etudiant,
                        ID_Chambre = chambreChoisie.ID_Chambre,
                        Date_Debut = DateTime.Now,
                        Date_Fin = DateTime.Now.AddMonths(6),
                        Statut_Paiement = "Non payé"
                    };

                    // Mettre à jour la base de données avec la nouvelle réservation
                    newcontext.Reservations.Add(nouvelleReservation);

                    newcontext.SaveChanges();

                    // Mettre à jour la propriété Reservation_ID_Reservation de l'étudiant
                    selectedEtudiant.Reservations_ID_Reservation = nouvelleReservation.ID_Reservation;
                    newcontext.SaveChanges();

                    // Mettre à jour la propriété ID_Reservations du lit
                    var litAttribue = litsNonReserves.FirstOrDefault(lit => lit.Chambres.ID_Chambre == chambreChoisie.ID_Chambre);
                    if (litAttribue != null)
                    {
                        litAttribue.Reservations_ID_Reservation = nouvelleReservation.ID_Reservation;
                        newcontext.SaveChanges();
                    }

                    MessageBox.Show("L'étudiant " + selectedEtudiant.Nom + " a été attribué à la chambre " + chambreChoisie.Nom_Chambre, "Attribution de chambre", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }
    catch (Exception ex)
    {
        MessageBox.Show("Une erreur s'est produite : " + ex.Message, "ERREUR", MessageBoxButton.OK, MessageBoxImage.Error);
    }
}

        private void Supprimer_Click(object sender, RoutedEventArgs e)
        {
            // Demander une confirmation avant de supprimer
            MessageBoxResult result = MessageBox.Show("Êtes-vous sûr de vouloir supprimer cet étudiant ?", "Confirmation de suppression", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                // Utilisateur a confirmé, ajoutez le code pour supprimer l'étudiant de la base de données
                using (var context = new CiteUContext())
                {
                    Etudiants selectedEtudiant = DataContext as Etudiants;

                    try
                    {
                       
                            Etudiants etudiantToDelete = context.Etudiants.Find(selectedEtudiant.ID_Etudiant);

                            if (etudiantToDelete != null)
                            {
                                context.Etudiants.Remove(etudiantToDelete);
                                context.SaveChanges();
                            MessageBox.Show("L'étudiant a été supprimé avec succès.", "Suppression", MessageBoxButton.OK, MessageBoxImage.Information);

                        }



                        // Fermer la fenêtre après la suppression
                        this.Close();
                    }
                    catch (Exception ex)
                    {
                        // Gérer les erreurs, afficher un message approprié
                        MessageBox.Show("Erreur lors de la suppression de l'étudiant : " + ex.Message, "Erreur de suppression", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            // Si l'utilisateur clique sur "Non", ne faites rien
        }


    }
}


