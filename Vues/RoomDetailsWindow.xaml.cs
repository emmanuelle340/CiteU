using CiteU.Modele;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Contexts;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media.Effects;
using CiteUContext = CiteU.Modele.CiteU;

namespace CiteU.Vues
{
    public partial class RoomDetailsWindow : Window
    {
        // Room object to display the details
        public Chambres Room { get; set; }

        public RoomDetailsWindow(Chambres room)
        {
            InitializeComponent();
            UpdateChambre(room);
            DataContext = Room;
            // Centrer la fenêtre à l'écran
            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            // Appliquer un effet de flou à la grille principale de la fenêtre
            var blurEffect = new BlurEffect { Radius = 8 };
            var mainWindow = System.Windows.Application.Current.MainWindow;
            mainWindow.Effect = blurEffect;
            
            // Gérer l'événement Closed de la RoomDetailsWindow
            Closed += RoomDetailsWindow_Closed;
        }

        private void UpdateChambre(Chambres room )
        {

            using (var context = new CiteUContext())
                Room = context.Chambres.FirstOrDefault(a => a.ID_Chambre == room.ID_Chambre);
        }

        private void RemoveBed_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new CiteUContext())
            {
                int chambreID = Room.ID_Chambre;

                // Recherche du premier lit non réservé dans la chambre
                Lits litARetirer = context.Lits
                    .FirstOrDefault(lit => lit.ChambresID_Chambre == chambreID && lit.Reservations_ID_Reservation == null);

                if (litARetirer == null)
                {
                    MessageBox.Show("Tous les lits sont occupés ou hors service, vous ne pouvez pas en supprimer un.", "Erreur Suppression Lit", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Supprimer le lit de la chambre
                context.Lits.Remove(litARetirer);
                context.SaveChanges();

                // Mettre à jour le statut de la chambre et sa capacité
                UpdateRoomStatusAndCapacity(chambreID);
            }

            MessageBox.Show("Un lit a été supprimé de la chambre.", "Lit supprimé", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void UpdateRoomStatusAndCapacity(int chambreID)
        {
            using (var context = new CiteUContext())
            {
                // Rechercher la chambre dans le contexte
                Chambres chambre = context.Chambres.FirstOrDefault(c => c.ID_Chambre == chambreID);

                if (chambre != null)
                {
                    // Mettre à jour le statut de la chambre
                    chambre.Statut = context.Lits.Any(lit => lit.ChambresID_Chambre == chambreID && lit.Reservations_ID_Reservation == null)
                        ? "Occupée"
                        : "Aucun Lit";

                    // Mettre à jour la capacité de la chambre
                    chambre.Capacite = context.Lits.Count(lit => lit.ChambresID_Chambre == chambreID && lit.Reservations_ID_Reservation == null);

                    // Enregistrer les modifications dans la base de données
                    context.SaveChanges();
                }
            }
        }

        private void DivideRoom_Click(object sender, RoutedEventArgs e)
        {
            // Logique pour diviser la chambre
            using (var context = new CiteUContext())
            {
                // Récupérer l'ID de la chambre à partir de Room
                int chambreID = Room.ID_Chambre;

                // Charger la chambre à partir du nouveau contexte
                Chambres chambreExistante = context.Chambres.FirstOrDefault(c => c.ID_Chambre == chambreID);

                // Vérifier si la capacité de la chambre est suffisante pour la division
                if (chambreExistante.Capacite <= 1)
                {
                    MessageBox.Show("La chambre ne peut pas être divisée car elle a une capacité insuffisante.", "Erreur Division Chambre", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Récupérer le bâtiment associé à la chambre
                Batiments batiments = context.Batiments.FirstOrDefault(b => b.ID_Batiment == chambreExistante.ID_Batiment);

                // Mettre à jour le nombre maximum de chambres dans le bâtiment
                batiments.Nombre_max_chambre += 1;

                // Créer une nouvelle chambre pour la division
                Chambres nouvelleChambre = new Chambres
                {
                    ID_Batiment = chambreExistante.ID_Batiment,
                    Nom_Chambre = $"{batiments.Nom_Batiment}-{chambreExistante.Etage}-{batiments.Nombre_max_chambre}",
                    Statut = "Disponible",
                    Etage = chambreExistante.Etage,
                };

                // Calculer la nouvelle capacité pour chaque chambre
                int nouvelleCapacite = (int)chambreExistante.Capacite / 2;

                // Déterminer le nombre de lits à transférer à la nouvelle chambre
                int litsATransferer = chambreExistante.Capacite % 2 == 0 ? nouvelleCapacite : nouvelleCapacite + 1;

                // Afficher la fenêtre de répartition des chambres
                RepartitionChambresWindow repartitionWindow = new RepartitionChambresWindow(nouvelleCapacite, litsATransferer);

                // Centrer la fenêtre sur la fenêtre principale
                repartitionWindow.Owner = Application.Current.MainWindow;

                // Appliquer un effet de flou à la fenêtre principale
                var blurEffect = new BlurEffect { Radius = 8 };
                Application.Current.MainWindow.Effect = blurEffect;

                bool? result = repartitionWindow.ShowDialog();

                // Vérifier si l'utilisateur a cliqué sur Enregistrer
                if (result == true)
                {
                    // Mettre à jour la capacité des chambres avec les nouvelles valeurs
                    chambreExistante.Capacite = repartitionWindow.CapaciteChambre1;
                    nouvelleChambre.Capacite = repartitionWindow.CapaciteChambre2;

                    // Transférer les lits de la chambre existante à la nouvelle chambre
                    var litsATransfererList = context.Lits
                        .Where(lit => lit.ChambresID_Chambre == chambreExistante.ID_Chambre && lit.Reservations_ID_Reservation == null)
                        .Take(litsATransferer)
                        .ToList();

                    foreach (var lit in litsATransfererList)
                    {
                        lit.ChambresID_Chambre = nouvelleChambre.ID_Chambre;
                    }

                    // Ajouter la nouvelle chambre à la base de données
                    context.Chambres.Add(nouvelleChambre);

                    // Mettre à jour le bâtiment dans la base de données
                    context.Entry(batiments).Property(b => b.Nombre_max_chambre).IsModified = true;

                    // Mettre à jour la chambre existante dans la base de données
                    context.Entry(chambreExistante).Property(c => c.Capacite).IsModified = true;

                    // Enregistrer les changements dans la base de données
                    context.SaveChanges();
                    UpdateChambre(chambreExistante);

                    // Fermer la fenêtre de répartition des chambres
                    Application.Current.MainWindow.Effect = null;

                    MessageBox.Show("La chambre a été divisée avec succès. Deux nouvelles chambres ont été créées avec une capacité partagée.", "Chambre Divisée", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    // Si l'utilisateur a annulé, retirer l'effet de flou
                    Application.Current.MainWindow.Effect = null;
                }
            }
        }

        private void RoomDetailsWindow_Closed(object sender, System.EventArgs e)
        {
            var mainWindow = System.Windows.Application.Current.MainWindow;
            mainWindow.Effect = null;
        }

        private void AddBed_Click(object sender, RoutedEventArgs e)
        {
            // Ajouter un nouveau lit à la chambre
            Lits nouveauLit = new Lits();
            nouveauLit.ChambresID_Chambre = Room.ID_Chambre;
            Room.Capacite += 1;

            using (var context = new CiteUContext())
            {
                // Check if the Room entity is already tracked by the context
                var entry = context.Entry(Room);
                if (entry.State == EntityState.Detached)
                {
                    context.Chambres.Attach(Room);
                }

                //if (Room.Capacite == 0) Room.Statut = "Your logic here";
                if (Room.Statut == "Aucun Lit") Room.Statut = "Disponible";

                // Enregistrer les modifications dans la base de données
                context.Lits.Add(nouveauLit);
                context.SaveChanges();

                // Mark the properties as modified
                entry.Property(c => c.Capacite).IsModified = true;
                entry.Property(c => c.Statut).IsModified = true;

                // Set the state to Modified
                entry.State = EntityState.Modified;

                context.SaveChanges();
            }

            // Afficher un message de succès
            MessageBox.Show("Un lit a été ajouté à la chambre.", "Lit ajouté", MessageBoxButton.OK, MessageBoxImage.Information);
        }

    }



}

/*
 
 
 */