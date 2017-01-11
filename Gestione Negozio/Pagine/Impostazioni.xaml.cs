using ClosedXML.Excel;
using IniParser;
using IniParser.Model;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Interaction logic for Impostazioni.xaml
    /// </summary>
    public partial class Impostazioni : Page
    {
        int i = 0;
        
        ClosedXML.Excel.XLWorkbook wbook = new ClosedXML.Excel.XLWorkbook();
        ClosedXML.Excel.XLWorkbook wbook_posta = new ClosedXML.Excel.XLWorkbook();
        ClosedXML.Excel.XLWorkbook wbook_cat = new ClosedXML.Excel.XLWorkbook();
        BackgroundWorker bgw = new BackgroundWorker();
        BackgroundWorker bgw2 = new BackgroundWorker();
        BackgroundWorker bgw3 = new BackgroundWorker();
        string percorso = "";
        string percorso2 = "";

        public Impostazioni()
        {
            InitializeComponent();
            progress_tab.Visibility = Visibility.Hidden;
            bgw.DoWork += new DoWorkEventHandler(Quadernino);
            bgw.ProgressChanged += new ProgressChangedEventHandler(bgw_ProgressChanged);
            bgw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgw_RunWorkerCompleted);
            bgw.WorkerReportsProgress = true;

            bgw2.DoWork += new DoWorkEventHandler(Posta);
            bgw2.ProgressChanged += new ProgressChangedEventHandler(Posta_ProgressChanged);
            bgw2.RunWorkerCompleted += new RunWorkerCompletedEventHandler(Posta_RunWorkerCompleted);
            bgw2.WorkerReportsProgress = true;

            bgw3.DoWork += new DoWorkEventHandler(Cat);
            bgw3.ProgressChanged += new ProgressChangedEventHandler(Cat_ProgressChanged);
            bgw3.RunWorkerCompleted += new RunWorkerCompletedEventHandler(Cat_RunWorkerCompleted);
            bgw3.WorkerReportsProgress = true;


            Leggi_INI();
            Leggi_percorso();
            List_gruppi();
            List_utenti();
        }

        


        private void Leggi_INI()
        {
            string path = Directory.GetCurrentDirectory();
            var parser = new FileIniDataParser();
            IniData data = parser.ReadFile(path + "\\" + "Config.ini");
            percorso = data["Generale"]["Percorso"];



        }

        private void List_gruppi()
        {
            try
            {

                Listbox_gruppi.Items.Clear();
                string path = Directory.GetCurrentDirectory();
                Console.WriteLine(path);
                DataTable dt = new DataTable();

                dt.Columns.Add("gruppi");
                percorso = percorso.Replace(@"\\", @"\\\");
                string ConString = "Data Source=" + percorso + ";Version=3;";
                SQLiteConnection connection = new SQLiteConnection(ConString);
                SQLiteCommand command = connection.CreateCommand();
                SQLiteDataReader Reader;
                command.CommandText = "SELECT * FROM gruppi";
                connection.Open();
                Reader = command.ExecuteReader();
                if (Reader.HasRows)
                {
                  while (Reader.Read())
                    {
                      Listbox_gruppi.Items.Add(Reader["gruppi"].ToString());
                      Listbox_gruppi.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription("", System.ComponentModel.ListSortDirection.Ascending));
                    }
                   
                }
                Reader.Close();
            }
            catch (Exception e)
            {

                MessageBox.Show("ERRORE!: ", e.ToString());

            }

        }

        private void Cancella_Gruppo_Click(object sender, RoutedEventArgs e)
        {
            string gruppo = Listbox_gruppi.SelectedValue.ToString();


            if (Listbox_gruppi.SelectedIndex != -1)
            {

                MessageBoxResult resulto = MessageBox.Show("Vuoi eliminare il gruppo selezionato?", "Conferma eliminazione", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (resulto == MessageBoxResult.Yes)
                {




                  //  DataRowView drv = (DataRowView)Listbox_gruppi.SelectedItem;

                  //  var currentRowIndex = Listbox_gruppi.SelectedIndex;
                    
                  //  string gruppo_del = drv["gruppo"].ToString();
                    
                 //   drv.Row.Delete();
                    string path = Directory.GetCurrentDirectory();
                    percorso = percorso.Replace(@"\\", @"\\\");
                    SQLiteConnection cancella = new SQLiteConnection("Data Source=" + percorso + ";Version=3;");
                    cancella.Open();
                    string sql = "delete from gruppi where gruppi='" + gruppo + "'";



                    SQLiteCommand command = new SQLiteCommand(sql, cancella);
                    command.ExecuteNonQuery();
                    cancella.Close();

                    List_gruppi();
                    

                }





            }
            else MessageBox.Show("Seleziona una riga da Eliminare!");
        }

        private void Cancella_Utente_Click(object sender, RoutedEventArgs e)
        {
            string utente = Listbox_utenti.SelectedValue.ToString();


            if (Listbox_utenti.SelectedIndex != -1)
            {

                MessageBoxResult resulto = MessageBox.Show("Vuoi eliminare il gruppo selezionato?", "Conferma eliminazione", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (resulto == MessageBoxResult.Yes)
                {




                    //  DataRowView drv = (DataRowView)Listbox_gruppi.SelectedItem;

                    //  var currentRowIndex = Listbox_gruppi.SelectedIndex;

                    //  string gruppo_del = drv["gruppo"].ToString();

                    //   drv.Row.Delete();
                    string path = Directory.GetCurrentDirectory();
                    percorso = percorso.Replace(@"\\", @"\\\");
                    SQLiteConnection cancella = new SQLiteConnection("Data Source=" + percorso + ";Version=3;");
                    cancella.Open();
                    string sql = "delete from utente where nome='" + utente + "'";



                    SQLiteCommand command = new SQLiteCommand(sql, cancella);
                    command.ExecuteNonQuery();
                    cancella.Close();

                    List_utenti();


                }





            }
            else MessageBox.Show("Seleziona una riga da Eliminare!");
        }

        private void List_utenti()
        {
            try
            {
                Listbox_utenti.Items.Clear();
                string path = Directory.GetCurrentDirectory();
                Console.WriteLine(path);
                percorso = percorso.Replace(@"\\", @"\\\");
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


                        Listbox_utenti.Items.Add(Reader["nome"].ToString());
                        Listbox_utenti.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription("", System.ComponentModel.ListSortDirection.Ascending));

                    }

                }
                Reader.Close();

            }
            catch (Exception e)
            {

                MessageBox.Show("ERRORE!: ", e.ToString());

            }

        }

        private void Aggiungi_gruppi_Click(object sender, RoutedEventArgs e)
        {
            Aggiungi_Gruppo aggiungi_gruppo = new Aggiungi_Gruppo();
            aggiungi_gruppo.ShowDialog();
            // string vv = Application.Current.Properties["PassGate"].ToString();
            List_gruppi();
        }

        private void Aggiungi_utenti_Click(object sender, RoutedEventArgs e)
        {
            Aggiungi_Utente aggiungi_utenti = new Aggiungi_Utente();
            aggiungi_utenti.ShowDialog();
            // string vv = Application.Current.Properties["PassGate"].ToString();
            List_utenti();
        }

        private void Modifica_Gruppo_Click(object sender, RoutedEventArgs e)
        {
            try
            {


                

                string gruppo = Listbox_gruppi.SelectedValue.ToString();

                Application.Current.Properties["gruppo"] = gruppo;
                
                Modifica_Gruppo modifica_gruppo = new Modifica_Gruppo();
                modifica_gruppo.ShowDialog();

                string vv = Application.Current.Properties["PassGate"].ToString();
                List_gruppi();
            }
            catch
            {

                MessageBox.Show("Seleziona un gruppo da modificare!");

            }
        }

        private void Leggi_percorso()
        {
            percorso_database_txt.Text = percorso;



        }


        private void Modifica_utenti_Click(object sender, RoutedEventArgs e)
        {
            try
            {




                string utente = Listbox_utenti.SelectedValue.ToString();

                Application.Current.Properties["utente"] = utente;

                Modifica_Utente modifica_utente = new Modifica_Utente();
                modifica_utente.ShowDialog();

                string vv = Application.Current.Properties["PassGate"].ToString();
                List_utenti();
            }
            catch
            {

                MessageBox.Show("Seleziona un utente da modificare!");

            }
        }

        private void Scegli_persorso_database_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new Microsoft.Win32.OpenFileDialog() { Filter = "SQLite Files (*.sqlite) |*.sqlite"};
            var result = ofd.ShowDialog();
            if (result == false) return;
            percorso_database_txt.Text = ofd.FileName;
        }


        private void aggiungi_percorso(string voce, string valore)
        {
           

        }



        private void Salva_persorso_database_Click(object sender, RoutedEventArgs e)
        {
            if (percorso_database_txt.Text != "")
            {

                percorso = percorso_database_txt.Text;
                string path = Directory.GetCurrentDirectory();
                var parser = new FileIniDataParser();
                IniData data = parser.ReadFile(path + "\\" + "Config.ini");
                data["Generale"]["Percorso"] = percorso;
                parser.WriteFile("Config.ini", data);

            }
            else
            {

                MessageBox.Show("Seleziona il percorso!");
            }


        }

        private void backup_database_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dialog = new CommonOpenFileDialog();
                dialog.IsFolderPicker = true;
               
                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    Console.WriteLine(dialog.FileName.ToString());
                    string s = DateTime.Now.ToString("dd-MM-yy_HHmmss", new CultureInfo("it-IT"));
                    File.Copy(percorso, dialog.FileName.ToString() + "\\backup_gestione_studio_" + s + ".dat");
                }
            }
            catch
            {
                MessageBox.Show("Backup non eseguito! Riprovare");

            }
        }

        private void ripristina_database_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var ofd = new Microsoft.Win32.OpenFileDialog() { Filter = "Dat Files (*.dat) |*.dat" };
                var result = ofd.ShowDialog();
                if (result == false) return;
                string ripristino = ofd.FileName.ToString();

                if (MessageBox.Show("Ripristinando il backup, il database attuale verra' cancellato! proseguo?", "Ripristino Database", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                {
                    //do no stuff
                }
                else
                {
                    File.Copy(ripristino, percorso, true);
                    MessageBox.Show("Ripristino eseguito con successo!");
                }
            }
            catch
            {
                MessageBox.Show("Ripristino FALLITO! Riprovare!");

            }
           



        }

        private void azzera(string tabella)
        {
            SQLiteConnection cancella = new SQLiteConnection("Data Source=" + percorso + ";Version=3;");
            cancella.Open();
            string sql = "DELETE FROM " + tabella + " WHERE id > -1";



            SQLiteCommand command = new SQLiteCommand(sql, cancella);
            command.ExecuteNonQuery();



        }

        private void elimina_database_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MessageBox.Show("ATTENZIONE!! Il Database verrà azzerato! Proseguo?", "Reset Database", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                {
                    //do no stuff
                }
                else
                {
                    azzera("quadernino");
                    azzera("fondocat");
                    azzera("fondoposta");
                    azzera("fondocassafiscale");
                    MessageBox.Show("Database azzerato con successo!!");
                }
            }
            catch
            {

                MessageBox.Show("Azzeramento FALLITO! Riprovare");

            }
        }


        public void Riepilogo()
        {



            try
            {



                string path = Directory.GetCurrentDirectory();
                Console.WriteLine(path);
                DataTable dt = new DataTable();

                dt.Columns.Add("Gruppo");
                dt.Columns.Add("Gennaio", typeof(decimal), null);
                dt.Columns.Add("Febbraio", typeof(decimal), null);
                dt.Columns.Add("Marzo", typeof(decimal), null);
                dt.Columns.Add("Aprile", typeof(decimal), null);
                dt.Columns.Add("Maggio", typeof(decimal), null);
                dt.Columns.Add("Giugno", typeof(decimal), null);
                dt.Columns.Add("Luglio", typeof(decimal), null);
                dt.Columns.Add("Agosto", typeof(decimal), null);
                dt.Columns.Add("Settembre", typeof(decimal), null);
                dt.Columns.Add("Ottobre", typeof(decimal), null);
                dt.Columns.Add("Novembre", typeof(decimal), null);
                dt.Columns.Add("Dicembre", typeof(decimal), null);
                dt.Columns.Add("TOTALE", typeof(decimal), null);

                string ConString = "Data Source=" + percorso + ";Version=3;";

                SQLiteConnection connection = new SQLiteConnection(ConString);
                SQLiteCommand command = connection.CreateCommand();
                SQLiteDataReader Reader;
                command.CommandText = "select gruppo,  REPLACE(sum(CAST(REPLACE(case when[mese] = 'GENNAIO' then importo else 0 end, ',', '.')AS REAL)), '.', ',') Gennaio,  REPLACE(sum(CAST(REPLACE(case when[mese] = 'FEBBRAIO' then importo else 0 end, ',', '.')AS REAL)), '.', ',') Febbraio,  REPLACE(sum(CAST(REPLACE(case when[mese] = 'MARZO' then importo else 0 end, ',', '.')AS REAL)), '.', ',') Marzo,  REPLACE(sum(CAST(REPLACE(case when[mese] = 'APRILE' then importo else 0 end, ',', '.')AS REAL)), '.', ',') Aprile,  REPLACE(sum(CAST(REPLACE(case when[mese] = 'MAGGIO' then importo else 0 end, ',', '.')AS REAL)), '.', ',') Maggio,  REPLACE(sum(CAST(REPLACE(case when[mese] = 'GIUGNO' then importo else 0 end, ',', '.')AS REAL)), '.', ',') Giugno,  REPLACE(sum(CAST(REPLACE(case when[mese] = 'LUGLIO' then importo else 0 end, ',', '.')AS REAL)), '.', ',') Luglio,  REPLACE(sum(CAST(REPLACE(case when[mese] = 'AGOSTO' then importo else 0 end, ',', '.')AS REAL)), '.', ',') Agosto,  REPLACE(sum(CAST(REPLACE(case when[mese] = 'SETTEMBRE' then importo else 0 end, ',', '.')AS REAL)), '.', ',') Settembre,  REPLACE(sum(CAST(REPLACE(case when[mese] = 'OTTOBRE' then importo else 0 end, ',', '.')AS REAL)), '.', ',') Ottobre,  REPLACE(sum(CAST(REPLACE(case when[mese] = 'NOVEMBRE' then importo else 0 end, ',', '.')AS REAL)), '.', ',') Novembre,  REPLACE(sum(CAST(REPLACE(case when[mese] = 'DICEMBRE' then importo else 0 end, ',', '.')AS REAL)), '.', ',') Dicembre from quadernino group by gruppo;";

                connection.Open();

                Reader = command.ExecuteReader();



                if (Reader.HasRows)
                {

                    while (Reader.Read())
                    {

                        DataRow ne = dt.NewRow();
                        string gruppo = Reader["gruppo"].ToString();



                        string gennaio = Reader["Gennaio"].ToString();
                        string febbraio = Reader["Febbraio"].ToString();
                        string marzo = Reader["Marzo"].ToString();
                        string aprile = Reader["Aprile"].ToString();
                        string maggio = Reader["Maggio"].ToString();
                        string giugno = Reader["Giugno"].ToString();
                        string luglio = Reader["Luglio"].ToString();
                        string agosto = Reader["Agosto"].ToString();
                        string settembre = Reader["Settembre"].ToString();
                        string ottobre = Reader["Ottobre"].ToString();
                        string novembre = Reader["Novembre"].ToString();
                        string dicembre = Reader["Dicembre"].ToString();



                        ne["gruppo"] = gruppo;
                        ne["gennaio"] = gennaio;
                        ne["febbraio"] = febbraio;
                        ne["marzo"] = marzo;
                        ne["aprile"] = aprile;
                        ne["maggio"] = maggio;
                        ne["giugno"] = giugno;
                        ne["luglio"] = luglio;
                        ne["agosto"] = agosto;
                        ne["settembre"] = settembre;
                        ne["ottobre"] = ottobre;
                        ne["novembre"] = novembre;
                        ne["dicembre"] = dicembre;

                        decimal tot = Convert.ToDecimal(gennaio) + Convert.ToDecimal(febbraio) + Convert.ToDecimal(marzo) + Convert.ToDecimal(aprile) + Convert.ToDecimal(maggio) + Convert.ToDecimal(giugno) + Convert.ToDecimal(luglio) + Convert.ToDecimal(agosto) + Convert.ToDecimal(settembre) + Convert.ToDecimal(ottobre) + Convert.ToDecimal(novembre) + Convert.ToDecimal(dicembre);
                        ne["TOTALE"] = tot.ToString();



                        dt.Rows.Add(ne);


                    }


                }
                Reader.Close();










                DataRow ned = dt.NewRow();
                foreach (DataColumn col in dt.Columns)
                {

                    decimal colTotal = 0;
                    foreach (DataRow row in col.Table.Rows)
                    {
                        if (row[0].ToString() != "01-PRATICHE")

                        {
                            decimal number;
                            if (Decimal.TryParse(row[col].ToString(), out number))
                                colTotal += Decimal.Parse(row[col].ToString());
                            else
                            {
                                //colTotal = "TOTALE USCITE";
                            }
                        }


                    }

                    ned[col.ColumnName] = colTotal;
                    ned[0] = "TOTALE USCITE";
                }
                dt.Rows.Add(ned);
                wbook.Worksheets.Add(dt, "RIEPILOGO");

               
                var ws1 = wbook.Worksheet("RIEPILOGO");
                ws1.Tables.FirstOrDefault().ShowAutoFilter = false;

                ws1.Range("B2:N200").Columns().Style.NumberFormat.Format = "€ #,##0.00;[Red]-€ #,##0.00";
                ws1.Range("B2:N200").DataType = XLCellValues.Number;


                ws1.Columns().AdjustToContents();
                //  export(dt, "RIEPILOGO");





                // riepilogo_table.ItemsSource = ds.Tables["Table1"].DefaultView;

            }
            catch { }
        }

       

        public void Quadernino(object sender, DoWorkEventArgs e)
        {
            var backgroundWorker = sender as BackgroundWorker;
            string[] array_mese = { "GENNAIO", "FEBBRAIO", "MARZO","APRILE","MAGGIO","GIUGNO","LUGLIO","AGOSTO","SETTEMBRE","OTTOBRE","NOVEMBRE","DICEMBRE" };
           
            foreach (string mese_scelto in array_mese)
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

                
                    string ConString = "Data Source=" + percorso + ";Version=3;";




                    SQLiteConnection connection = new SQLiteConnection(ConString);
                    SQLiteCommand command = connection.CreateCommand();
                    SQLiteDataReader Reader;

                    command.CommandText = "SELECT * FROM quadernino where mese='"+mese_scelto + "'";

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
                            
                            i++;
                            var progress = i / 10;
                            backgroundWorker.ReportProgress(progress);
                            
                            
                        }

                        export_quadernino(dt, mese_scelto);
                        // quadernino_table.ItemsSource = ds.Tables["Table1"].DefaultView;


                    }
                    Reader.Close();
                   
                }

            
            catch 
            {

                MessageBox.Show("ERRORE!: ", e.ToString());

            }


                

            }
            Riepilogo();
