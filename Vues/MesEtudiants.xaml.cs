using CiteU.Modele;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using CiteUContext = CiteU.Modele.CiteU;

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
            ListOfEtudiants = context.Etudiants.ToList();
        }

        private void ImporterEtudiant_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string filePath = "MesEtudiants.txt";

                var dernierEtudiant = context.Etudiants.OrderByDescending(t => t.ID_Etudiant).FirstOrDefault();

                string val1 = $"NomEtudiant{dernierEtudiant.ID_Etudiant + 1},PrenomEtudiant{dernierEtudiant.ID_Etudiant + 1},2004-05-15,M,654879547,Etudiant{dernierEtudiant.ID_Etudiant + 1}@example.com,0";
                string val2 = $"NomEtudiant{dernierEtudiant.ID_Etudiant + 2},PrenomEtudiant{dernierEtudiant.ID_Etudiant + 2},1999-12-10,F,699584123,Etudiant{dernierEtudiant.ID_Etudiant + 2}@example.com,1";
                string val3 = $"NomEtudiant{dernierEtudiant.ID_Etudiant + 3},PrenomEtudiant{dernierEtudiant.ID_Etudiant + 3},2001-08-05,M,690152362,Etudiant{dernierEtudiant.ID_Etudiant + 3}@example.com,0";

                List<string> lignesValeursParDefaut = new List<string> { val1, val2, val3 };

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
                    }
                    else
                    {
                        MessageBox.Show("Format de ligne incorrect : " + line, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }

                MessageBox.Show("Importation réussie", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
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
    }
}