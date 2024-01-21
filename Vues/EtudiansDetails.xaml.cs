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
                if (selectedEtudiant.Reservations!=null)
                {
                    MessageBox.Show("Cet etudiant possede deja une chambre", "ERREUR", MessageBoxButton.OK, MessageBoxImage.Error);
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

                    if (chambresAuRezDeChaussee.Count <= 0)
                    {
                        // Aucune chambre au rez-de-chaussée n'est disponible

                        MessageBoxResult result = MessageBox.Show("Aucune chambre au rez-de-chaussée n'est disponible. Voulez-vous attribuer une chambre non au rez-de-chaussée à une personne handicapée ?", "Attribution de chambre", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (result == MessageBoxResult.No) return;
                        if (result == MessageBoxResult.Yes)
                        {
                            // L'utilisateur a cliqué sur "Oui", attribuer n'importe quelle chambre
                            // à l'étage suivant (modifier la logique selon vos besoins)
                            List<Chambres> chambresAuNiveauSuivant = ToutesChambre
                                .Where(c => c.Etage == 1) // Modifier selon la logique nécessaire
                                .ToList();

                            if (selectedEtudiant.Sexe == "F")
                            {
                                chambresAuNiveauSuivant = chambresAuNiveauSuivant
                                    .Where(c => c.Etage % 2 != 0 && c.Etage != 0) // Filles aux étages pairs
                                    .ToList();
                            }
                            else if (selectedEtudiant.Sexe == "M")
                            {
                                chambresAuNiveauSuivant = chambresAuNiveauSuivant
                                    .Where(c => c.Etage % 2 != 1) // Garçons aux étages impairs
                                    .ToList();
                            }

                            if (chambresAuNiveauSuivant.Count > 0)
                            {
                                
                                Chambres chambreAttribuee = chambresAuNiveauSuivant.First();
                                Lits litAttribue=chambreAttribuee.Lits.First(d => d.Reservations_ID_Reservation==null);
                                Reservations reservations = new Reservations
                                {
                                    ID_Etudiant = selectedEtudiant.ID_Etudiant,
                                    ID_Chambre = chambreAttribuee.ID_Chambre,
                                    Date_Debut = DateTime.Now,
                                    Date_Fin = DateTime.Now.AddMonths(6),
                                    Statut_Paiement = "Non payé"
                                  
                                };
                                reservations.Lits = litAttribue;
                                //chambreAttribuee.Batiments.IncrementerNombreVide(1);
                                reservations.Etudiants.Add(selectedEtudiant);
                                context.Reservations.Add(reservations); // Mettre à jour l'objet Batiments dans la base de données
                                context.SaveChanges(); // Enregistrer les modifications dans la base de donnée

                                MessageBox.Show("L'etudiant " + selectedEtudiant.Nom + " a ete attribue a la chambre "+chambreAttribuee.Nom_Chambre+" pour 6mois , il doit maintenant l'a payer ","Attribution chambre",MessageBoxButton.OK,MessageBoxImage.Information);
                                return;
                            }
                            else
                            {
                                MessageBox.Show("Il n'y a plus de place au 1er etage, on ne peux pas mettre un handicape a d'autre etage que le rez de chausse ou le 1er etage", "ERREUR", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
                        }
                    }
                    else
                    {
                        // Au moins une chambre au rez-de-chaussée est disponible
                        // Faire quelque chose avec la chambre attribuée (par exemple, affecter à selectedEtudiant)
                        Chambres chambreAttribuee = chambresAuRezDeChaussee.First();
                        Lits litAttribue = chambreAttribuee.Lits.First(d => d.Reservations_ID_Reservation == null);
                        Reservations reservations = new Reservations
                        {
                            ID_Etudiant = selectedEtudiant.ID_Etudiant,
                            ID_Chambre = chambreAttribuee.ID_Chambre,
                            Date_Debut = DateTime.Now,
                            Date_Fin = DateTime.Now.AddMonths(6),
                            Statut_Paiement = "Non payé"

                        };
                        reservations.Lits = litAttribue;
                        //chambreAttribuee.Batiments.IncrementerNombreVide(1);
                        reservations.Etudiants.Add(selectedEtudiant);
                        context.Reservations.Add(reservations); // Mettre à jour l'objet Batiments dans la base de données
                        context.SaveChanges(); // Enregistrer les modifications dans la base de donnée

                        MessageBox.Show("L'etudiant " + selectedEtudiant.Nom + " a ete attribue a la chambre " + chambreAttribuee.Nom_Chambre + " pour 6mois , il doit maintenant la payer ", "Attribution chambre", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                }
                
                else if (selectedEtudiant.Sexe == "F")
                {
                    List<Chambres> chambresEtagesImpairs = ToutesChambre
                        .Where(c => c.Etage % 2 != 0 && c.Etage != 0) // Sélectionner les étages impairs, sauf l'étage 0
                        .ToList();

                    if (chambresEtagesImpairs.Count <= 0)
                    {
                        // Aucune chambre aux étages impairs n'est disponible pour les filles.
                        MessageBox.Show("Aucune chambre aux étages filles(impairs) n'est disponible pour les filles.", "Attribution de chambre", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    // Attribution d'une chambre aux étages impairs pour une fille
                    Chambres chambreAttribuee = chambresEtagesImpairs.First();
                    Lits litAttribue = chambreAttribuee.Lits.First(d => d.Reservations_ID_Reservation == null);
                    Reservations reservations = new Reservations
                    {
                        ID_Etudiant = selectedEtudiant.ID_Etudiant,
                        ID_Chambre = chambreAttribuee.ID_Chambre,
                        Date_Debut = DateTime.Now,
                        Date_Fin = DateTime.Now.AddMonths(6),
                        Statut_Paiement = "Non payé"
                    };
                    reservations.Lits = litAttribue;
                    reservations.Etudiants.Add(selectedEtudiant);
                    context.Reservations.Add(reservations);
                    context.SaveChanges();

                    MessageBox.Show("L'étudiante " + selectedEtudiant.Nom + " a été attribuée à la chambre " + chambreAttribuee.Nom_Chambre + " pour 6 mois. Elle doit maintenant payer.", "Attribution chambre", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                else if (selectedEtudiant.Sexe == "M")
                {
                    List<Chambres> chambresEtagesImpairs = ToutesChambre
                        .Where(c => c.Etage % 2 != 1) // Sélectionner les étages impairs
                        .ToList();

                    if (chambresEtagesImpairs.Count <= 0)
                    {
                        // Aucune chambre aux étages impairs n'est disponible pour les garçons.
                        MessageBox.Show("Aucune chambre aux étages impairs n'est disponible pour les garçons.", "Attribution de chambre", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    // Attribution d'une chambre aux étages impairs pour un garçon
                    Chambres chambreAttribuee = chambresEtagesImpairs.First();
                    Lits litAttribue = chambreAttribuee.Lits.First(d => d.Reservations_ID_Reservation == null);
                    Reservations reservations = new Reservations
                    {
                        ID_Etudiant = selectedEtudiant.ID_Etudiant,
                        ID_Chambre = chambreAttribuee.ID_Chambre,
                        Date_Debut = DateTime.Now,
                        Date_Fin = DateTime.Now.AddMonths(6),
                        Statut_Paiement = "Non payé"
                    };
                    reservations.Lits = litAttribue;
                    reservations.Etudiants.Add(selectedEtudiant);
                    context.Reservations.Add(reservations);
                    context.SaveChanges();

                    MessageBox.Show("L'étudiant " + selectedEtudiant.Nom + " a été attribué à la chambre " + chambreAttribuee.Nom_Chambre + " pour 6 mois. Il doit maintenant payer.", "Attribution chambre", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                
            }
        }

    }

}


