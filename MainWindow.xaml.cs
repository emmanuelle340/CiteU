using CiteU.Modele;
using CiteU.Vues;
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

namespace CiteU
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            //deplacer dans Trace REservation
            using(var context= new CiteUContext())
            {
                var reservationsToMove = context.Reservations.Where(r => r.Date_Fin <= DateTime.Now).ToList();
                
                if (reservationsToMove.Count > 0)
                {
                    // Déplacer les réservations vers la table Trace_Reservations
                    foreach (var reservation in reservationsToMove)
                    {
                        var traceReservation = new Trace_ReservationSet
                        {
                            ID_Chambre =(int) reservation.ID_Chambre,
                            Date_Fin = reservation.Date_Fin.GetValueOrDefault(),
                            Lits_id = reservation.Lits_id,
                            Etudiants_ID_Etudiant = reservation.Etudiants_ID_Etudiant
                            // Ajoutez d'autres propriétés de Trace_ReservationSet au besoin
                        };
                        MessageBox.Show("je suis ici");
                        Lits lits,lit;
                        lits=context.Lits.FirstOrDefault(c=>c.id==reservation.Lits_id);
                        lit = new Lits
                        {
                            ChambresID_Chambre = (int) reservation.ID_Chambre
                        };
                        context.Lits.Add(lit);
                        context.Lits.Remove(lits);

                        // Ajouter à la table Trace_Reservations
                        context.Trace_ReservationSet.Add(traceReservation);

                        // Supprimer de la table Reservations
                        context.Reservations.Remove(reservation);
                    }

                    // Enregistrer les modifications dans la base de données
                    context.SaveChanges();

                    // Afficher un message
                    MessageBox.Show($"{reservationsToMove.Count} réservations ont été expirées.", "Notification",MessageBoxButton.OK,MessageBoxImage.Exclamation);
                }


            }
        }

        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            IconMesEtudiants.Foreground = new SolidColorBrush(Color.FromRgb(0x3C, 0x40, 0x48));
            IconMesPayements.Foreground = new SolidColorBrush(Color.FromRgb(0x3C, 0x40, 0x48));
            IconTrace.Foreground = new SolidColorBrush(Color.FromRgb(0x3C, 0x40, 0x48));
            IconHome.Foreground = new SolidColorBrush(Colors.White);
            ChangerCouleurBordure(btnHomeBorder);
            setActiveUserControl(Mesbatiments);
            

        }

        private void btnCreditCard_Click(object sender, RoutedEventArgs e)
        {
            IconHome.Foreground = new SolidColorBrush(Color.FromRgb(0x3C, 0x40, 0x48));
            IconMesEtudiants.Foreground = new SolidColorBrush(Color.FromRgb(0x3C, 0x40, 0x48));
            IconTrace.Foreground = new SolidColorBrush(Color.FromRgb(0x3C, 0x40, 0x48));
            IconMesPayements.Foreground = new SolidColorBrush(Colors.White);
            ChangerCouleurBordure(btnCreditCardBorder);
            setActiveUserControl(MesPayements);
        }


        private void btnMesEtudiants_Click(object sender, RoutedEventArgs e)
        {
            IconHome.Foreground = new SolidColorBrush(Color.FromRgb(0x3C, 0x40, 0x48));
            IconMesPayements.Foreground = new SolidColorBrush(Color.FromRgb(0x3C, 0x40, 0x48));
            IconTrace.Foreground = new SolidColorBrush(Color.FromRgb(0x3C, 0x40, 0x48));
            IconMesEtudiants.Foreground = new SolidColorBrush(Colors.White);
            ChangerCouleurBordure(btnMesEtudiantsBorder);
            setActiveUserControl(MesEtudiants);
        }


        public void setActiveUserControl(UserControl control)
        {
            Mesbatiments.Visibility = Visibility.Collapsed;
            MesEtudiants.Visibility = Visibility.Collapsed;
            MesChambres.Visibility = Visibility.Collapsed;
            MesPayements.Visibility = Visibility.Collapsed;
            MesTracesReservations.Visibility = Visibility.Collapsed;

            control.Visibility = Visibility.Visible;
            if (control == Mesbatiments)
            {
                MesChambres.Visibility= Visibility.Visible;
            }
        }

        public void ChangerCouleurBordure(Border border)
        {
            btnCreditCardBorder.Background = new SolidColorBrush(Colors.Transparent);
            btnMesEtudiantsBorder.Background = new SolidColorBrush(Colors.Transparent);
            btnHomeBorder.Background = new SolidColorBrush(Colors.Transparent);
            btnTrace.Background = new SolidColorBrush(Colors.Transparent);

            border.Background = new SolidColorBrush(Color.FromRgb(0x3C, 0x40, 0x48));
        }

        private void Trace_Click(object sender, RoutedEventArgs e)
        {
            IconHome.Foreground = new SolidColorBrush(Color.FromRgb(0x3C, 0x40, 0x48));
            IconMesPayements.Foreground = new SolidColorBrush(Color.FromRgb(0x3C, 0x40, 0x48));
            IconMesEtudiants.Foreground = new SolidColorBrush(Color.FromRgb(0x3C, 0x40, 0x48));
            IconTrace.Foreground = new SolidColorBrush(Colors.White);
            ChangerCouleurBordure(btnTrace);
            setActiveUserControl(MesTracesReservations);

        }
    }
}
