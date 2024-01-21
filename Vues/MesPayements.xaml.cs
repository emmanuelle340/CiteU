using CiteU.Modele;
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
    /// Logique d'interaction pour MesPayements.xaml
    /// </summary>
    public partial class MesPayements : UserControl
    {
        /*public ObservableCollection<Reservations> ListOfReservation
        {
            get { return (ObservableCollection<Reservations>)GetValue(ListOfReservationProperty); }
            set { SetValue(ListOfReservationProperty, value); }
        }

        public static readonly DependencyProperty ListDeChambreProperty =
            DependencyProperty.Register(" ListOfReservation", typeof(ObservableCollection<Reservations>), typeof(MesPayements));
*/
         
        public MesPayements()
        {
            InitializeComponent();
            // Initialiser la liste des bâtiments lors de la création de la classe
            //ListOfReservation = new ObservableCollection<Reservations>();
            //ListOfReservation.CollectionChanged += (s, e) => { /* Mettre à jour l'interface utilisateur ici */ };

            

            //DataContext = ListOfReservation;
        }

        /*public void UpdateListOfReservation()
        {
            using(var context= new CiteUContext())
            {
                ListOfReservation.Clear();
                foreach (var reservation in context.Reservations.ToList())
                {
                    ListOfReservation.Add(reservation);
                }
                MessageBox.Show(ListOfReservation[0].Date_Debut.ToString());
            }
        }*/
    }
}
