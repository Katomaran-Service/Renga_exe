using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace rengaas
{
    /// <summary>
    /// Interaction logic for customer.xaml
    /// </summary>
    public partial class customer : Page
    {
        public int position,status,cn,cs,cg,ca,cp,cf,ce;
        public string[] fileNames;
        public string ph="",auth;
        int i = 1;
        
        public class Item
        {
            public string num { get; set; }
            public string customer_name { get; set; }
            public string shop_name { get; set; }
            public string gst_no { get; set; }
            public string address { get; set; }
            public string phone_no { get; set; }
            public string fax { get; set; }
            public string email { get; set; }
        }
        public class pitem
        {
            public string num { get; set; }
            public string customer_name { get; set; }
            public string shop_name { get; set; }
            public string gst_no { get; set; }
            public string address { get; set; }
            public string phone_no { get; set; }
            public string fax { get; set; }
            public string email { get; set; }
        }
        public List<Item> _items = new List<Item>();
        public List<pitem> _items1 = new List<pitem>();

        public List<Item> Items
        {
            get { return _items; }
            set { _items = value; }
        }
        public List<pitem> pitems
        {
            get { return _items1; }
            set { _items1 = value; }
        }
        public customer()
        {
            InitializeComponent();
            get_cust();
        }

       

        private void cus_name_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (cus_name.Text == "Customer name")
            {
                cus_name.Text = "";
                cus_name.Foreground = new SolidColorBrush(Colors.White);
                if (cn == 17)
                    status -= cn;
                cn = 0;

            }
            else if (cus_name.Text == "")
            {
                cus_name.Text = "Customer name";
                cus_name.Foreground = new SolidColorBrush(Colors.Gray);
                if (cn == 17)
                    status -= cn;
                cn = 0;
            }
            else
            {
                if (cn == 0)
                {
                    cn = 17;
                    status += cn;
                }
            }
            status_update();
        }

        private void pro_code_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (cus_reg.Text == "GST number")
            {
                cus_reg.Text = "";
                cus_reg.Foreground = new SolidColorBrush(Colors.White);
                if (cg == 17)
                    status -= cg;
                cg = 0;

            }
            else if (cus_reg.Text == "")
            {
                cus_reg.Text = "GST number";
                cus_reg.Foreground = new SolidColorBrush(Colors.Gray);
                if (cg == 17)
                    status -= cg;
                cg = 0;
                
            }
            else
            {
                if (cg == 0)
                {
                    cg = 17;
                    status += cg;
                }
            }
            status_update();
        }

        private void cus_address_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (cus_address.Text == "Full address")
            {
                cus_address.Text = "";
                cus_address.Foreground = new SolidColorBrush(Colors.White);
                if (ca == 17)
                    status -= ca;
                ca = 0;

            }
            else if (cus_address.Text == "")
            {
                cus_address.Text = "Full address";
                cus_address.Foreground = new SolidColorBrush(Colors.Gray);
                if (ca == 17)
                    status -= ca;
                ca = 0;
            }
            else
            {
                if (ca == 0)
                {
                    ca = 17;
                    status += ca;
                }
            }
            status_update();
        }

        private void cus_tphone_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (cus_tphone.Text == "Phone number")
            {
                cus_tphone.Text = "";
                cus_tphone.Foreground = new SolidColorBrush(Colors.White);
                if (cp == 17)
                    status -= cp;
                cp = 0;

            }
            else if (cus_tphone.Text == "")
            {
                cus_tphone.Text = "Phone number";
                cus_tphone.Foreground = new SolidColorBrush(Colors.Gray);
                if (cp == 17)
                    status -= cp;
                cp = 0;
            }
            else
            {
                if (cp == 0)
                {
                    cp = 17;
                    status += cp;
                }

            }
            status_update();
        }

        private void cus_fax_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (cus_fax.Text == "Fax")
            {
                cus_fax.Text = "";
                cus_fax.Foreground = new SolidColorBrush(Colors.White);
                if (cf == 17)
                    status -= cf;
                cf = 0;

            }
            else if (cus_fax.Text == "")
            {
                cus_fax.Text = "Fax";
                cus_fax.Foreground = new SolidColorBrush(Colors.Gray);
                if (cf == 17)
                    status -= cf;
                cf = 0;
            }
            else
            {
                if (cf == 0)
                {
                    cf = 17;
                    status += cf;
                }
            }
            status_update();
        }

        private void cus_email_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (cus_email.Text == "E-mail")
            {
                cus_email.Text = "";
                cus_email.Foreground = new SolidColorBrush(Colors.White);
                if (ce == 17)
                    status -= ce;
                ce = 0;

            }
            else if (cus_email.Text == "")
            {
                cus_email.Text = "E-mail";
                cus_email.Foreground = new SolidColorBrush(Colors.Gray);
                if (ce == 17)
                    status -= ce;
                ce = 0;
            }
            else
            {
                if (ce == 0)
                {
                    ce = 17;
                    status += ce;
                }
            }
            status_update();
        }
        

        private void EDIT_Click(object sender, RoutedEventArgs e)
        {
            position = customer_grid.SelectedIndex;
            head_box.Text = "EDIT CUSTOMER";
            submit_but.Visibility = Visibility.Hidden;
            editpro_but.Visibility = Visibility.Visible;
            cancel_but.Visibility = Visibility.Visible;
            dynamic list = customer_grid.SelectedItems[0];
            status_block.Visibility = Visibility.Hidden;
            status_progress.Visibility = Visibility.Hidden;
            cus_name.Text = list.customer_name;
            cus_name.Foreground = new SolidColorBrush(Colors.White);
            cus_shop.Text = list.shop_name;
            cus_shop.Foreground = new SolidColorBrush(Colors.White);
            cus_reg.Text = list.gst_no;
            cus_reg.Foreground = new SolidColorBrush(Colors.White);
            cus_address.Text = list.address;
            cus_address.Foreground = new SolidColorBrush(Colors.White);
            ph = list.phone_no;
            cus_tphone.Text = list.phone_no;
            cus_tphone.Foreground = new SolidColorBrush(Colors.White);
            cus_fax.Text = list.fax;
            cus_fax.Foreground = new SolidColorBrush(Colors.White);
            cus_email.Text = list.email;
            cus_email.Foreground = new SolidColorBrush(Colors.White);
        }

        private void DELETE_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                dynamic list = customer_grid.SelectedItems[0];
                string ms = "Do you want to delete \n customer: " + list.customer_name;
                var dialog = new MyDialog("DELETE CUSTOMER", ms);
                dialog.Show();
                dialog.Closing += async (sender1, e1) =>
                {
                    var d = sender1 as MyDialog;
                    if (!d.Canceled)
                    {

                        try
                        {
                            var response = await connect.client.DeleteAsync(connect.customer_url + "/" + list.phone_no);

                            var responseString = await response.Content.ReadAsStringAsync();
                            if (connect.IsValidJson(responseString))
                            {
                                var obj = JObject.Parse(responseString);
                                if (string.Equals(obj["status"].ToString(), "true", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    MessageBox.Show("CUSTOMER DELETED");
                                }
                                else
                                {
                                    MessageBox.Show("CUSTOMER NOT DELETED\nMESSAGE:" + obj["data"].ToString());
                                }
                                customer_grid.ItemsSource = null;
                                get_cust();
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
            catch(Exception m)
            {
                MessageBox.Show(m.Message);
            }
            
        }

        private void submit_but_Click(object sender, RoutedEventArgs e)
        {
            if (connect.CheckForInternetConnection())
            {
                submit_but.IsEnabled = false;
                try
                {
                    if ((cus_tphone.Text == "Phone number") )
                    {
                        MessageBox.Show("PLEASE ENTER THE PHONE NUMBER");
                    }
                    else
                    {
                        if (head_box.Text == "ADD CUSTOMER")
                        {
                            string ms = "Do you want to add \n customer name: " + cus_name.Text;
                            var dialog = new MyDialog("ADD CUSTOMER", ms);
                            dialog.Show();
                            dialog.Closing += async (sender1, e1) =>
                            {
                                var d = sender1 as MyDialog;
                                if (!d.Canceled)
                                {
                                    if(cus_name.Text== "Customer name")
                                    {
                                        cus_name.Text = "nil";
                                    }
                                    if (cus_shop.Text == "Shop name")
                                    {
                                        cus_shop.Text = "nil";
                                    }
                                    if (cus_reg.Text == "GST number")
                                    {
                                        cus_reg.Text = "nil";
                                    }
                                    if (cus_address.Text == "Full address")
                                    {
                                        cus_address.Text = "nil";
                                    }
                                    if (cus_email.Text == "E-mail")
                                    {
                                        cus_email.Text = "";
                                        email_st.Text = "";
                                        cus_email.BorderBrush = new SolidColorBrush(Colors.White);
                                    }
                                    if (cus_fax.Text == "Fax")
                                    {
                                        cus_fax.Text = "nil";
                                    }
                                    try
                                    {
                                        var final_val = new Dictionary<string, Dictionary<string, string>>
                                {
                                    {"customer",new Dictionary<string, string> {
                                        { "name", cus_name.Text },
                                        { "shop_name", cus_shop.Text },
                                        { "gst_no", cus_reg.Text },
                                        { "address", cus_address.Text },
                                        { "phone", cus_tphone.Text },
                                        { "fax", cus_fax.Text },
                                        { "email", cus_email.Text }
                                       }
                                    }
                                };

                                        string json = JsonConvert.SerializeObject(final_val);
                                        var response = await connect.client.PostAsync(connect.customer_url, new StringContent(json, Encoding.UTF8, "application/json"));
                                        var responseString = await response.Content.ReadAsStringAsync();
                                        if (connect.IsValidJson(responseString))
                                        {
                                            var obj = JObject.Parse(responseString);
                                            if (string.Equals(obj["status"].ToString(), "true", StringComparison.CurrentCultureIgnoreCase))
                                            {
                                                MessageBox.Show("CUSTOMER ADDED");
                                            }
                                            else
                                            {
                                                MessageBox.Show("CUSTOMER NOT ADDED\nMESSAGE:"+ obj["data"].ToString());
                                                
                                                
                                            }

                                            customer_grid.ItemsSource = null;
                                            get_cust();
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
                }
                catch(Exception m)
                {
                    MessageBox.Show(m.Message);
                }
                submit_but.IsEnabled = true;
            }
            
        }

        private void editpro_but_Click(object sender, RoutedEventArgs e)
        {
            if (connect.CheckForInternetConnection())
            {
                editpro_but.IsEnabled = false;
                try
                {
                    if ((cus_tphone.Text == "Phone number"))
                    {
                        MessageBox.Show("PLEASE ENTER THE PHONE NUMBER");
                    }
                    else
                    {
                        if (head_box.Text == "EDIT CUSTOMER")
                        {
                            
                            
                            string ms = "Do you want to EDIT \n customer ph no: " + ph;
                            var dialog = new MyDialog("EDIT CUSTOMER", ms);
                            dialog.Show();
                            dialog.Closing += async (sender1, e1) =>
                            {
                                var d = sender1 as MyDialog;
                                if (!d.Canceled)
                                {
                                    if (cus_name.Text == "Customer name")
                                    {
                                        cus_name.Text = "nil";
                                    }
                                    if (cus_shop.Text == "Shop name")
                                    {
                                        cus_shop.Text = "nil";
                                    }
                                    if (cus_reg.Text == "GST number")
                                    {
                                        cus_reg.Text = "nil";
                                    }
                                    if (cus_address.Text == "Full address")
                                    {
                                        cus_address.Text = "nil";
                                    }
                                    if (cus_email.Text == "E-mail")
                                    {
                                        cus_email.Text = "";
                                        email_st.Text = "";
                                        cus_email.BorderBrush = new SolidColorBrush(Colors.White);
                                    }
                                    if (cus_fax.Text == "Fax")
                                    {
                                        cus_fax.Text = "nil";
                                    }
                                    var final_val = new Dictionary<string, Dictionary<string, string>>
                                {
                                    {"customer",new Dictionary<string, string> {
                                        { "name", cus_name.Text },
                                        { "shop_name", cus_shop.Text },
                                        { "gst_no", cus_reg.Text },
                                        { "address", cus_address.Text },
                                        { "phone", cus_tphone.Text },
                                        { "fax", cus_fax.Text },
                                        { "email", cus_email.Text }
                                       }
                                    }
                                };
                                 
                                    string json = JsonConvert.SerializeObject(final_val);
                                    var response = await connect.client.PutAsync(connect.customer_url+"/"+ph, new StringContent(json, Encoding.UTF8, "application/json"));

                                    var responseString = await response.Content.ReadAsStringAsync();
                                    if (connect.IsValidJson(responseString))
                                    {
                                        var obj = JObject.Parse(responseString);
                                        if (string.Equals(obj["status"].ToString(), "true", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            MessageBox.Show("CUSTOMER EDITED");
                                        }
                                        else
                                        {
                                            MessageBox.Show("CUSTOMER NOT EDITED\nMESSAGE:" + obj["data"].ToString());

                                        }
                                        customer_grid.ItemsSource = null;
                                        get_cust();
                                        clear();
                                    }
                                    else
                                    {
                                        MessageBox.Show("NO RESPONSE");
                                    }
                                    
                                }

                            };

                        }

                    }
                }
                catch(Exception m)
                {
                    MessageBox.Show(m.Message);
                }
                editpro_but.IsEnabled = true;
            }
            
        }

        private void cus_email_KeyDown(object sender, KeyEventArgs e)
        {
            if (connect.valid_email(cus_email.Text))
            {
                email_st.Text = "";
                cus_email.BorderBrush = new SolidColorBrush(Colors.White);
            }
            else
            {
                email_st.Text = "invalid email";
                cus_email.BorderBrush = new SolidColorBrush(Colors.Red);
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

        private void cus_shop_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (cus_shop.Text == "Shop name")
            {
                cus_shop.Text = "";
                cus_shop.Foreground = new SolidColorBrush(Colors.White);
                if (cs == 15)
                    status -= cs;
                cs = 0;

            }
            else if (cus_shop.Text == "")
            {
                cus_shop.Text = "Shop name";
                cus_shop.Foreground = new SolidColorBrush(Colors.Gray);
                if (cs == 15)
                    status -= cs;
                cs = 0;
            }
            else
            {
                if (cs == 0)
                {
                    cs = 15;
                    status += cs;
                }
            }
            status_update();
        }

        private void cus_email_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete || e.Key == Key.Back)
            {
                if (connect.valid_email(cus_email.Text))
                {
                    email_st.Text = "";
                    cus_email.BorderBrush = new SolidColorBrush(Colors.White);
                }
                else
                {
                    email_st.Text = "invalid email";
                    cus_email.BorderBrush = new SolidColorBrush(Colors.Red);
                }
            }
        }

       

        private void search_box_TextChanged(object sender, TextChangedEventArgs e)
        {
     
            if (Items.Count!=0)
            {
                pitems.Clear();

                foreach (var aitems in Items)
                {
                    if (search_box.Text == "" || search_box.Text == "Search customer's name")
                    {
                        i = 1;
                        customer_grid.ItemsSource = Items;
                    }
                    else
                    {
                        if (aitems.customer_name.StartsWith(search_box.Text, StringComparison.InvariantCultureIgnoreCase))
                        {
                            _items1.Add(new pitem {num=i.ToString(), customer_name = aitems.customer_name, shop_name = aitems.shop_name, address = aitems.address, phone_no = aitems.phone_no, fax = aitems.fax, email = aitems.email, gst_no = aitems.gst_no });
                            i++;
                            customer_grid.ItemsSource = pitems;
                        }
                    }

                }
            }
           
        }

        private void search_box_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {

            }
        }

        private void search_box_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (search_box.Text == "Search customer's name")
            {
                search_box.Text = "";
                search_box.Foreground = new SolidColorBrush(Colors.White);
            }
            else if (search_box.Text == "")
            {
                search_box.Text = "Search customer's name";
                search_box.Foreground = new SolidColorBrush(Colors.White);
            }
        }

        private void cancel_but_Click(object sender, RoutedEventArgs e)
        {
            clear();
        }
        public void clear()
        {
            submit_but.IsEnabled = true;
            status_block.Visibility = Visibility.Visible;
            status_progress.Visibility = Visibility.Visible;
            head_box.Text = "ADD CUSTOMER";
            submit_but.Visibility = Visibility.Visible;
            editpro_but.Visibility = Visibility.Hidden;
            cancel_but.Visibility = Visibility.Hidden;
            cn=cs=cg=ca=cp=cf=ce=0;
            status = 0;
            status_progress.Value = 0;
            email_st.Text = "";
            cus_email.BorderBrush = new SolidColorBrush(Colors.White);
            status_block.Text = "";
            cus_name.Text = "Customer name";
            cus_name.Foreground = new SolidColorBrush(Colors.Gray);
            cus_shop.Text = "Shop name";
            cus_shop.Foreground = new SolidColorBrush(Colors.Gray);
            cus_reg.Text = "GST number";
            cus_reg.Foreground = new SolidColorBrush(Colors.Gray);
            cus_address.Text = "Full address";
            cus_address.Foreground = new SolidColorBrush(Colors.Gray);
            cus_tphone.Text = "Phone number";
            cus_tphone.Foreground = new SolidColorBrush(Colors.Gray);
            cus_fax.Text = "Fax";
            cus_fax.Foreground = new SolidColorBrush(Colors.Gray);
            cus_email.Text = "E-mail";
            cus_email.Foreground = new SolidColorBrush(Colors.Gray);
        }
        public void get_cust()
        {
            if (connect.CheckForInternetConnection())
            {
                
                try
                {
                    HttpWebRequest req = WebRequest.Create(connect.customer_url) as HttpWebRequest;
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
                            Items.Clear();
                            int i = 1;
                            if (connect.IsValidJson(obj2["data"].ToString()))
                            {
                                Items.Clear();
                                
                                foreach (DataRow dr in ((DataTable)JsonConvert.DeserializeObject(obj2["data"].ToString(), typeof(DataTable))).Rows)
                                {
                                    _items.Add(new Item { num = i.ToString(), customer_name = dr["name"].ToString(),shop_name=dr["shop_name"].ToString(),  gst_no = dr["gst_no"].ToString(), address = dr["address"].ToString(), phone_no = dr["phone"].ToString(), email = dr["email"].ToString(), fax = dr["fax"].ToString() });
                                    i++;
                                }
                            }
                        }
                        customer_grid.ItemsSource = Items;
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

        
        public void status_update()
        {
            status_block.Text = status.ToString() + "% done";
            status_progress.Value = status;
        }
    }
}
