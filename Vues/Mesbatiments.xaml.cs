using CiteU.Modele;
using CiteU;
using CiteUContext = CiteU.Modele.CiteU;
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
using System.Windows.Media.Effects;

namespace CiteU.Vues
{
    /// <summary>
    /// Logique d'interaction pour Mesbatiments.xaml
    /// </summary>
    public partial class Mesbatiments : UserControl
    {
        public ObservableCollection<Batiments> ListOfBatiments { get; set; }
        public string nomBatiment { get; set; }

        // Propriété statique pour stocker l'instance actuelle
        public static Mesbatiments Instance { get; private set; }
        public Mesbatiments()
        {
            InitializeComponent();

            using (var context = new CiteUContext())  // Utilisez le nom de votre classe DbContext ici
            {
                ListOfBatiments = new ObservableCollection<Batiments>(context.Batiments.ToList());

            }
            DataContext = this;

        }


        // Propriété de visibilité pour le nouveau UserControl
        public Visibility NouveauUserControlVisibility
        {
            get { return (Visibility)GetValue(NouveauUserControlVisibilityProperty); }
            set { SetValue(NouveauUserControlVisibilityProperty, value); }
        }

        public static readonly DependencyProperty NouveauUserControlVisibilityProperty =
            DependencyProperty.Register("NouveauUserControlVisibility", typeof(Visibility), typeof(Mesbatiments), new PropertyMetadata(Visibility.Collapsed));

        // Méthode pour changer la visibilité du nouveau UserControl
        private void AfficherNouveauUserControl()
        {
            NouveauUserControlVisibility = Visibility.Visible;
        }


        private void BoutonBatiment_Click(object sender, RoutedEventArgs e)
        {
            // Obtenir le bouton cliqué
            Button bouton = (Button)sender;

            // Obtenir le DataContext du bouton (le modèle associé)
            Batiments modele = bouton.DataContext as Batiments;

            if (modele != null)
            {
                // Accéder à la propriété Nom_Batiment du modèle
                string nomBatiment = modele.Nom_Batiment;
                Instance = this;

                // Passer la valeur de nomBatiment lors de la création de l'instance de MesChambres
                MesChambres mesChambres = new MesChambres(nomBatiment);

                // Rendre MesChambres visible
                AfficherNouveauUserControl();
            }
        }


        private void AjouterBatiment_Click(object sender, RoutedEventArgs e)
        {
            // Accéder à la fenêtre principale
            Window mainWindow = Window.GetWindow(this);

            if (mainWindow != null)
            {
                // Appliquer un effet de flou à la fenêtre principale
                BlurEffect blurEffect = new BlurEffect { Radius = 5 };
                mainWindow.Effect = blurEffect;

                var dialog = new AjouterBatimentDialog();
                // Définir la fenêtre principale comme propriétaire de la boîte de dialogue
                dialog.Owner = mainWindow;

                // Centrer la boîte de dialogue par rapport à la fenêtre principale
                dialog.Left = mainWindow.Left + (mainWindow.Width - dialog.Width) / 2;
                dialog.Top = mainWindow.Top + (mainWindow.Height - dialog.Height) / 2;

                // Gérer la fermeture de la boîte de dialogue
                dialog.Closed += (s, args) =>
                {

                    // Restaurer la fenêtre principale sans flou
                    mainWindow.Effect = null;
                };

                // Afficher la boîte de dialogue
                dialog.ShowDialog();
            }
        }
    }
}
