using CiteU.Modele;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;

namespace CiteU.Vues
{
    public class MesChambresViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Chambres> _listDeChambre;
        public ObservableCollection<Chambres> ListDeChambre
        {
            get { return _listDeChambre; }
            set
            {
                if (_listDeChambre != value)
                {
                    _listDeChambre = value;
                    OnPropertyChanged(nameof(ListDeChambre));
                }
            }
        }

        public MesChambresViewModel()
        {
            LoadData();
        }

        private void LoadData()
        {
            using (var context = new CiteU.Modele.CiteU())
            {
                string nomBatiment = "A";//(string)Application.Current.Properties["NomBatiment"];

                MessageBox.Show("NomBatiment: " + nomBatiment);

                // Logique pour charger les données avec filtre
                var query = context.Chambres
                    .Where(chambre => chambre.ID_Batiment == context.Batiments
                        .Where(batiment => batiment.Nom_Batiment == nomBatiment)
                        .Select(batiment => batiment.ID_Batiment)
                        .FirstOrDefault());

                //MessageBox.Show("Query result count: " + query.Count());

                ListDeChambre = new ObservableCollection<Chambres>(query.ToList());

                //MessageBox.Show("ListDeChambre Count: " + ListDeChambre?.Count);
            }
        }


        // Implémentez l'interface INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
