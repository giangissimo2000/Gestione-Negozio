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
using System.Windows.Shapes;

namespace Gestione_Studio
{
    /// <summary>
    /// Interaction logic for Dettaglio_Riepilogo.xaml
    /// </summary>
    public partial class Dettaglio_Riepilogo : Window
    {

        string percorso = "";
        string user = "";
        string password = "";
        string dbname = "";
        public Dettaglio_Riepilogo()
        {
            InitializeComponent();
            Verifica_Database();
            leggi();
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

        public void leggi()
        {
            try
            {
                var myObject = this.Owner as MainWindow;
                string riga = Application.Current.Properties["nome_riga_pass"].ToString();
                string colonna = Application.Current.Properties["nome_colonna_pass"].ToString();


                DataTable dt = new DataTable();
                dt.Columns.Add("data");
                dt.Columns.Add("descrizione");
                dt.Columns.Add("importo", typeof(decimal), null);
                dt.Columns.Add("utente");


                string ConString = "SERVER=" + percorso + ";" + "DATABASE=" + dbname + ";" + "UID=" + user + ";" + "PASSWORD=" + password + ";";

                MySqlConnection connection = new MySqlConnection(ConString);
                MySqlCommand command = connection.CreateCommand();
                MySqlDataReader Reader;

                command.CommandText = "SELECT data,descrizione,importo,utente FROM quadernino WHERE mese = '" + colonna + "' and  gruppo = '" +riga + "'";

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



                        string descrizione = Reader["descrizione"].ToString();
                        string importo = Reader["importo"].ToString();
                        string utente = Reader["utente"].ToString();



                        ne["data"] = date.ToShortDateString();
                        ne["descrizione"] = descrizione;
                        ne["importo"] = importo;
                        ne["utente"] = utente;
                        dt.Rows.Add(ne);


                    }
                    DataSet ds = new DataSet("table");
                    ds.Tables.Add(dt);
                    dettaglio_table.ItemsSource = ds.Tables["Table1"].DefaultView;


                }
                Reader.Close();



            }

            catch (Exception e)
            {

                MessageBox.Show("ERRORE!: ", e.ToString());

            }


        }
    }
}


