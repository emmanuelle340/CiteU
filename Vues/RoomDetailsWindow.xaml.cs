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
            Room = room;
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

        private void RemoveBed_Click(object sender, RoutedEventArgs e)
        {
            // Logique pour supprimer un lit de la chambre
            // Rechercher les lits non occupés
            using (var context = new CiteUContext())
            {
                int chambreID = Room.ID_Chambre;

                List<Lits> litsNonReserves = context.Lits
                    .Where(lit => lit.ChambresID_Chambre == chambreID && lit.Reservations_ID_Reservation == null)
                    .ToList();

                if (Room.Capacite == 0)
                {
                    MessageBox.Show("Il n'y a pas de lits dans cette chambre", "Erreur Suppression Lit", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (litsNonReserves.Count == 0)
                {
                    MessageBox.Show("Tous les lits sont occupés, vous ne pouvez pas en supprimer un.", "Erreur Suppression Lit", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                
                int index = 0;
                Lits litARetirer = litsNonReserves[index];

                // Retirer le lit de la liste
                litsNonReserves.RemoveAt(index);

                // Supprimer le lit de la base de données
                context.Lits.Remove(litARetirer);
                context.SaveChanges();

                // Rechercher la chambre dans le contexte
                Chambres chambre = context.Chambres.FirstOrDefault(c => c.ID_Chambre == chambreID);
                if (chambre != null)
                {
                    chambre.Capacite -= 1;

                    // Enregistrer la mise à jour de la capacité dans la base de données
                    context.Entry(chambre).Property(c => c.Capacite).IsModified = true;
                    context.SaveChanges();
                }
            }
            MessageBox.Show("Un lit a été supprimé de la chambre.", "Lit supprimé", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void DivideRoom_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("La chambre a été divisée.");
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
            ObservableCollection<Lits> list = new ObservableCollection<Lits>();
            list.Add(nouveauLit);
            using (var context = new CiteUContext())
            {
                // Enregistrer les modifications dans la base de données
                context.Lits.Add(nouveauLit);
                context.SaveChanges();
                context.Entry(  Room).State = EntityState.Modified;
                context.SaveChanges();
            }
            // Afficher un message de succès
            MessageBox.Show("Un lit a été ajouté à la chambre.", "Lit ajouté", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}