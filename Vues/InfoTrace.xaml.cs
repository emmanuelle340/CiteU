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
using CiteUContext = CiteU.Modele.CiteU;

namespace CiteU.Vues
{
    /// <summary>
    /// Logique d'interaction pour InfoTrace.xaml
    /// </summary>
    public partial class InfoTrace : Window
    {
        // Propriétés nécessaires pour la réservation
        public int ID_Reservation { get; set; }
        public string NomEtudiant { get; set; }
        public string NomChambre { get; set; }
        public int Id_Etudiant;
        public DateTime DateFin { get; set; }

        public InfoTrace(int idReservation, string nomEtudiant,int IdEtudiant, string nomChambre, DateTime dateFin)
        {
            InitializeComponent();

            // Initialisez les propriétés avec les informations de la réservation
            ID_Reservation = idReservation;
            NomEtudiant = nomEtudiant;
            NomChambre = nomChambre;
            Id_Etudiant = IdEtudiant;
            DateFin = dateFin;
            

            // Affichez les informations dans l'interface graphique
            UpdateUI();
        }




        private void UpdateUI()
        {
            // Affichez les informations dans les TextBlocks de l'interface graphique
            NomEtudiantTextBlock.Text = $"Nom de l'étudiant : {NomEtudiant}";
            NomChambreTextBlock.Text = $"Chambre : {NomChambre}";

            DateFinTextBlock.Text = $"Date de fin : {DateFin:d}";


            // Désactivez le bouton si le statut est déjà "Payé"
            //ModifierStatutButton.IsEnabled = (StatutPaiement != "Payé");
        }

        private void ModifierStatutButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using(var context= new CiteUContext()) {
                    var trace = context.Trace_ReservationSet.FirstOrDefault(c=>c.Id==ID_Reservation);
                    var chambre= context.Chambres.FirstOrDefault(c=>c.Nom_Chambre== NomChambre);
                    var litAttribue = context.Lits.FirstOrDefault(c => c.Reservations_ID_Reservation == null && c.ChambresID_Chambre== chambre.ID_Chambre);
                    if (litAttribue == null) {
                        MessageBox.Show("La chambre ne possede plus de lits ", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    var etudiant = context.Reservations.Select(c => c.Etudiants_ID_Etudiant).ToList();

                    string etat = "Non payé";
                    if (etudiant.Contains(Id_Etudiant))
                    {
                        MessageBoxResult result = MessageBox.Show("Cet etudiant a deja une chambre , operation impossible", "ERREUR", MessageBoxButton.OK, MessageBoxImage.Error);

                        return;
                    }

                    Reservations reservations = new Reservations
                    {
                        Etudiants_ID_Etudiant = Id_Etudiant,
                        ID_Chambre = chambre.ID_Chambre,
                        Date_Debut = DateTime.Now,
                        Date_Fin = DateTime.Now.AddMonths(12),
                        Statut_Paiement = "Non payé"
                    };
                    reservations.Lits = litAttribue;
                    reservations.Lits.Chambres.Statut = "Occupee";

                    context.Reservations.Add(reservations);
                    context.Trace_ReservationSet.Remove(trace);
                    context.SaveChanges();

                    litAttribue.Reservations_ID_Reservation = reservations.ID_Reservation;

                    context.SaveChanges();
                    MessageBox.Show($"L'étudiant {NomEtudiant} a été re-attribué à la chambre {chambre.Nom_Chambre} pour 1an. Il doit maintenant la payer.", "Attribution chambre", MessageBoxButton.OK, MessageBoxImage.Information);

                }

            }
            catch (Exception ex)
            {
                // Gérez les exceptions ici (par exemple, affichez un message d'erreur)
                MessageBox.Show($"Une erreur s'est produite : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
