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
using System;
using System.Windows;
using CiteUContext = CiteU.Modele.CiteU;
using CiteU.Modele;

namespace CiteU.Vues
{
    public partial class ModifierStatutWindow : Window
    {
        // Propriétés nécessaires pour la réservation
        public int ID_Reservation { get; set; }
        public string NomEtudiant { get; set; }
        public string NomChambre { get; set; }
        public DateTime DateDebut { get; set; }
        public DateTime DateFin { get; set; }
        public string StatutPaiement { get; set; }

        public ModifierStatutWindow(int idReservation, string nomEtudiant, string nomChambre, DateTime dateDebut, DateTime dateFin, string statutPaiement)
        {
            InitializeComponent();

            // Initialisez les propriétés avec les informations de la réservation
            ID_Reservation = idReservation;
            NomEtudiant = nomEtudiant;
            NomChambre = nomChambre;
            DateDebut = dateDebut;
            DateFin = dateFin;
            StatutPaiement = statutPaiement;

            // Affichez les informations dans l'interface graphique
            UpdateUI();
        }

        private void UpdateUI()
        {
            // Affichez les informations dans les TextBlocks de l'interface graphique
            NomEtudiantTextBlock.Text = $"Nom de l'étudiant : {NomEtudiant}";
            NomChambreTextBlock.Text = $"Chambre : {NomChambre}";
            DateDebutTextBlock.Text = $"Date de début : {DateDebut:d}";
            DateFinTextBlock.Text = $"Date de fin : {DateFin:d}";
            StatutPaiementTextBlock.Text = $"Statut : {StatutPaiement}";

            // Désactivez le bouton si le statut est déjà "Payé"
            //ModifierStatutButton.IsEnabled = (StatutPaiement != "Payé");
        }

        private void ModifierStatutButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Vérifiez si le statut n'est pas déjà "Payé"
                if (StatutPaiement != "Payé")
                {
                    // Affichez un message de confirmation
                    var result = MessageBox.Show("Voulez-vous vraiment modifier le statut du paiement ?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        using(var context = new CiteUContext())
                        {
                            Reservations reservations = context.Reservations.FirstOrDefault(c=>c.ID_Reservation==ID_Reservation);
                            reservations.Statut_Paiement = "Payé";
                            context.SaveChanges();
                        }
                        // Mettez à jour l'interface graphique
                        StatutPaiement = "Payé";
                        UpdateUI();

                        // Affichez un message de succès
                        MessageBox.Show("Le statut du paiement a été modifié avec succès.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    // Affichez un message indiquant que le statut est déjà "Payé"
                    MessageBox.Show("Le paiement a déjà été effectué. Aucune modification nécessaire.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
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
