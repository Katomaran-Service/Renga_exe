using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
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
    /// Interaction logic for user.xaml
    /// </summary>
    public partial class user : Page
    {
        public string[] fileNames;
        public string ph="";
        public int position,status,ns,ne,np,nde,ndg,nu,npass,ndt;
        public class Item
        {
            public string name { get; set; }
            public string email { get; set; }
            public string phone_no { get; set; }
            public string location { get; set; }
        }
        public class Item1
        {
            public string name { get; set; }
            public string email { get; set; }
            public string phone_no { get; set; }
            public string designation { get; set; }
            public string password { get; set; }
            public string user_name { get; set; }
        }
        public class Item2
        {
            public string name { get; set; }
            public string email { get; set; }
            public string phone_no { get; set; }
            public string location { get; set; }
        }
        public List<Item> _items = new List<Item>();
        public List<Item1> _items1 = new List<Item1>();
        public List<Item2> _items2 = new List<Item2>();

        public List<Item> Items
        {
            get { return _items; }
            set { _items = value; }
        }
        public List<Item1> Items1
        {
            get { return _items1; }
            set { _items1 = value; }
        }
        public List<Item2> Items2
        {
            get { return _items2; }
            set { _items2 = value; }
        }
        public user()
        {
            InitializeComponent();
            status = 0;
            ns =np=ne=nde=ndg= 0;
            add_select.Items.Add("ADD ANDROID USER");
            add_select.Items.Add("ADD DASHBOARD USER");
            
            add_select.SelectedItem = add_select.Items[0];
            //android_yes_get();
            android_no_get();
            software_get();
        }

        private void aname_text_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (aname_text.Text == "Name")
            {
                aname_text.Text = "";
                aname_text.Foreground = new SolidColorBrush(Colors.White);
                if (ns == 17)
                    status -= ns;
                ns = 0;

            }
            else if (aname_text.Text == "")
            {
                aname_text.Text = "Name";
                aname_text.Foreground = new SolidColorBrush(Colors.Gray);
                if (ns == 17)
                    status -= ns;
                ns = 0;
            }
            else
            {
                if (ns == 0)
                {
                    ns = 17;
                    status += ns;
                }

            }
            status_update();
        }

        private void aemail_text_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (aemail_text.Text == "E-mail id")
            {
                aemail_text.Text = "";
                aemail_text.Foreground = new SolidColorBrush(Colors.White);
                if (ne == 16)
                    status -= ne;
                ne = 0;
            }
            else if (aemail_text.Text == "")
            {
                aemail_text.Text = "E-mail id";
                aemail_text.Foreground = new SolidColorBrush(Colors.Gray);
                if (ne == 16)
                    status -= ne;
                ne = 0;
            }
            else
            {
                if (ne == 0)
                {
                    ne = 16;
                    status += ne;
                }
            }
            status_update();
        }

        private void aphone_text_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (aphone_text.Text == "Phone number")
            {
                aphone_text.Text = "";
                aphone_text.Foreground = new SolidColorBrush(Colors.White);
                if (np == 17)
                    status -= np;
                np = 0;
            }
            else if (aphone_text.Text == "")
            {
                aphone_text.Text = "Phone number";
                aphone_text.Foreground = new SolidColorBrush(Colors.Gray);
                if (np == 17)
                    status -= np;
                np = 0;
            }
            else
            {
                if (np == 0)
                {
                    np = 17;
                    status += np;
                }
            }
            status_update();
        }

        private void adesign_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (adesign_text.Text == "Designation")
            {
                adesign_text.Text = "";
                adesign_text.Foreground = new SolidColorBrush(Colors.White);
                if (ndg == 16)
                    status -= ndg;
                ndg = 0;

            }
            else if (adesign_text.Text == "")
            {
                adesign_text.Text = "Designation";
                adesign_text.Foreground = new SolidColorBrush(Colors.Gray);
                if (ndg == 16)
                    status -= ndg;
                ndg = 0;
            }
            else
            {
                if (ndg == 0)
                {
                    ndg = 16;
                    status += ndg;
                }
            }
            status_update();
        }
        

        private void submit_but_Click(object sender, RoutedEventArgs e)
        {
            bool success = connect.CheckForInternetConnection();
            if (success)
            {
                submit_but.IsEnabled = false;
                if (head_box.Text == "ADD APP USER")
                {
                    if (aname_text.Text == "Name" || aphone_text.Text == "Phone number" || aemail_text.Text == "E-mail id" || adest_text.Text=="Destination" || new_pass_box.Password=="" || retype_pass_box.Password=="" || retype_pass_st.Text!="password matched" || connect.valid_email(aemail_text.Text) != true )
                    {
                        MessageBox.Show("Invalid fields");
                    }
                    else
                    {
                        string ms = "Do you want to add android user \n name: " + aname_text.Text;
                        var dialog = new MyDialog("ADD ANDROID USER", ms);
                        dialog.Show();
                        dialog.Closing += async (sender1, e1) =>
                        {
                            try
                            {
                                var d = sender1 as MyDialog;
                                if (!d.Canceled)
                                {
                                    if (auser_name.Text == "User name")
                                    {
                                        auser_name.Text = "";
                                    }
                                    var final_val = new Dictionary<string, Dictionary<string, string>>
                                {
                                    {"user",new Dictionary<string, string> {
                                        { "first_name", aname_text.Text },
                                        { "user_name", auser_name.Text },
                                        { "phone", aphone_text.Text },
                                        { "email", aemail_text.Text },
                                        { "location", adest_text.Text },
                                        { "password", new_pass_box.Password },
                                        { "role", "user" }
                                       }
                                    }
                                };
                                    string json = JsonConvert.SerializeObject(final_val);
                                    var response = await connect.client.PostAsync(connect.as_add_url, new StringContent(json, Encoding.UTF8, "application/json"));

                                    var responseString = await response.Content.ReadAsStringAsync();
                                    var obj = JObject.Parse(responseString);
                                    if (string.Equals(obj["status"].ToString(), "true", StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        MessageBox.Show("ANDROID USER ADDED");
                                    }
                                    else
                                    {
                                        MessageBox.Show("ANDROID USER NOT ADDED" + responseString);
                                    }

                                    adatagrid.ItemsSource = null;
                                    android_no_get();
                                    clear();
                                }
                            
                            }
                            catch(Exception m)
                            {
                                MessageBox.Show(m.Message);
                            }
                        };
                    }
                }
                else if(head_box.Text=="ADD DASHBOARD USER")
                {
                    if (aname_text.Text == "Name" || aphone_text.Text == "Phone number" || aemail_text.Text == "E-mail id" || auser_name.Text=="User name" || adesign_text.Text == "Designation" || new_pass_box.Password == "" || retype_pass_box.Password == "" || retype_pass_st.Text != "password matched" || connect.valid_email(aemail_text.Text) != true )
                    {
                        MessageBox.Show("Invalid fields");
                    }
                    else
                    {
                        string ms = "Do you want to add dashboard user \n name: " + aname_text.Text;
                        var dialog = new MyDialog("ADD DASHBOARD USER", ms);
                        dialog.Show();
                        dialog.Closing += async (sender1, e1) =>
                        {
                            var d = sender1 as MyDialog;
                            if (!d.Canceled)
                            {
                                try
                                {
                                    var final_val = new Dictionary<string, Dictionary<string, string>>
                                {
                                    {"user",new Dictionary<string, string> {
                                        { "first_name", aname_text.Text },
                                        { "user_name", auser_name.Text },
                                        { "phone", aphone_text.Text },
                                        { "email", aemail_text.Text },
                                        { "location", adesign_text.Text },
                                        { "password", new_pass_box.Password },
                                        { "role", "admin" }
                                       }
                                    }
                                };
                                    string json = JsonConvert.SerializeObject(final_val);
                                    var response = await connect.client.PostAsync(connect.as_add_url, new StringContent(json, Encoding.UTF8, "application/json"));

                                    var responseString = await response.Content.ReadAsStringAsync();
                                    if (connect.IsValidJson(responseString))
                                    {
                                        var obj = JObject.Parse(responseString);
                                        if (string.Equals(obj["status"].ToString(), "true", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            MessageBox.Show("DASHBOARD USER ADDED");
                                        }
                                        else
                                        {
                                            MessageBox.Show("DASHBOARD USER NOT ADDED" + responseString);
                                        }
                                        sdatagrid.ItemsSource = null;
                                        software_get();
                                        clear();
                                    }
                                    else
                                    {
                                        MessageBox.Show("NO RESPONSE");
                                    }
                                }catch(Exception m)
                                {
                                    MessageBox.Show(m.Message);
                                }
                                
                            }

                        };
                    }
                }
                submit_but.IsEnabled = true;
            }
            
        }

        private void editpro_but_Click(object sender, RoutedEventArgs e)
        {
            bool success = connect.CheckForInternetConnection();
           

            if (success)
            {
                
                editpro_but.IsEnabled = false;
                if (aname_text.Text == "Name" || aphone_text.Text == "Phone number" || aemail_text.Text == "E-mail id" || (adest_text.Text == "Destination" && adesign_text.Text=="Designation") || connect.valid_email(aemail_text.Text)!=true)
                    {
                        MessageBox.Show("Invalid fields");
                    }
                    else
                    {
                    ph = aphone_text.Text;
                    string val = adesign_text.Text; 
                    
                    string head = "dashboard", head_json = "admin";
                    if (head_box.Text=="EDIT APP USER")
                        {
                            head = "android"; head_json="user";
                        val = adest_text.Text;

                    }
                        string ms = "Do you want to edit \n "+head+" user\n phone number: " + ph;
                        var dialog = new MyDialog("EDIT "+head.ToUpper()+" USER", ms);
                        dialog.Show();
                        dialog.Closing += async (sender1, e1) =>
                        {
                            var d = sender1 as MyDialog;
                            if (!d.Canceled)
                            {
                                try
                                {
                                    var final_val = new Dictionary<string, Dictionary<string, string>>
                                {
                                    {"user",new Dictionary<string, string> {
                                        { "first_name", aname_text.Text },
                                        { "email", aemail_text.Text },
                                        { "location", val },
                                        {"role", head_json }
                                       }
                                    }
                                };


                                    string json = JsonConvert.SerializeObject(final_val);
                                    var response = await connect.client.PutAsync(connect.as_add_url + "/" + ph, new StringContent(json, Encoding.UTF8, "application/json"));

                                    var responseString = await response.Content.ReadAsStringAsync();
                                    if (connect.IsValidJson(responseString))
                                    {
                                        var obj = JObject.Parse(responseString);
                                        if (string.Equals(obj["status"].ToString(), "true", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            MessageBox.Show(head.ToUpper() + " USER EDITED");
                                        }
                                        else
                                        {
                                            MessageBox.Show(head.ToUpper() + " USER NOT EDITED" + responseString);
                                        }
                                        switch (head)
                                        {
                                            case "android":
                                                adatagrid.ItemsSource = null;
                                                android_no_get();
                                                break;
                                            case "dashboard":
                                                sdatagrid.ItemsSource = null;
                                                software_get();
                                                break;
                                        }

                                        clear();
                                    }
                                    else
                                    {
                                        MessageBox.Show("NO RESPONSE");
                                    }
                           
                                }catch(Exception m)
                                {
                                    MessageBox.Show(m.Message);
                                }
                            }

                        };
                    }
                editpro_but.IsEnabled = true;
            }
            
        }

        private void upload_image_but_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void EDIT_Click(object sender, RoutedEventArgs e)
        {
            add_select.IsEnabled = false;
            position = adatagrid.SelectedIndex;
            head_box.Text = "EDIT APP USER";

            status_block.Visibility = Visibility.Hidden;
            status_progress.Visibility = Visibility.Hidden;
            submit_but.Visibility = Visibility.Hidden;
            editpro_but.Visibility = Visibility.Visible;
            cancel_but.Visibility = Visibility.Visible;
            auser_name.Visibility = Visibility.Hidden;
            aphone_text.IsEnabled = false;
            
            new_pass_box.Visibility = Visibility.Hidden;
            new_pass_label.Visibility = Visibility.Hidden;
            retype_pass_box.Visibility = Visibility.Hidden;
            retype_pass_label.Visibility = Visibility.Hidden;
            dynamic list = adatagrid.SelectedItems[0];
            aname_text.Text = list.name;
            aname_text.Foreground = new SolidColorBrush(Colors.White);
            aemail_text.Text = list.email;
            aemail_text.Foreground = new SolidColorBrush(Colors.White);
            aphone_text.Text = list.phone_no;
            ph = list.phone_no;
            aphone_text.Foreground = new SolidColorBrush(Colors.White);
            adest_text.Text = list.location;
            adest_text.Foreground = new SolidColorBrush(Colors.White);
        }

        private void DELETE_Click(object sender, RoutedEventArgs e)
        {
            bool success = connect.CheckForInternetConnection();
            if (success)
            {
                dynamic list = adatagrid.SelectedItems[0];
                string ms = "Do you want to delete \nandroid user name: " + list.name;
                var dialog = new MyDialog("DELETE ANDROID USER", ms);
                dialog.Show();
                dialog.Closing += async (sender1, e1) =>
                {
                    var d = sender1 as MyDialog;
                    if (!d.Canceled)
                    {
                        try
                        {
                            var response = await connect.client.DeleteAsync(connect.as_delete_url + "/" + list.phone_no);

                            var responseString = await response.Content.ReadAsStringAsync();
                            if (connect.IsValidJson(responseString))
                            {
                                var obj = JObject.Parse(responseString);
                                if (string.Equals(obj["status"].ToString(), "true", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    MessageBox.Show("ANDROID USER DELETED");
                                }
                                else
                                {
                                    MessageBox.Show("ANDROID USER NOT DELETED" + responseString);
                                }
                                adatagrid.ItemsSource = null;
                                android_no_get();
                                clear();
                            }
                            else
                            {
                                MessageBox.Show("NO RESPONSE");
                            }
                        }
                        catch(Exception m)
                        {
                            MessageBox.Show(m.Message);
                        }

                    }

                };
            }
        }
        
        private void adownbut_Click(object sender, RoutedEventArgs e)
        {
            aupbutton.Visibility = Visibility.Visible;
            adownbut.Visibility = Visibility.Collapsed;
        }

        private void aupbutton_Click(object sender, RoutedEventArgs e)
        {
            aupbutton.Visibility = Visibility.Collapsed;
            adownbut.Visibility = Visibility.Visible;
        }

        private void adownbut1_Click(object sender, RoutedEventArgs e)
        {
            aupbutton1.Visibility = Visibility.Visible;
            adownbut1.Visibility = Visibility.Collapsed;
        }

        private void aupbutton1_Click(object sender, RoutedEventArgs e)
        {
            aupbutton1.Visibility = Visibility.Collapsed;
            adownbut1.Visibility = Visibility.Visible;
        }

        private void new_pass_box_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((new_pass_label.Visibility == Visibility.Visible) || (new_pass_box.Password != ""))
            {
                new_pass_label.Visibility = Visibility.Hidden;
                
            }
                
            else if (new_pass_label.Visibility == Visibility.Hidden)
            {
                retype_pass_st.Text = "";
                new_pass_label.Visibility = Visibility.Visible;
            }
                
        }

        private void retype_pass_box_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((retype_pass_label.Visibility == Visibility.Visible) || (retype_pass_box.Password != ""))
            {
               
                retype_pass_label.Visibility = Visibility.Hidden;
            }
               
            else if (retype_pass_label.Visibility == Visibility.Hidden)
            {
                retype_pass_st.Text = "";
                if (npass == 17)
                    status -= npass;
                npass = 0;
                status_update();
                retype_pass_label.Visibility = Visibility.Visible;
            }
                
        }

        private void SEDIT_Click(object sender, RoutedEventArgs e)
        {
            add_select.IsEnabled = false;
            position = sdatagrid.SelectedIndex;
            head_box.Text = "EDIT DASHBOARD USER";
            status_block.Visibility = Visibility.Hidden;
            status_progress.Visibility = Visibility.Hidden;
            submit_but.Visibility = Visibility.Hidden;
            editpro_but.Visibility = Visibility.Visible;
            cancel_but.Visibility = Visibility.Visible;
            auser_name.Visibility = Visibility.Visible;
            adest_text.Visibility = Visibility.Hidden;
            adesign_text.Visibility = Visibility.Visible;
            auser_name.Visibility = Visibility.Hidden;
            aphone_text.IsEnabled = false;
            new_pass_box.Visibility = Visibility.Hidden;
            new_pass_label.Visibility = Visibility.Hidden;
            retype_pass_box.Visibility = Visibility.Hidden;
            retype_pass_label.Visibility = Visibility.Hidden;
            dynamic list = sdatagrid.SelectedItems[0];
            aname_text.Text = list.name;
            aname_text.Foreground = new SolidColorBrush(Colors.White);
            aemail_text.Text = list.email;
            aemail_text.Foreground = new SolidColorBrush(Colors.White);
            aphone_text.Text = list.phone_no;
            aphone_text.Foreground = new SolidColorBrush(Colors.White);
            adesign_text.Text = list.designation;
            adesign_text.Foreground = new SolidColorBrush(Colors.White);
            auser_name.Text = list.user_name;
            auser_name.Foreground = new SolidColorBrush(Colors.White);
        }

        private void SDELETE_Click(object sender, RoutedEventArgs e)
        {
            bool success = connect.CheckForInternetConnection();
            if (success)
            {
                if (sdatagrid.Items.Count > 1)
                {
                    dynamic list = sdatagrid.SelectedItems[0];
                    string[] detail = connect.details.Split('\n');
                    string[] phone1 = detail[1].Split(':');
                   string ms = "Do you want to delete \ndashboard user name: " + list.name;
                    var dialog = new MyDialog("DELETE DASHBOARD USER", ms);
                    dialog.Show();
                    dialog.Closing += async (sender1, e1) =>
                    {
                        var d = sender1 as MyDialog;
                        if (!d.Canceled)
                        {
                            try
                            {
                                if (phone1[1] == list.phone_no)
                                {
                                    MainWindow.t.Stop();
                                }
                                var response = await connect.client.DeleteAsync(connect.as_delete_url + "/" + list.phone_no);

                                var responseString = await response.Content.ReadAsStringAsync();
                                if (connect.IsValidJson(responseString))
                                {
                                    var obj = JObject.Parse(responseString);
                                    if (string.Equals(obj["status"].ToString(), "true", StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        Mouse.OverrideCursor = Cursors.Arrow;
                                        if (phone1[1] == list.phone_no)
                                        {

                                            var w = Application.Current.Windows[0];
                                            w.Hide();

                                            startupscreen s1 = new startupscreen();
                                            s1.Show();
                                            w.Close();
                                        }
                                        else
                                        {
                                            MainWindow.t.Start();
                                            MessageBox.Show("DASHBOARD USER DELETED");
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("NO RESPONSE");
                                    }


                                }
                                else
                                {
                                    MessageBox.Show("DASHBOARD USER NOT DELETED" + responseString);
                                }
                                sdatagrid.ItemsSource = null;
                                software_get();
                                clear();
                            }
                            catch(Exception m)
                            {
                                MessageBox.Show(m.Message);
                            }

                        }

                    };
                }
                else
                {
                    MessageBox.Show("User count cannot be less than one");
                }
                
            }
        }

        private void auser_name_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (auser_name.Text == "User name")
            {
                auser_name.Text = "";
                auser_name.Foreground = new SolidColorBrush(Colors.White);
                if (nu == 17)
                    status -= nu;
                nu = 0;

            }
            else if (auser_name.Text == "")
            {
                auser_name.Text = "User name";
                auser_name.Foreground = new SolidColorBrush(Colors.Gray);
                if (nu == 17)
                    status -= nu;
                nu = 0;
            }
            else
            {
                if (nu == 0)
                {
                    nu = 17;
                    status += nu;
                }
            }
            status_update();
        }

        private void cancel_but_Click(object sender, RoutedEventArgs e)
        {
            clear();
        }
        public void clear()
        {
            add_select.IsEnabled = true;
            head_box.Text = "ADD APP USER";
            auser_name.Visibility = Visibility.Visible;
            aphone_text.IsEnabled = true;
            new_pass_box.Visibility = Visibility.Visible;
            new_pass_label.Visibility = Visibility.Visible;
            retype_pass_box.Visibility = Visibility.Visible;
            retype_pass_label.Visibility = Visibility.Visible;
            aname_text.Text = "Name";
            aname_text.Foreground = new SolidColorBrush(Colors.Gray);
            aemail_text.Text = "E-mail id";
            aemail_text.Foreground = new SolidColorBrush(Colors.Gray);
            aphone_text.Text = "Phone number";
            aphone_text.Foreground = new SolidColorBrush(Colors.Gray);
            adesign_text.Text = "Designation";
            adesign_text.Foreground = new SolidColorBrush(Colors.Gray);
            adesign_text.Visibility = Visibility.Hidden;
            email_st.Text = "";
            adest_text.Text = "Destination";
            adest_text.Foreground = new SolidColorBrush(Colors.Gray);
            adest_text.Visibility = Visibility.Visible;
            auser_name.Text = "User name";
            auser_name.Foreground = new SolidColorBrush(Colors.Gray);
            new_pass_box.Password = "";
            retype_pass_box.Password = "";
            new_pass_label.Visibility = Visibility.Visible;
            retype_pass_label.Visibility = Visibility.Visible;
            retype_pass_st.Text = "";
            email_st.Text = "";
            status_progress.Value = 0;
            status_block.Text = "";
            np = npass = ns = nu = nde = ndg = ndt = ne =status= 0;
            add_select.SelectedItem = add_select.Items[0];
            submit_but.Visibility = Visibility.Visible;
            editpro_but.Visibility = Visibility.Collapsed;
            cancel_but.Visibility = Visibility.Collapsed;
            status_block.Visibility = Visibility.Visible;
            status_progress.Visibility = Visibility.Visible;
        }
        
        private void retype_pass_st_TextInput(object sender, TextCompositionEventArgs e)
        {

        }

        private void aemail_text_KeyDown(object sender, KeyEventArgs e)
        {
            
            if (connect.valid_email(aemail_text.Text))
            {
                email_st.Text = "";
                aemail_text.BorderBrush = new SolidColorBrush(Colors.White);
            }
            else
            {
                email_st.Text = "invalid email";
                aemail_text.BorderBrush = new SolidColorBrush(Colors.Red);
            }
        }

        private void aemail_text_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key==Key.Delete || e.Key == Key.Back)
            {
                if (connect.valid_email(aemail_text.Text))
                {
                    email_st.Text = "";
                    aemail_text.BorderBrush = new SolidColorBrush(Colors.White);
                }
                else
                {
                    email_st.Text = "invalid email";
                    aemail_text.BorderBrush = new SolidColorBrush(Colors.Red);
                }
            }
                
        }

        private void submit_but_MouseEnter(object sender, MouseEventArgs e)
        {
            submit_but.Background = new SolidColorBrush(Colors.Wheat);
        }

        private void submit_but_MouseLeave(object sender, MouseEventArgs e)
        {
            submit_but.Background = new SolidColorBrush(Colors.White);
        }

        private void editpro_but_MouseEnter(object sender, MouseEventArgs e)
        {
            editpro_but.Background = new SolidColorBrush(Colors.Wheat);
        }

        private void editpro_but_MouseLeave(object sender, MouseEventArgs e)
        {
            editpro_but.Background = new SolidColorBrush(Colors.White);
        }

        

        private void aprove_Click(object sender, RoutedEventArgs e)
        {

        }

        private void add_select_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (add_select.SelectedItem == add_select.Items[0])
            {
                clear();
            }
            else
            {
                head_box.Text = "ADD DASHBOARD USER";
                adesign_text.Visibility = Visibility.Visible;
                adest_text.Visibility = Visibility.Hidden;
                auser_name.Visibility = Visibility.Visible;
                aname_text.Text = "Name";
                aname_text.Foreground = new SolidColorBrush(Colors.Gray);
                aemail_text.Text = "E-mail id";
                aemail_text.Foreground = new SolidColorBrush(Colors.Gray);
                aphone_text.Text = "Phone number";
                aphone_text.Foreground = new SolidColorBrush(Colors.Gray);
                adesign_text.Text = "Designation";
                adesign_text.Foreground = new SolidColorBrush(Colors.Gray);
                auser_name.Text = "User name";
                auser_name.Foreground = new SolidColorBrush(Colors.Gray);
                new_pass_box.Password = "";
                retype_pass_box.Password = "";
                new_pass_label.Visibility = Visibility.Visible;
                retype_pass_label.Visibility = Visibility.Visible;
                retype_pass_st.Text = "";
                status_progress.Value = 0;
                status_block.Text = "";
                np = npass = ns = nu = nde = ndg = ndt = ne = status = 0;
            }
        }

        private void retype_pass_box_KeyUp(object sender, KeyEventArgs e)
        {
            if (new_pass_box.Password != retype_pass_box.Password)
            {
                retype_pass_st.Text = "*password didn't match";
                retype_pass_st.Foreground = new SolidColorBrush(Colors.Red);
                if (npass == 17)
                    status -= npass;
                npass = 0;
            }
            else
            {
                retype_pass_st.Text = "password matched";
                retype_pass_st.Foreground = new SolidColorBrush(Colors.Green);
                if (npass == 0)
                {
                    npass = 17;
                    status += npass;
                }
            }
            status_update();
        }

        private void adest_text_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (adest_text.Text == "Destination")
            {
                adest_text.Text = "";
                adest_text.Foreground = new SolidColorBrush(Colors.White);
                if (ndt == 16)
                    status -= ndt;
                ndt = 0;

            }
            else if (adest_text.Text == "")
            {
                adest_text.Text = "Destination";
                adest_text.Foreground = new SolidColorBrush(Colors.Gray);
                if (ndt == 16)
                    status -= ndt;
                ndt = 0;
            }
            else
            {
                if (ndt == 0)
                {
                    ndt = 16;
                    status += ndt;
                }
            }
            status_update();
        }

        private void aname_text_TextChanged(object sender, TextChangedEventArgs e)
        {
          
           
        }
        public void status_update()
        {
            status_block.Text = status.ToString() + "% done";
            status_progress.Value = status;
        }
        
        public void android_no_get()
        {
            bool success = connect.CheckForInternetConnection();
            if (success)
            {
                String url = connect.android_get_url;
                try
                {
                    HttpWebRequest req = WebRequest.Create(url) as HttpWebRequest;
                    req.Headers["AdminAuthToken"] = connect.auth_token;
                    string result = null;
                    Mouse.OverrideCursor = Cursors.Wait;
                    using (HttpWebResponse resp = req.GetResponse() as HttpWebResponse)
                    {
                        StreamReader reader = new StreamReader(resp.GetResponseStream());
                        result = reader.ReadToEnd();
                    }
                    if (connect.IsValidJson(result))
                    {
                        JObject obj2 = JObject.Parse(result);
                        if (Convert.ToBoolean(obj2["status"].ToString()))
                        {
                            
                            if (connect.IsValidJson(obj2["data"].ToString()))
                            {
                                Items2.Clear();
                                var dt = (DataTable)JsonConvert.DeserializeObject(obj2["data"].ToString(), (typeof(DataTable)));
                                foreach (DataRow dr in dt.Rows)
                                {
                                    _items2.Add(new Item2 { name = dr["first_name"].ToString(), location = dr["location"].ToString(), phone_no = dr["phone"].ToString(), email = dr["email"].ToString() });
                                }
                            }
                        }
                        adatagrid.ItemsSource = Items2;
                        Mouse.OverrideCursor = Cursors.Arrow;
                    }

                    req.Abort();

                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }
        public void software_get()
        {
            bool success = connect.CheckForInternetConnection();
            if (success)
            {
                String url =connect.software_get_url;
                try
                {
                    HttpWebRequest req = WebRequest.Create(url) as HttpWebRequest;
                    req.Headers["AdminAuthToken"] = connect.auth_token;
                    string result = null;
                    Mouse.OverrideCursor = Cursors.Wait;
                    using (HttpWebResponse resp = req.GetResponse() as HttpWebResponse)
                    {
                        StreamReader reader = new StreamReader(resp.GetResponseStream());
                        result = reader.ReadToEnd();
                    }
                    if (connect.IsValidJson(result))
                    {
                        JObject obj2 = JObject.Parse(result);
                        if (Convert.ToBoolean(obj2["status"].ToString()))
                        {
                           
                            if (connect.IsValidJson(obj2["data"].ToString()))
                            {
                                var dt = (DataTable)JsonConvert.DeserializeObject(obj2["data"].ToString(),(typeof(DataTable)));
                                Items1.Clear();
                                foreach (DataRow dr in dt.Rows)
                                {

                                    _items1.Add(new Item1 { name = dr["first_name"].ToString(), user_name = dr["user_name"].ToString(), designation = dr["location"].ToString(), phone_no = dr["phone"].ToString(), email = dr["email"].ToString() });
                                }
                            }
                        }
                           
                        sdatagrid.ItemsSource = Items1;
                        Mouse.OverrideCursor = Cursors.Arrow;
                    }
                    req.Abort();
                    
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }
    }
}
