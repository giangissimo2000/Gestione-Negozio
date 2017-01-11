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
    public partial class Modifica_Utente : Window
    {
        string id;
        string utente;
        string utente_old;
        string percorso = "";
      
        public Modifica_Utente()
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
        public void trova_id (string utente)
        {

            try
            {
                
                    
                    string path = Directory.GetCurrentDirectory();
                    string ConString = "Data Source=" + percorso + ";Version=3;";

                    SQLiteConnection connection = new SQLiteConnection(ConString);
                    SQLiteCommand command = connection.CreateCommand();
                    SQLiteDataReader Reader;
                utente = utente.Replace("'", "''");


                command.CommandText  = "select id from utente where nome='" + utente + "'";


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
            
           utente = Application.Current.Properties["utente"].ToString();
            utente_old = utente;
            utente_block.Text = utente;

            trova_id(utente);
            

        }



        private void aggiorna_database(string utente)
        {

            try
            {
                string path = Directory.GetCurrentDirectory();

                SQLiteConnection modifica = new SQLiteConnection("Data Source=" + percorso + ";Version=3;");
                modifica.Open();
                utente = utente.Replace("'", "''");
                string sql = "update utente set  nome='" + utente +   "' where id='" +id + "'";
                // string sqlh = "update Prodotti set Giacenza ='" + quantitanew + "'  where Codice ='" + codice + "'";
                //insert into Prodotti (CodiceAAMS,Prezzo_pacchetto,Tipologia) values ( '" + CodiceAAMS + "','" + Prezzo_pacc + "','" + Tipologia + "')";//


                SQLiteCommand command = new SQLiteCommand(sql, modifica);
                command.ExecuteNonQuery();
                modifica.Close();

            }
            catch { }

            aggiorna_database2(utente_old, utente, "quadernino");
            aggiorna_database2(utente_old, utente, "fondoposta");
            aggiorna_database2(utente_old, utente, "fondocat");

        }

        private void aggiorna_database2(string utente_old,string utente, string tabella)
        {

            try
            {
                string path = Directory.GetCurrentDirectory();

                SQLiteConnection modifica = new SQLiteConnection("Data Source=" + percorso + ";Version=3;");
                modifica.Open();
                utente = utente.Replace("'", "''");
                string sql = "update " + tabella + " set  utente='" + utente + "' where utente='" + utente_old + "'";
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
            
                       

                            if (utente_block.Text == "")
                            {

                                MessageBox.Show("Inserire utente!");
                            }
                            else
                            {
                

                                    
                                   
                                    string utente = utente_block.Text;
                                    



                                    aggiorna_database( utente);
                                    var myObject = this.Owner as MainWindow;
                                    Application.Current.Properties["PassGate"] = utente;
                                    // myObject.Read_Database(mese);
                                    //  myObject.totale();
                                    this.Close();
                                }


                            }
                 

        


       
    }
}
