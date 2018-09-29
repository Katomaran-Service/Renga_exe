using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace rengaas
{
    /// <summary>
    /// Interaction logic for startupscreen.xaml
    /// </summary>
    public partial class startupscreen : Window
    {
        DispatcherTimer t = new DispatcherTimer();
        int i = 0;
        private static readonly HttpClient client = new HttpClient();
        public startupscreen()
        {
            
            InitializeComponent();
            username_box.Visibility = Visibility.Hidden;
            passbox.Visibility = Visibility.Hidden;
            passlabel.Visibility = Visibility.Hidden;
            login_but.Visibility = Visibility.Hidden;
            t.Tick += new EventHandler(t_Tick);
            t.Interval = new TimeSpan(0, 0, 1);
            t.Start();

        }


       

        private void t_Tick(object sender, EventArgs e)
        {
            bool suc=connect.CheckForInternetConnection();
                if (suc)
                {
                login_status.Text = "Connected";
                login_status.Foreground = new SolidColorBrush(Colors.Green);
                username_box.Visibility = Visibility.Visible;
                    passbox.Visibility = Visibility.Visible;
                    passlabel.Visibility = Visibility.Visible;
                    login_but.Visibility = Visibility.Visible;
                t.Stop();

            }
                else
                {
                    login_status.Text = "No internet connection";
                login_status.Foreground = new SolidColorBrush(Colors.Red);
                
            }
                
            

        }

        private void username_box_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if(username_box.Text=="User name")
            {
                username_box.Text = "";
                username_box.Foreground = new SolidColorBrush(Colors.Black);
            }
            else if (username_box.Text == "")
            {
                username_box.Text = "User name";
                username_box.Foreground=new SolidColorBrush(Colors.Black);
            }
        }

        private void passbox_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((passlabel.Visibility == Visibility.Visible) || (passbox.Password != ""))
            {

                passlabel.Visibility = Visibility.Hidden;
            }

            else if (passlabel.Visibility == Visibility.Hidden)
            {
                passlabel.Visibility = Visibility.Visible;
            }
        }

        private void passbox_KeyDown(object sender, KeyEventArgs e)
        {
            if ((Keyboard.GetKeyStates(Key.CapsLock) & KeyStates.Toggled) == KeyStates.Toggled)
            {
                if (passbox.ToolTip == null)
                {
                    ToolTip tt = new ToolTip();
                    tt.Content = "Warning: CapsLock is on";
                    tt.PlacementTarget = sender as UIElement;
                    tt.Placement = PlacementMode.Bottom;
                    passbox.ToolTip = tt;
                    tt.IsOpen = true;
                }
            }
            else
            {
                var currentToolTip = passbox.ToolTip as ToolTip;
                if (currentToolTip != null)
                {
                    currentToolTip.IsOpen = false;
                }

                passbox.ToolTip = null;
            }
            if (e.Key == Key.Enter)
            {
                if(username_box.Text!="User name" && passbox.Password != "")
                {

                    checksum();
                }
                else
                {

                    login_status.Text="Invalid user name and password";
                }
                
            }
        }

        private void login_but_Click(object sender, RoutedEventArgs e)
        {
            if (username_box.Text != "User name" && passbox.Password != "")
            {
                
                checksum();
                
            }
            else
            {
                
                login_status.Text = "Invalid user name and password";
                
                
            }
        }

        private void close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        public async void checksum()
        {
            try
            {
                username_box.IsEnabled = false;
                passbox.IsEnabled = false;
                passlabel.IsEnabled = false;
                login_but.IsEnabled = false;
                login_status.Text = "Please wait";
                login_status.Foreground = new SolidColorBrush(Colors.Green);
                var values = new Dictionary<string, string>
                     {

                        { "user_name", username_box.Text },
                        { "password", passbox.Password }
                    };

                var content = new FormUrlEncodedContent(values);
                var response = await client.PostAsync(connect.login_url, content);
                var responseString = await response.Content.ReadAsStringAsync();
                if (connect.CheckForInternetConnection())
                {
                    if (connect.IsValidJson(responseString))
                    {
                        var obj = JObject.Parse(responseString);
                        
                        if (string.Equals(obj["status"].ToString(), "true", StringComparison.CurrentCultureIgnoreCase))
                        {
                            var obj1 = JObject.Parse(obj["data"].ToString());
                            t.Stop();
                            this.Hide();
                            connect.username = obj1["user_name"].ToString();
                            connect.details = "Name:" + obj1["first_name"] + "\nphone number:" + obj1["phone"] + "\nemail:" + obj1["email"] + "\ndesignation:" + obj1["role"];
                            connect.auth_token = obj1["auth_token"].ToString();
                            connect.client.DefaultRequestHeaders.Add("AdminAuthToken", connect.auth_token);
                            connect.client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            MainWindow m1 = new MainWindow();
                            m1.Show();
                            this.Close();
                        }
                        else
                        {
                            login_status.Text = "Incorrect username and password";
                            login_status.Foreground = new SolidColorBrush(Colors.Red);
                            Keyboard.ClearFocus();
                        }
                    }

                }
                username_box.IsEnabled = true;
                passbox.IsEnabled = true;
                passlabel.IsEnabled = true;
                login_but.IsEnabled = true;
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }

        }
    }
}
