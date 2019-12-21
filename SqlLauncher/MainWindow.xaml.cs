using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace SqlLauncher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        protected void Launcher_Click(object sender, RoutedEventArgs e)
        {
            string dbString = "";

            switch (((Button) sender).Content.ToString())
            {
                case "DEV":
                    dbString = "-S inet-sql-dev -d RDI_Development";
                    break;
                case "TEST":
                    dbString = "-S inet-sql-dev -d RDI_Test";
                    break;
                case "PROD":
                    dbString = "-S inet-sql-prod -d RDI_Production";
                    break;
            }

            Process.Start("ssms", dbString);

            Application.Current.Shutdown();
        }
    }
}
