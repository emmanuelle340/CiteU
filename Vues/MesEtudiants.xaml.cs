using CiteU.Modele;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Effects;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using CiteUContext =CiteU.Modele.CiteU;

namespace CiteU.Vues
{
    public partial class MesEtudiants : UserControl, INotifyPropertyChanged
    {
        private CiteUContext context = new CiteUContext();
        private List<Etudiants> listOfEtudiants = new List<Etudiants>();

        public List<Etudiants> ListOfEtudiants
        {
            get { return listOfEtudiants; }
            set
            {
                listOfEtudiants = value;
                OnPropertyChanged(nameof(ListOfEtudiants));
                OnPropertyChanged(nameof(TotalEtudiants));
            }
        }

        public int TotalEtudiants => ListOfEtudiants.Count;

        public MesEtudiants()
        {
            InitializeComponent();
            LoadEtudiants();
            DataContext = this;
        }

        private void LoadEtudiants()
        {
            ListOfEtudiants = context.Etudiants.OrderByDescending(e => e.ID_Etudiant).ToList();
        }


        private void ImporterEtudiant_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string filePath = "MesEtudiants.txt";

                var dernierEtudiant = context.Etudiants.OrderByDescending(t => t.ID_Etudiant).FirstOrDefault();
                int id;
                if (dernierEtudiant == null)
                {
                    id = 0;
                }
                else
                {
                    id = dernierEtudiant.ID_Etudiant;
                }

                string val1 = $"NomEtudiant{id + 1},PrenomEtudiant{id + 1},2004-05-15,M,654879547,Etudiant{id + 1}@example.com,0";
                string val2 = $"NomEtudiant{id + 2},PrenomEtudiant {id + 2},1999-12-10,F,699584123,Etudiant {id + 2}@example.com,1";
                string val3 = $"NomEtudiant{id + 3},PrenomEtudiant{id + 3},2001-08-05,M,690152362,Etudiant{id + 3}@example.com,1";
                string val4 = $"NomEtudiant{id + 4},PrenomEtudiant{id + 4},2001-08-05,F,690152362,Etudiant{id + 4}@example.com,0";

                List<string> lignesValeursParDefaut = new List<string> { val1, val2, val3, val4 };

                File.WriteAllLines(filePath, lignesValeursParDefaut);

                List<string> lignesExistantes = File.ReadAllLines(filePath).ToList();

                foreach (string line in lignesExistantes)
                {
                    string[] data = line.Split(',');

                    if (data.Length == 7)
                    {
                        Etudiants nouvelEtudiant = new Etudiants
                        {
                            Nom = data[0].Trim(),
                            Prenom = data[1].Trim(),
                            Date_Naissance = DateTime.TryParseExact(data[2].Trim(), "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out DateTime date) ? date : (DateTime?)null,
                            Sexe = data[3].Trim(),
                            Telephone = data[4].Trim(),
                            Email = data[5].Trim(),
                            Handicape = int.TryParse(data[6].Trim(), out int handicape) ? handicape : 0
                        };

                        ListOfEtudiants.Add(nouvelEtudiant);
                        context.Etudiants.Add(nouvelEtudiant);
                        context.SaveChanges();
                        LoadEtudiants();
                    }
                    else
                    {
                        MessageBox.Show("Format de ligne incorrect : " + line, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }

                MessageBox.Show("Vous venez d'importer 4 nouveaux etudiants ", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de l'importation : " + ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Etudiants selectedEtudiant = (sender as FrameworkElement)?.DataContext as Etudiants;

            if (selectedEtudiant != null)
            {
                EtudiansDetails etudiantsDetailsWindow = new EtudiansDetails(selectedEtudiant);

                Window mainWindow = Window.GetWindow(this);

                
                    // Définir la fenêtre principale comme propriétaire de la boîte de dialogue
                    etudiantsDetailsWindow.Owner = Window.GetWindow(this);

                
                

                // Positionner la fenêtre modale par rapport à la fenêtre principale
                if (etudiantsDetailsWindow.Owner != null)
                {
                    etudiantsDetailsWindow.Left = etudiantsDetailsWindow.Owner.Left + 750; // Décalage vers la droite
                    etudiantsDetailsWindow.Top = etudiantsDetailsWindow.Owner.Top + 170;  // Décalage vers le bas
                }

                etudiantsDetailsWindow.Closed += (s, args) =>
                {

                };

                // Afficher la fenêtre modale en utilisant ShowDialog()
                bool? result = etudiantsDetailsWindow.ShowDialog();
                

                // Vous pouvez ajouter du code ici pour traiter le résultat si nécessaire
            }
        }


    }
}