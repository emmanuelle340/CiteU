using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using CiteUContext = CiteU.Modele.CiteU;

namespace CiteU.Vues
{
    /// <summary>
    /// Logique d'interaction pour MesTracesReservations.xaml
    /// </summary>
    public partial class MesTracesReservations : UserControl
    {
        public ObservableCollection<AutresInfo> ListOfReservation { get; set; }

        public class AutresInfo
        {
            public int ID_Reservation { get; set; }
            public int? ID_Etudiant { get; set; }
            public int? ID_Chambre { get; set; }
            public DateTime? Date_Debut { get; set; }
            public DateTime? Date_Fin { get; set; }



            public int Lits_id { get; set; }
            public string NomChambre { get; set; }
            public string NomEtudiant { get; set; }
            // Ajoutez d'autres propriétés au besoin...

            public AutresInfo(int idReservation, int? idEtudiant, int? idChambre, DateTime? dateFin, int litsId, string nomChambre, string nomEtudiant)
            {
                ID_Reservation = idReservation;
                ID_Etudiant = idEtudiant;
                ID_Chambre = idChambre;

                Date_Fin = dateFin?.Date;
                Lits_id = litsId;
                NomChambre = nomChambre;
                NomEtudiant = nomEtudiant;
            }
        }

        public MesTracesReservations()
        {
            InitializeComponent();
            ListOfReservation = new ObservableCollection<AutresInfo>();
            LoadReservationsFromDatabase();
            DataContext = this; // Définissez le DataContext sur votre UserControl

            

            // Assurez-vous d'ajouter cet événement TextChanged pour gérer les modifications de la barre de recherche.
            RechercheTextBox.TextChanged += RechercheTextBox_TextChanged;

            // Ajoutez un gestionnaire d'événements à l'événement GotFocus pour effacer le texte au clic
            RechercheTextBox.GotFocus += RechercheTextBox_GotFocus;
        }
     

        private void RechercheTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Répondez aux modifications de la barre de recherche ici
            string termeRecherche = RechercheTextBox.Text;


            // Filtrer les réservations qui contiennent le terme de recherche
            List<AutresInfo> resultatsRecherche = ListOfReservation
                .Where(reservation => reservation.NomEtudiant.Contains(termeRecherche)

                )
                .ToList();

            foreach (AutresInfo yo in resultatsRecherche) { ListOfReservation.Add(yo); }
            DataContext = ListOfReservation;

        }


        private void RechercheTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            // Effacez le texte lorsque la TextBox obtient le focus
            RechercheTextBox.Text = string.Empty;
        }

        private void LoadReservationsFromDatabase()
        {
            // Utilisez votre contexte de base de données (CiteUContext) pour récupérer toutes les réservations.
            using (var context = new CiteUContext())
            {


                var reservations = context.Trace_ReservationSet
                    .OrderByDescending(r => r.Id)
                    .ToList();

                AutresInfo tmp;
                // Ajoutez les réservations à ListOfReservation
                foreach (var reservation in reservations)
                {
                    string nomChambre = context.Chambres
                        .Where(c => c.ID_Chambre == reservation.ID_Chambre)
                        .Select(c => c.Nom_Chambre)
                        .FirstOrDefault();

                    string nomEtudiant = context.Etudiants
                        .Where(t => t.ID_Etudiant == reservation.Etudiants_ID_Etudiant)
                        .Select(t => t.Nom)
                        .FirstOrDefault();
                    tmp = new AutresInfo(
                        reservation.Id,
                        (int)reservation.Etudiants_ID_Etudiant,
                        (int)reservation.ID_Chambre,

                        reservation.Date_Fin,

                        reservation.Lits_id,
                        nomChambre,
                        nomEtudiant
                        );
                    ListOfReservation.Add(tmp);
                }
            }
        }

        private void clickmoi_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button)sender;
            AutresInfo selectedReservation = clickedButton.DataContext as AutresInfo;

            if (selectedReservation != null)
            {
                // Créez une nouvelle instance de la fenêtre de modification du statut
                InfoTrace modifierStatutWindow = new InfoTrace(selectedReservation.ID_Reservation, selectedReservation.NomEtudiant,(int) selectedReservation.ID_Etudiant,selectedReservation.NomChambre, selectedReservation.Date_Fin.GetValueOrDefault());
                Window mainWindow = Window.GetWindow(this);


                // Définir la fenêtre principale comme propriétaire de la boîte de dialogue
                modifierStatutWindow.Owner = Window.GetWindow(this);




                // Positionner la fenêtre modale par rapport à la fenêtre principale
                if (modifierStatutWindow.Owner != null)
                {
                    modifierStatutWindow.Left = modifierStatutWindow.Owner.Left + 750; // Décalage vers la droite
                    modifierStatutWindow.Top = modifierStatutWindow.Owner.Top + 170;  // Décalage vers le bas
                }

                modifierStatutWindow.Closed += (s, args) =>
                {

                };

                // Passez les informations nécessaires à la fenêtre de modification du statut
                modifierStatutWindow.DataContext = selectedReservation;

                // Affichez la fenêtre de modification du statut
                modifierStatutWindow.ShowDialog();
            }
        }

        private void Rafraichir_Click(object sender, RoutedEventArgs e)
        {
            // Videz la liste actuelle des réservations
            ListOfReservation.Clear();

            // Rechargez les réservations depuis la base de données
            LoadReservationsFromDatabase();
        }

    }
}
