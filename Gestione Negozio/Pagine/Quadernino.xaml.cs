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

namespace Gestione_Studio.Pagine
{
    /// <summary>
    /// Interaction logic for Quadernino.xaml
    /// </summary>
    public partial class Quadernino : Page
    {
        string percorso = "";
        string user = "";
        string password = "";
        string dbname = "";

        public Quadernino()
        {
            InitializeComponent();
            Verifica_Database();
            string month = DateTime.Now.ToString("MMMM", new CultureInfo("it-IT"));
            string s = new CultureInfo("it-IT").TextInfo.ToTitleCase(month.ToUpper());
            comboBox.SelectedValue = s;
            Read_Database(s);
          //  ShowHideMenu("sbHideLeftMenu", btnLeftMenuHide, btnLeftMenuShow, pnlLeftMenu);
            totale();
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


        public void Read_Database(string nomemese)
        {
            try
            {
                string path = Directory.GetCurrentDirectory();
                Console.WriteLine(path);
                DataTable dt = new DataTable();

                dt.Columns.Add("data");

                dt.Columns.Add("mese");
                dt.Columns.Add("gruppo");
                dt.Columns.Add("descrizione");
                dt.Columns.Add("importo", typeof(decimal), null);
                dt.Columns.Add("tipo_mov");
                dt.Columns.Add("banca");
                dt.Columns.Add("utente");


                string ConString = "User id=" + user + ";Password=" + password + ";Server=" + percorso + ";" + "Database=" + dbname + ";";

                MySqlConnection connection = new MySqlConnection(ConString);
                MySqlCommand command = connection.CreateCommand();
                MySqlDataReader Reader;

                command.CommandText = "SELECT * FROM quadernino WHERE mese = '" + nomemese + "'";

                connection.Open();

                Reader = command.ExecuteReader();



                if (Reader.HasRows)
                {

                    while (Reader.Read())
                    {

                        DataRow ne = dt.NewRow();
                        string data = Reader["data"].ToString();
                        DateTime date = DateTime.ParseExact(data, "yyyy/MM/dd", new CultureInfo("it-IT"));
                        var date2 = date.ToShortDateString();


                        string mese = Reader["mese"].ToString();
                        string gruppo = Reader["gruppo"].ToString();
                        string descrizione = Reader["descrizione"].ToString();
                        string importo = Reader["importo"].ToString();
                        string tipo_mov = Reader["tipo_mov"].ToString();
                        string banca = Reader["banca"].ToString();
                        string utente = Reader["utente"].ToString();



                        ne["data"] = date.ToShortDateString();
                        ne["mese"] = mese;
                        ne["gruppo"] = gruppo;
                        ne["descrizione"] = descrizione;
                        ne["importo"] = importo;
                        ne["tipo_mov"] = tipo_mov;
                        ne["banca"] = banca;
                        ne["utente"] = utente;

                        dt.Rows.Add(ne);


                    }
                    DataSet ds = new DataSet("table");
                    ds.Tables.Add(dt);
                    quadernino_table.ItemsSource = ds.Tables["Table1"].DefaultView;
                  

                }
                Reader.Close();



            }
            catch (Exception e)
            {

                MessageBox.Show("ERRORE!: ", e.ToString());

            }
        }
        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {


            string month = ((ComboBoxItem)comboBox.SelectedItem).Content.ToString();
            Read_Database(month);
            totale();
        }

        public void totale()
        {
            List<string> myCollection = new List<string>();
            List<string> myCollection2 = new List<string>();
            List<string> myCollection3 = new List<string>();
            decimal sum = 0;
            decimal sum_varie = 0;
            decimal sum_banca = 0;
            decimal entrat = 0;
            decimal uscit = 0;
            decimal entrat_varie = 0;
            decimal uscit_varie = 0;
            decimal entrat_banca = 0;
            decimal uscit_banca = 0;
            for (int i = 0; i < quadernino_table.Items.Count; ++i)
            {
                //(decimal.Parse((tblData.SelectedCells[3].Column.GetCellContent(item) as TextBlock).Text))
                string el = ((quadernino_table.Items[i] as DataRowView).Row.ItemArray[6].ToString());
                if (el == "C")
                {
                    myCollection.Add(((quadernino_table.Items[i] as DataRowView).Row.ItemArray[4].ToString()));
                }

                if (el == "P")
                {
                    myCollection.Add(((quadernino_table.Items[i] as DataRowView).Row.ItemArray[4].ToString()));
                }

                if (el =="A")
                {
                    myCollection2.Add(((quadernino_table.Items[i] as DataRowView).Row.ItemArray[4].ToString()));

                }

                if (el == "B")
                {
                    myCollection3.Add(((quadernino_table.Items[i] as DataRowView).Row.ItemArray[4].ToString()));

                }

                if (el == "P")
                {
                    myCollection3.Add(((quadernino_table.Items[i] as DataRowView).Row.ItemArray[4].ToString()));

                }

                if (el == "BN")
                {
                    myCollection3.Add(((quadernino_table.Items[i] as DataRowView).Row.ItemArray[4].ToString()));

                }
                if (el == "BN")
                {
                    myCollection.Add(((quadernino_table.Items[i] as DataRowView).Row.ItemArray[4].ToString()));

                }
                if (el == "D")
                {
                    myCollection.Add(((quadernino_table.Items[i] as DataRowView).Row.ItemArray[4].ToString()));

                }

                if (el == "D")
                {
                    string val = ((quadernino_table.Items[i] as DataRowView).Row.ItemArray[4].ToString());
                    decimal val1 = (Convert.ToDecimal(val))*-1;

                    myCollection3.Add(val1.ToString());

                }
                //sum += (decimal.Parse((quadernino_table.Columns[3].GetCellContent(quadernino_table.Items[i]) as TextBlock).Text));
            }

            var myarray = myCollection.ToArray();
            var myarray2 = myCollection2.ToArray();
            var myarray3 = myCollection3.ToArray();

            for (int i = 0; i < myarray.Length; ++i)
            {

                if (Convert.ToDecimal(myarray[i]) > 0)
                {
                    entrat += Convert.ToDecimal(myarray[i]);
                }

                if (Convert.ToDecimal(myarray[i]) < 0)
                {
                    uscit += Convert.ToDecimal(myarray[i]);
                }

                sum += Convert.ToDecimal(myarray[i]);

            }

            for (int i = 0; i < myarray2.Length; ++i)
            {

                if (Convert.ToDecimal(myarray2[i]) > 0)
                {
                    entrat_varie += Convert.ToDecimal(myarray2[i]);
                }

                if (Convert.ToDecimal(myarray2[i]) < 0)
                {
                    uscit_varie += Convert.ToDecimal(myarray2[i]);
                }

                sum_varie += Convert.ToDecimal(myarray2[i]);

            }

            for (int i = 0; i < myarray3.Length; ++i)
            {

                if (Convert.ToDecimal(myarray3[i]) > 0)
                {
                    entrat_banca += Convert.ToDecimal(myarray3[i]);
                }

                if (Convert.ToDecimal(myarray3[i]) < 0)
                {
                    uscit_banca += Convert.ToDecimal(myarray3[i]);
                }

                sum_banca += Convert.ToDecimal(myarray3[i]);

            }

            in_total.Content =  "Entrate:    " + entrat.ToString("N", new CultureInfo("is-IS")) + " €";// Uscite: " + uscit.ToString("N", new CultureInfo("is-IS")) + " € Totale: " + sum.ToString("N", new CultureInfo("is-IS")) + " €" ;
            out_total.Content = "Uscite :    " + uscit.ToString("N", new CultureInfo("is-IS")) + "€";// Uscite Varie: " + uscit_varie.ToString("N", new CultureInfo("is-IS")) + "€ Totale Varie: " + sum_varie.ToString("N", new CultureInfo("is-IS")) + " €";
            total.Content =     "Totale :    " + sum.ToString("N", new CultureInfo("is-IS")) + " €";


            out_varie.Content = "Uscite :    " + uscit_varie.ToString("N", new CultureInfo("is-IS")) + "€";
            in_varie.Content = "Entrate:    " + entrat_varie.ToString("N", new CultureInfo("is-IS")) + "€";
            total_varie.Content = "Totale :    " + sum_varie.ToString("N", new CultureInfo("is-IS")) + " €";

            out_banca.Content = "Uscite :    " + uscit_banca.ToString("N", new CultureInfo("is-IS")) + "€";
            in_banca.Content = "Entrate:    " + entrat_banca.ToString("N", new CultureInfo("is-IS")) + "€";
            total_banca.Content = "Totale :    " + sum_banca.ToString("N", new CultureInfo("is-IS")) + " €";


        }

        private void quadernino_table_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            DataRowView item = e.Row.Item as DataRowView;
            if (item != null)
            {
                DataRow row = item.Row;

                decimal giac = Convert.ToDecimal(row["Importo"]);



                if (giac < 0)
                {

                    e.Row.Foreground = new SolidColorBrush(Colors.Red);

                    // Lista_Prodotti.Items.Add(cod + "        " + desc);

                }
                else e.Row.Foreground = new SolidColorBrush(Colors.Green);






            }
        }

