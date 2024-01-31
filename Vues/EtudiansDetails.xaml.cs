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
using System.Collections.ObjectModel;

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
                ChambresTextBlock.Text = "Chambre réservée: ";
                Reservations reservation;

                using (var context = new CiteUContext())
                {
                    var etudiant = context.Reservations.Select(c => c.Etudiants_ID_Etudiant).ToList();

                    if (etudiant.Contains(selectedEtudiant.ID_Etudiant))
                    {
                        reservation = context.Reservations.FirstOrDefault(c => c.Etudiants_ID_Etudiant == selectedEtudiant.ID_Etudiant);
                        ChambresTextBlock.Text = "Chambre " + reservation.Lits.Chambres.Nom_Chambre;
                    }
                    else
                    {
                        ChambresTextBlock.Text = "Aucune chambre réservée";
                    }
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

                    var etudiant = newcontext.Reservations.Select(c => c.Etudiants_ID_Etudiant).ToList();

                    string etat = "Non payé";
                    if (etudiant.Contains(selectedEtudiant.ID_Etudiant))
                    {
                        MessageBoxResult result = MessageBox.Show("Cet etudiant a deja une chambre , voulez vous la changer ?", "Confirmation de suppression", MessageBoxButton.YesNo, MessageBoxImage.Question);

                        if (result == MessageBoxResult.No)
                            return;

                        var anciennereser= newcontext.Reservations.FirstOrDefault(c=>c.Etudiants_ID_Etudiant==selectedEtudiant.ID_Etudiant);
                        etat = anciennereser.Statut_Paiement;
                       
                       Lits lits = newcontext.Lits.FirstOrDefault(d => d.Reservations_ID_Reservation == anciennereser.ID_Reservation);
                        
                        Lits
                        lit = new Lits
                        {
                          ChambresID_Chambre = (int)anciennereser.ID_Chambre
                        };
                        newcontext.Lits.Add(lit);
                        newcontext.Lits.Remove(lits);
                        
                        newcontext.Reservations.Remove(anciennereser);
                        newcontext.SaveChanges();
                    
                    }

                    List<Chambres> ToutesChambre = litsNonReserves
                        .Select(l => newcontext.Chambres.FirstOrDefault(c => c.ID_Chambre == l.ChambresID_Chambre))
                        .Where(c => c != null)
                        .ToList();

                    List<Chambres> chambresAuRezDeChaussee = ToutesChambre
                        .Where(c => c.Etage == 0)
                        .ToList();

                    List<Chambres> chambresFille = ToutesChambre
                        .Where(c => c.Etage % 2 == 0 && c.Etage != 0)
                        .ToList();

                    List<Chambres> chambresGarcon = ToutesChambre
                        .Where(c => c.Etage % 2 == 1)
                        .ToList();

                    Chambres chambreAttribuee = new Chambres();
                    Lits litAttribue = new Lits();


                    if (selectedEtudiant.Handicape == 1)
                    {
                        if (chambresAuRezDeChaussee.Count <= 0)
                        {
                            MessageBox.Show("Aucune chambre pour handicape n'est disponible", "ERREUR", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        chambreAttribuee = chambresAuRezDeChaussee.First();


                    }
                    else if (selectedEtudiant.Sexe == "F" && selectedEtudiant.Handicape==0)
                    {
                        if (chambresFille.Count <= 0)
                        {
                            MessageBox.Show("Aucune chambre pour Fille n'est disponible", "ERREUR", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        chambreAttribuee = chambresFille[0];
                    }
                    else if (selectedEtudiant.Sexe == "M" && selectedEtudiant.Handicape == 0)
                    {
                        if (chambresGarcon.Count <= 0)
                        {
                            MessageBox.Show("Aucune chambre pour garcon n'est disponible", "ERREUR", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;

                        }
                        chambreAttribuee = chambresGarcon[0];
                    }


                    litAttribue = chambreAttribuee.Lits.FirstOrDefault(c => c.Reservations_ID_Reservation == null);
                    Reservations reservations = new Reservations
                    {
                        Etudiants_ID_Etudiant = selectedEtudiant.ID_Etudiant,
                        ID_Chambre = chambreAttribuee.ID_Chambre,
                        Date_Debut = DateTime.Now,
                        Date_Fin = DateTime.Now.AddMonths(12),
                        Statut_Paiement = etat
                    };
                    reservations.Lits = litAttribue;
                    reservations.Lits.Chambres.Statut = "Occupee";

                    newcontext.Reservations.Add(reservations);
                    newcontext.SaveChanges();

                    selectedEtudiant.Reservations_ID_Reservation = reservations.ID_Reservation;
                    litAttribue.Reservations_ID_Reservation = reservations.ID_Reservation;

                    newcontext.SaveChanges();
                    MessageBox.Show($"L'étudiant {selectedEtudiant.Nom} a été attribué à la chambre {chambreAttribuee.Nom_Chambre} pour 1an. Il doit maintenant la payer.", "Attribution chambre", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (DbUpdateException dbUpdateEx)
            {
                // Logguer l'exception ou afficher un message plus détaillé si nécessaire
                MessageBox.Show("Erreur lors de la mise à jour de la base de données. Veuillez vérifier les contraintes de clé étrangère.\n" + dbUpdateEx.Message, "ERREUR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                // Logguer l'exception ou afficher un message plus détaillé si nécessaire
                MessageBox.Show($"Une erreur s'est produite : {ex.Message}", "ERREUR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Attribution_Click(object sender, RoutedEventArgs e)
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

                    var etudiant = newcontext.Reservations.Select(c => c.Etudiants_ID_Etudiant).ToList();

                    string etat = "Non payé";
                    if (etudiant.Contains(selectedEtudiant.ID_Etudiant))
                    {
                        MessageBoxResult result = MessageBox.Show("Cet etudiant a deja une chambre , voulez vous la changer ?", "Confirmation de suppression", MessageBoxButton.YesNo, MessageBoxImage.Question);

                        if (result == MessageBoxResult.No)
                            return;
                        
                        var anciennereser = newcontext.Reservations.FirstOrDefault(c => c.Etudiants_ID_Etudiant == selectedEtudiant.ID_Etudiant);
                        etat = anciennereser.Statut_Paiement;
                        var mon= newcontext.Lits.FirstOrDefault(d => d.Reservations_ID_Reservation == anciennereser.ID_Reservation);
                        mon.Reservations_ID_Reservation = null;
                        // Marquer la propriété comme modifiée
                        newcontext.Entry(mon).Property(x => x.Reservations_ID_Reservation).IsModified = true;

                        newcontext.Reservations.Remove(anciennereser);
                        newcontext.SaveChanges();

                    }

                    var chambresDisponibles = litsNonReserves
                        .Select(l => l.Chambres)
                        .Where(c => selectedEtudiant.Handicape == 1 ? c.Etage == 0 : (selectedEtudiant.Sexe == "F" ? c.Etage % 2 != 0 && c.Etage != 0 : c.Etage % 2 == 1 && c.Etage != 0))
                        .ToList();

                    if (chambresDisponibles.Count == 0)
                    {
                        MessageBox.Show("Aucune chambre disponible pour ce type d'étudiant.", "ERREUR", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    var choixChambreWindow = new ChoixChambreWindow(chambresDisponibles);
                    if (choixChambreWindow.ShowDialog() == true)
                    {
                        var chambreChoisie = choixChambreWindow.ChambreChoisie;

                        using (var context = new CiteUContext())
                        {
                            try
                            {
                                var litAttribue= context.Lits.FirstOrDefault(c=>c.ChambresID_Chambre==chambreChoisie.ID_Chambre && c.Reservations_ID_Reservation==null );
                                var nouvelleReservation = new Reservations
                                {
                                    Etudiants_ID_Etudiant = selectedEtudiant.ID_Etudiant,
                                    Lits_id =litAttribue.id,
                                    ID_Chambre = chambreChoisie.ID_Chambre,
                                    Date_Debut = DateTime.Now,
                                    Date_Fin = DateTime.Now.AddMonths(12),
                                    Statut_Paiement = "Non payé"
                                };
                                newcontext.Reservations.Add(nouvelleReservation);
                                
                                selectedEtudiant.Reservations_ID_Reservation = nouvelleReservation.ID_Reservation;
                                nouvelleReservation.Lits.Chambres.Statut = "Occupee";

                                newcontext.SaveChanges();

                               MesPayements payements = new MesPayements();

                                MessageBox.Show($"L'étudiant {selectedEtudiant.Nom} a été attribué à la chambre {chambreChoisie.Nom_Chambre} pour 1an", "Attribution de chambre", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                            catch (Exception ex)
                            {
                                
                                MessageBox.Show("Erreur lors de l'attribution de la chambre. Veuillez réessayer.\n" + ex.Message, "ERREUR", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
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


