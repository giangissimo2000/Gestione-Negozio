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
    /// Interaction logic for Posta.xaml
    /// </summary>
    public partial class Sospesi : Page
    {
        string percorso = "";
        string user = "";
        string password = "";
        string dbname = "";
        public Sospesi()
        {
            InitializeComponent();
            Verifica_Database();
            string month = DateTime.Now.ToString("MMMM", new CultureInfo("it-IT"));
            string s = new CultureInfo("it-IT").TextInfo.ToTitleCase(month.ToUpper());
            comboBox.SelectedValue = s;
           // Read_Database(month);
            //  ShowHideMenu("sbHideLeftMenu", btnLeftMenuHide, btnLeftMenuShow, pnlLeftMenu);
          //  totale();
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

            DataTable dt = new DataTable();

            try
            {
                string path = Directory.GetCurrentDirectory();
                Console.WriteLine(path);
                

                dt.Columns.Add("data");

                dt.Columns.Add("mese");
                
                dt.Columns.Add("descrizione");
                dt.Columns.Add("importo", typeof(decimal), null);
               
                dt.Columns.Add("utente");

                string ConString = "SERVER=" + percorso + ";" + "DATABASE=" + dbname + ";" + "UID=" + user + ";" + "PASSWORD=" + password + ";";

                MySqlConnection connection = new MySqlConnection(ConString);
                MySqlCommand command = connection.CreateCommand();
                MySqlDataReader Reader;
                if (nomemese != "Anno")
                {
                    command.CommandText = "SELECT * FROM sospesi WHERE mese = '" + nomemese + "'";
                }

                else
                {

                    command.CommandText = "SELECT * FROM sospesi ";

                }
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
                       
                        string descrizione = Reader["descrizione"].ToString();
                        string importo1 = Reader["importo"].ToString();
                        decimal imp = Convert.ToDecimal(importo1);
                        string importo = Convert.ToString(imp);
                        
                        
                        string utente = Reader["utente"].ToString();



                        ne["data"] = date.ToShortDateString();
                        ne["mese"] = mese;
                        
                        ne["descrizione"] = descrizione;
                        ne["importo"] = importo;
                        
                        
                        ne["utente"] = utente;

                        dt.Rows.Add(ne);


                    }
                    DataSet dy = new DataSet("table");
                    dy.Tables.Add(dt);
                  

                }
                Reader.Close();


                DataTable dtAll = new DataTable();
                dtAll = dt.Copy();
                

                cat_table.ItemsSource = dtAll.DefaultView;




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
            
            decimal entrat = 0;
            decimal uscit = 0;
            for (int i = 0; i < cat_table.Items.Count; ++i)
            {
                
                    myCollection.Add(((cat_table.Items[i] as DataRowView).Row.ItemArray[3].ToString()));
                
            }

            var myarray2 = myCollection.ToArray();
            

            
            for (int i = 0; i < myarray2.Length; ++i)
            {

                
                
                    uscit += Convert.ToDecimal(myarray2[i]);
                

               // sum += Convert.ToDecimal(myarray2[i]);

            }

            in_total.Content = uscit.ToString("N", new CultureInfo("is-IS")) + " €";
          //  out_total.Content = entrat.ToString("N", new CultureInfo("is-IS")) + " €";
          //  total.Content = ((Convert.ToDecimal(uscit) - Convert.ToDecimal(entrat)).ToString("N", new CultureInfo("is-IS")) + " €");

                //.ToString("N", new CultureInfo("is-IS")) + " €";


        }


        private void Nuovo_fondocassa_Click(object sender, RoutedEventArgs e)
        {
            string s = DateTime.Now.ToString("MMMM", new CultureInfo("it-IT"));
            string mese = new CultureInfo("it-IT").TextInfo.ToTitleCase(s.ToUpper());
            comboBox.SelectedValue = mese;

            Aggiungi_Sospeso Aggiungi_Sospeso = new Aggiungi_Sospeso();
            Aggiungi_Sospeso.ShowDialog();
            string vv = Application.Current.Properties["PassGate"].ToString();
            Read_Database(vv);
            totale();
        }

        private void Cancella_Voce_Click(object sender, RoutedEventArgs e)
        {
            string mese = comboBox.SelectedValue.ToString();
            DataRowView drv1 = (DataRowView)cat_table.SelectedItem;
            
            
                if (cat_table.SelectedIndex != -1)
                {

                    MessageBoxResult resulto = MessageBox.Show("Vuoi eliminare la riga selezionata?", "Conferma eliminazione", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (resulto == MessageBoxResult.Yes)
                    {




                        DataRowView drv = (DataRowView)cat_table.SelectedItem;

                        var currentRowIndex = cat_table.SelectedIndex;
                        string data1 = drv["data"].ToString();
                        //   DateTime data2 = Convert.ToDateTime(data1);
                        string data3 = DateTime.ParseExact(data1, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy/MM/dd");

                        string mese_del = drv["mese"].ToString();
                        string descrizione_del = drv["descrizione"].ToString();
                        descrizione_del = descrizione_del.Replace("'", "''");
                        string importo_del = drv["importo"].ToString();
                        drv.Row.Delete();
                        string path = Directory.GetCurrentDirectory();

                        MySqlConnection cancella = new MySqlConnection("SERVER=" + percorso + ";" + "DATABASE=" + dbname + ";" + "UID=" + user + ";" + "PASSWORD=" + password + ";");
                        cancella.Open();
                        string sql = "delete from sospesi where data='" + data3 + "' and mese='" + mese_del + "' and descrizione='" + descrizione_del + "' and importo='" + importo_del + "'";



                        MySqlCommand command = new MySqlCommand(sql, cancella);
                        command.ExecuteNonQuery();
                        cancella.Close();

                        Read_Database(mese);
                        totale();

                    }





                }
                else MessageBox.Show("Seleziona una riga da Eliminare!");
            
        }

        private void Modifica_Voce_Click(object sender, RoutedEventArgs e)
        {
            try
            {


                string mese = comboBox.SelectedValue.ToString();
                DataRowView drv1 = (DataRowView)cat_table.SelectedItem;
                
                




                    string descrizione_mod = "";

                    DataRowView rowview = cat_table.SelectedItem as DataRowView;

                    Application.Current.Properties["data_mod"] = rowview.Row["data"].ToString();
                    Application.Current.Properties["mese_mod"] = rowview.Row["mese"].ToString();


                    descrizione_mod = rowview.Row["descrizione"].ToString();
                    Application.Current.Properties["descrizione_mod"] = descrizione_mod;
                    Application.Current.Properties["importo_mod"] = rowview.Row["importo"].ToString();
                    
                    Application.Current.Properties["utente_mod"] = rowview.Row["utente"].ToString();
                    Modifica_Sospesi Modifica_Sospesi = new Modifica_Sospesi();
                Modifica_Sospesi.ShowDialog();

                    //  string vv = Application.Current.Properties["PassGate"].ToString();
                    Read_Database(mese);
                    totale();
                

                

            }
            catch
            {

                MessageBox.Show("Seleziona una riga da modificare!");

            }
        }

        private void posta_table_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            DataRowView item = e.Row.Item as DataRowView;
            if (item != null)
            {
                DataRow row = item.Row;
                e.Row.Foreground = new SolidColorBrush(Colors.Red);

                

                






            }
        }

        private void Nuovo_fondocat_Click(object sender, RoutedEventArgs e)
        {
            string s = DateTime.Now.ToString("MMMM", new CultureInfo("it-IT"));
            string mese = new CultureInfo("it-IT").TextInfo.ToTitleCase(s);
            comboBox.SelectedValue = mese.ToUpper();

            Aggiungi_Sospeso Aggiungi_Sospseso = new Aggiungi_Sospeso();
            Aggiungi_Sospseso.ShowDialog();
            string vv = Application.Current.Properties["PassGate"].ToString();
            Read_Database(vv);
            totale();
        }
    }
}
