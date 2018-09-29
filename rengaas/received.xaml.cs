using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Newtonsoft.Json.Linq;

namespace rengaas
{
    /// <summary>
    /// Interaction logic for received.xaml
    /// </summary>
    public partial class received : Page
    {

        private static  HttpClient client = new HttpClient();
        public static string address_val,position;
        public static int viewed = 0,dt=0,check1=0;
        public class Item
        {
            public string num { get; set; }
            public string product_code { get; set; }
            public string product_name { get; set; }
            public string pquantity { get; set; }
            public string poquantity { get; set; }
            public string price { get; set; }
            public static string message { get; internal set; }
        }
        public class Item1
        {
            public string num { get; set; }
            public string date { get; set; }
            public string time { get; set; }
            public string po { get; set; }
            public string customer_name { get; set; }
            public string sales_man { get; set; }
            public string color { get; set; }
            public string stat { get; set; }
            public string product_code { get; set; }
            public string product_name { get; set; }
            public string pquantity { get; set; }
            public string poquantity { get; set; }
            public string price { get; set; }
            public JObject customer_details { get; set; }
        }
        public class check
        {
            public string product_code { get; set; }
            public string product_count { get; set; }
        }
        public class product_count
        {
            public string Id { get; set; }
            public string Cost { get; set; }
            public string count { get; set; }
            public string Name { get; set; }
            public string Date { get; set; }
            public string Time { get; set; }
            public string Customer { get; set; }

        }
        public List<Item> _items = new List<Item>();
        public static List<Item1> _items1 = new List<Item1>();
        public List<check> _items2 = new List<check>();
        public List<Item> Items
        {
            get { return _items; }
            set { _items = value; }
        }
        public static List<Item1> Items1
        {
            get { return _items1; }
            set { _items1 = value; }
        }
        public List<check> Item2
        {
            get { return _items2; }
            set { _items2 = value; }
        }
        public received()
        {
            check1 = 0;

            
            this.InitializeComponent();
            check1 = 1;
            order_get();
            
            print_doc.IsEnabled = false;
            purchase_grid.ItemsSource = Items1;
            dt = 1;
        }

        private void print_Click(object sender, RoutedEventArgs e)
        {

        }

        private void view_Click(object sender, RoutedEventArgs e)
        {

            view();
            viewed = 1;
        }
        public async void view()
        {
            if (connect.CheckForInternetConnection())
            {
                try
                {
                    print_doc.IsEnabled = true;
                    podatagrid.ItemsSource = null;
                    Items.Clear();
                    dynamic list = purchase_grid.SelectedItems[0];
                    position = list.po;

                    string val = "true";
                    if (list.color == "Red")
                    {
                        var values1 = new Dictionary<string, string>
                     {
                        { "product_code", list.po },
                    };
                        var content1 = new FormUrlEncodedContent(values1);
                        var response1 = await connect.client.PutAsync(connect.order_get_url + "/" + list.po + "/admin_view", content1);

                        var responseString1 = await response1.Content.ReadAsStringAsync();
                        var obj = JObject.Parse(responseString1);
                        
                        val = obj["status"].ToString();
                    }
                   
                    if (string.Equals(val, "true", StringComparison.CurrentCultureIgnoreCase))
                    {
                        JObject jc = (JObject)list.customer_details;
                        address_val = jc["name"].ToString() + "\n" + jc["address"].ToString() + "\nPhone number:" + jc["phone"].ToString() + "\nFax:" + jc["fax"].ToString() + "\nEmail:" + jc["email"].ToString() + "\nReg no:" + jc["gst_no"].ToString();
                        purchase_address.Text = address_val;
                        po_text.Text = list.po;
                        date_text.Text = list.date + " " + list.time;
                        sales_text.Text = list.sales_man;
                        string[] pcode = list.product_code.Split(',');
                        string[] pname = list.product_name.Split(',');
                        string[] pqty = list.pquantity.Split(',');
                        string[] poqty = list.poquantity.Split(',');
                        string[] price1 = list.price.Split(',');
                        for (int i = 0; i < pcode.Count() - 1; i++)
                        {
                            _items.Add(new Item { num = "1", product_code = pcode[i], product_name = pname[i], pquantity = pqty[i], poquantity = poqty[i], price = price1[i] });
                        }

                        podatagrid.ItemsSource = Items;

                        string[] pri = list.price.Split(',');
                        int sum = 0;
                        foreach (string i in pri)
                        {
                            if (i != "")
                            {
                                sum += Convert.ToInt32(i);
                            }

                        }
                        total_amt.Text = sum.ToString();
                        
                    }
                    else
                    {
                        MessageBox.Show("Details unavailable");
                    }


                    purchase_grid.ItemsSource = null;
                    order_get();
                }
                catch(Exception e)
                {
                    MessageBox.Show(e.Message);
                }
                
            }
        }

        

