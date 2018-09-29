using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
namespace rengaas
{
    public partial class received_ord : Page
    {
        public int set = 0;
        public int position;
        public int status;
        public int pn;
        public int pc;
        public int pcat;
        public int pq;
        public int pbq;
        public int pv;
        public int pp;
        public int pi;
        public string[] fileNames = new string[2];
        public string imurl = "";
        public string aurl = "";
        public string omurl;
        public int i = 1;
        public string opcode = "";
        public List<Item> Items
        {
            get { return _items; }
            set { _items = value; }
        }

        public List<citem> citems
        {
            get { return _items1; }
            set { _items1 = value; }
        }





        public class citem
        {


            public string num { get; set; }

            public string image_url { get; set; }

            public string product_code { get; set; }

            public string product_name { get; set; }

            public string category { get; set; }

            public string box_quantity { get; set; }

            public string box_available { get; set; }

            public string product_quantity { get; set; }

            public string uom { get; set; }

            public string pprice { get; set; }

            public string bprice { get; set; }
        }

        public class Item
        {


            public string num { get; set; }
            public string oimage_url { get; set; }
            public string url { get; set; }
            public string image_url { get; set; }
            public string product_code { get; set; }

            public string product_name { get; set; }

            public string category { get; set; }

            public string product_quantity { get; set; }

            public string pprice { get; set; }

            public string box_available { get; set; }

            public string box_quantity { get; set; }

            public string bprice { get; set; }

            public string uom { get; set; }
        }
        public List<Item> _items = new List<Item>();
        public List<citem> _items1 = new List<citem>();

        public received_ord()
        {
            InitializeComponent();
            set = 1;
            this.product_get();
            this.status_progress.Visibility = Visibility.Hidden;
            this.status_block.Visibility = Visibility.Hidden;
            this.status_block.Text = "0% done";
        }

        

