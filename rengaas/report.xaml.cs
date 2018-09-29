using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
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

namespace rengaas
{
    /// <summary>
    /// Interaction logic for report.xaml
    /// </summary>
    public partial class report : Page
    {
        public report()
        {
            InitializeComponent();
            report_type.Items.Add("ORDER REPORT");
            report_type.SelectedItem = report_type.Items[0];
        }

        private void report_but_Click(object sender, RoutedEventArgs e)
        {
            string fod="";
            
            if (from_date.Text!="" && to_date.Text != "")
            {
                DateTime d1 = DateTime.Parse(to_date.Text);
                string tod = d1.ToString("yyyy-MM-dd");
                DateTime d2 = DateTime.Parse(from_date.Text);
                fod = d2.ToString("yyyy-MM-dd");
                msg(fod, tod);

            }
            else
            {
                MessageBox.Show("no date range is selected");
            }
           
            
        }
        public void msg(string f,string t)
        {
            string ms = "DATE:\n from: " + f + "\n to: " + t;
            var dialog = new MyDialog("REPORT GENERATION",ms);
            dialog.Show();
            dialog.Closing += (sender, e) =>
            {
                var d = sender as MyDialog;
                if (!d.Canceled)
                {
                    bool suc=software_get(f,t);
                    if (suc)
                        MessageBox.Show("Report generated");
                    else
                        MessageBox.Show("Report not generated");
                }

            };
        }
        private void from_date_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            to_date.SelectedDate = null;
            to_date.DisplayDateStart = DateTime.Parse(from_date.SelectedDate.ToString()).AddDays(1);
        }

        private void report_type_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        public Boolean software_get(string fromdate,string todate)
        {
            bool success = connect.CheckForInternetConnection();
            if (success)
            {
                try
                {

                    Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
                    dlg.FileName = "report"; // Default file name
                    dlg.DefaultExt = ".pdf"; // Default file extension
                    dlg.Filter = "PDF document (.pdf)|*.pdf"; // Filter files by extension
                    string url = "http://api.rangas.katomaran.com/api/v1/orders/reports.pdf?from_date="+fromdate+"&to_date="+todate;
                    // Show save file dialog box
                    Nullable<bool> result = dlg.ShowDialog();

                    // Process save file dialog box results
                    if (result == true)
                    {
                        Mouse.OverrideCursor = Cursors.Wait;
                        string filename = dlg.FileName;
                        using (WebClient client = new WebClient())
                        {
                            client.DownloadFile(url, filename);
                            Mouse.OverrideCursor = Cursors.Arrow;
                            return true;

                        }
                    }
                       

                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
                
            }
            return false;
        }

        private void report_but_MouseEnter(object sender, MouseEventArgs e)
        {
            report_but.Background = new SolidColorBrush(Colors.Wheat);
        }

        private void report_but_MouseLeave(object sender, MouseEventArgs e)
        {
            report_but.Background = new SolidColorBrush(Colors.White);
        }
    }
}
