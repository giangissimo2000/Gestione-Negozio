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
    public partial class FondoCassaCat : Window
    {
        string percorso = "";
        public FondoCassaCat()
        {
            InitializeComponent();
            Verifica_Database();
            Read_Utenti();

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

        private void Read_Utenti()
        {
            try
            {
                DataTable dt = new DataTable();

                dt.Columns.Add("utente");
                string path = Directory.GetCurrentDirectory();
                string ConString = "Data Source=" + percorso + ";Version=3;";

                SQLiteConnection connection = new SQLiteConnection(ConString);
                SQLiteCommand command = connection.CreateCommand();
                SQLiteDataReader Reader;



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

        private bool IsNumberKey(Key inKey)
        {
            if (inKey < Key.D0 || inKey > Key.D9)
            {
                if (inKey < Key.NumPad0 || inKey > Key.NumPad9)
                {
                    return false;
                }
            }
            return true;
        }

        private bool IsActionKey(Key inKey)
        {
            return inKey == Key.Delete || inKey == Key.Back || inKey == Key.Return || Keyboard.Modifiers.HasFlag(ModifierKeys.Alt);


        }

        private void importo_block_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9-,]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Salva_Click(object sender, RoutedEventArgs e)
        {
            if (utenti_combo.Text != "")
            {
                if (descrizione_block.Text != "")
                {
                    if (importo_block.Text == "")

                    {
                        MessageBox.Show("Inserire importo!");
                    }
                    else
                    {
                        string number = importo_block.Text;
                        decimal number_;
                        if (!Decimal.TryParse(number, out number_))
                        {
                            MessageBox.Show("Importo non corretto!");
                        }

                        else
                        {
                            string data = DateTime.Now.ToString("yyyy/MM/dd");
                            string movimento = "";
                            string s = DateTime.Now.ToString("MMMM", new CultureInfo("it-IT"));
                            string mese = new CultureInfo("it-IT").TextInfo.ToTitleCase(s.ToUpper());
                            string descrizione = descrizione_block.Text;
                            string utente = utenti_combo.Text;
                            string importo = importo_block.Text;
                          //  decimal number1_;
                           /* if (Decimal.TryParse(importo, out number1_))
                            {
                                if (number1_ > 0) { movimento = "ENTRATA"; } else { movimento = "USCITA"; }

                            }*/
                            movimento = "USCITA";
                            aggiungi_fondo(data, mese, descrizione, importo, movimento, utente);
                            Application.Current.Properties["PassGate"] = mese;
                            this.Close();
                        }
                    }

                }
                else MessageBox.Show("Inserire descrizione!");
            }
            else MessageBox.Show("Selezionare operatore!");
        }






        private void aggiungi_fondo(string data, string mese, string descrizione, string importo, string movimento, string utente)
        {
            try
            {
                string path = Directory.GetCurrentDirectory();
                string gruppo = "USCITA CAT";
                SQLiteConnection aggiungi = new SQLiteConnection("Data Source=" + percorso + ";Version=3;");
                aggiungi.Open();
                descrizione = descrizione.Replace("'", "''");
                string sql = "insert into fondocat(data,mese,gruppo,descrizione,importo,tipo_mov,utente) values ('" + data + "','" + mese + "','" + gruppo + "','" + descrizione + "','" + importo + "','" + movimento + "','"  + utente + "')";
                // string sqlh = "update Prodotti set Giacenza ='" + quantitanew + "'  where Codice ='" + codice + "'";
                //insert into Prodotti (CodiceAAMS,Prezzo_pacchetto,Tipologia) values ( '" + CodiceAAMS + "','" + Prezzo_pacc + "','" + Tipologia + "')";//


                SQLiteCommand command = new SQLiteCommand(sql, aggiungi);
                command.ExecuteNonQuery();
                aggiungi.Close();

            }
            catch { }

        }

        private void Esci_Click(object sender, RoutedEventArgs e)
        {
            string s = DateTime.Now.ToString("MMMM", new CultureInfo("it-IT"));
            string mese = new CultureInfo("it-IT").TextInfo.ToTitleCase(s);
            Application.Current.Properties["PassGate"] = mese;
            this.Close();
        }
    }

}






    