        private void Cancella_Voce_Click(object sender, RoutedEventArgs e)
        {
            string mese = comboBox.SelectedValue.ToString();


            if (quadernino_table.SelectedIndex != -1)
            {

                MessageBoxResult resulto = MessageBox.Show("Vuoi eliminare la riga selezionata?", "Conferma eliminazione", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (resulto == MessageBoxResult.Yes)
                {




                    DataRowView drv = (DataRowView)quadernino_table.SelectedItem;

                    var currentRowIndex = quadernino_table.SelectedIndex;
                    string data1 = drv["data"].ToString();
                    //   DateTime data2 = Convert.ToDateTime(data1);
                    string data3 = DateTime.ParseExact(data1, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy/MM/dd");

                    string mese_del = drv["mese"].ToString();
                    string descrizione_del = drv["descrizione"].ToString();
                    string importo_del = drv["importo"].ToString();
                    drv.Row.Delete();
                    string path = Directory.GetCurrentDirectory();

                    MySqlConnection cancella = new MySqlConnection( "SERVER=" + percorso + ";" + "DATABASE=" + dbname + ";" + "UID=" + user + ";" + "PASSWORD=" + password + ";");
                    cancella.Open();
                    descrizione_del = descrizione_del.Replace("'", "''");
                    string sql = "delete from quadernino where data='" + data3 + "' and mese='" + mese_del + "' and descrizione='" + descrizione_del + "' and importo='" + importo_del + "'";



                    MySqlCommand command = new MySqlCommand(sql, cancella);
                    command.ExecuteNonQuery();
                    cancella.Close();

                    Read_Database(mese);
                    totale();

                }





            }
            else MessageBox.Show("Seleziona una riga da Eliminare!");
        }

        private void Nuova_voce_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string s = DateTime.Now.ToString("MMMM", new CultureInfo("it-IT"));
                string mese = new CultureInfo("it-IT").TextInfo.ToTitleCase(s.ToUpper());
                comboBox.SelectedValue =mese;

                Aggiungi aggiungi = new Aggiungi();
                aggiungi.ShowDialog();
                string vv = Application.Current.Properties["PassGate"].ToString();
                Read_Database(vv);
                totale();
                // Aggiungi win2 = new Aggiungi();
                // win2.
                // win2.Owner = this;
                // win2.Show();
            }
            catch { }
        }