wbook.SaveAs(percorso2 + "\\" + "Quadernino.xlsx");
            
        }

        public void Posta(object sender, DoWorkEventArgs e)
        {

            

            var backgroundWorker2 = sender as BackgroundWorker;

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
              //  dt.Columns.Add("tipo_mov");
              //  dt.Columns.Add("banca");
                dt.Columns.Add("utente");

                string ConString = "Data Source=" + percorso + ";Version=3;";

                SQLiteConnection connection = new SQLiteConnection(ConString);
                SQLiteCommand command = connection.CreateCommand();
                SQLiteDataReader Reader;
                

                    command.CommandText = "SELECT * FROM quadernino WHERE  gruppo='POSTA'";

                
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
                      //  string tipo_mov = Reader["tipo_mov"].ToString();

                        string utente = Reader["utente"].ToString();



                        ne["data"] = date.ToShortDateString();
                        ne["mese"] = mese;
                        ne["gruppo"] = gruppo;
                        ne["descrizione"] = descrizione;
                        ne["importo"] = importo;
                    //    ne["tipo_mov"] = tipo_mov;

                        ne["utente"] = utente;

                        dt.Rows.Add(ne);
                        i++;
                        var progress = i / 10;
                        backgroundWorker2.ReportProgress(progress);

                    }
                    DataSet dy = new DataSet("table");
                    dy.Tables.Add(dt);


                }
                Reader.Close();







            }
            catch 
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
              //  ds.Columns.Add("tipo_mov");

                ds.Columns.Add("utente");

                string ConString = "Data Source=" + percorso + ";Version=3;";

                SQLiteConnection connection = new SQLiteConnection(ConString);
                SQLiteCommand command = connection.CreateCommand();
                SQLiteDataReader Reader;

                
                    command.CommandText = "SELECT * FROM fondoposta WHERE  gruppo='FONDO CASSA POSTA'";

                
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
                     //   string tipo_mov = Reader["tipo_mov"].ToString();

                        string utente = Reader["utente"].ToString();



                        ne["data"] = date.ToShortDateString();
                        ne["mese"] = mese;
                        ne["gruppo"] = gruppo;
                        ne["descrizione"] = descrizione;
                        ne["importo"] = importo;
                   //     ne["tipo_mov"] = tipo_mov;

                        ne["utente"] = utente;

                        ds.Rows.Add(ne);

                        i++;
                        var progress = i / 10;
                        backgroundWorker2.ReportProgress(progress);
                    }
                    DataSet df = new DataSet("table");
                    df.Tables.Add(ds);

                }
                Reader.Close();

                DataTable dtAll = new DataTable();
                dtAll = dt.Copy();
                dtAll.Merge(ds, true);

                // posta_table.ItemsSource = dtAll.DefaultView;

                export_posta(dtAll);
              //  wbook_posta.SaveAs("Posta.xlsx");

            }
            catch (Exception )
            {

                MessageBox.Show("ERRORE!: ", e.ToString());

            }
            wbook_posta.SaveAs(percorso2 + "\\" + "Posta.xlsx");
        }
        public void Cat(object sender, DoWorkEventArgs e)
        {
            var backgroundWorker3 = sender as BackgroundWorker;
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
               // dt.Columns.Add("tipo_mov");
               // dt.Columns.Add("banca");
                dt.Columns.Add("utente");

                string ConString = "Data Source=" + percorso + ";Version=3;";

                SQLiteConnection connection = new SQLiteConnection(ConString);
                SQLiteCommand command = connection.CreateCommand();
                SQLiteDataReader Reader;
                

                    command.CommandText = "SELECT * FROM quadernino WHERE  gruppo='GESTIONE DIRITTI'";

               
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
                    //    string tipo_mov = Reader["tipo_mov"].ToString();

                        string utente = Reader["utente"].ToString();



                        ne["data"] = date.ToShortDateString();
                        ne["mese"] = mese;
                        ne["gruppo"] = gruppo;
                        ne["descrizione"] = descrizione;
                        ne["importo"] = importo;
                    //    ne["tipo_mov"] = tipo_mov;

                        ne["utente"] = utente;

                        dt.Rows.Add(ne);


                    }
                    DataSet dy = new DataSet("table");
                    dy.Tables.Add(dt);
                    i++;
                    var progress = i / 10;
                    backgroundWorker3.ReportProgress(progress);

                }
                Reader.Close();







            }
            catch 
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
              //  ds.Columns.Add("tipo_mov");

                ds.Columns.Add("utente");

                string ConString = "Data Source=" + percorso + ";Version=3;";

                SQLiteConnection connection = new SQLiteConnection(ConString);
                SQLiteCommand command = connection.CreateCommand();
                SQLiteDataReader Reader;

               
                    command.CommandText = "SELECT * FROM fondocat WHERE  gruppo='USCITA CAT'";

                
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
                 //       string tipo_mov = Reader["tipo_mov"].ToString();

                        string utente = Reader["utente"].ToString();



                        ne["data"] = date.ToShortDateString();
                        ne["mese"] = mese;
                        ne["gruppo"] = gruppo;
                        ne["descrizione"] = descrizione;
                        ne["importo"] = importo;
                      //  ne["tipo_mov"] = tipo_mov;

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

                //  cat_table.ItemsSource = dtAll.DefaultView;
                //  export(dtAll, "CAT");
                export_cat(dtAll);


            }
            catch 
            {

                MessageBox.Show("ERRORE!: ", e.ToString());

            }
            wbook_cat.SaveAs(percorso2 + "\\" + "Cat.xlsx");
        }
        public void export_cat(DataTable tabella)
        {

            wbook_cat.Worksheets.Add(tabella, "Cat");

            var ws3 = wbook_cat.Worksheet("Cat");
            ws3.Tables.FirstOrDefault().ShowAutoFilter = false;
            ws3.Range("E2:E2000").Columns().Style.NumberFormat.Format = "€ #,##0.00;[Red]-€ #,##0.00";
            ws3.Range("E2:E2000").DataType = XLCellValues.Number;
            ws3.Range("F2:F2000").AddConditionalFormat().WhenContains("USCITA").Font.FontColor = XLColor.Red;
            int NumberOfLastRow = ws3.LastRowUsed().RowNumber();
            IXLCell CellForNewData2 = ws3.Cell(NumberOfLastRow + 1, 1);

            ws3.Cell("J2").Value = "TOTALE USCITE SU FATTURA";
            var entrate2 = ws3.Cell("K2");
            entrate2.FormulaA1 = "=SUMIF(C2:C" + NumberOfLastRow + ",\"*GESTIONE*\",E2:E" + NumberOfLastRow + ")";

            ws3.Cell("J3").Value = "TOTALE USCITE C/O CAT";
            var uscite2 = ws3.Cell("K3");
            uscite2.FormulaA1 = "=SUMIF(C2:C" + NumberOfLastRow + ",\"*USCITA*\",E2:E" + NumberOfLastRow + ")";
            // ws1.Cell(NumberOfLastRow + 1, 4).Value = "1000";

            int NumberOfLastRow2 = ws3.LastRowUsed().RowNumber();
            ws3.Cell("J4").Value = "RESIDUO";
            var residuo2 = ws3.Cell("K4");
            residuo2.FormulaR1C1 = "=(K2-K3)";
            ws3.Cell("K2").Style.NumberFormat.Format = "€ #,##0.00;[Red]-€ #,##0.00";
            ws3.Cell("K3").Style.NumberFormat.Format = "€ #,##0.00;-€ #,##0.00";
            ws3.Cell("K4").Style.NumberFormat.Format = "€ #,##0.00;[Red]-€ #,##0.00";
            ws3.Cell("K2").DataType = XLCellValues.Number;
            ws3.Cell("K3").DataType = XLCellValues.Number;
            ws3.Cell("K4").DataType = XLCellValues.Number;

            // ws1.Cell(NumberOfLastRow + 1, 6).Value = "1000";
            ws3.Columns().AdjustToContents();
            for (int u = 1; u <= NumberOfLastRow; u++)
            {
              //  var row = ws3.Row(u);
                // string cell = row.Cell(3).Value.ToString();
                string cell = ws3.Cell(u, 3).Value.ToString();
                if (cell == "USCITA CAT")
                {
                    var range4 = ws3.Range("A" + u, "F" +u);
                    range4.Style.Fill.BackgroundColor = XLColor.BabyBlue;
                    range4.Style.Font.SetBold(true);

                }


            }

        }
        public void export_posta(DataTable tabella)
        {
            
            wbook_posta.Worksheets.Add(tabella,"Posta");

            var ws2 = wbook_posta.Worksheet("Posta");
            ws2.Tables.FirstOrDefault().ShowAutoFilter = false;
            ws2.Range("E2:E2000").Columns().Style.NumberFormat.Format = "€ #,##0.00;[Red]-€ #,##0.00";
            ws2.Range("E2:E2000").DataType = XLCellValues.Number;
            ws2.Range("F2:F2000").AddConditionalFormat().WhenContains("USCITA").Font.FontColor = XLColor.Red;
            int NumberOfLastRow = ws2.LastRowUsed().RowNumber();
            IXLCell CellForNewData = ws2.Cell(NumberOfLastRow + 1, 1);

            ws2.Cell("J2").Value = "TOTALE ENTRATE";
            var entrate = ws2.Cell("K2");
            entrate.FormulaA1 = "=SUMIF(C2:C" + NumberOfLastRow + ",\"*FONDO*\",E2:E" + NumberOfLastRow + ")";

            ws2.Cell("J3").Value = "TOTALE USCITE";
            var uscite = ws2.Cell("K3");
            uscite.FormulaA1 = "=SUMIF(C2:C" + NumberOfLastRow + ",\"POSTA\",E2:E" + NumberOfLastRow + ")";
            // ws1.Cell(NumberOfLastRow + 1, 4).Value = "1000";

            int NumberOfLastRow2 = ws2.LastRowUsed().RowNumber();
            ws2.Cell("J4").Value = "RESIDUO";
            var residuo = ws2.Cell("K4");
            residuo.FormulaR1C1 = "=(K2-K3)";
            ws2.Cell("K2").Style.NumberFormat.Format = "€ #,##0.00;[Red]-€ #,##0.00";
            ws2.Cell("K3").Style.NumberFormat.Format = "€ #,##0.00;-€ #,##0.00";
            ws2.Cell("K4").Style.NumberFormat.Format = "€ #,##0.00;[Red]-€ #,##0.00";
            ws2.Cell("K2").DataType = XLCellValues.Number;
            ws2.Cell("K3").DataType = XLCellValues.Number;
            ws2.Cell("K4").DataType = XLCellValues.Number;

            // ws1.Cell(NumberOfLastRow + 1, 6).Value = "1000";
            ws2.Columns().AdjustToContents();
            for (int u = 1; u <= NumberOfLastRow; u++)
            {
                //  var row = ws3.Row(u);
                // string cell = row.Cell(3).Value.ToString();
                string cell = ws2.Cell(u, 3).Value.ToString();
                if (cell == "FONDO CASSA POSTA")
                {
                    var range4 = ws2.Range("A" + u, "F" + u);
                    range4.Style.Fill.BackgroundColor = XLColor.BabyBlue;
                    range4.Style.Font.SetBold(true);

                }


            }



        }
        public void export_quadernino (DataTable tabella, string foglio)
        {
            
            wbook.Worksheets.Add(tabella, foglio);
            
                var ws1 = wbook.Worksheet(foglio);
                ws1.Tables.FirstOrDefault().ShowAutoFilter = false;
                
                ws1.Range("E2:E2000").Columns().Style.NumberFormat.Format = "€ #,##0.00;[Red]-€ #,##0.00";  
                ws1.Range("E2:E2000").DataType = XLCellValues.Number;
                ws1.Range("F2:F2000").AddConditionalFormat().WhenContains("USCITA").Font.FontColor = XLColor.Red;
                int NumberOfLastRow = ws1.LastRowUsed().RowNumber();
                IXLCell CellForNewData = ws1.Cell(NumberOfLastRow + 1, 1);
                
                

                ws1.Cell("J2").Value = "TOTALE ENTRATE";
                var entrate =  ws1.Cell("K2");
                entrate.FormulaR1C1 = "=SUMIF(F2:F" + NumberOfLastRow+ ",\"ENTRATA\",E2:E"+NumberOfLastRow + ")";

                ws1.Cell("J3").Value = "TOTALE USCITE";
                var uscite = ws1.Cell("K3");
                uscite.FormulaR1C1 = "=SUMIF(F2:F" + NumberOfLastRow + ",\"USCITA\",E2:E" + NumberOfLastRow + ")";
                // ws1.Cell(NumberOfLastRow + 1, 4).Value = "1000";

                int NumberOfLastRow2 = ws1.LastRowUsed().RowNumber();
                ws1.Cell("J4").Value = "RESIDUO";
                var residuo = ws1.Cell("K4");
                residuo.FormulaR1C1 = "=(K2+K3)";


                ws1.Cell("K2").Style.NumberFormat.Format = "€ #,##0.00;[Red]-€ #,##0.00";
                ws1.Cell("K3").Style.NumberFormat.Format = "€ #,##0.00;-€ #,##0.00";
                ws1.Cell("K4").Style.NumberFormat.Format = "€ #,##0.00;[Red]-€ #,##0.00";
                ws1.Cell("K2").DataType = XLCellValues.Number;
                ws1.Cell("K3").DataType = XLCellValues.Number;
                ws1.Cell("K4").DataType = XLCellValues.Number;

                // ws1.Cell(NumberOfLastRow + 1, 6).Value = "1000";
                ws1.Columns().AdjustToContents();

            

           /* if (foglio == "RIEPILOGO")
            {
                var ws1 = wbook.Worksheet(foglio);
                ws1.Tables.FirstOrDefault().ShowAutoFilter = false;

                ws1.Range("B2:N200").Columns().Style.NumberFormat.Format = "€ #,##0.00;[Red]-€ #,##0.00";
                ws1.Range("B2:N200").DataType = XLCellValues.Number;
                

                ws1.Columns().AdjustToContents();
            }*/



        }

        private void personalizza()
        {
            string fileName = "HelloWorld.xlsx";
            var workbook = new XLWorkbook(fileName);
            var ws1 = workbook.Worksheet("QUADERNINO");
           
            var co = 5;
            var ro = 2;
           // ws1.Cell(10, 2).Value = "prova";
            //ws1.Cell(ro,co).Style.Fill.BackgroundColor =  XLColor.Red;
            ws1.Cell(ro, co).Style.NumberFormat.Format = "$0.00";
            ws1.Cell(ro, co).DataType = XLCellValues.Number;
            // ws1.Range("E2:E2000").Columns().Style.NumberFormat.Format = "€ #.##0,00;[Rosso]-€ #.##0,00";
          


        }


        
        void bgw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progress.Value = e.ProgressPercentage;
        }

        void Posta_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progress.Value = e.ProgressPercentage;
        }

        void Cat_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progress.Value = e.ProgressPercentage;
        }

        void bgw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            lbl_bar.Content = "Esportazione in corso: QUADERNINO...";
            progress.Value = 100;
            MessageBox.Show("Esportazione completata!", "Esporta Database", MessageBoxButton.OK, MessageBoxImage.Information);
            progress_tab.Visibility = Visibility.Hidden;
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
            {
                FileName = percorso2,
                UseShellExecute = true,
                Verb = "open"
            });
            this.NavigationService.Refresh();

        }
        void Cat_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            lbl_bar.Content = "Esportazione in corso: CAT...";
            progress.Value = 100;
            MessageBox.Show("Esportazione completata!", "Esporta Database", MessageBoxButton.OK, MessageBoxImage.Information);
            progress_tab.Visibility = Visibility.Hidden;
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
            {
                FileName = percorso2,
                UseShellExecute = true,
                Verb = "open"
            });
            this.NavigationService.Refresh();
        }
        void Posta_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            i = 0;
            lbl_bar.Content = "Esportazione in corso: POSTA...";
            progress.Value = 100;
            
            bgw3.RunWorkerAsync();
        }
        private void esporta_database_Click(object sender, RoutedEventArgs e)
        {
            if (percorso2 == "")
            {
                var dialog = new CommonOpenFileDialog();
                dialog.IsFolderPicker = true;
                dialog.EnsureReadOnly = true;
                dialog.AllowNonFileSystemItems = false;
                dialog.Multiselect = false;
                dialog.Title = "Seleziona cartella exportazione Database";
                
                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                {

                    percorso2 = dialog.FileName.ToString();
                    if (File.Exists(percorso2 + "\\" + "Quadernino.xlsx")) { File.Delete(percorso2 + "\\" + "Quadernino.xlsx"); }
                    if (File.Exists(percorso2 + "\\" + "Posta.xlsx")) { File.Delete(percorso2 + "\\" + "Posta.xlsx"); }
                    if (File.Exists(percorso2 + "\\" + "Cat.xlsx")) { File.Delete(percorso2 + "\\" + "Cat.xlsx"); }
                    lbl_bar.Content = "Esportazione in corso: QUADERNINO...";
                    progress_tab.Visibility = Visibility.Visible;
                    bgw.RunWorkerAsync();
                }
            }
           
           
        }
    }
}
