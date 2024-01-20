using CiteU.Modele;
using System;
using System.IO;
using System.Collections.Generic;
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
    /// Logique d'interaction pour MesEtudiants.xaml
    /// </summary>
    public partial class MesEtudiants : UserControl
    {
        public MesEtudiants()
        {
            InitializeComponent();
        }

        private void ImporterEtudiant_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string filePath = "MesEtudiants.txt"; // Chemin du fichier texte

                // Charger le fichier texte en tant que ressource
                var lines = File.ReadAllLines(filePath).ToList();
                var linesToRemove = new List<string>(); // Lignes à supprimer du fichier

                foreach (var line in lines)
                {
                    // Séparer les informations par des virgules
                    string[] data = line.Split(',');

                    if (data.Length == 7) // Vérifier si le nombre d'éléments est correct
                    {
                        MessageBox.Show(data[0]);
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

                        using (var context = new CiteUContext())
                        {
                            context.Etudiants.Add(nouvelEtudiant);
                            context.SaveChanges();

                            // Ajouter la ligne à supprimer du fichier
                            linesToRemove.Add(line);
                        }
                    }
                    else
                    {
                        //MessageBox.Show("Format de ligne incorrect : " + line, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }

                // Supprimer les lignes du fichier
                foreach (var lineToRemove in linesToRemove)
                {
                    lines.Remove(lineToRemove);
                }

                // Réécrire le fichier avec le nouveau contenu
                //File.WriteAllLines(filePath, lines.ToArray());

                MessageBox.Show("Importation réussie", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Erreur lors de l'importation : " + ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void UpdateTextFile(Uri uri, List<string> linesToRemove)
        {
            // Charger le contenu actuel du fichier
            var currentContent = File.ReadAllLines(uri.LocalPath).ToList();

            // Supprimer les lignes à supprimer
            currentContent.RemoveAll(line => linesToRemove.Contains(line));

            // Réécrire le fichier avec le nouveau contenu
            System.IO.File.WriteAllLines(uri.LocalPath, currentContent.ToArray());
        }



    }
}