        public void Modifica_Voce_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                
                string descrizione_mod = "";

                DataRowView rowview = quadernino_table.SelectedItem as DataRowView;

                Application.Current.Properties["data_mod"] = rowview.Row["data"].ToString();
                Application.Current.Properties["mese_mod"] = rowview.Row["mese"].ToString();
                Application.Current.Properties["gruppo_mod"] = rowview.Row["gruppo"].ToString();

               descrizione_mod = rowview.Row["descrizione"].ToString();
              //  descrizione_mod = descrizione_mod.Replace("", @"\'");
                //  descrizione_mod = descrizione_mod2.Replace("'", @"\'");
                Application.Current.Properties["descrizione_mod"] = descrizione_mod;
                Application.Current.Properties["importo_mod"] = rowview.Row["importo"].ToString();
                Application.Current.Properties["tipo_mod"] = rowview.Row["tipo_mov"].ToString();
                Application.Current.Properties["banca_mod"] = rowview.Row["banca"].ToString();
                Application.Current.Properties["utente_mod"] = rowview.Row["utente"].ToString();
                Modifica modifica = new Modifica();
                modifica.ShowDialog();
                
                string vv = Application.Current.Properties["PassGate"].ToString();
                if (vv != "")
                {
                    Read_Database(vv);
                    totale();
                }
            }
            catch
            {

                MessageBox.Show("Seleziona una riga da modificare!");

            }
        }

        private void riepilogo_btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Cerca_Voce_Click(object sender, RoutedEventArgs e)
        {
            NavigationService ns = NavigationService.GetNavigationService(this);
            ns.Navigate(new Uri("/Pagine/Ricerca.xaml", UriKind.Relative));
        }
    }
}
