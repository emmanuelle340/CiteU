using CiteU.Modele;
using System.Collections.ObjectModel;
using System.Linq;
using CiteUContext = CiteU.Modele.CiteU;
using System.Windows.Controls;
using System.Windows;

namespace CiteU.Vues
{
    public partial class MesChambres : UserControl
    {
        // Utilisez une propriété de dépendance pour ListDeChambre
        public ObservableCollection<Chambres> ListDeChambre
        {
            get { return (ObservableCollection<Chambres>)GetValue(ListDeChambreProperty); }
            set { SetValue(ListDeChambreProperty, value); }
        }

        public static readonly DependencyProperty ListDeChambreProperty =
            DependencyProperty.Register("ListDeChambre", typeof(ObservableCollection<Chambres>), typeof(MesChambres));

        public MesChambres()
        {
            InitializeComponent();
            LoadData();
            DataContext = this;
        }

        private void LoadData()
        {
            using (var context = new CiteUContext())
            {
                // Logique pour charger les données
                ListDeChambre = new ObservableCollection<Chambres>(context.Chambres.ToList());
            }
        }
    }
}
