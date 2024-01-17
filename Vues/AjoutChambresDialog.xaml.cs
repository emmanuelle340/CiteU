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


        public AjoutChambresDialog()
        {
            InitializeComponent();
            chambreEtageLits = new ObservableCollection<ChambreEtageLits>();
            DataContext = this;
            Batiments batiment;
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

        }

        private void btnAnnuler_Click(object sender, RoutedEventArgs e)
        {

        }
    }
    class ChambreEtageLits
    {
        public int NumeroEtage { get; set; }
        public int NombreChambre { get; set; }
        public int NombreLits { get; set; }
    }
}