        public  void order_get()
        {
            
            if (connect.CheckForInternetConnection())
            {
                string d1 = date_slect.SelectedDate.Value.Date.ToString("MM-dd-yyyy");

                String url = connect.order_get_url;
                try
                {
                    HttpWebRequest req = WebRequest.Create(url) as HttpWebRequest;
                    req.Headers["AdminAuthToken"] = connect.auth_token;
                    string result = null;
                    Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                   
                    using (HttpWebResponse resp = req.GetResponse() as HttpWebResponse)
                    {
                        StreamReader reader = new StreamReader(resp.GetResponseStream());
                        

                        result = reader.ReadToEnd();
                    }
                   
                    if (connect.IsValidJson(result))
                    {
                        int i = 1;
                        JToken token = JToken.Parse(result);
                        if (string.Equals(token.SelectToken("status").ToString(), "true", StringComparison.CurrentCultureIgnoreCase))
                        {
                            Items1.Clear();
                            
                            int size = token.SelectToken("data").Count();
                            for (int v = 0; v < size; v++)
                            {
                                string pcode = "", pname = "", qty = "", cust = "", price1 = "", date = "", time = "", piece_count = "", porge_count = "";
                                string color1 = "Red";
                                string st = "View";
                                JArray details = (JArray)token.SelectToken("data[" + v.ToString() + "].details");
                                foreach (JToken m in details)
                                {
                                    if (m["Customer"] != null)
                                        pcode += m["Customer"].ToString() + ",";
                                    else
                                        pcode += "0,";
                                    if(m["Name"]!=null)
                                        pname += m["Name"].ToString() + ",";
                                    else
                                        pname += "0,";
                                    if (m["Pro_Img"]!=null)
                                        porge_count += m["Pro_Img"].ToString() + ",";
                                    else
                                        porge_count += "0,";
                                    if (m["count"]!=null)
                                        piece_count += m["count"].ToString() + ",";
                                    else
                                        piece_count += "0,";
                                    if (m["Cost"]!=null)
                                        price1 += m["Cost"].ToString() + ",";
                                    else
                                        price1 += "0,";
                                    if (m["Date"]!=null)
                                        date = m["Date"].ToString();
                                    else
                                        date += "0,";
                                    if (m["Time"]!=null)
                                        time = m["Time"].ToString();
                                    else
                                        time += "0,";
                                }
                                
                                if (Convert.ToBoolean(token.SelectToken("data[" + v.ToString() + "].admin_view").ToString()) == true)
                                {
                                    color1 = "Green";
                                    st = "Seen";
                                }

                                JObject ju = (JObject)token.SelectToken("data[" + v.ToString() + "].user");
                                JObject jc = (JObject)token.SelectToken("data[" + v.ToString() + "].customer");
                                if (date == d1)
                                {
                                    Console.WriteLine(token.SelectToken("data[" + v.ToString() + "].id").ToString()+color1);
                                    _items1.Add(new Item1 { po = token.SelectToken("data[" + v.ToString() + "].id").ToString(), product_code = pcode, product_name = pname, pquantity = piece_count, poquantity = porge_count, price = price1, date = date, time = time, color = color1, stat = st, customer_name = jc["name"].ToString(), sales_man = ju["first_name"].ToString(),customer_details=jc });
                                    i++;
                                }
                            }
                            
                         
                        }
                        Items1.Sort((y, x) => x.time.CompareTo(y.time));
                        purchase_grid.ItemsSource = Items1;
                        Mouse.OverrideCursor = Cursors.Arrow;
                    }


                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
            
        }
       
      

        private void print_doc_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (podatagrid.Items != null)
                {
                    Button clickedButton = (Button)sender;
                    if (clickedButton.Name == "print")
                    {
                        dynamic list2 = purchase_grid.SelectedItems[0];
                        if (viewed == 1 && position == list2.po)
                        {
                            print_document();
                        }
                        else
                        {
                            MessageBox.Show("Please view the purchase order before printing");
                        }
                    }
                    else
                    {
                        print_document();
                    }
                    
                }
            }
            catch(Exception m)
            {
                MessageBox.Show(m.Message);
            }
            
            
            
        }
        void print_document()
        {
            print_doc.IsEnabled = true;
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "report"; // Default file name
            dlg.DefaultExt = ".pdf"; // Default file extension
            dlg.Filter = "PDF document (.pdf)|*.pdf"; // Filter files by extension

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                if (Items.Count == 0)
                {
                    view();
                }
                // Save document
                try
                {
                    string filename = dlg.FileName;
                    iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance("Rengas.png");
                    image.ScaleToFit(50f, 50f);
                    image.Alignment = Element.ALIGN_CENTER;
                    Document doc = new Document(iTextSharp.text.PageSize.A4, 10, 10, 10, 10);

                    //string fileName = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".pdf";
                    PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(filename, FileMode.Create));
                    File.SetAttributes(filename, FileAttributes.Normal);
                    doc.Open();
                    iTextSharp.text.Paragraph p1 = new iTextSharp.text.Paragraph();
                    string h1 = "PURCHASE ORDER";
                    p1.Alignment = Element.ALIGN_CENTER;
                    p1.Font = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 15f, BaseColor.WHITE);
                    p1.Add(h1);
                    PdfPTable table_main = new PdfPTable(1);
                    PdfPCell cell16 = new PdfPCell();
                    cell16.AddElement(p1);
                    cell16.BackgroundColor = new BaseColor(0, 0, 0);
                    table_main.AddCell(cell16);
                    doc.Add(table_main);

