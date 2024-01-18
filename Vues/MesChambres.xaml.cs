using CiteU.Modele;
using System.Collections.ObjectModel;
using System.Linq;
using CiteUContext = CiteU.Modele.CiteU;
using System.Windows;
using System.Windows.Controls;

namespace CiteU.Vues
{
    public partial class MesChambres : UserControl
    {
        // Nouvelle propriété pour les chambres du bâtiment sélectionné
        public ObservableCollection<Chambres> ListDeChambre { get; set; }

        public MesChambres()
        {
            InitializeComponent();


        }

        public MesChambres(string nomBatiment)
        {
            InitializeComponent();

            // Utiliser la valeur de nomBatiment
            MessageBox.Show("Nom du bâtiment dans MesChambres : " + nomBatiment, "Message", MessageBoxButton.OK, MessageBoxImage.Information);

            using (var context = new CiteUContext())
            {
                var batiment = context.Batiments.FirstOrDefault(b => b.Nom_Batiment == nomBatiment);

                if (batiment != null)
                {
                    // Récupération de la liste des chambres pour le bâtiment trouvé
                    ListDeChambre = new ObservableCollection<Chambres>(context.Chambres.Where(c => c.ID_Batiment == batiment.ID_Batiment).ToList());
                    DataContext = this;

                    if (ListDeChambre != null && ListDeChambre.Count > 0)
                        MessageBox.Show("Nom de la première chambre : " + ListDeChambre[0].Nom_Chambre, "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }
    }
}
