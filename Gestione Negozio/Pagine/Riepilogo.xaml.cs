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
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Gestione_Studio
{
    /// <summary>
    /// Interaction logic for Riepilogo.xaml
    /// </summary>
    /// 
    class ValueToForegroundColorConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            SolidColorBrush brush = new SolidColorBrush(Colors.Black);

            Double doubleValue = 0.0;
            Double.TryParse(value.ToString(), out doubleValue);

            if (doubleValue < 0)
                brush = new SolidColorBrush(Colors.Red);
            if (doubleValue >= 0)
                brush = new SolidColorBrush(Colors.Green);

            return brush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public partial class Riepilogo : Page
    {
        string percorso = "";
        string user = "";
        string password = "";
        string dbname = "";
        List<string> gruppi_list = new List<string>();
        List<string> mesi_list = new List<string>();

        public Riepilogo()
        {
            InitializeComponent();
            Verifica_Database();
            Read_Database();
            
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
        public static bool TryToDecimal32(object value, out decimal result)
        {
            if (value == null)
            {
                result = 0;
                return false;
            }
            var typeConverter = System.ComponentModel.TypeDescriptor.GetConverter(value);
            if (typeConverter != null && typeConverter.CanConvertTo(typeof(decimal)))
            {
                var convertTo = typeConverter.ConvertTo(value, typeof(decimal));
                if (convertTo != null)
                {
                    result = (decimal)convertTo;
                    return true;
                }
            }
            return decimal.TryParse(value.ToString(), out result);
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
                        gruppi_list.Add(gruppi);
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


        public void Read_Database()
        {



            try
            {



                string path = Directory.GetCurrentDirectory();
                Console.WriteLine(path);
                DataTable dt = new DataTable();

                dt.Columns.Add("Gruppo");
                dt.Columns.Add("Gennaio", typeof(decimal), null);
                dt.Columns.Add("Febbraio", typeof(decimal),null);
                dt.Columns.Add("Marzo", typeof(decimal), null);
                dt.Columns.Add("Aprile", typeof(decimal), null);
                dt.Columns.Add("Maggio", typeof(decimal), null);
                dt.Columns.Add("Giugno", typeof(decimal), null);
                dt.Columns.Add("Luglio", typeof(decimal), null);
                dt.Columns.Add("Agosto", typeof(decimal), null);
                dt.Columns.Add("Settembre", typeof(decimal), null);
                dt.Columns.Add("Ottobre", typeof(decimal),null);
                dt.Columns.Add("Novembre", typeof(decimal), null);
                dt.Columns.Add("Dicembre", typeof(decimal), null);
                dt.Columns.Add("TOTALE", typeof(decimal), null);

                string ConString = "SERVER=" + percorso + ";" + "DATABASE=" + dbname + ";" + "UID=" + user + ";" + "PASSWORD=" + password + ";";

                MySqlConnection connection = new MySqlConnection(ConString);
                MySqlCommand command = connection.CreateCommand();
                MySqlDataReader Reader;
                command.CommandText = "select gruppo, (sum(REPLACE(case when(mese) = 'GENNAIO' then importo else 0 end, ',', '.')))As Gennaio,(sum(REPLACE(case when(mese) = 'FEBBRAIO' then importo else 0 end, ',', '.')))As Febbraio,(sum(REPLACE(case when(mese) = 'MARZO' then importo else 0 end, ',', '.')))As Marzo,(sum(REPLACE(case when(mese) = 'APRILE' then importo else 0 end, ',', '.')))As Aprile,(sum(REPLACE(case when(mese) = 'MAGGIO' then importo else 0 end, ',', '.')))As Maggio,(sum(REPLACE(case when(mese) = 'GIUGNO' then importo else 0 end, ',', '.')))As Giugno,(sum(REPLACE(case when(mese) = 'LUGLIO' then importo else 0 end, ',', '.')))As Luglio,(sum(REPLACE(case when(mese) = 'AGOSTO' then importo else 0 end, ',', '.')))As Agosto,(sum(REPLACE(case when(mese) = 'SETTEMBRE' then importo else 0 end, ',', '.')))As Settembre,(sum(REPLACE(case when(mese) = 'OTTOBRE' then importo else 0 end, ',', '.')))As Ottobre,(sum(REPLACE(case when(mese) = 'NOVEMBRE' then importo else 0 end, ',', '.')))As Novembre,(sum(REPLACE(case when(mese) = 'DICEMBRE' then importo else 0 end, ',', '.')))As Dicembre from quadernino group by gruppo";

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
                            else {
                                //colTotal = "TOTALE USCITE";
                                 }
                             }


                        }
                        
                     //   ned[col.ColumnName] = colTotal;
                      //  ned[0] = "TOTALE USCITE";
                    }
                    dt.Rows.Add(ned);

                


                DataSet ds = new DataSet("table");
                ds.Tables.Add(dt);

               
            


                riepilogo_table.ItemsSource = ds.Tables["Table1"].DefaultView;

            }
            catch { }
        }




        private void riepilogo_table_LoadingRow(object sender, DataGridRowEventArgs e)
        {


            /* int index = e.Row.GetIndex();
             if (index == 0)
                 e.Row.Background = Brushes.Yellow;*/

            




            DataGridRow row = e.Row;
            DataRowView rView = row.Item as DataRowView;
            if (rView != null && rView.Row.ItemArray[0].ToString().Contains("TOTALE USCITE"))
            {
                e.Row.Background = Brushes.Yellow;
            }
            


            /*  RowDataContext RowDataContext = e.Row.RowDataContext as OptionChain;

              if (RowDataContext != null)
              {
                  if (RowDataContext.ochgdir == "Down")
                      e.Row.Background = Brushes.Beige as Brush;
                  else
                      e.Row.Background = Brushes.GreenYellow as Brush;
              }






          }*/
        }

        private void TextBlock_ToolTipOpening(object sender, ToolTipEventArgs e)
        {

        }

        private void OnDoubleClick(object sender, MouseButtonEventArgs e)
        {


            int riga = riepilogo_table.SelectedIndex;

            DataRowView dr = riepilogo_table.SelectedItem as DataRowView;
            DataRow dr1 = dr.Row;
            string nome_Riga = Convert.ToString(dr1.ItemArray[0]);

           
            string nome_colonna = (string)riepilogo_table.CurrentCell.Column.Header.ToString();

            if ((nome_colonna == "TOTALE") || (nome_colonna == "") || (nome_Riga =="TOTALE USCITE" ))
            {

            }
            else
            {
                Application.Current.Properties["nome_riga_pass"] = nome_Riga;
                Application.Current.Properties["nome_colonna_pass"] = nome_colonna;

                Dettaglio_Riepilogo Dettaglio = new Dettaglio_Riepilogo();
                Dettaglio.ShowDialog();
                // MessageBox.Show(nome_Riga + " " + nome_colonna);
            }
                // string selectedColumnHeader = (string)riepilogo_table.SelectedCells[1].Column.Header;
        }

        private void riepilogo_table_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
           
        }
    }
}