                    iTextSharp.text.Paragraph p9 = new iTextSharp.text.Paragraph("\n");
                    doc.Add(p9);
                    PdfPTable table = new PdfPTable(2);
                    float[] widths = new float[] { 40f, 60f };
                    table.SetWidths(widths);
                    table.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    iTextSharp.text.Paragraph p5 = new iTextSharp.text.Paragraph();
                    string h5 = "PURCHASE ORDER ADDRESS\n";
                    p5.Alignment = Element.ALIGN_CENTER;
                    p5.Font = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12f, BaseColor.BLACK);
                    p5.Add(h5);
                    iTextSharp.text.Paragraph p6 = new iTextSharp.text.Paragraph();
                    p6.Add(purchase_address.Text);
                    PdfPCell cell = new PdfPCell();
                    cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    cell.BackgroundColor = new BaseColor(218, 218, 218);
                    cell.AddElement(p5);
                    cell.AddElement(p6);
                    table.AddCell(cell);
                    PdfPCell cell9 = new PdfPCell();
                    string h2 = "\nRENGA TRADING";
                    cell9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    Chunk h = new Chunk();
                    h.Font = FontFactory.GetFont("Segoe UI", 15f, Font.BOLD, BaseColor.RED);
                    h.Append(h2);
                    Chunk h4 = new Chunk(image, 0, 0);
                    iTextSharp.text.Paragraph p = new iTextSharp.text.Paragraph();
                    p.Add(h4);
                    p.Add(h);
                    p.Alignment = Element.ALIGN_CENTER;
                    cell9.AddElement(p);
                    iTextSharp.text.Paragraph p10 = new iTextSharp.text.Paragraph("\n");
                    doc.Add(p9);
                    PdfPTable table2 = new PdfPTable(2);
                    table2.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    PdfPCell cell6 = new PdfPCell(new Phrase("P.O"));
                    cell6.BackgroundColor = new BaseColor(218, 218, 218);

