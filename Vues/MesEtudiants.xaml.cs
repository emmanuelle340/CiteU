using CiteU.Modele;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
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
using System.Runtime.Remoting.Contexts;


namespace CiteU.Vues
{
    public partial class MesEtudiants : UserControl
    {
        public CiteUContext context = new CiteUContext();
        public int TotalEtudiants => ListOfEtudiants.Count;

        // Ajoutez une propriété observable pour la liste d'étudiants
        public List<Etudiants> ListOfEtudiants { get; set; } = new List<Etudiants>();

        public MesEtudiants()
        {
            InitializeComponent();
            // Chargez la liste initiale d'étudiants
            LoadEtudiants();
        }

        private void LoadEtudiants()
        {
            // Assurez-vous de vider la liste avant de la remplir à nouveau
            ListOfEtudiants.Clear();

            // Chargez les étudiants depuis la base de données
            ListOfEtudiants = context.Etudiants.ToList();

            // Mettez à jour l'interface utilisateur en appelant NotifyPropertyChanged
            OnPropertyChanged(nameof(ListOfEtudiants));
            OnPropertyChanged(nameof(TotalEtudiants)); // Ajout de cette ligne
        }

        // ... (autres méthodes)

        // Méthode appelée lors du clic sur le bouton d'importation
        private void ImporterEtudiant_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                 string filePath = "MesEtudiants.txt";
                
                    var dernierEtudiant = context.Etudiants
                       .OrderByDescending(t => t.ID_Etudiant)
                       .FirstOrDefault();

                    string val1 = "NomEtudiant" + (dernierEtudiant.ID_Etudiant + 1) + ",PrenomEtudiant" + (dernierEtudiant.ID_Etudiant + 1) + ",2004-05-15,M,654879547,Etudiant" + (dernierEtudiant.ID_Etudiant + 1) + "@example.com,0";
                    string val2 = "NomEtudiant" + (dernierEtudiant.ID_Etudiant + 2) + ",PrenomEtudiant" + (dernierEtudiant.ID_Etudiant + 2) + ",1999-12-10,F,699584123,Etudiant" + (dernierEtudiant.ID_Etudiant + 2) + "@example.com,1";
                    string val3 = "NomEtudiant" + (dernierEtudiant.ID_Etudiant + 3) + ",PrenomEtudiant" + (dernierEtudiant.ID_Etudiant + 3) + ",2001-08-05,M,690152362,Etudiant" + (dernierEtudiant.ID_Etudiant + 3) + "@example.com,0;";

                    // Créer et initialiser les lignes de valeurs par défaut
                    List<string> lignesValeursParDefaut = new List<string>();
                    lignesValeursParDefaut.Add(val1);
                    lignesValeursParDefaut.Add(val2);
                    lignesValeursParDefaut.Add(val3);

                    // Écriture des lignes de valeurs par défaut dans le fichier
                    File.WriteAllLines(filePath, lignesValeursParDefaut);
             

                // Lecture des lignes existantes
                List<string> lignesExistantes = File.ReadAllLines(filePath).ToList();

                // Traitement des lignes existantes et ajout à la base de données
                foreach (string line in lignesExistantes)
                {
                    // Séparer les informations par des virgules
                    string[] data = line.Split(',');

                    if (data.Length == 7) // Vérifier si le nombre d'éléments est correct
                    {
                        // Créer un nouvel étudiant avec les informations
                        Etudiants nouvelEtudiant = new Etudiants
                        {
                            Nom = data[0].Trim(),
                            Prenom = data[1].Trim(),
                            // Utiliser TryParseExact pour gérer le format de date
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
                        //MessageBox.Show("Format de ligne incorrect : " + line, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }

                MessageBox.Show("Importation réussie", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                //MessageBox.Show("Données écrites avec succès dans le fichier", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
            
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de l'importation : " + ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Méthode pour notifier les changements aux liaisons de données
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }

        // Événement pour notifier les changements aux liaisons de données
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
    }
}

 