        private void box_price_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.box_price.Text == "Box Price")
            {
                this.box_price.Text = "";
                this.box_price.Foreground = new SolidColorBrush(Colors.White);
                if (this.pq == 15)
                {
                    this.status -= this.pq;
                }
                this.pq = 0;
            }
            else if (this.box_price.Text != "")
            {
                if (this.pq == 0)
                {
                    this.pq = 15;
                    this.status += this.pq;
                }
            }
            else
            {
                this.box_price.Text = "Box Price";
                this.box_price.Foreground = new SolidColorBrush(Colors.Gray);
                if (this.pq == 15)
                {
                    this.status -= this.pq;
                }
                this.pq = 0;
            }
        }

        private void box_qty_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.box_qty.Text == "Piece Per Box")
            {
                this.box_qty.Text = "";
                this.box_qty.Foreground = new SolidColorBrush(Colors.White);
                if (this.pbq == 15)
                {
                    this.status -= this.pbq;
                }
                this.pbq = 0;
            }
            else if (this.box_qty.Text != "")
            {
                if (this.pbq == 0)
                {
                    this.pbq = 15;
                    this.status += this.pbq;
                }
            }
            else
            {
                this.box_qty.Text = "Piece Per Box";
                this.box_qty.Foreground = new SolidColorBrush(Colors.Gray);
                if (this.pbq == 15)
                {
                    this.status -= this.pbq;
                }
                this.pbq = 0;
            }
        }

        private void cancel_but_Click(object sender, RoutedEventArgs e)
        {
            this.clear();
        }

        public void clear()
        {
            this.head_box.Text = "ADD PRODUCTS";
            this.submit_but.Visibility = Visibility.Visible;
            this.editpro_but.Visibility = Visibility.Hidden;
            this.cancel_but.Visibility = Visibility.Hidden;
            this.pro_name.Text = "Product Name";
            this.pro_name.Foreground = new SolidColorBrush(Colors.Gray);
            this.pro_code.Text = "Product Code";
            this.pro_code.Foreground = new SolidColorBrush(Colors.Gray);
            this.pro_category.Text = "Category";
            this.pro_category.Foreground = new SolidColorBrush(Colors.Gray);
            this.pro_uom.Text = "Uom";
            this.pro_uom.Foreground = new SolidColorBrush(Colors.Gray);
            this.box_price.Text = "Box Price";
            this.box_price.Foreground = new SolidColorBrush(Colors.Gray);
            this.box_qty.Text = "Piece Per Box";
            this.box_qty.Foreground = new SolidColorBrush(Colors.Gray);
            this.pro_price.Text = "Piece Price";
            this.pro_price.Foreground = new SolidColorBrush(Colors.Gray);
        }

        private void DELETE_Click(object sender, RoutedEventArgs e)
        {
            if (connect.CheckForInternetConnection())
            {
                try
                {
                    dynamic list = this.product_grid.SelectedItems[0];
                    string ms = "Do you want to delete \n product:  " + list.product_name;
                    var dialog = new MyDialog("DELETE PRODUCT", ms);
                    dialog.Show();
                    dialog.Closing += async (sender1, e1) =>
                    {
                        var d = sender1 as MyDialog;
                        if (!d.Canceled)
                        {
                            try
                            {
                                var response = await connect.client.DeleteAsync(connect.product_url + "/" + list.product_code);

                                var responseString = await response.Content.ReadAsStringAsync();
                                if (connect.IsValidJson(responseString))
                                {
                                    var obj = JObject.Parse(responseString);
                                    if (string.Equals(obj["status"].ToString(), "true", StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        MessageBox.Show("PRODUCT DELETED");
                                    }
                                    else
                                    {
                                        MessageBox.Show("PRODUCT NOT DELETED" + responseString);
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("NO RESPONSE");
                                }


                                product_grid.ItemsSource = null;
                                product_get();
                                clear();
                            }
                            catch(Exception m)
                            {
                                MessageBox.Show(m.Message);
                            }

                        }

                    };
                }
                catch (Exception m)
                {
                    MessageBox.Show(m.Message);
                }
                
            }
        }

        private void EDIT_Click(object sender, RoutedEventArgs e)
        {
            position = product_grid.SelectedIndex;
            head_box.Text = "EDIT PRODUCT";
            submit_but.Visibility = Visibility.Hidden;
            editpro_but.Visibility = Visibility.Visible;
            cancel_but.Visibility = Visibility.Visible;
            dynamic list = product_grid.SelectedItems[0];
            this.omurl = list.oimage_url;
            this.imurl = list.image_url;
            this.aurl = list.url;
            this.pro_name.Text = list.product_name;
            this.pro_name.Foreground = new SolidColorBrush(Colors.White);
            this.pro_code.Text = list.product_code;
            this.pro_code.Foreground = new SolidColorBrush(Colors.White);
            this.opcode = list.product_code;
            this.box_qty.Text = list.box_quantity;
            this.box_qty.Foreground = new SolidColorBrush(Colors.White);
            this.box_price.Text = list.bprice;
            this.box_price.Foreground = new SolidColorBrush(Colors.White);
            this.pro_category.Text = list.category;
            this.pro_category.Foreground = new SolidColorBrush(Colors.White);
            this.pro_uom.Text = list.uom;
            this.pro_uom.Foreground = new SolidColorBrush(Colors.White);
            this.pro_price.Text = list.pprice;
            this.pro_price.Foreground = new SolidColorBrush(Colors.White);
        }

        public void edit_msg()
        {
        }

        private void editpro_but_Click(object sender, RoutedEventArgs e)
        {
            if (connect.CheckForInternetConnection())
            {
                editpro_but.IsEnabled = false;
                int num1;
                if ((this.pro_code.Text == "Product Code") || (this.pro_name.Text == "Product Name") || (this.pro_category.Text == "Category") ||  (this.pro_uom.Text == "Uom") || (this.pro_price.Text == "Piece price"))
                {
                    num1 = 1;
                }
                else
                {
                    num1 = 0;
                }
                if (num1 != 0)
                {
                    MessageBox.Show("EMPTY FIELDS");
                }
                else
                {
                    bool val = false;
                    
                    if (this.box_price.Text == "Box Price")
                    {
                        this.box_price.Text = "0";
                    }
                    if (this.box_qty.Text == "Piece Per Box")
                    {
                        this.box_qty.Text = "0";
                    }
                    if (pro_price.Text == "Piece Price")
                    {
                        pro_price.Text = "0";
                    }
                    if (this.head_box.Text == "EDIT PRODUCT")
                    {
                        string input = "Do you want to edit \n Product Code: " + this.opcode;
                        var dialog = new MyDialog("EDIT PRODUCT", input);
                        dialog.Show();
                        dialog.Closing += async (sender1, e1) =>
                        {
                            var d = sender1 as MyDialog;
                            if (!d.Canceled)
                            {
                                try
                                {
                                    string file1 = omurl;
                                    dynamic file2 = aurl;
                                    var final_val = new Dictionary<string, Dictionary<string, string>>
                                {
                                    {"product",new Dictionary<string, string> {
                                        { "image_url", "Rengas logo.png" },
                                        { "product_name", pro_name.Text },
                                        { "product_code", pro_code.Text },
                                        { "porge_avaiable", val.ToString() },
                                        { "piece_per_porge", box_qty.Text },
                                        { "porge_price", box_price.Text },
                                        { "piece_price", pro_price.Text },
                                        { "uom", pro_uom.Text },
                                        { "category", pro_category.Text }
                                       }
                                    }
                                };
                                    string json = JsonConvert.SerializeObject(final_val);
                                    var response = await connect.client.PutAsync(connect.product_url + "/" + opcode, new StringContent(json, Encoding.UTF8, "application/json"));

                                    var responseString = await response.Content.ReadAsStringAsync();
                                    if (connect.IsValidJson(responseString))
                                    {
                                        var obj = JObject.Parse(responseString);
                                        if (string.Equals(obj["status"].ToString(), "true", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            MessageBox.Show("PRODUCT EDITED");
                                        }
                                        else
                                        {
                                            MessageBox.Show("PRODUCT NOT EDITED");
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("NO RESPONSE");
                                    }

                                    product_grid.ItemsSource = null;
                                    product_get();
                                    clear();
                                }
                                catch(Exception E)
                                {
                                    MessageBox.Show(E.Message);
                                }
                            }

                        };
                    }

                    
                }
                editpro_but.IsEnabled = true;

            }
        }

        private static System.Drawing.Image image_compress(System.Drawing.Image org_img, System.Drawing.Size newsize)
        {
            System.Drawing.Image image = new Bitmap(newsize.Width, newsize.Height);
            using (Graphics graphics = Graphics.FromImage((Bitmap) image))
            {
                graphics.DrawImage(org_img, new System.Drawing.Rectangle(System.Drawing.Point.Empty, newsize));
            }
            return image;
        }

        

        private void pro_category_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.pro_category.Text != "Category")
            {
                if (this.pcat == 0)
                {
                    this.pcat = 15;
                    this.status += this.pcat;
                }
            }
            else
            {
                if (this.pcat == 15)
                {
                    this.status -= this.pcat;
                }
                this.pcat = 0;
            }
            this.status_update();
        }

        private void pro_code_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.pro_code.Text == "Product Code")
            {
                this.pro_code.Text = "";
                this.pro_code.Foreground = new SolidColorBrush(Colors.White);
                if (this.pc == 15)
                {
                    this.status -= this.pc;
                }
                this.pc = 0;
            }
            else if (this.pro_code.Text != "")
            {
                if (this.pc == 0)
                {
                    this.pc = 15;
                    this.status += this.pc;
                }
            }
            else
            {
                this.pro_code.Text = "Product Code";
                this.pro_code.Foreground = new SolidColorBrush(Colors.Gray);
                if (this.pc == 15)
                {
                    this.status -= this.pc;
                }
                this.pc = 0;
            }
            this.status_update();
        }

        private void pro_name_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.pro_name.Text == "Product Name")
            {
                this.pro_name.Text = "";
                this.pro_name.Foreground = new SolidColorBrush(Colors.White);
                if (this.pn == 15)
                {
                    this.status -= this.pn;
                }
                this.pn = 0;
            }
            else if (this.pro_name.Text != "")
            {
                if (this.pn == 0)
                {
                    this.pn = 15;
                    this.status += this.pn;
                }
            }
            else
            {
                this.pro_name.Text = "Product Name";
                this.pro_name.Foreground = new SolidColorBrush(Colors.Gray);
                if (this.pn == 15)
                {
                    this.status -= this.pn;
                }
                this.pn = 0;
            }
            this.status_update();
        }

        private void pro_price_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.pro_price.Text == "Piece Price")
            {
                this.pro_price.Text = "";
                this.pro_price.Foreground = new SolidColorBrush(Colors.White);
                if (this.pp == 15)
                {
                    this.status -= this.pp;
                }
                this.pp = 0;
            }
            else if (this.pro_price.Text != "")
            {
                if (this.pp == 0)
                {
                    this.pp = 15;
                    this.status += this.pp;
                }
            }
            else
            {
                this.pro_price.Text = "Piece Price";
                this.pro_price.Foreground = new SolidColorBrush(Colors.Gray);
                if (this.pp == 15)
                {
                    this.status -= this.pp;
                }
                this.pp = 0;
            }
        }

        
        

        public void product_get()
        {

            if (connect.CheckForInternetConnection())
            {
                string requestUriString = connect.product_url;
                try
                {
                    HttpWebRequest request = WebRequest.Create(connect.product_url) as HttpWebRequest;
                    request.Headers["AdminAuthToken"] = connect.auth_token;
                    string strInput = null;
                    using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                    {
                        Mouse.OverrideCursor = Cursors.Wait;
                        strInput = new StreamReader(response.GetResponseStream()).ReadToEnd();
                    }
                    if (!connect.IsValidJson(strInput))
                    {
                        return;
                    }
                    else
                    {
                        JObject obj2 = JObject.Parse(strInput);
                        if (Convert.ToBoolean(obj2["status"].ToString()))
                        {
                            this.Items.Clear();
                            int num1 = 1;
                            if (connect.IsValidJson(obj2["data"].ToString()))
                            {
                                foreach (DataRow row in ((DataTable)JsonConvert.DeserializeObject(obj2["data"].ToString(), typeof(DataTable))).Rows)
                                {
                                    string str3 = "Rengas logo.png";
                                    if (row["product_code"].ToString() != "")
                                    {

                                        _items.Add(new Item { num = num1.ToString(), url = row["url"].ToString(), image_url = str3, oimage_url = row["image_url"].ToString(), product_code = row["product_code"].ToString(), product_name = row["product_name"].ToString(), category = row["category"].ToString(), box_quantity = row["piece_per_porge"].ToString(), bprice = row["porge_price"].ToString(),  pprice = row["piece_price"].ToString(), uom = row["uom"].ToString() });

                                        num1++;
                                    }
                                }
                            }
                            
                        }
                    }
                    product_grid.ItemsSource = null;
                    product_grid.ItemsSource = Items;
                    Mouse.OverrideCursor = Cursors.Arrow;
                    request.Abort();
                }
                catch (Exception exception1)
                {
                    MessageBox.Show(exception1.Message);
                }
            }
        }

        private void search_box_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (search_box.Text == "Search product's name")
            {
                search_box.Text = "";
                search_box.Foreground = new SolidColorBrush(Colors.White);
            }
            else if (search_box.Text == "")
            {
                search_box.Text = "Search product's name";
                search_box.Foreground = new SolidColorBrush(Colors.White);
            }
        }

        private void search_box_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
            }
        }

        private void search_box_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.Items.Count != 0)
            {
                this.citems.Clear();
                int num = 1;
                if (this.Items != null)
                {
                    foreach (Item item in this.Items)
                    {
                        if ((this.search_box.Text == "") || (search_box.Text == "Search product's name"))
                        {
                            num = 1;
                            product_grid.ItemsSource = Items;
                        }
                        else if (item.product_name.StartsWith(search_box.Text, StringComparison.InvariantCultureIgnoreCase))
                        {
                            _items1.Add(new citem { num = num.ToString(), image_url = item.image_url, product_code = item.product_code, product_name = item.product_name, category = item.category, box_quantity = item.box_quantity, uom = item.uom });
                            num++;
                            this.product_grid.ItemsSource = citems;
                        }
                    }
                }
            }
        }

        public void status_update()
        {
            this.status_block.Text = this.status.ToString() + "% done";
            this.status_progress.Value = this.status;
        }

        private void submit_but_Click(object sender, RoutedEventArgs e)
        {
            
            if (connect.CheckForInternetConnection())
            {
                submit_but.IsEnabled = false;
                int num1;
                if ((this.pro_code.Text == "Product Code") || (this.pro_name.Text == "Product Name") || (this.pro_category.Text == "Category")  || (this.pro_uom.Text == "Uom") || (this.pro_price.Text == "Piece price"))
                {
                    num1 = 1;
                }
                else
                {
                    num1 = 0;
                }
                if (num1 != 0)
                {
                    MessageBox.Show("EMPTY FIELDS");
                }
                else
                {
                    bool val = false;
                    bool flag3 = connect.CheckForInternetConnection();
                  
                    if (box_price.Text == "Box Price")
                    {
                        box_price.Text = "0";
                    }
                    if (box_qty.Text == "Piece Per Box")
                    {
                        box_qty.Text = "0";
                    }
                    if (pro_price.Text == "Piece Price")
                    {
                        pro_price.Text = "0";
                    }
                    if (flag3 &&head_box.Text == "ADD PRODUCTS")
                    {
                        string input = "Do you want to add \n Product Name: " + pro_name.Text;
                        var dialog = new MyDialog("ADD PRODUCT", input);
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
                                    {"product",new Dictionary<string, string> {
                                        { "image_url", "Rengas logo.png" },
                                        { "product_name", pro_name.Text },
                                        { "product_code", pro_code.Text },
                                        { "porge_avaiable", val.ToString() },
                                        { "piece_per_porge", box_qty.Text },
                                        { "porge_price", box_price.Text },
                                        { "piece_price", pro_price.Text },
                                        { "uom", pro_uom.Text },
                                        { "category", pro_category.Text }
                                       }
                                    }
                                };

                                    string json = JsonConvert.SerializeObject(final_val);
                                    var response = await connect.client.PostAsync(connect.product_url, new StringContent(json, Encoding.UTF8, "application/json"));

                                    var responseString = await response.Content.ReadAsStringAsync();
                                    if (connect.IsValidJson(responseString))
                                    {
                                        var obj = JObject.Parse(responseString);
                                        if (string.Equals(obj["status"].ToString(), "true", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            MessageBox.Show("PRODUCT ADDED");
                                        }
                                        else
                                        {
                                            MessageBox.Show("PRODUCT NOT ADDED");
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("NO RESPONSE");
                                    }

                                    product_grid.ItemsSource = null;
                                    product_get();
                                    clear();
                                }
                                catch(Exception m)
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

        

        

        private void product_grid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void pro_category_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.pro_category.Text == "Category")
            {
                this.pro_category.Text = "";
                this.pro_category.Foreground = new SolidColorBrush(Colors.White);
                if (this.pcat == 15)
                {
                    this.status -= this.pcat;
                }
                this.pcat = 0;
            }
            else if (this.pro_category.Text != "")
            {
                if (this.pcat == 0)
                {
                    this.pcat = 15;
                    this.status += this.pcat;
                }
            }
            else
            {
                this.pro_category.Text = "Category";
                this.pro_category.Foreground = new SolidColorBrush(Colors.Gray);
                if (this.pcat == 15)
                {
                    this.status -= this.pcat;
                }
                this.pcat = 0;
            }
            
            this.status_update();
        }

        private void pro_uom_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.pro_uom.Text == "Uom")
            {
                this.pro_uom.Text = "";
                this.pro_uom.Foreground = new SolidColorBrush(Colors.White);
                if (this.pv == 15)
                {
                    this.status -= this.pv;
                }
                this.pv = 0;
            }
            else if (this.pro_uom.Text != "")
            {
                if (this.pv == 0)
                {
                    this.pv = 15;
                    this.status += this.pv;
                }
            }
            else
            {
                this.pro_uom.Text = "Uom";
                this.pro_uom.Foreground = new SolidColorBrush(Colors.Gray);
                if (this.pv == 15)
                {
                    this.status -= this.pv;
                }
                this.pv = 0;
            }

            this.status_update();
        }

        private void box_qty_TextChanged(object sender, TextChangedEventArgs e)
        {
            int parsedvalue;
            if (box_qty.Text!="Piece Per Box" && set==1 && box_qty.Text!="")
            {
                if (!int.TryParse(box_qty.Text, out parsedvalue))
                {
                    MessageBox.Show("invalid value");
                    Keyboard.ClearFocus();
                    box_qty.Text = "Piece Per Box";
                    box_qty.Foreground = new SolidColorBrush(Colors.Gray);
                    
                }
            }
        }

        private void box_price_TextChanged(object sender, TextChangedEventArgs e)
        {
            double parsedvalue;
            if (box_price.Text != "Box Price" && set == 1 && box_price.Text != "")
            {
                if (!double.TryParse(box_price.Text, out parsedvalue))
                {
                    MessageBox.Show("invalid value");
                    Keyboard.ClearFocus();
                    box_price.Text = "Box Price";
                    box_price.Foreground = new SolidColorBrush(Colors.Gray);

                }
            }
        }

        private void pro_price_TextChanged(object sender, TextChangedEventArgs e)
        {
            double parsedvalue;
            if (pro_price.Text != "Piece Price" && set == 1 && pro_price.Text != "")
            {
                if (!double.TryParse(pro_price.Text, out parsedvalue))
                {
                    MessageBox.Show("invalid value");
                    Keyboard.ClearFocus();
                    pro_price.Text = "Piece Price";
                    pro_price.Foreground = new SolidColorBrush(Colors.Gray);

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

        private void pro_code_TextChanged(object sender, TextChangedEventArgs e)
        {
            var regexItem = new Regex("^[a-zA-Z0-9 ]*$");
            if (pro_code.Text!="Product Code" && !regexItem.IsMatch(pro_code.Text))
            {
                MessageBox.Show("NO SPECIAL CHARACTERS ALLOWED");
                Keyboard.ClearFocus();
                pro_code.Text = "Product Code";
                pro_code.Foreground = new SolidColorBrush(Colors.Gray);
            }
        }

        
    }
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    