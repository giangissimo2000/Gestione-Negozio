using IniParser;
using IniParser.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

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
    public partial class Modifica_Sospesi : Window
    {
        string id;
        string data3;
        string percorso = "";
        string user = "";
        string password = "";
        string dbname = "";
        string mese;
       
        string descrizione;
        string importo;
       
        string tipo;
        string utente;
        public Modifica_Sospesi()
        {
            InitializeComponent();
            Verifica_Database();
            Read_Utenti();
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
                user = data["Generale"]["User"];
                password = data["Generale"]["Password"];
                dbname = data["Generale"]["DatabaseName"];
            }
            catch
            {

                MessageBox.Show("Impossibile trovare il file Config.ini!");

            }


           

        }
        public void trova_id (string data, string mese,  string descrizione, string importo,  string utente)
        {

            try
            {
                
                    
                    string path = Directory.GetCurrentDirectory();
                    string ConString = "SERVER=" + percorso + ";" + "DATABASE=" + dbname + ";" + "UID=" + user + ";" + "PASSWORD=" + password + ";";

                MySqlConnection connection = new MySqlConnection(ConString);
                    MySqlCommand command = connection.CreateCommand();
                    MySqlDataReader Reader;

                descrizione = descrizione.Replace("'", "''");

                command.CommandText  = "select id from sospesi where data ='" + data + "' and mese='" + mese + "' and descrizione='" + descrizione + "' and importo='" + importo +  "' and utente='" + utente + "'";


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
            string data = Application.Current.Properties["data_mod"].ToString();
            data3 = DateTime.ParseExact(data, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy/MM/dd");
            mese = Application.Current.Properties["mese_mod"].ToString();
            
            descrizione = Application.Current.Properties["descrizione_mod"].ToString();
            descrizione_block.Text = Application.Current.Properties["descrizione_mod"].ToString();
            importo = Application.Current.Properties["importo_mod"].ToString();
            importo_block.Text = Application.Current.Properties["importo_mod"].ToString();
            
            
            utente = Application.Current.Properties["utente_mod"].ToString();
            utenti_combo.SelectedValue = Application.Current.Properties["utente_mod"].ToString();
            

            trova_id(data3, mese,  descrizione, importo,utente);

        }



        private void aggiorna_database(string data, string mese,  string descrizione, string importo,   string utente)
        {

            try
            {
                string path = Directory.GetCurrentDirectory();

                MySqlConnection modifica = new MySqlConnection("SERVER=" + percorso + ";" + "DATABASE=" + dbname + ";" + "UID=" + user + ";" + "PASSWORD=" + password + ";");
                modifica.Open();
                descrizione = descrizione.Replace("'", "''");
                string sql = "update sospesi set data ='" + data + "', mese='" + mese + "', descrizione='" + descrizione + "', importo='" + importo +   "', utente='" + utente + "'  where id='" + id + "'";
                // string sqlh = "update Prodotti set Giacenza ='" + quantitanew + "'  where Codice ='" + codice + "'";
                //insert into Prodotti (CodiceAAMS,Prezzo_pacchetto,Tipologia) values ( '" + CodiceAAMS + "','" + Prezzo_pacc + "','" + Tipologia + "')";//


                MySqlCommand command = new MySqlCommand(sql, modifica);
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
            
                }

                





        


        private void Read_Utenti()
        {
            try
            {
                DataTable dt = new DataTable();

                dt.Columns.Add("utente");
                string path = Directory.GetCurrentDirectory();
                string ConString = "SERVER=" + percorso + ";" + "DATABASE=" + dbname + ";" + "UID=" + user + ";" + "PASSWORD=" + password + ";";

                MySqlConnection connection = new MySqlConnection(ConString);
                MySqlCommand command = connection.CreateCommand();
                MySqlDataReader Reader;



                command.CommandText = "SELECT * FROM utente";


                connection.Open();
                Reader = command.ExecuteReader();
                if (Reader.HasRows)
                {

                    while (Reader.Read())
                    {
                        DataRow ne = dt.NewRow();
                        string utenti = Reader["nome"].ToString();
                        utenti_combo.Items.Add(utenti);
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

        private void importo_block_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

        private void importo_block_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9-,]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void modifica_btn_Click(object sender, RoutedEventArgs e)
        {
            if (utenti_combo.Text != "")
            {

                if (descrizione_block.Text != "")
                {
                    if (importo_block.Text == "")

                    {



                        MessageBox.Show("Inserire importo");
                    }
                    else
                    {





                        string number = importo_block.Text;
                        decimal number_;
                        if (!Decimal.TryParse(number, out number_))
                        {
                            MessageBox.Show("Importo non coretto!");
                        }

                        else
                        {




                            

                            string s = DateTime.Now.ToString("MMMM", new CultureInfo("it-IT"));
                            string mese = new CultureInfo("it-IT").TextInfo.ToTitleCase(s.ToUpper());
                            string descrizione = descrizione_block.Text;

                            string utente = utenti_combo.Text;
                            string importo = importo_block.Text;
                           // decimal number1_;
                           /* if (Decimal.TryParse(importo, out number1_))
                            {
                                if (number1_ > 0) { movimento = "ENTRATA"; } else { movimento = "USCITA"; }

                            }*/

                            




                            aggiorna_database(data3, mese, descrizione, importo, utente);
                            var myObject = this.Owner as MainWindow;
                            Application.Current.Properties["PassGate"] = mese;
                            // myObject.Read_Database(mese);
                            //  myObject.totale();
                            this.Close();
                        }


                    }

                }
                else MessageBox.Show("Digitare Importo!!");
            }
            else MessageBox.Show("Selezionare utente!!");
        }
    }
}
