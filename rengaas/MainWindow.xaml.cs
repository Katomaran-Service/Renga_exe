using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;

namespace rengaas
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int i ;
        public static DispatcherTimer t = new DispatcherTimer();
        public  class Item
        {
            public string message { get; set; }
            public string date { get; internal set; }
        }
        public  List<Item> _items = new List<Item>();
        public  List<Item> Items
        {
            get { return _items; }
            set { _items = value; }
        }

        public MainWindow()
        {
            InitializeComponent();
            //this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            //this.MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
            // Automatically resize height and width relative to content
            notify();
            t.Tick += new EventHandler(t_Tick);
            t.Interval = new TimeSpan(0, 0, 5);
            t.Start();

            user_name.Text = connect.username;
            
        }

        private void t_Tick(object sender, EventArgs e)
        {
            notify();
        }
        void notify()
        {

            if (connect.CheckForInternetConnection())
            {
                
                String url = connect.order_get_url;
                try
                {
                    HttpWebRequest req = WebRequest.Create(url) as HttpWebRequest;
                    req.Headers["AdminAuthToken"] = connect.auth_token;
                    string result = null;
                    using (HttpWebResponse resp = req.GetResponse() as HttpWebResponse)
                    {
                        StreamReader reader = new StreamReader(resp.GetResponseStream());
                        result = reader.ReadToEnd();
                    }
                    if (connect.IsValidJson(result))
                    {
                       
                        JToken token = JToken.Parse(result);
                        if (string.Equals(token.SelectToken("status").ToString(), "true", StringComparison.CurrentCultureIgnoreCase))
                        {
                            Items.Clear();
                            int size = token.SelectToken("data").Count();
                            i = 0;
                            for (int v = 0; v < size; v++)
                            {
                                if (Convert.ToBoolean(token.SelectToken("data[" + v.ToString() + "].admin_view").ToString()) == false)
                                {
                                    string date1 = "", cust = "", time = "";
                                    JArray details = (JArray)token.SelectToken("data[" + v.ToString() + "].details");
                                    foreach (JToken m in details)
                                    {
                                        if (m["Date"] != null)
                                            date1 = m["Date"].ToString();
                                        else
                                            date1 += "0,";
                                        if (m["Time"] != null)
                                            time = m["Time"].ToString();
                                        else
                                            time += "0,";
                                        
                                    }
                                    JObject ju = (JObject)token.SelectToken("data[" + v.ToString() + "].user");
                                    JObject jc = (JObject)token.SelectToken("data[" + v.ToString() + "].customer");
                                    cust = jc["shop_name"].ToString();
                                    _items.Add(new Item { date = date1+time, message = "order id:" + token.SelectToken("data[" + v.ToString() + "].invoice_id").ToString() + " shop name:" + cust +"\nsales man:"+ ju["first_name"].ToString() + "\ntime:" + time });
                                    i++;
                                }
                            }
                        }
                            
                        
                        
                        Items.Sort((y, x) => x.date.CompareTo(y.date));
                        noty_count.Text = i.ToString();
                        dataGrid.ItemsSource = Items;
                    }
                }
                catch (Exception m)
                {
                    dataGrid.ItemsSource = null;
                    Items.Clear();
                    _items.Add(new Item {date="", message = "notification error" });
                    dataGrid.ItemsSource = Items;
                }
            }
        }
        private void close_Click(object sender, RoutedEventArgs e)
        {

            App.Current.Shutdown();
        }

        private void close_MouseEnter(object sender, MouseEventArgs e)
        {
            close.Background = new SolidColorBrush(Colors.Red);
        }

        private void close_MouseLeave(object sender, MouseEventArgs e)
        {
            close.Background = new SolidColorBrush(Colors.Transparent);
        }

        private void maximize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Maximized;
            //Height = SystemParameters.MaximizedPrimaryScreenHeight;
            //Width = SystemParameters.MaximizedPrimaryScreenWidth;
            //this.SizeToContent = SizeToContent.WidthAndHeight;
            maximize.Visibility = Visibility.Collapsed;
            restore.Visibility = Visibility.Visible;
        }

        private void minimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void restore_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Normal;
            restore.Visibility = Visibility.Collapsed;
            maximize.Visibility = Visibility.Visible;
           
        }

        private void product_but_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            pro_image.Visibility = Visibility.Hidden;
            pro_image2.Visibility = Visibility.Visible;
            cus_image.Visibility = Visibility.Visible;
            cus_image2.Visibility = Visibility.Hidden;
            rec_image.Visibility = Visibility.Visible;
            rec_image2.Visibility = Visibility.Hidden;
            report_image.Visibility = Visibility.Visible;
            report_image2.Visibility = Visibility.Hidden;
            user_image.Visibility = Visibility.Visible;
            user_image2.Visibility = Visibility.Hidden;
            user_label.Foreground = new SolidColorBrush(Colors.White);
            pro_label.Foreground = new SolidColorBrush(Color.FromRgb(30, 81, 251));
            
            cus_label.Foreground = new SolidColorBrush(Colors.White);
            
            rec_label.Foreground = new SolidColorBrush(Colors.White);
            
            report_label.Foreground = new SolidColorBrush(Colors.White);
           
            main_frame.Content = new received_ord();
        }

        private void cust_but_Click(object sender, RoutedEventArgs e)
        {
            pro_image2.Visibility = Visibility.Hidden;
            pro_image.Visibility = Visibility.Visible;
            cus_image.Visibility = Visibility.Hidden;
            cus_image2.Visibility = Visibility.Visible;
            rec_image.Visibility = Visibility.Visible;
            rec_image2.Visibility = Visibility.Hidden;
            report_image.Visibility = Visibility.Visible;
            report_image2.Visibility = Visibility.Hidden;
            user_image.Visibility = Visibility.Visible;
            user_image2.Visibility = Visibility.Hidden;
            user_label.Foreground = new SolidColorBrush(Colors.White);
            pro_label.Foreground = new SolidColorBrush(Colors.White);
            
            cus_label.Foreground = new SolidColorBrush(Color.FromRgb(30, 81, 251));
            
            rec_label.Foreground = new SolidColorBrush(Colors.White);
            
            report_label.Foreground = new SolidColorBrush(Colors.White);
            main_frame.Content = new customer();
        }

        private void received_but_Click(object sender, RoutedEventArgs e)
        {
            pro_image2.Visibility = Visibility.Hidden;
            pro_image.Visibility = Visibility.Visible;
            cus_image.Visibility = Visibility.Visible;
            cus_image2.Visibility = Visibility.Hidden;
            rec_image.Visibility = Visibility.Hidden;
            rec_image2.Visibility = Visibility.Visible;
            report_image.Visibility = Visibility.Visible;
            report_image2.Visibility = Visibility.Hidden;
            user_image.Visibility = Visibility.Visible;
            user_image2.Visibility = Visibility.Hidden;
            user_label.Foreground = new SolidColorBrush(Colors.White);
            pro_label.Foreground = new SolidColorBrush(Colors.White);
            
            cus_label.Foreground = new SolidColorBrush(Colors.White);
            
            rec_label.Foreground = new SolidColorBrush(Color.FromRgb(30, 81, 251));
            
            report_label.Foreground = new SolidColorBrush(Colors.White);
            main_frame.Content = new received();
        }

        private void report_but_Click(object sender, RoutedEventArgs e)
        {
            pro_image.Visibility = Visibility.Visible;
            pro_image2.Visibility = Visibility.Hidden;
            cus_image.Visibility = Visibility.Visible;
            cus_image2.Visibility = Visibility.Hidden;
            rec_image.Visibility = Visibility.Visible;
            rec_image2.Visibility = Visibility.Hidden;
            report_image.Visibility = Visibility.Hidden;
            report_image2.Visibility = Visibility.Visible;
            user_image.Visibility = Visibility.Visible;
            user_image2.Visibility = Visibility.Hidden;
            user_label.Foreground = new SolidColorBrush(Colors.White);
            pro_label.Foreground = new SolidColorBrush(Colors.White);
            
            cus_label.Foreground = new SolidColorBrush(Colors.White);
            
            rec_label.Foreground = new SolidColorBrush(Colors.White);
            
            report_label.Foreground = new SolidColorBrush(Color.FromRgb(30, 81, 251));
            main_frame.Content = new report();
        }

        private void setting_but_Click(object sender, RoutedEventArgs e)
        {
            if (setting_grid.Visibility == Visibility.Collapsed)
                setting_grid.Visibility = Visibility.Visible;
            else if (setting_grid.Visibility == Visibility.Visible)
                setting_grid.Visibility = Visibility.Collapsed;
                
        }

        private void notification_but_Click(object sender, RoutedEventArgs e)
        {
            if(noti_grid.Visibility == Visibility.Collapsed)
            {
                noti_grid.Visibility = Visibility.Visible;
            }
            else if(noti_grid.Visibility == Visibility.Visible)
            {
                noti_grid.Visibility = Visibility.Collapsed;
            }
        }

        private void user_but_Click(object sender, RoutedEventArgs e)
        {
            pro_image2.Visibility = Visibility.Hidden;
            pro_image.Visibility = Visibility.Visible;
            cus_image.Visibility = Visibility.Visible;
            cus_image2.Visibility = Visibility.Hidden;
            rec_image.Visibility = Visibility.Visible;
            rec_image2.Visibility = Visibility.Hidden;
            report_image.Visibility = Visibility.Visible;
            report_image2.Visibility = Visibility.Hidden;
            user_image2.Visibility = Visibility.Visible;
            user_image.Visibility = Visibility.Hidden;
            user_label.Foreground = new SolidColorBrush(Color.FromRgb(30, 81, 251));
            pro_label.Foreground = new SolidColorBrush(Colors.White);

            cus_label.Foreground = new SolidColorBrush(Colors.White);

            rec_label.Foreground = new SolidColorBrush(Colors.White);

            report_label.Foreground = new SolidColorBrush(Colors.White);
            main_frame.Content = new user();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void sign_out_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            startupscreen s1 = new startupscreen();

            s1.Show();
            this.Close();
        }

        private void info_but_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new MyDialog("User details", connect.details);
            dialog.Show();
            dialog.Closing += (sender1, e1) =>
            {
                var d = sender1 as MyDialog;
                if (!d.Canceled)
                {
                    
                }

            };
        }

        private void received_but_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void pro_but_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Hand;
        }

        private void pro_but_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void pro_but_MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void cust_but_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Hand;
        }

        private void cust_but_MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void report_but_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Hand;
        }

        private void report_but_MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void received_but_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Hand;
        }

        private void received_but_MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void user_but_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Hand;
        }

        private void user_but_MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void notification_but_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Hand;
        }

        private void notification_but_MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            rec_image.Visibility = Visibility.Hidden;
            rec_image2.Visibility = Visibility.Visible;
            rec_label.Foreground = new SolidColorBrush(Color.FromRgb(30, 81, 251));
            main_frame.Content = new received();
        }

        private void notification_but_LostMouseCapture(object sender, MouseEventArgs e)
        {
            if (noti_grid.Visibility == Visibility.Visible)
                noti_grid.Visibility = Visibility.Collapsed;
        }
    }
    public class RatioConverter : MarkupExtension, IValueConverter
    {
        private static RatioConverter _instance;

        public RatioConverter() { }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        { // do not let the culture default to local to prevent variable outcome re decimal syntax
            double size = System.Convert.ToDouble(value) * System.Convert.ToDouble(parameter, CultureInfo.InvariantCulture);
            return size.ToString("G0", CultureInfo.InvariantCulture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        { // read only converter...
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _instance ?? (_instance = new RatioConverter());
        }

    }
}