                    cell6.BorderWidth = 2f;

                    cell6.BorderColor = new BaseColor(255, 255, 255);
                    table2.AddCell(cell6);
                    PdfPCell cell13 = new PdfPCell(new Phrase(po_text.Text));
                    cell13.BackgroundColor = new BaseColor(218, 218, 218);
                    cell13.BorderWidth = 2f;
                    cell13.BorderColor = new BaseColor(255, 255, 255);
                    table2.AddCell(cell13);
                    PdfPCell cell7 = new PdfPCell(new Phrase("Date & time"));

                    cell7.BackgroundColor = new iTextSharp.text.BaseColor(218, 218, 218);
                    cell7.BorderWidth = 2f;

                    cell7.BorderColor = new BaseColor(255, 255, 255);
                    table2.AddCell(cell7);
                    PdfPCell cell14 = new PdfPCell(new Phrase(date_text.Text));
                    cell14.BackgroundColor = new BaseColor(218, 218, 218);
                    cell14.BorderWidth = 2f;
                    cell14.BorderColor = new BaseColor(255, 255, 255);
                    table2.AddCell(cell14);
                    PdfPCell cell8 = new PdfPCell(new Phrase("Sales man"));

                    cell8.BackgroundColor = new iTextSharp.text.BaseColor(218, 218, 218);
                    cell8.BorderWidth = 2f;
                    cell8.BorderColor = new BaseColor(255, 255, 255);
                    table2.AddCell(cell8);
                    PdfPCell cell15 = new PdfPCell(new Phrase(sales_text.Text));
                    cell15.BackgroundColor = new BaseColor(218, 218, 218);
                    cell15.BorderWidth = 2f;
                    cell15.BorderColor = new BaseColor(255, 255, 255);
                    table2.AddCell(cell15);
                    cell9.AddElement(table2);
                    table.AddCell(cell9);

                    doc.Add(table);
                    iTextSharp.text.Paragraph p11 = new iTextSharp.text.Paragraph("\n");
                    doc.Add(p11);
                    PdfPTable table1 = new PdfPTable(6);
                    float[] widths1 = new float[] { 10f, 20f, 30f, 10f, 10f, 30f };
                    table1.SetWidths(widths1);
                    iTextSharp.text.Paragraph hs = new iTextSharp.text.Paragraph("S.NO");
                    hs.Font = FontFactory.GetFont("Segoe UI", 10f, Font.BOLD, BaseColor.BLACK);
                    hs.Alignment = Element.ALIGN_CENTER;

