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
    public partial class Modifica : Window
    {
        string id;
        string data3;
        string data4;
        string percorso = "";
        string user = "";
        string password = "";
        string dbname = "";
        string mese;
        string gruppo;
        string descrizione;
        string importo;
        string banca;
        string tipo;
        string utente;
        public Modifica()
        {
            InitializeComponent();
            Verifica_Database();
            Read_Gruppi();
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
        public void trova_id (string data, string mese, string gruppo, string descrizione, string importo, string movimento, string banca, string utente)
        {

            try
            {
                
                    
                    string path = Directory.GetCurrentDirectory();
                    string ConString = "SERVER=" + percorso + ";" + "DATABASE=" + dbname + ";" + "UID=" + user + ";" + "PASSWORD=" + password + ";";

                    MySqlConnection connection = new MySqlConnection(ConString);
                    MySqlCommand command = connection.CreateCommand();
                    MySqlDataReader Reader;

                descrizione = descrizione.Replace("'", "''");

                command.CommandText  = "select id from quadernino where data ='" + data + "' and mese='" + mese + "' and gruppo='" + gruppo + "' and descrizione='" + descrizione + "' and importo='" + importo + "' and tipo_mov='" + movimento + "' and banca='" + banca + "' and utente='" + utente + "'";


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
            data4 = Application.Current.Properties["data_mod"].ToString();
            mese = Application.Current.Properties["mese_mod"].ToString();
            gruppo = Application.Current.Properties["gruppo_mod"].ToString();
            gruppi_combo.SelectedValue = Application.Current.Properties["gruppo_mod"];
            descrizione = Application.Current.Properties["descrizione_mod"].ToString();
            descrizione_block.Text = Application.Current.Properties["descrizione_mod"].ToString();
            importo = Application.Current.Properties["importo_mod"].ToString();
            importo_block.Text = Application.Current.Properties["importo_mod"].ToString();
            banca = Application.Current.Properties["banca_mod"].ToString();
            tipo  = Application.Current.Properties["tipo_mod"].ToString();
            utente = Application.Current.Properties["utente_mod"].ToString();
            utenti_combo.SelectedValue = Application.Current.Properties["utente_mod"].ToString();
            if (banca =="C") { fiscale.IsChecked = true; }
            if (banca == "A") { altro.IsChecked = true; }
            if (banca == "P") { pos.IsChecked = true; }
            if (banca == "B") { banca_c.IsChecked = true; }
            if (banca == "BN") { bonifico.IsChecked = true; }
            if (banca == "D") { deposito.IsChecked = true; }

            trova_id(data3, mese, gruppo, descrizione, importo, tipo, banca,utente);

        }



        private void aggiorna_database(string data, string mese, string gruppo, string descrizione, string importo, string movimento, string banca, string utente)
        {

            try
            {
                string path = Directory.GetCurrentDirectory();

                MySqlConnection modifica = new MySqlConnection("SERVER=" + percorso + ";" + "DATABASE=" + dbname + ";" + "UID=" + user + ";" + "PASSWORD=" + password + ";");
                modifica.Open();
                descrizione = descrizione.Replace("'", "''");
                string sql = "update quadernino set data ='" + data + "', mese='" + mese + "', gruppo='" + gruppo + "', descrizione='" + descrizione + "', importo='" + importo + "', tipo_mov='"  + movimento + "', banca='" + banca + "', utente='" + utente + "'  where id='" + id + "'";
                // string sqlh = "update Prodotti set Giacenza ='" + quantitanew + "'  where Codice ='" + codice + "'";
                //insert into Prodotti (CodiceAAMS,Prezzo_pacchetto,Tipologia) values ( '" + CodiceAAMS + "','" + Prezzo_pacc + "','" + Tipologia + "')";//


                MySqlCommand command = new MySqlCommand(sql, modifica);
                command.ExecuteNonQuery();
                modifica.Close();

            }
            catch { }



        }


        private void Read_Gruppi()
        {
            try
            {
                DataTable dt = new DataTable();

                dt.Columns.Add("gruppi");
                string path = Directory.GetCurrentDirectory();
                string ConString = "SERVER=" + percorso + ";" + "DATABASE=" + dbname + ";" + "UID=" + user + ";" + "PASSWORD=" + password + ";";

                MySqlConnection connection = new MySqlConnection(ConString);
                MySqlCommand command = connection.CreateCommand();
                MySqlDataReader Reader;



                command.CommandText = "SELECT * FROM gruppi";


                connection.Open();
                Reader = command.ExecuteReader();
                if (Reader.HasRows)
                {

                    while (Reader.Read())
                    {
                        DataRow ne = dt.NewRow();
                        string gruppi = Reader["gruppi"].ToString();
                        gruppi_combo.Items.Add(gruppi);
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

        private void Esci_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Properties["PassGate"] = "";
            this.Close();
        }

        private void Modifica_Click(object sender, RoutedEventArgs e)
        {
            if (utenti_combo.Text != "")
            {
                if (gruppi_combo.Text != "")
                {
                    if (descrizione_block.Text != "")
                    {
                        if (importo_block.Text != "")

                        {

                            if (fiscale.IsChecked.Value == false && altro.IsChecked.Value == false && banca_c.IsChecked.Value == false && pos.IsChecked==false && bonifico.IsChecked==false && deposito.IsChecked==false)
                            {

                                MessageBox.Show("Selezionare Conanti, Pos, Bonifico, Deposito, Banca o Altro!");
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




                                    string movimento = "";
                                    string banca = "";
                                    string s = DateTime.Now.ToString("MMMM", new CultureInfo("it-IT"));
                                    string mese1 = new CultureInfo("it-IT").TextInfo.ToTitleCase(s.ToUpper());
                                    //string mese = 
                                    string descrizione = descrizione_block.Text;
                                    string gruppo = gruppi_combo.Text;
                                    string utente = utenti_combo.Text;
                                    string importo = importo_block.Text;
                                    decimal number1_;
                                    if (Decimal.TryParse(importo, out number1_))
                                    {
                                        if (number1_ > 0) { movimento = "ENTRATA"; } else { movimento = "USCITA"; }

                                    }


                                    if (fiscale.IsChecked.Value == true) { banca = "C"; }
                                    if (altro.IsChecked.Value == true) { banca = "A"; }
                                    if (banca_c.IsChecked.Value == true) { banca = "B"; }
                                    if (pos.IsChecked.Value == true) { banca = "P"; }
                                    if (bonifico.IsChecked.Value == true) { banca = "BN"; }
                                    if (deposito.IsChecked.Value == true) { banca = "D"; }



                                    aggiorna_database(data3, mese, gruppo, descrizione, importo, movimento, banca, utente);
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
                    else MessageBox.Show("Digitare Descrizione!");
                }

                else MessageBox.Show("Selezionare gruppo!");

            }
            else MessageBox.Show("Selezionare utente!");





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
    }
}
