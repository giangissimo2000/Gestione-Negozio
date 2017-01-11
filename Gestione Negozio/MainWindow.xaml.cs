using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using System.Data.SQLite;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Globalization;
using IniParser.Model;
using IniParser;

namespace Gestione_Studio
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    

    public partial class MainWindow : Window
    {

        string percorso = "";
      
        string user = "";
        string password = "";
        string dbname = "";

        public MainWindow()
        {
            InitializeComponent();
            Verifica_Database();
            frame.Source = new Uri("/Pagine/Quadernino.xaml", UriKind.RelativeOrAbsolute); // initialize frame with the "test1" view
                                                                             // qua.Visibility = Visibility.Collapsed;
                                                                             //  frame.Navigate(new System.Uri("/Pagine/Quadernino.xaml", UriKind.RelativeOrAbsolute));

        }


        public void Navigate(UserControl nextPage)
        {
            this.Content = nextPage;
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
                this.Close();
            }


            

        }


        private void CustomMenu_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void btnLeftMenuShow_Click(object sender, RoutedEventArgs e)
        {
            ShowHideMenu("sbShowLeftMenu", btnLeftMenuHide, btnLeftMenuShow, pnlLeftMenu);
        }

        private void btnLeftMenuHide_Click(object sender, RoutedEventArgs e)
        {
            ShowHideMenu("sbHideLeftMenu", btnLeftMenuHide, btnLeftMenuShow, pnlLeftMenu);
        }

        private void ShowHideMenu(string Storyboard, Button btnHide, Button btnShow, StackPanel pnl)
        {
            Storyboard sb = Resources[Storyboard] as Storyboard;
            sb.Begin(pnl);

            if (Storyboard.Contains("Show"))
            {
                btnHide.Visibility = System.Windows.Visibility.Visible;
                btnShow.Visibility = System.Windows.Visibility.Hidden;
            }
            else if (Storyboard.Contains("Hide"))
            {
                btnHide.Visibility = System.Windows.Visibility.Hidden;
                btnShow.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void quadernino_btn_Click(object sender, RoutedEventArgs e)
        {


            
            frame.Navigate(new System.Uri("/Pagine/Quadernino.xaml", UriKind.RelativeOrAbsolute));
         
            ShowHideMenu("sbHideLeftMenu", btnLeftMenuHide, btnLeftMenuShow, pnlLeftMenu);
         
        }

        private void riepilogo_btn_Click(object sender, RoutedEventArgs e)
        {
            frame.Navigate(new System.Uri("/Pagine/Riepilogo.xaml", UriKind.RelativeOrAbsolute));
            ShowHideMenu("sbHideLeftMenu", btnLeftMenuHide, btnLeftMenuShow, pnlLeftMenu);
        }

        private void posta_btn_Click(object sender, RoutedEventArgs e)
        {
            frame.Navigate(new System.Uri("/Pagine/Posta.xaml", UriKind.RelativeOrAbsolute));
            ShowHideMenu("sbHideLeftMenu", btnLeftMenuHide, btnLeftMenuShow, pnlLeftMenu);
        }

        private void cat_btn_Click(object sender, RoutedEventArgs e)
        {
            frame.Navigate(new System.Uri("/Pagine/Cat.xaml", UriKind.RelativeOrAbsolute));
            ShowHideMenu("sbHideLeftMenu", btnLeftMenuHide, btnLeftMenuShow, pnlLeftMenu);
        }

        private void impostazioni_btn_Click(object sender, RoutedEventArgs e)
        {
            frame.Navigate(new System.Uri("/Pagine/Impostazioni.xaml", UriKind.RelativeOrAbsolute));
            ShowHideMenu("sbHideLeftMenu", btnLeftMenuHide, btnLeftMenuShow, pnlLeftMenu);
        }

        private void cassa_btn_Click(object sender, RoutedEventArgs e)
        {
            frame.Navigate(new System.Uri("/Pagine/Cassa_Fiscale.xaml", UriKind.RelativeOrAbsolute));
            ShowHideMenu("sbHideLeftMenu", btnLeftMenuHide, btnLeftMenuShow, pnlLeftMenu);
        }


        
    }
}
