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
    /// Interaction logic for Ricerca.xaml
    /// </summary>
    public partial class Ricerca : Page
    {
        string percorso = "";
        public Ricerca()
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

        private void OnChecked(object sender, RoutedEventArgs e)
        {
            

        }


        public void totale()
        {





          /*  List<string> myCollection = new List<string>();
            decimal sum = 0;
            decimal entrat = 0;
            decimal uscit = 0;
            for (int i = 0; i < ricerca_table.Items.Count; ++i)
            {
                //(decimal.Parse((tblData.SelectedCells[3].Column.GetCellContent(item) as TextBlock).Text))

                myCollection.Add(((ricerca_table.Items[i] as DataRowView).Row.ItemArray[4].ToString()));
                //sum += (decimal.Parse((quadernino_table.Columns[3].GetCellContent(quadernino_table.Items[i]) as TextBlock).Text));
            }

            var myarray = myCollection.ToArray();

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

            in_total.Content = entrat.ToString("N", new CultureInfo("is-IS")) + " €";
            out_total.Content = uscit.ToString("N", new CultureInfo("is-IS")) + " €";
            total.Content = sum.ToString("N", new CultureInfo("is-IS")) + " €";*/




        }

       

        private void Cerca_Click(object sender, RoutedEventArgs e)
        {
           


            try
            {

                
                string month = ((ComboBoxItem)comboBox.SelectedItem).Content.ToString();
                string path = Directory.GetCurrentDirectory();
                Console.WriteLine(path);
                DataTable dt = new DataTable();

              //  dt.Columns.Add("data");

             //   dt.Columns.Add("mese");
             //   dt.Columns.Add("gruppo");
                dt.Columns.Add("descrizione");
                dt.Columns.Add("importo", typeof(decimal), null);
            //    dt.Columns.Add("tipo_mov");
                dt.Columns.Add(new DataColumn("aggiungi", typeof(bool)));
               // dt.Rows.Add(1);
               



                string ConString = "Data Source=" + percorso + ";Version=3;";

                SQLiteConnection connection = new SQLiteConnection(ConString);
                SQLiteCommand command = connection.CreateCommand();
                SQLiteDataReader Reader;

                command.CommandText = "select * from quadernino where descrizione LIKE '" + cerca_txt.Text +"%' and mese='" + month + "'";

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
                        



                       // ne["data"] = date.ToShortDateString();
                      //  ne["mese"] = mese;
                      //  ne["gruppo"] = gruppo;
                        ne["descrizione"] = descrizione;
                        ne["importo"] = importo;
                      //  ne["tipo_mov"] = tipo_mov;
                        ne["aggiungi"] = 1;
                        //dt.Rows.Add(1);


                        dt.Rows.Add(ne);


                    }
                    DataSet ds = new DataSet("table");
                    ds.Tables.Add(dt);
                    ricerca_table.ItemsSource = ds.Tables["Table1"].DefaultView;

                }
                else { MessageBox.Show("Nessun risultato per '" + cerca_txt.Text + "'"); }
                Reader.Close();
                totale();


            }
            catch 
            {

                MessageBox.Show("Seleziona il mese!");

            }





        }

        

        private void CopyDataToClipboard(DataTable DT)
        {
            StringBuilder Output = new StringBuilder();

            //The first "line" will be the Headers.
          /*  for (int i = 0; i < DT.Columns.Count; i++)
            {
                Output.Append(DT.Columns[i].ColumnName + "\t");
            }*/

           /* Output.Append("\n");*/

            //Generate Cell Value Data
            foreach (DataRow Row in DT.Rows)
            {
                for (int i = 0; i < Row.ItemArray.Length; i++)
                {
                    //Handling the last cell of the line.
                    if (i == (Row.ItemArray.Length - 1))
                    {

                        Output.Append(Row.ItemArray[i].ToString() + "\n");
                    }
                    else
                    {

                        Output.Append(Row.ItemArray[i].ToString() + "\t");
                    }
                }
            }

            Clipboard.SetText(Output.ToString());
        }

        




        private void cerca_calcola()
        {

            List<string> myCollection = new List<string>();
            decimal sum = 0;
            decimal entrat = 0;
            decimal uscit = 0;
            DataTable dt = new DataTable();
            dt = ((DataView)ricerca_table.ItemsSource).ToTable();

            for (int i = 0; i < ricerca_table.Items.Count; ++i)
            {

                string abc = dt.Rows[i]["aggiungi"].ToString();
                if (abc == "True")
                {

                    myCollection.Add(((ricerca_table.Items[i] as DataRowView).Row.ItemArray[1].ToString()));

                }


            }
            var myarray = myCollection.ToArray();

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

            in_total.Content = entrat.ToString("N", new CultureInfo("is-IS")) + " €";
            out_total.Content = uscit.ToString("N", new CultureInfo("is-IS")) + " €";
            total.Content = sum.ToString("N", new CultureInfo("is-IS")) + " €";

        }
    
        private void OnCheck(object sender, RoutedEventArgs e)
        {

            cerca_calcola();
            
        }

        
        private void OnUncheck(object sender, RoutedEventArgs e)
        {
            cerca_calcola();
        }

        private void Copia_Click(object sender, RoutedEventArgs e)
        {
            List<string> myCollection = new List<string>();
            
            DataTable dt = new DataTable();
            DataTable ds = new DataTable();
            
            dt.Columns.Add("descrizione");
            dt.Columns.Add("importo", typeof(decimal), null);
            




            ds = ((DataView)ricerca_table.ItemsSource).ToTable();

            for (int i = 0; i < ricerca_table.Items.Count; ++i)
            {
                DataRow ne = dt.NewRow();
                string abc = ds.Rows[i]["aggiungi"].ToString();
                if (abc == "True")
                {

               // ne["data"] = (ricerca_table.Items[i] as DataRowView).Row.ItemArray[0].ToString();
              //  ne["gruppo"] = (ricerca_table.Items[i] as DataRowView).Row.ItemArray[2].ToString();
                ne["descrizione"] = (ricerca_table.Items[i] as DataRowView).Row.ItemArray[0].ToString();
                ne["importo"] = (ricerca_table.Items[i] as DataRowView).Row.ItemArray[1].ToString();
                dt.Rows.Add(ne);
                }

                
            }
          
            CopyDataToClipboard(dt);
        }
    }
}
