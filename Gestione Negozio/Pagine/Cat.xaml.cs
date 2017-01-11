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
    public partial class Cat : Page
    {
        string percorso = "";
        public Cat()
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
        public void Read_Database(string nomemese)
        {

            DataTable dt = new DataTable();

            try
            {
                string path = Directory.GetCurrentDirectory();
                Console.WriteLine(path);
                

                dt.Columns.Add("data");

                dt.Columns.Add("mese");
                dt.Columns.Add("gruppo");
                dt.Columns.Add("descrizione");
                dt.Columns.Add("importo", typeof(decimal), null);
                dt.Columns.Add("tipo_mov");
                dt.Columns.Add("banca");
                dt.Columns.Add("utente");

                string ConString = "Data Source=" + percorso + ";Version=3;";

                SQLiteConnection connection = new SQLiteConnection(ConString);
                SQLiteCommand command = connection.CreateCommand();
                SQLiteDataReader Reader;
                if (nomemese != "Anno")
                {
                    command.CommandText = "SELECT * FROM quadernino WHERE mese = '" + nomemese + "' and gruppo='GESTIONE DIRITTI'";
                }

                else
                {

                    command.CommandText = "SELECT * FROM quadernino WHERE  gruppo='GESTIONE DIRITTI'";

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
                        string gruppo = Reader["gruppo"].ToString();
                        string descrizione = Reader["descrizione"].ToString();
                        string importo1 = Reader["importo"].ToString();
                        decimal imp = Convert.ToDecimal(importo1);
                        string importo = Convert.ToString(imp * (-1));
                        string tipo_mov = Reader["tipo_mov"].ToString();
                        
                        string utente = Reader["utente"].ToString();



                        ne["data"] = date.ToShortDateString();
                        ne["mese"] = mese;
                        ne["gruppo"] = gruppo;
                        ne["descrizione"] = descrizione;
                        ne["importo"] = importo;
                        ne["tipo_mov"] = tipo_mov;
                        
                        ne["utente"] = utente;

                        dt.Rows.Add(ne);


                    }
                    DataSet dy = new DataSet("table");
                    dy.Tables.Add(dt);
                  

                }
                Reader.Close();







           }
            catch (Exception e)
            {

                MessageBox.Show("ERRORE!: ", e.ToString());

            }
            DataTable ds = new DataTable();
            try
            {
                string path = Directory.GetCurrentDirectory();
                Console.WriteLine(path);
                ;

                ds.Columns.Add("data");

                ds.Columns.Add("mese");
                ds.Columns.Add("gruppo");
                ds.Columns.Add("descrizione");
                ds.Columns.Add("importo", typeof(decimal), null);
                ds.Columns.Add("tipo_mov");
               
                ds.Columns.Add("utente");

                string ConString = "Data Source=" + percorso + ";Version=3;";

                SQLiteConnection connection = new SQLiteConnection(ConString);
                SQLiteCommand command = connection.CreateCommand();
                SQLiteDataReader Reader;

                if (nomemese != "Anno")
                {
                    command.CommandText = "SELECT * FROM fondocat WHERE mese = '" + nomemese + "' and gruppo='USCITA CAT'";
                }


                else
                {
                    command.CommandText = "SELECT * FROM fondocat WHERE  gruppo='USCITA CAT'";

                }
                connection.Open();

                Reader = command.ExecuteReader();



                if (Reader.HasRows)
                {

                    while (Reader.Read())
                    {

                        DataRow ne = ds.NewRow();
                        string data = Reader["data"].ToString();
                        DateTime date = DateTime.ParseExact(data, "yyyy/MM/dd", new CultureInfo("it-IT"));
                        var date2 = date.ToShortDateString();


                        string mese = Reader["mese"].ToString();
                        string gruppo = Reader["gruppo"].ToString();
                        string descrizione = Reader["descrizione"].ToString();
                        string importo = Reader["importo"].ToString();
                        string tipo_mov = Reader["tipo_mov"].ToString();

                        string utente = Reader["utente"].ToString();



                        ne["data"] = date.ToShortDateString();
                        ne["mese"] = mese;
                        ne["gruppo"] = gruppo;
                        ne["descrizione"] = descrizione;
                        ne["importo"] = importo;
                        ne["tipo_mov"] = tipo_mov;

                        ne["utente"] = utente;

                        ds.Rows.Add(ne);


                    }
                    DataSet df = new DataSet("table");
                    df.Tables.Add(ds);
                    
                }
                Reader.Close();

                DataTable dtAll = new DataTable();
                dtAll = dt.Copy();
                dtAll.Merge(ds, true);

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
                string group = (cat_table.Items[i] as DataRowView).Row.ItemArray[2].ToString();
                if (group == "GESTIONE DIRITTI")
                {
                    myCollection.Add(((cat_table.Items[i] as DataRowView).Row.ItemArray[4].ToString()));
                }

                if (group == "USCITA CAT")
                {
                    myCollection2.Add(((cat_table.Items[i] as DataRowView).Row.ItemArray[4].ToString()));
                }
            }

            var myarray2 = myCollection.ToArray();
            var myarray = myCollection2.ToArray();

            for (int i = 0; i < myarray.Length; ++i)
            {

                
                
                    entrat += Convert.ToDecimal(myarray[i]);
                
            }
            for (int i = 0; i < myarray2.Length; ++i)
            {

                
                
                    uscit += Convert.ToDecimal(myarray2[i]);
                

               // sum += Convert.ToDecimal(myarray2[i]);

            }

            in_total.Content = uscit.ToString("N", new CultureInfo("is-IS")) + " €";
            out_total.Content = entrat.ToString("N", new CultureInfo("is-IS")) + " €";
            total.Content = ((Convert.ToDecimal(uscit) - Convert.ToDecimal(entrat)).ToString("N", new CultureInfo("is-IS")) + " €");

                //.ToString("N", new CultureInfo("is-IS")) + " €";


        }


        private void Nuovo_fondocassa_Click(object sender, RoutedEventArgs e)
        {
            string s = DateTime.Now.ToString("MMMM", new CultureInfo("it-IT"));
            string mese = new CultureInfo("it-IT").TextInfo.ToTitleCase(s.ToUpper());
            comboBox.SelectedValue = mese;

            FondoCassaPosta FondoCassaPosta = new FondoCassaPosta();
            FondoCassaPosta.ShowDialog();
            string vv = Application.Current.Properties["PassGate"].ToString();
            Read_Database(vv);
            totale();
        }

        private void Cancella_Voce_Click(object sender, RoutedEventArgs e)
        {
            string mese = comboBox.SelectedValue.ToString();
            DataRowView drv1 = (DataRowView)cat_table.SelectedItem;
            string data2 = drv1["gruppo"].ToString();
            if (data2 == "USCITA CAT")
            {
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

                        SQLiteConnection cancella = new SQLiteConnection("Data Source=" + percorso + ";Version=3;");
                        cancella.Open();
                        string sql = "delete from fondocat where data='" + data3 + "' and mese='" + mese_del + "' and descrizione='" + descrizione_del + "' and importo='" + importo_del + "'";



                        SQLiteCommand command = new SQLiteCommand(sql, cancella);
                        command.ExecuteNonQuery();
                        cancella.Close();

                        Read_Database(mese);
                        totale();

                    }





                }
                else MessageBox.Show("Seleziona una riga da Eliminare!");
            }
            else MessageBox.Show("Puoi eliminare solo le righe USCITA CAT!");
        }

        private void Modifica_Voce_Click(object sender, RoutedEventArgs e)
        {
            try
            {


                string mese = comboBox.SelectedValue.ToString();
                DataRowView drv1 = (DataRowView)cat_table.SelectedItem;
                string data2 = drv1["gruppo"].ToString();
                if (data2 == "USCITA CAT")
                {




                    string descrizione_mod = "";

                    DataRowView rowview = cat_table.SelectedItem as DataRowView;

                    Application.Current.Properties["data_mod"] = rowview.Row["data"].ToString();
                    Application.Current.Properties["mese_mod"] = rowview.Row["mese"].ToString();


                    descrizione_mod = rowview.Row["descrizione"].ToString();
                    Application.Current.Properties["descrizione_mod"] = descrizione_mod;
                    Application.Current.Properties["importo_mod"] = rowview.Row["importo"].ToString();
                    Application.Current.Properties["tipo_mod"] = rowview.Row["tipo_mov"].ToString();
                    Application.Current.Properties["utente_mod"] = rowview.Row["utente"].ToString();
                    Modifica_Fondocassacat modifica_fondocassacat = new Modifica_Fondocassacat();
                    modifica_fondocassacat.ShowDialog();

                    //  string vv = Application.Current.Properties["PassGate"].ToString();
                    Read_Database(mese);
                    totale();
                }

                else
                { MessageBox.Show("Puoi modificare solo le righe USCITA CAT!"); }

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


                if( row["gruppo"].ToString() == "GESTIONE DIRITTI")
                { e.Row.Foreground = new SolidColorBrush(Colors.Black); }
                else
                { e.Row.Foreground = new SolidColorBrush(Colors.Red); }

                






            }
        }

        private void Nuovo_fondocat_Click(object sender, RoutedEventArgs e)
        {
            string s = DateTime.Now.ToString("MMMM", new CultureInfo("it-IT"));
            string mese = new CultureInfo("it-IT").TextInfo.ToTitleCase(s);
            comboBox.SelectedValue = mese.ToUpper();

            FondoCassaCat FondoCassaCat = new FondoCassaCat();
            FondoCassaCat.ShowDialog();
            string vv = Application.Current.Properties["PassGate"].ToString();
            Read_Database(vv);
            totale();
        }
    }
}