                    PdfPCell cell1 = new PdfPCell(hs);
                    cell1.AddElement(hs);
                    cell1.BackgroundColor = new iTextSharp.text.BaseColor(218, 218, 218);
                    table1.AddCell(cell1);
                    iTextSharp.text.Paragraph hp = new iTextSharp.text.Paragraph("PRODUCT CODE");
                    hp.Font = FontFactory.GetFont("Segoe UI", 10f, Font.BOLD, BaseColor.BLACK);
                    hp.Alignment = Element.ALIGN_CENTER;
                    PdfPCell cell2 = new PdfPCell();
                    cell2.AddElement(hp);
                    cell2.BackgroundColor = new iTextSharp.text.BaseColor(218, 218, 218);
                    table1.AddCell(cell2);
                    iTextSharp.text.Paragraph hpn = new iTextSharp.text.Paragraph("PRODUCT NAME");
                    hpn.Font = FontFactory.GetFont("Segoe UI", 10f, Font.BOLD, BaseColor.BLACK);
                    hpn.Alignment = Element.ALIGN_CENTER;
                    PdfPCell cell3 = new PdfPCell();
                    cell3.AddElement(hpn);
                    cell3.BackgroundColor = new iTextSharp.text.BaseColor(218, 218, 218);
                    table1.AddCell(cell3);
                    iTextSharp.text.Paragraph hpq = new iTextSharp.text.Paragraph("PIECE QTY");
                    hpq.Font = FontFactory.GetFont("Segoe UI", 10f, Font.BOLD, BaseColor.BLACK);
                    hpq.Alignment = Element.ALIGN_CENTER;
                    PdfPCell cell4 = new PdfPCell(hpq);
                    cell4.AddElement(hpq);
                    cell4.BackgroundColor = new iTextSharp.text.BaseColor(218, 218, 218);
                    table1.AddCell(cell4);
                    iTextSharp.text.Paragraph hbq = new iTextSharp.text.Paragraph("BOX QTY");
                    hbq.Font = FontFactory.GetFont("Segoe UI", 10f, Font.BOLD, BaseColor.BLACK);
                    hbq.Alignment = Element.ALIGN_CENTER;
                    PdfPCell cell12 = new PdfPCell(hbq);
                    cell12.AddElement(hbq);
                    cell12.BackgroundColor = new iTextSharp.text.BaseColor(218, 218, 218);
                    table1.AddCell(cell12);
                    iTextSharp.text.Paragraph hpp = new iTextSharp.text.Paragraph("PRICE");
                    hpp.Font = FontFactory.GetFont("Segoe UI", 10f, Font.BOLD, BaseColor.BLACK);
                    hpp.Alignment = Element.ALIGN_CENTER;
                    PdfPCell cell5 = new PdfPCell();
                    cell5.AddElement(hpp);
                    cell5.BackgroundColor = new iTextSharp.text.BaseColor(218, 218, 218);
                    table1.AddCell(cell5);
                    foreach (Item l1 in podatagrid.ItemsSource)
                    {
                        table1.AddCell(l1.num);
                        table1.AddCell(l1.product_code);
                        table1.AddCell(l1.product_name);
                        table1.AddCell(l1.pquantity);
                        table1.AddCell(l1.poquantity);
                        table1.AddCell(l1.price);
                    }
                    doc.Add(table1);
                    iTextSharp.text.Paragraph p12 = new iTextSharp.text.Paragraph("\n\n");
                    doc.Add(p12);
                    PdfPTable table3 = new PdfPTable(4);
                    PdfPCell cell110 = new PdfPCell(new Phrase(""));
                    cell110.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    PdfPCell cell120 = new PdfPCell(new Phrase(""));
                    cell120.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    table3.AddCell(cell110);
                    table3.AddCell(cell120);
                    PdfPCell cell10 = new PdfPCell(new Phrase("Total amount"));
                    cell10.BackgroundColor = new iTextSharp.text.BaseColor(218, 218, 218);
                    cell10.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    table3.AddCell(cell10);
                    PdfPCell cell11 = new PdfPCell(new Phrase(total_amt.Text));
                    table3.AddCell(cell11);
                    doc.Add(table3);
                    MessageBox.Show("Purchase order generated");
                    doc.Close();
                    //pdf_print(filename);
                    viewed = 0;
                }
                catch(Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }
        private void date_slect_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dt == 1 && check1==1)
            {
                Items1.Clear();
                purchase_grid.ItemsSource = null;
                podatagrid.ItemsSource = null;
                order_get();

            }
               
        }

        
    }
}
