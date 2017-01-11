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
    public partial class Aggiungi_Gruppo : Window
    {

        string percorso = "";
        public Aggiungi_Gruppo()
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
        
        private void gruppo_block_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9-,]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Salva_Click(object sender, RoutedEventArgs e)
        {
            
                if (gruppo_block.Text == "")
                {
                    
                        MessageBox.Show("Inserire gruppo!");
                    }
                    else
                    {
                            string gruppo = gruppo_block.Text;
                            aggiungi_gruppo(gruppo);
                            this.Close();
                        }
                    }
        
        private void aggiungi_gruppo(string gruppo)
        {
            try
            {
                string path = Directory.GetCurrentDirectory();
                SQLiteConnection aggiungi = new SQLiteConnection("Data Source=" + percorso+ ";Version=3;");
                aggiungi.Open();
                gruppo = gruppo.Replace("'", "''");
                string sql = "insert into gruppi (gruppi) values ('"  + gruppo + "')";
                SQLiteCommand command = new SQLiteCommand(sql, aggiungi);
                command.ExecuteNonQuery();
                MessageBox.Show("Gruppo '" + gruppo + "' inserito!");
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






    