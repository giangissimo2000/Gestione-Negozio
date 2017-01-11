using IniParser;
using IniParser.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Gestione_Studio
{
    
    /// <summary>
    /// Interaction logic for Aggiungi.xaml
    /// </summary>
    public partial class Aggiungi_Utente : Window
    {
            string percorso = "";


        public Aggiungi_Utente()
        {
            InitializeComponent();
            Verifica_Database();
            
        }

        private void Verifica_Database()
        {
            try
            {
                string path = Directory.GetCurrentDirectory();
                var parser = new FileIniDataParser();
                IniData data = parser.ReadFile(path + "\\" + "Config.ini");
                percorso = data["Generale"]["Percorso"];
            }
            catch
            {

                MessageBox.Show("Impossibile trovare il file Config.ini!");

            }


            try
            {
                if (File.Exists(percorso))
                {
                    percorso = percorso.Replace(@"\\", @"\\\");
                }
                else
                {
                    MessageBox.Show("Impossibile trovare il Database!");
                }
            }
            catch
            {
                MessageBox.Show("Impossibile trovare il Database!");

            }

        }
        private void importo_block_KeyDown(object sender, KeyEventArgs e)
        {

        }
        
        private void utente_block_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9-,]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Salva_Click(object sender, RoutedEventArgs e)
        {
            
                if (utente_block.Text == "")
                {
                    
                        MessageBox.Show("Inserire utente!");
                    }
                    else
                    {
                            string utente = utente_block.Text;
                            aggiungi_utente(utente);
                            this.Close();
                        }
                    }
        
        private void aggiungi_utente(string utente)
        {
            try
            {
                string path = Directory.GetCurrentDirectory();
                SQLiteConnection aggiungi = new SQLiteConnection("Data Source=" + percorso + ";Version=3;");
                aggiungi.Open();
                utente = utente.Replace("'", "''");
                string sql = "insert into utente (nome) values ('"  + utente + "')";
                SQLiteCommand command = new SQLiteCommand(sql, aggiungi);
                command.ExecuteNonQuery();
                MessageBox.Show("Utente '" + utente + "' inserito!");
                aggiungi.Close();
            }
            catch { }

        }

        private void Esci_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }

}






    