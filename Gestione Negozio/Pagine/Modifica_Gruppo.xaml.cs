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
    /// Interaction logic for Modifica.xaml
    /// </summary>
    public partial class Modifica_Gruppo : Window
    {
        string percorso = "";
        string id;
        string gruppo;
        string gruppo_old;

        public Modifica_Gruppo()
        {
            InitializeComponent();
            Verifica_Database();
            leggi_tabella();
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
        public void trova_id (string gruppo)
        {

            try
            {
                
                    
                    string path = Directory.GetCurrentDirectory();
                    string ConString = "Data Source=" + percorso + ";Version=3;";

                    SQLiteConnection connection = new SQLiteConnection(ConString);
                    SQLiteCommand command = connection.CreateCommand();
                    SQLiteDataReader Reader;

                gruppo = gruppo.Replace("'", "''");

                command.CommandText  = "select id from gruppi where gruppi='" + gruppo + "'";


                    connection.Open();
                    Reader = command.ExecuteReader();
                    if (Reader.HasRows)
                    {

                        while (Reader.Read())
                        {
                            
                            id = Reader["id"].ToString();
                           
                        }

                        //gruppi_combo.ItemsSource = dt.DefaultView;
                    }
                    Reader.Close();




                }
                catch (Exception e)
                {

                    MessageBox.Show("ERRORE!: ", e.ToString());

                }
                
                

           




        }


        public void leggi_tabella()
        {
            var myObject = this.Owner as MainWindow;
            
            gruppo = Application.Current.Properties["gruppo"].ToString();
            gruppo_old = gruppo;
            gruppo_block.Text = gruppo;

            trova_id(gruppo);

        }



        private void aggiorna_database(string gruppo)
        {

            try
            {
                string path = Directory.GetCurrentDirectory();

                SQLiteConnection modifica = new SQLiteConnection("Data Source=" + percorso + ";Version=3;");
                modifica.Open();
                gruppo = gruppo.Replace("'", "''");
                string sql = "update gruppi set  gruppi='" + gruppo +   "' where id='" +id + "'";
                // string sqlh = "update Prodotti set Giacenza ='" + quantitanew + "'  where Codice ='" + codice + "'";
                //insert into Prodotti (CodiceAAMS,Prezzo_pacchetto,Tipologia) values ( '" + CodiceAAMS + "','" + Prezzo_pacc + "','" + Tipologia + "')";//


                SQLiteCommand command = new SQLiteCommand(sql, modifica);
                command.ExecuteNonQuery();
                modifica.Close();

            }
            catch { }

            aggiorna_database2(gruppo_old, gruppo, "quadernino");
            aggiorna_database2(gruppo_old, gruppo, "fondoposta");
            aggiorna_database2(gruppo_old, gruppo, "fondocat");

        }

        private void aggiorna_database2(string gruppo_old, string gruppo, string tabella)
        {

            try
            {
                string path = Directory.GetCurrentDirectory();

                SQLiteConnection modifica = new SQLiteConnection("Data Source=" + percorso + ";Version=3;");
                modifica.Open();
                gruppo = gruppo.Replace("'", "''");
                string sql = "update " + tabella + " set  gruppo='" + gruppo + "' where gruppo='" + gruppo_old + "'";
                // string sqlh = "update Prodotti set Giacenza ='" + quantitanew + "'  where Codice ='" + codice + "'";
                //insert into Prodotti (CodiceAAMS,Prezzo_pacchetto,Tipologia) values ( '" + CodiceAAMS + "','" + Prezzo_pacc + "','" + Tipologia + "')";//


                SQLiteCommand command = new SQLiteCommand(sql, modifica);
                command.ExecuteNonQuery();
                modifica.Close();

            }
            catch { }



        }


        private void Esci_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Modifica_Click(object sender, RoutedEventArgs e)
        {
            
                       

                            if (gruppo_block.Text == "")
                            {

                                MessageBox.Show("Inserire gruppo!");
                            }
                            else
                            {
                

                                    
                                   
                                    string gruppo = gruppo_block.Text;
                                    



                                    aggiorna_database( gruppo);
                                    var myObject = this.Owner as MainWindow;
                                    Application.Current.Properties["PassGate"] = gruppo;
                                    // myObject.Read_Database(mese);
                                    //  myObject.totale();
                                    this.Close();
                                }


                            }
                 

        


       
    }
}
