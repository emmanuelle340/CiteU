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
        }

        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            IconMesEtudiants.Foreground = new SolidColorBrush(Color.FromRgb(0x3C, 0x40, 0x48));
            IconMesPayements.Foreground = new SolidColorBrush(Color.FromRgb(0x3C, 0x40, 0x48));
            IconHome.Foreground = new SolidColorBrush(Colors.White);
            ChangerCouleurBordure(btnHomeBorder);
            setActiveUserControl(Mesbatiments);

        }

        private void btnCreditCard_Click(object sender, RoutedEventArgs e)
        {
            IconHome.Foreground = new SolidColorBrush(Color.FromRgb(0x3C, 0x40, 0x48));
            IconMesEtudiants.Foreground = new SolidColorBrush(Color.FromRgb(0x3C, 0x40, 0x48));
            IconMesPayements.Foreground = new SolidColorBrush(Colors.White);
            ChangerCouleurBordure(btnCreditCardBorder);
            setActiveUserControl(MesPayements);
        }


        private void btnMesEtudiants_Click(object sender, RoutedEventArgs e)
        {
            IconHome.Foreground = new SolidColorBrush(Color.FromRgb(0x3C, 0x40, 0x48));
            IconMesPayements.Foreground = new SolidColorBrush(Color.FromRgb(0x3C, 0x40, 0x48));
            IconMesEtudiants.Foreground = new SolidColorBrush(Colors.White);
            ChangerCouleurBordure(btnMesEtudiantsBorder);
            setActiveUserControl(MesEtudiants);
        }


        public void setActiveUserControl(UserControl control)
        {
            Mesbatiments.Visibility = Visibility.Collapsed;
            MesEtudiants.Visibility = Visibility.Collapsed;

            control.Visibility = Visibility.Visible;
        }
        public void ChangerCouleurBordure(Border border)
        {
            btnCreditCardBorder.Background = new SolidColorBrush(Colors.Transparent);
            btnMesEtudiantsBorder.Background = new SolidColorBrush(Colors.Transparent);
            btnHomeBorder.Background = new SolidColorBrush(Colors.Transparent);

            border.Background = new SolidColorBrush(Color.FromRgb(0x3C, 0x40, 0x48));
        }

        
    }
}
