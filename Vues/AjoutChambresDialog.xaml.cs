using CiteU.Modele;
using CiteUContext=CiteU.Modele.CiteU;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Windows.Documents;
using System.Windows.Controls;

namespace CiteU.Vues
{
    public partial class AjoutChambresDialog : Window
    {
        private int dernierIdBatiment;
        ObservableCollection<ChambreEtageLits> chambreEtageLits { get; set; }
        Batiments batiment;

        public AjoutChambresDialog()
        {
            InitializeComponent();
            chambreEtageLits = new ObservableCollection<ChambreEtageLits>();
            DataContext = this;
            
            using (var context = new CiteUContext())
            {
                dernierIdBatiment = context.Batiments.OrderByDescending(b => b.ID_Batiment).Select(b => b.ID_Batiment).FirstOrDefault();
                // Récupérer le bâtiment à partir de son ID_Batiment
                batiment = context.Batiments.FirstOrDefault(b => b.ID_Batiment == dernierIdBatiment);
            }
            int? NombreDeChambreParEtage = batiment.Nombre_max_chambre / batiment.Nombre_Etages;
            int? resteChambre= batiment.Nombre_max_chambre % batiment.Nombre_Etages;
            for (int i=0; i<batiment.Nombre_Etages; i++)
            {
                ChambreEtageLits chambre = new ChambreEtageLits();
                chambre.NumeroEtage = i;
                // Attribuer le nombre de chambres par étage
                chambre.NombreChambre = (int)NombreDeChambreParEtage;

                // Si c'est le dernier étage, ajoutez le reste des chambres
                if (i == batiment.Nombre_Etages - 1 && resteChambre.HasValue)
                {
                    chambre.NombreChambre += resteChambre.Value;
                }
                chambre.NombreLits = 2;

                chambreEtageLits.Add(chambre);
            }
            listBoxEtages.ItemsSource = chambreEtageLits;


        }

        private void btnEnregistrer_Click(object sender, RoutedEventArgs e)
        {
            /*try
            {*/
                // Valider et enregistrer les données des chambres
                if (ValiderEtEnregistrerDonnees())
                {
                    // Données validées et enregistrées avec succès

                    // TODO: Ajouter le code pour enregistrer les données des chambres dans la base de données

                    MessageBox.Show("Données des chambres enregistrées avec succès !", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Fermer la boîte de dialogue ou effectuer d'autres actions selon les besoins
                    this.Close();
                }
                else
                {
                    // La validation des données a échoué
                    MessageBox.Show("Veuillez saisir des données valides pour toutes les chambres.", "Erreur de validation", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            /*}
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }*/
        }

        private bool ValiderEtEnregistrerDonnees()
        {
            using (var context = new CiteUContext())
            {
                // Calculer le nombre total de chambres
                int nombreTotalChambres = chambreEtageLits.Sum(c => c.NombreChambre);

                // Vérifier si le nombre total de chambres est égal à Nombre_max_chambre du bâtiment
                if (nombreTotalChambres != batiment.Nombre_max_chambre)
                {
                    MessageBox.Show($"Le nombre total de chambres est {nombreTotalChambres}. Au lieu de {batiment.Nombre_max_chambre}", "Erreur de validation", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }

                dernierIdBatiment = context.Batiments.OrderByDescending(b => b.ID_Batiment).Select(b => b.ID_Batiment).FirstOrDefault();
                // Vérifier et enregistrer les données pour chaque chambre
                foreach (var chambre in chambreEtageLits)
                {
                    // TODO: Valider les données pour chaque chambre (par exemple, vérifier si elles sont déjà dans la base de données)

         
                    
                    for (int i = 0; i < chambre.NombreChambre; i++)
                    {
                        // Récupérer le dernier ID de Chambre dans la base de données
                        int dernierIdChambre = context.Chambres.OrderByDescending(c => c.ID_Chambre).Select(c => c.ID_Chambre).FirstOrDefault();

                        // Vérifier si les IDs sont nuls ou s'il n'y a aucun élément, auquel cas, initialisez-les à 0
                        int nouvelIdChambre = (dernierIdChambre == 0) ? 0 : dernierIdChambre + 1;

                        // Créer un nouvel objet Chambres
                        Chambres nouvelleChambre = new Chambres
                        {
                           
                            ID_Batiment = dernierIdBatiment,
                            Nom_Chambre = $"{batiment.Nom_Batiment}-{chambre.NumeroEtage}-{i}",
                          
                            Etage = chambre.NumeroEtage,
                            Statut = "Disponible",
                            Numero_Chambre = chambre.NombreChambre // Vous pouvez ajuster cela en fonction de vos besoins
                        };

                        // Ajouter la chambre à la base de données
                        context.Chambres.Add(nouvelleChambre);
                        context.SaveChanges();

                        // Incrémenter l'ID pour la prochaine chambre
                        nouvelIdChambre++;

                        // Récupérer l'ID de la nouvelle chambre
                        int idNouvelleChambre = nouvelleChambre.ID_Chambre;

                        // Ajouter des lits associés à la nouvelle chambre
                        for (int j = 0; j < chambre.NombreLits; j++)
                        {
                            // Récupérer le dernier ID de Lit dans la base de données
                            int dernierIdLit = context.Lits.OrderByDescending(l => l.id).Select(l => l.id).FirstOrDefault();
                            int nouvelIdLit = (dernierIdLit == 0) ? 0 : dernierIdLit + 1;

                            Lits nouveauLit = new Lits
                            {
                                
                                ChambresID_Chambre = idNouvelleChambre
                                // Vous pouvez ajouter d'autres propriétés de lit ici
                            };

                            // Ajouter le lit à la base de données
                            context.Lits.Add(nouveauLit);
                            context.SaveChanges(); // Enregistrez le lit immédiatement
                            nouvelIdLit++; // Incrémenter l'ID pour le prochain lit
                        }
                    }
                }
            }

            // Données validées et enregistrées avec succès
            return true;
        }



        private void btnAnnuler_Click(object sender, RoutedEventArgs e)
        {
            // Afficher un message de confirmation pour l'annulation
            MessageBoxResult result = MessageBox.Show("Voulez-vous vraiment annuler l'ajout des chambres ? Le Batiment et les Chambres associes ne seront pas ajouter", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                // L'utilisateur a confirmé l'annulation, supprimer le bâtiment et fermer la fenêtre
                using (var context = new CiteUContext())
                {
                    // Récupérer le bâtiment à partir de son ID_Batiment
                    var batimentASupprimer = context.Batiments.FirstOrDefault(b => b.ID_Batiment == dernierIdBatiment);

                    if (batimentASupprimer != null)
                    {
                        // Supprimer le bâtiment
                        context.Batiments.Remove(batimentASupprimer);

                        // Enregistrez les modifications dans la base de données
                        context.SaveChanges();
                    }
                }

                // Fermer la fenêtre sans enregistrer
                this.Close();
            }
            // Sinon, l'utilisateur a choisi de ne pas annuler, et la fenêtre reste ouverte
        }

    }
    class ChambreEtageLits
    {
        public int NumeroEtage { get; set; }
        public int NombreChambre { get; set; }
        public int NombreLits { get; set; }
    }
}
