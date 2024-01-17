using CiteU.Modele;
using CiteU;
using CiteUContext = CiteU.Modele.CiteU;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace CiteU.Vues
{
    /// <summary>
    /// Logique d'interaction pour AjouterBatimentDialog.xaml
    /// </summary>
    public partial class AjouterBatimentDialog : Window
    {
        public string NomBatiment { get; set; }
        public int NombreChambres { get; set; }
        public int NombreEtages { get; set; }

        public AjouterBatimentDialog()
        {
            InitializeComponent();
        }

        private Mesbatiments _mesBatiments;

        public AjouterBatimentDialog(Mesbatiments mesBatiments)
        {
            InitializeComponent();
            _mesBatiments = mesBatiments;
        }

        private void btnEnregistrer_Click(object sender, RoutedEventArgs e)
        {
            // Validez les entrées utilisateur et affectez les valeurs aux propriétés
            if (ValidateInputs())
            {
                try
                {
                    // Recherchez le dernier ID de bâtiment dans la base de données
                    int dernierIdBatiment;
                    using (var context = new CiteUContext())
                    {

                        dernierIdBatiment = context.Batiments.OrderByDescending(b => b.ID_Batiment).Select(b => b.ID_Batiment).FirstOrDefault();
                        // Vérifiez si le nom du bâtiment existe déjà
                        if (context.Batiments.Any(b => b.Nom_Batiment == txtNomBatiment.Text))
                        {
                            MessageBox.Show("Le nom du bâtiment existe déjà. Veuillez saisir un nom différent.", "Erreur de validation", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                    }

                    // Incrémentez l'ID pour le nouveau bâtiment
                    int nouvelIdBatiment = dernierIdBatiment + 1;

                    // Créez un nouvel objet Batiments
                    Batiments nouveauBatiment = new Batiments
                    {
                        ID_Batiment = nouvelIdBatiment,
                        Nom_Batiment = txtNomBatiment.Text,
                        Nombre_max_chambre = Convert.ToInt32(txtNombreChambres.Text),
                        Nombre_Etages = Convert.ToInt32(txtNombreEtages.Text),
                        // Ajoutez les autres propriétés selon votre modèle
                    };

                    // Ajoutez le nouvel objet à la base de données
                    using (var context = new CiteUContext())
                    {
                        context.Batiments.Add(nouveauBatiment);

                        // Enregistrez les modifications dans la base de données
                        context.SaveChanges();
                    }

                    AjoutChambresDialog ajoutChambresDialog = new AjoutChambresDialog();

                    // Centrer la fenêtre par rapport à la fenêtre parente (this)
                    ajoutChambresDialog.Owner = this;
                    ajoutChambresDialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;


                    ajoutChambresDialog.ShowDialog();
                    

                    DialogResult = true; // Ferme la fenêtre avec un résultat positif
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur lors de l'ajout du bâtiment : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }


        private void btnAnnuler_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false; // Ferme la fenêtre avec un résultat négatif
        }
        private bool ValidateInputs()
        {
            // Assurez-vous que les champs obligatoires sont remplis
            if (string.IsNullOrEmpty(txtNomBatiment.Text) ||
                string.IsNullOrEmpty(txtNombreChambres.Text) ||
                string.IsNullOrEmpty(txtNombreEtages.Text))
            {
                MessageBox.Show("Veuillez remplir tous les champs obligatoires.", "Erreur de validation", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            // Vérifiez si les valeurs pour txtNombreChambres et txtNombreEtages sont des entiers
            int nombreChambres;
            int nombreEtages;

            if (!int.TryParse(txtNombreChambres.Text, out nombreChambres) || !int.TryParse(txtNombreEtages.Text, out nombreEtages))
            {
                MessageBox.Show("Veuillez saisir des valeurs numériques valides pour les champs Nombre de chambres et Nombre d'étages.", "Erreur de validation", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            // Vous pouvez ajouter d'autres validations selon vos besoins

            return true;
        }

    }
}

    